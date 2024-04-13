namespace SqsOperations.Api.Messaging;

public class QueueSettings()
{
    public string QueueName { get; set; } 
    public const string QueueConfigurationKey = nameof(QueueSettings);
};
