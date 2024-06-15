using Dima.Api.Common.Api;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Identity
{
    public class GetRolesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/roles", Handle)
            .RequireAuthorization();

        private static IResult Handle(ClaimsPrincipal user)
        {
            //vai no banco para pegar os roles usa a classe UserManager<User>
            //var user = await userManager.FindByEmailAsync("");
            if (user.Identity is null || !user.Identity.IsAuthenticated)
                return Results.Unauthorized();

            var identity = (ClaimsIdentity)user.Identity;
            var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });

            return TypedResults.Json(roles);
        }
    }
}
