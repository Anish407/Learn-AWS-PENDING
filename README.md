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
  - To Run Lambdas locally , install the test tool shown above and 
     - build the solution ( dotnet build)
     - Open Cmd and navigate to the folder that contains the function and run the below command
     - dotnet lambda-test-tool-8.0 (I am using .net 8, so it will try to find the artifacts after building the project in the bin/Debug/{dotnetversion})
     - **We get this error, if an incorrect version of dotnet is specified: Unknown error occurred causing process exit: Could not find a part of the path 'D:\Learn AWs\Learn.AWS\SimpleLambda\src\SimpleLambda\bin\Debug\net6.0'**
     - One last step is to attach the debugger to the lambda process
     - ![image](https://github.com/user-attachments/assets/f4d3ea7e-19bb-40b3-a629-789ecc0cfecf)

   - To debug lambdas locally we need to create a new configuration.  Create the configuration with the following parameters
   - ![WhatsApp Image 2024-08-02 at 20 58 55_0496d8de](https://github.com/user-attachments/assets/880c2c0e-88b5-497c-823d-9c56f65ba99e)
   - Exe Path is where the dotnet lambda test tool resides on our machine
   - ![WhatsApp Image 2024-08-02 at 20 57 21_668281fb](https://github.com/user-attachments/assets/dbd2bf10-79c5-45d2-97b9-35117ac2904f)

   - Working directory is the path where the code for the lambda resides on our machine
 
   -![image](https://github.com/user-attachments/assets/e3b06049-8103-42ca-b794-8d469767ad8d)

  
