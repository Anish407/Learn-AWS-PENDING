using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using SNS.Common.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleNotificationService>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/PublishToSNS", PublishMessageHandler)
    .WithName("PublishToSNS")
    .WithOpenApi();

app.Run();

static async Task<Results<Ok<string>, BadRequest<string>>> PublishMessageHandler(
    IAmazonSimpleNotificationService snsClient)
{
    try
    {
        string topicName = "Customers";
        string topicArn = await GetTopicArn(snsClient, topicName);

        var messageRequest = GetMessageRequest(topicArn);
        await snsClient.PublishAsync(messageRequest);
        return TypedResults.Ok("Message sent");
    }
    catch (Exception e)
    {
        return TypedResults.BadRequest(e.Message);
    }
};
// Add this to app settings if we need to specify the region separately,
//  
//  "AWS": {
//    "Profile": "default",
//    "Region": "ap-south-1"
//  }

static async Task<string> GetTopicArn(IAmazonSimpleNotificationService amazonSimpleNotificationService, string s)
{
    var topicResponse = await amazonSimpleNotificationService.FindTopicAsync(s);
    return topicResponse.TopicArn;
}

static PublishRequest GetMessageRequest(string s)
{
    var randomNumber = new Random().Next();
    OrderCreatedEvent @event =
        new OrderCreatedEvent(Id: randomNumber,
            $"OrderId:{Guid.NewGuid()}",
            Date: DateTime.Now,
            $"Anish:{randomNumber}");

    return new PublishRequest()
    {
        TopicArn = s,
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
}


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}