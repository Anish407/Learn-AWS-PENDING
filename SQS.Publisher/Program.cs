// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.Publisher.Events;

string queueName = "Customer";
try
{
    var sqsClient = new AmazonSQSClient();

    var queueDetails = await sqsClient.GetQueueUrlAsync(queueName);
    OrderCreatedEvent @event = new OrderCreatedEvent()
    {
        Date = DateTime.Now,
        Details = $"OrderId:{Guid.NewGuid()}",
        Id = 1,
        UserName = "Anish"
    };
    await sqsClient.SendMessageAsync(queueDetails.QueueUrl, JsonSerializer.Serialize(@event));
    // await ListAllQueues(sqsClient);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


Console.ReadLine();

async Task ListAllQueues(AmazonSQSClient amazonSqsClient)
{
    var response = await amazonSqsClient.ListQueuesAsync(
        new ListQueuesRequest()
        {
            MaxResults = 5
        });
}