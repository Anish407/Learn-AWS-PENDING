using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Microsoft.Extensions.Options;
using Sqs.Api.Consumer.BackgroundService.Dto;
using Sqs.Api.Consumer.MessageHandlers;
using Sqs.Common.Messaging;
using static Sqs.Common.Constants;

namespace Sqs.Api.Consumer.BackgroundService;

public class QueueConsumerServiceV2 : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IMediator _mediator;
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly ILogger<QueueConsumerServiceV1> _logger;
    private string queueName;

    public QueueConsumerServiceV2(IAmazonSQS sqs, IMediator mediator, IOptions<QueueSettings> queueSettings,
        ILogger<QueueConsumerServiceV1> logger)
    {
        _sqs = sqs;
        _mediator = mediator;
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
                        Type? type = Type.GetType($"Sqs.Api.Consumer.BackgroundService.Dto.{messageType}");

                        if (type is null)
                        {
                            _logger.LogError($"Unknown Message Type:{messageType}", messageType);
                            throw new Exception($"Unknown Message Type:{messageType}");
                        }
                        
                        // Used keyed services instead of mediatr, easier to debug
                        var typedMessage = (IMessage)JsonSerializer.Deserialize(message.Body, type);
                        await _mediator.Send(typedMessage, stoppingToken); 

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