using System.Runtime.CompilerServices;

namespace LearnAws.DynamoDb.Core.Entities;
#nullable disable
public class Customers:BaseEntity
{
    public override string PartitionKey => Id;
    
    // we can have different sort and partition Keys
    // Documents with the same partition key will be stored together and will be sorted 
    // based on the SortKey
    public override string SortKey => FirstName; 

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string ContactNumber { get; set; }
}