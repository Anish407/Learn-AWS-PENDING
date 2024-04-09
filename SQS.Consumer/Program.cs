// See https://aka.ms/new-console-template for more information

using Amazon.SQS;
using Amazon.SQS.Model;

string queueName = "Customer";
AmazonSQSClient sqsClient = new AmazonSQSClient();

GetQueueUrlResponse queueDetails = await sqsClient.GetQueueUrlAsync(queueName);
CancellationTokenSource cancellationToken = new CancellationTokenSource();

ReceiveMessageRequest requestMessage = new ReceiveMessageRequest()
{
    QueueUrl = queueDetails.QueueUrl,
    // specify the system attributes names that are needed when consuming the request
    AttributeNames = ["All"],
    MessageAttributeNames = ["All"]

};

while (!cancellationToken.IsCancellationRequested)
{
    ReceiveMessageResponse response = await sqsClient.ReceiveMessageAsync(requestMessage, cancellationToken.Token);
    
    foreach (Message message in response.Messages)
    {
        Console.WriteLine($"MessageId: {message.MessageId}");
        Console.WriteLine($"Message: {message.Body}");

        // acknowledge and delete the message from the queue 
        await sqsClient.DeleteMessageAsync(queueDetails.QueueUrl, message.ReceiptHandle); 
    }

    await Task.Delay(1000);
}


Console.ReadLine();