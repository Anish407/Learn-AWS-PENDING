using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LearnAws.DynamoDb.Core.Entities;
using LearnAws.DynamoDb.Core.Repositories;
using Amazon.DynamoDBv2.DocumentModel;

namespace LearnAws.DynamoDb.Infra.Repositories;

public class CustomersRepository:DynamoDBRepository<Customers>,ICustomersRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;

    public CustomersRepository(IAmazonDynamoDB dynamoDBClient) : base(dynamoDBClient)
    {
        _dynamoDbClient = dynamoDBClient;
    }

    protected override string Tablename  => "Customers";
    
    public  async Task<Customers> GetByEmail(string emailId)
    {
        var request = new QueryRequest()
        {
            TableName = Tablename,
            // This is an index that is created on the table,
            // An index is created on a field that is not the PK 
            // Dynamo will create a replica and maintain it separately 
            // Now everytime we write a document to DynamoDb, We are actually writing it into the 
            // table and the index. So it doubles the cost 
            IndexName = "MyEmailIndex",
            KeyConditionExpression = "email = :v_email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { "v_email", new AttributeValue { S = emailId } },
            }
        };
        var response = await _dynamoDbClient.QueryAsync(request);
        if (response.Items.Count == 0)
        {
            return null;
        }

        var document = Document.FromAttributeMap(response.Items[0]);
        return JsonSerializer.Deserialize<Customers>(document.ToJson())!;
    }
}