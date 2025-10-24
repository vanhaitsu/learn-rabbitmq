using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared.Interfaces;
using Shared.Utils;

namespace Shared.Services;

public class RabbitMqService : IRabbitMqService
{
    /*
     * Connection:
     * A TCP connection between application and RabbitMQ.
     * Expensive to create and must be resued across the application lifetime.
     * Thread-safe, can safely share one connection in whole app.
     *
     * Recommendation:
     * Use singleton pattern or DI container to ensure one connection.
     */
    private IConnection? _connection;
    private ConnectionFactory? _factory;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(ILogger<RabbitMqService> logger)
    {
        _logger = logger;
    }

    public async Task InitializeAsync(string host, string username, string password)
    {
        if (_factory != null) return;

        _factory = new ConnectionFactory
        {
            HostName = host,
            UserName = username,
            Password = password
        };

        _connection = await _factory.CreateConnectionAsync();
    }

    public async Task PublishAsync<T>(string exchangeName, string queueName, string routingKey, T message)
    {
        if (_connection == null || !_connection.IsOpen)
            throw new InvalidOperationException("RabbitMQ connection is not initialized.");

        /*
         * Channel:
         * A virtual connection within a connection.
         * Lightweight compared to connections.
         * Not thread-safe, each thread (or task) must have its own.
         *
         * Recommendation:
         * Create one per logical operation or maintain a pool/thread-local storage across multiple threads.
         */
        var channel = await _connection.CreateChannelAsync();

        /*
         * Exchange:
         * Not a resource per se, it's a configuration in RabbitMQ.
         * Declare the same exchange repeatedly is safe and idempotent.
         * However, it's wasteful to declare every time publish.
         *
         * Recommendation:
         * Declare exchanges during app initialization or once per channel reuse.
         */
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
        await channel.QueueDeclareAsync(queueName, true, false, false);
        await channel.QueueBindAsync(queueName, exchangeName, routingKey);
        var messageString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageString);
        await channel.BasicPublishAsync(exchangeName, routingKey, body);
        _logger.LogInformation(StringTools.GenerateSuccessMessage(messageString));
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection?.IsOpen == true)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}