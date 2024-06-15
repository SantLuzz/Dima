using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;
using Dima.Api.Endpoints.Identity;
using Dima.Api.Endpoints.Transactions;
using Dima.Api.Models;

namespace Dima.Api.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoint = app
                .MapGroup("");

            endpoint.MapGroup("/")
                .WithTags("Health Check")
                .MapGet("/", () => new { Message = "OK" });

            endpoint.MapGroup("v1/categories")
                .WithTags("Categories")
                .RequireAuthorization()
                .MapEndpoints<CreateCategoryEndpoint>()
                .MapEndpoints<UpdateCategoryEndpoint>()
                .MapEndpoints<DeleteCategoryEndpoint>()
                .MapEndpoints<GetCategoryByIdEndpoint>()
                .MapEndpoints<GetAllCategoriesEndpoints>();

            endpoint.MapGroup("v1/transactions")
                .WithTags("Transactions")
                .RequireAuthorization()
                .MapEndpoints<CreateTransactionEndpoint>()
                .MapEndpoints<UpdateTransactionEndpoint>()
                .MapEndpoints<DeleteTransactionEndpoint>()
                .MapEndpoints<GetTransactionByIdEndpoint>()
                .MapEndpoints<GetTransactionByPeriodEndpoint>();

            //mapeando os endpoints do identity
            endpoint.MapGroup("v1/identity")
                .WithTags("Identity")
                .MapIdentityApi<User>();

            endpoint.MapGroup("v1/identity")
                .WithTags("Identity")
                .MapEndpoints<LogoutEndpoint>()
                .MapEndpoints<GetRolesEndpoint>();
        }

        private static IEndpointRouteBuilder MapEndpoints<TEndpoint> (this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
