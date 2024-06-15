﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
           => app.MapDelete("/{id}", HandleAsync)
           .WithName("Categories: Delete")
           .WithSummary("Exclui uma categoria")
           .WithDescription("Exclui uma categoria")
           .WithOrder(3)
           .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(ICategoryHandler handler,
            ClaimsPrincipal user,
            long id)
        {
            var request = new DeleteCategoryRequest
            {
                Id = id,
                UserId = user.Identity?.Name ?? string.Empty
            };

            var result = await handler.DeleteAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }
    }
}