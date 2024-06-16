using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using MudBlazor.Services;
using Dima.Web.Security;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();
builder.Services.AddMudServices();

//Microsoft.Extensions.Http use esse para requisição http
builder.Services.AddHttpClient(Configuration.HttpClientName, opt =>
{
    opt.BaseAddress = new Uri(Configuration.BackendUrl);
}).AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();
