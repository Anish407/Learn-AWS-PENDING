// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.Common.Events;

string queueName = "Customer";
try
{
    var sqsClient = new AmazonSQSClient();

    var queueDetails = await sqsClient.GetQueueUrlAsync(queueName);
    await SendMessage(sqsClient, queueDetails);
    await SendMessage(sqsClient, queueDetails);
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

async Task SendMessage(AmazonSQSClient sqsClient1, GetQueueUrlResponse getQueueUrlResponse)
{
    var randomNumber = new Random().Next();
    OrderCreatedEvent @event = new OrderCreatedEvent()
    {
        Date = DateTime.Now,
        Details = $"OrderId:{Guid.NewGuid()}",
        Id = randomNumber,
        UserName = $"Anish:{randomNumber}"
    };
    await sqsClient1.SendMessageAsync(getQueueUrlResponse.QueueUrl, JsonSerializer.Serialize(@event));
}