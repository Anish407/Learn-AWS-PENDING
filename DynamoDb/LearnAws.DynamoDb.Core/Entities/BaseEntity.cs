using System.Text.Json.Serialization;

namespace LearnAws.DynamoDb.Core.Entities;

public abstract class BaseEntity
{
    [JsonPropertyName("pk")]
    public abstract string PartitionKey { get; }
    [JsonPropertyName("sk")]
    public abstract string SortKey { get; }

    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    
}