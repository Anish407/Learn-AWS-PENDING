## Learn AWS

### https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html

### Components
- Sqs
  - <a href="./Sqs/SQS.ConsoleApp.Consumer">Sqs Consumer in a console app</a>
  - <a href="./Sqs/SQS.ConsoleApp.Publisher">Sqs Publisher in a console app</a>
  - <a href="./Sqs/Sqs.Api.Consumer">Sqs consumer in a Web API</a>
  - <a href="./Sqs/SqsOperations.Api">Sqs consumer in a Web API</a>

- SNS
  - <a href="./SNS/SnsPublisher.Api">SNS Publisher in a Web API</a>
  - <a href="./SNS/SNS.Publisher/Program.cs">SNS Publisher in a Console App</a>

- DynamoDb
  - <a href="./DynamoDb/LearnAws.DynamoDb.Web/Program.cs">DynamoDb CRUD Operation Web API (Repository Implemented but Only added the Get Controller)</a>

- S3
  - <a href="./S3/S3.ConsoleApp">S3 operations in a console app</a> 
  - <a href="./S3/LearnS3.Api">S3 operations in an Api</a>

- Lambdas
  - https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html -- Install the project templates for lamdba (dotnet new install 
   Amazon.Lambda.Templates)
  - dotnet tool install -g Amazon.Lambda.Tools 
  - dotnet lambda deploy-serverless SimpleLambda (deploy using Cloud Formation)
  - dotnet lambda deploy-function SimpleLambda -- Deploying without the cloud formation template
  - dotnet lambda delete-serverless SimpleLambda -- To delete the deployment
  - dotnet tool install -g Amazon.Lambda.TestTool-6.0 -- Install the Lambda test tool for debugging locally
