using Amazon.SQS;
using Sqs.Api.Consumer.BackgroundService;
using Sqs.Common.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.QueueConfigurationKey));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
