using MediatR;
using SqsOperations.Api.SqsPublisher.Request;

// ReSharper disable once CheckNamespace
namespace  Microsoft.AspNetCore.Routing;

public static class SqsPublisherEndpoints
{
    public static void UseSqsPublisherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/publish", PublishMessage);
    }

    private static async Task PublishMessage(HttpContext context,PublishMessageRequest messageRequest ,IMediator mediator)
    {
       await mediator.Send(messageRequest);
    }
}