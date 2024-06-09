using LearnAws.DynamoDb.Core.Entities;

namespace LearnAws.DynamoDb.Core.Repositories;

public interface IRepository<T>
{
    Task<T> GetItemAsync(string Id, string FirstName);
    Task<bool> CreateItemAsync(T item);
    Task<bool> UpdateItemAsync(T item);
    Task DeleteItemAsync(T item);
 
}