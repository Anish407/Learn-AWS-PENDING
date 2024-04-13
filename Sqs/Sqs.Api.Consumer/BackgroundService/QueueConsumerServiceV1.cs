using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Sqs.Api.Consumer.BackgroundService.Dto;
using Sqs.Common.Messaging;
using static Sqs.Common.Constants;

namespace Sqs.Api.Consumer.BackgroundService;

public class QueueConsumerServiceV1 : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly ILogger<QueueConsumerServiceV1> _logger;
    private string queueName;

    public QueueConsumerServiceV1(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings,
        ILogger<QueueConsumerServiceV1> logger)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
        _logger = logger;
        queueName = _queueSettings.Value.QueueName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var queueUrlResponse = await _sqs.GetQueueUrlAsync(queueName);

            ReceiveMessageRequest requestMessage = new ReceiveMessageRequest()
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                // specify the system attributes names that are needed when consuming the request
                AttributeNames = ["All"],
                MessageAttributeNames = ["All"]
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqs.ReceiveMessageAsync(requestMessage);

                foreach (Message message in response.Messages)
                {
                    try
                    {
                        var messageType = message.MessageAttributes[MessageTypeAttributeName].StringValue;

                        if (messageType == "PublishMessageRequest")
                        {
                            var publishMessage = JsonSerializer.Deserialize<PublishMessageRequest>(message.Body);
                            if (publishMessage is not null)
                                _logger.LogInformation("Id:{id}, Name:{name}, quantity:{quantity}", publishMessage.Id,
                                    publishMessage.ProductName, publishMessage.Quantity);
                        }
                        else
                        {
                            throw new Exception($"Unknown Message Type:{messageType}");
                        }

                        Console.WriteLine($"MessageId: {message.MessageId}");
                        Console.WriteLine($"Message: {message.Body}");

                        // acknowledge and delete the message from the queue 
                        await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Error While Reading Messages, Message:{exception}", e);
                        continue;
                    }
                }

                await Task.Delay(1000);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error While Reading Messages, Message:{exception}", e);
        }
    }
}