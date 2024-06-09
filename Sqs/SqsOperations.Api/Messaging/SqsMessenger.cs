using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Sqs.Common.Messaging;
using static Sqs.Common.Constants;

namespace SqsOperations.Api.Messaging;

public class SqsMessenger(IAmazonSQS sqs, ILogger<SqsMessenger> logger, IOptions<QueueSettings> queueSettings)
    : IMessenger
{
    public async Task PublishMessage<TMessage>(TMessage message)
    {
        try
        {
            string queueName = queueSettings.Value.QueueName;
            var queueUrlResponse = await sqs.GetQueueUrlAsync(queueName);
            await sqs.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                MessageBody = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        MessageTypeAttributeName, new MessageAttributeValue()
                        {
                            DataType = "String",
                            StringValue = typeof(TMessage).Name
                        }
                    }
                }
            });
        }
        catch (Exception e)
        {
            logger.LogInformation("Error while publishing message to sqs ,exceptionMessage:{exception}",e);
            throw;
        }
    }
}
 
public interface IMessenger
{

    Task PublishMessage<TMessage>(TMessage message);
}