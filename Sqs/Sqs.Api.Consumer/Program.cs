using Amazon.SQS;
using Sqs.Api.Consumer.BackgroundService;
using Sqs.Common.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddHostedService<QueueConsumerServiceV1>();
builder.Services.AddHostedService<QueueConsumerServiceV2>();
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.QueueConfigurationKey));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
