namespace Shared.Interfaces;

public interface IRabbitMqService : IAsyncDisposable
{
    Task InitializeAsync(string host, string username, string password);
    Task PublishAsync<T>(string exchangeName, string queueName, string routingKey, T message);
}