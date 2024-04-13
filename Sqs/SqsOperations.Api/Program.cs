using Amazon.SQS;
using Sqs.Common.Messaging;
using SqsOperations.Api.Messaging;
using SqsOperations.Api.SqsPublisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMessenger,SqsMessenger>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.QueueConfigurationKey));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSqsPublisherEndpoints();

app.Run();
