namespace SNS.Common.Events;

public record OrderCreatedEvent(int Id, string Details, DateTime Date, string UserName);