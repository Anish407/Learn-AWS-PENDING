// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SNS.Common.Events;

var randomNumber = new Random().Next();
OrderCreatedEvent @event =
    new OrderCreatedEvent(Id: randomNumber, 
        $"OrderId:{Guid.NewGuid()}", 
        Date: DateTime.Now, 
        $"Anish:{randomNumber}");

var snsClient = new AmazonSimpleNotificationServiceClient();

var topiArnResponse = await snsClient.FindTopicAsync("Customers");

var publishRequest = new PublishRequest()
{
  TopicArn = topiArnResponse.TopicArn,
  Message = JsonSerializer.Serialize(@event),
  MessageAttributes = new Dictionary<string, MessageAttributeValue>
  {
      {
          "MessageType", new MessageAttributeValue()
          {
              DataType = "String",
              StringValue = nameof(OrderCreatedEvent)
          }
      }
  }
};

var response = await snsClient.PublishAsync(publishRequest);

Console.Read();