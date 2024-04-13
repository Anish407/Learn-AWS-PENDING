using MediatR;
using Sqs.Api.Consumer.BackgroundService.Dto;

namespace Sqs.Api.Consumer.MessageHandlers;

public class PublishMessageHandler(ILogger<PublishMessageHandler> logger):IRequestHandler<PublishMessageRequest>
{
    public async Task Handle(PublishMessageRequest publishMessage, CancellationToken cancellationToken)
    {
        if (publishMessage is not null)
            logger.LogInformation("Id:{id}, Name:{name}, quantity:{quantity}", publishMessage.Id,
                publishMessage.ProductName, publishMessage.Quantity);
        await DoSomething();
    }

    private  Task DoSomething()
    {
        return Task.CompletedTask;
    }
}