using MediatR;

namespace SqsOperations.Api.SqsPublisher.Request;

public record PublishMessageRequest(int Id, string ProductName, int Quantity) : IRequest;