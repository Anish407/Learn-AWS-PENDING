namespace Sqs.Common.Messaging;

public class QueueSettings()
{
    public string QueueName { get; set; } 
    public const string QueueConfigurationKey = nameof(QueueSettings);
};
