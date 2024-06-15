using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core;
using Microsoft.AspNetCore.Mvc;
using Dima.Api.Common.Api;
using Dima.Api.Models;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Transactions
{
    public class GetTransactionByPeriodEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get All")
            .WithSummary("Recupera todas as transações no período")
            .WithDescription("Recupera todas as transações no período")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>?>>();


        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ITransactionHandler handler,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetTransactionsByPeriodRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserId = user.Identity?.Name ?? string.Empty
            };

            var result = await handler.GetByPeriodAsync(request);
            return result.IsSuccess
               ? TypedResults.Ok(result)
               : TypedResults.NotFound(result);
        }
    }
}
