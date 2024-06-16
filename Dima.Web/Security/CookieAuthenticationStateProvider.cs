using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Linq;

namespace Dima.Web.Security
{
    public class CookieAuthenticationStateProvider (IHttpClientFactory httpClientFactory)
        : AuthenticationStateProvider, ICookieAuthenticationStateProvider
    {
        private bool _isAuthenticated = false;
        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _isAuthenticated;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _isAuthenticated = false;

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            //obtendo as informações do usuário
            var userInfo = await GetUser();

            if(userInfo is null)
                return new AuthenticationState(user);

            var claims = await GetClaims(userInfo);

            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
            user = new ClaimsPrincipal(id);

            _isAuthenticated = true;
            return new AuthenticationState(user);
        }

        public void NotifyAuthenticationStateChanged()
         => base.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        
        private async Task<User?> GetUser()
        {
            try
            {
                 return await _client.GetFromJsonAsync<User?>("v1/identity/manage/info");
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            //criando os claims que referem aos dados do usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Email,user.Email)
            };

            //carregando os demais claims do usuário
            claims.AddRange(user.Claims
                .Where(x => x.Key != ClaimTypes.Name && x.Key != ClaimTypes.Email)
                .Select(x => new Claim(x.Key, x.Value)));

            
            //obtendo os roles do usuário
            RoleClaim[]? roles;

            try
            {
                roles = await _client.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
            }
            catch
            {
                return claims;
            }

            /*
             * o Código a baixo é esse foreach porém usando linq
             foreach (var role in roles ?? [])
            {
                if(!string.IsNullOrWhiteSpace(role.Type) && !string.IsNullOrWhiteSpace(role.Value))
                    claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
            }
             */
            claims.AddRange(from role in roles ?? []
                            where !string.IsNullOrWhiteSpace(role.Type) && !string.IsNullOrWhiteSpace(role.Value)
                            select new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
            return claims;
        }
    }
}
