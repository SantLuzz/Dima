﻿using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
           => app.MapGet("/{id}", HandleAsync)
           .WithName("Categories: Get By Id")
           .WithSummary("Recupera uma categoria")
           .WithDescription("Recupera uma categoria")
           .WithOrder(4)
           .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(ICategoryHandler handler,
            ClaimsPrincipal user,
            long id)
        {
            var request = new GetCategoryByIdRequest
            {
                Id = id,
                UserId = user.Identity?.Name ?? string.Empty
            };

            var result = await handler.GetByIdAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }
    }
}
