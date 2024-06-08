using LearnAws.DynamoDb.Core.Entities;
using LearnAws.DynamoDb.Core.Handlers.Customers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Microsoft.AspNetCore.Routing;
public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder UseCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");
        group.MapGet("/Customer/{id:guid}/{firstName}", GetCustomer);
        return app;
    }

    private static async Task<Results<Ok<Customers>, BadRequest<string>>> GetCustomer
    (string id,
        string firstName,
        ICustomerHandler customerHandler,
        HttpContext context)
    {
        try
        {
            var customer = await customerHandler.GetCustomer(id, firstName);
            return TypedResults.Ok(customer);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }
}