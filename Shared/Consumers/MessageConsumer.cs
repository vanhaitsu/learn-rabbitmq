using MassTransit;
using Shared.Models;

namespace Shared.Consumers;

public class MessageConsumer : IConsumer<Message>
{
    public Task Consume(ConsumeContext<Message> context)
    {
        Console.WriteLine($"({DateTime.Now}) Message submitted: {context.Message.Text}");

        return Task.CompletedTask;
    }
}