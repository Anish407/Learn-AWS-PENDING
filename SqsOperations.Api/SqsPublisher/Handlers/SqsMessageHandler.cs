using MediatR;
using SqsOperations.Api.Messaging;
using SqsOperations.Api.SqsPublisher.Request;

namespace SqsOperations.Api.SqsPublisher.Handlers;

public class SqsMessageHandler(IMessenger messenger) : IRequestHandler<PublishMessageRequest>
{
    public async Task Handle(PublishMessageRequest request, CancellationToken cancellationToken)
    {
       if(IsValidRequest());
       await messenger.PublishMessage(request);
    }

    private bool IsValidRequest()
    {
        // Validate Request
        return true;
    }
}