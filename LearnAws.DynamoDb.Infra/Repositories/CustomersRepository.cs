using Amazon.DynamoDBv2;
using LearnAws.DynamoDb.Core.Entities;
using LearnAws.DynamoDb.Core.Repositories;

namespace LearnAws.DynamoDb.Infra.Repositories;

public class CustomersRepository:DynamoDBRepository<Customers>,ICustomersRepository
{
    public CustomersRepository(IAmazonDynamoDB dynamoDBClient) : base(dynamoDBClient)
    {
    }

    protected override string Tablename  => "Customers";
}