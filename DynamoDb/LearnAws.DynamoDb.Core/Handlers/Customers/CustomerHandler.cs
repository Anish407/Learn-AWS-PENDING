using LearnAws.DynamoDb.Core.Repositories;

namespace LearnAws.DynamoDb.Core.Handlers.Customers;

public interface ICustomerHandler
{
    Task<Entities.Customers> GetCustomer(string firstName, string id);
}

public class CustomerHandler(ICustomersRepository customersRepository) : ICustomerHandler
{
    public async Task<Entities.Customers> GetCustomer(string firstName, string id)
    {
        return await customersRepository.GetItemAsync(id, firstName);
    }
}