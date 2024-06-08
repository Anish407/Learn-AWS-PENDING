using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using LearnAws.DynamoDb.Core.Entities;
using LearnAws.DynamoDb.Core.Repositories;

namespace LearnAws.DynamoDb.Infra.Repositories;

public abstract class DynamoDBRepository<T> : IRepository<T> where T : BaseEntity
{
    protected abstract string Tablename { get; }
    private readonly IAmazonDynamoDB _dynamoDbClient;

    public DynamoDBRepository(IAmazonDynamoDB dynamoDBClient)
    {
        _dynamoDbClient = dynamoDBClient;
    }

    public async Task<T> GetItemAsync(string Id, string FirstName)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = Tablename,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = Id } },
                { "sk", new AttributeValue { S = FirstName } }
            }
        };

        var response = await _dynamoDbClient.GetItemAsync(getItemRequest);
        if (response.Item.Count == 0)
            return null; // Change to handle nulls better, create a null item

        var itemJson = Document.FromAttributeMap(response.Item).ToJson();
        return JsonSerializer.Deserialize<T>(itemJson);
    }

    public virtual async Task<bool> CreateItemAsync(T item)
    {
        item.CreatedAt = DateTime.UtcNow;
        return await UpSertitem(item);
    }

    private async Task<bool> UpSertitem(T item)
    {
        try
        {
            var itemAsAttributes = GetItemAsAttributes(item);
            var itemRequest = new PutItemRequest
            {
                TableName = Tablename,
                Item = itemAsAttributes
            };

            var response = await _dynamoDbClient.PutItemAsync(itemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            // Add logging
            return false;
        }
    }

    public async Task<bool> UpdateItemAsync(T item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        return await UpSertitem(item);
    }

    private static Dictionary<string, AttributeValue> GetItemAsAttributes(T item)
    {
        var itemAsJson = JsonSerializer.Serialize(item);
        var itemAsAttributes = Document.FromJson(itemAsJson).ToAttributeMap();
        return itemAsAttributes;
    }

    public async Task DeleteItemAsync(T Item)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = Tablename,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = Item.PartitionKey } },
                { "sk", new AttributeValue { S = Item.SortKey } }
            }
        };

        await _dynamoDbClient.DeleteItemAsync(deleteItemRequest);
    }
}