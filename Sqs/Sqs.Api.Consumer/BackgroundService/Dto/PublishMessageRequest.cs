namespace Sqs.Api.Consumer.BackgroundService.Dto;

public record PublishMessageRequest(int Id, string ProductName, int Quantity);