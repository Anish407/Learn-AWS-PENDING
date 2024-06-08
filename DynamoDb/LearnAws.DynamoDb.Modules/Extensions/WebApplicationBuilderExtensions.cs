using LearnAws.DynamoDb.Core.Handlers.Customers;
using LearnAws.DynamoDb.Core.Repositories;
using LearnAws.DynamoDb.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace  Microsoft.AspNetCore.Builder;
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddCustomerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddServices();
        builder.Services.AddRepositories();

        return builder;
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerHandler, CustomerHandler>();
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddSingleton<ICustomersRepository, CustomersRepository>();
    }
}