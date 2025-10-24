using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Entities;
using Shared.Utils;

namespace Shared.Consumers;

public class LetterConsumer(ILogger logger) : IConsumer<Letter>
{
    public Task Consume(ConsumeContext<Letter> context)
    {
        logger.LogInformation(StringTools.GenerateSuccessMessage(context.Message.ToString()));

        return Task.CompletedTask;
    }
}