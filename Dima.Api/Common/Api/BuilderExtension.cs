﻿using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Dima.Api.Common.Api
{
    public static class BuilderExtension
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            Configuration.ConnectionString = builder.Configuration
                .GetConnectionString("DefaultConnection") ?? string.Empty;

            Configuration.BackEndUrl = builder.Configuration.GetValue<string>("BackEndUrl") ?? string.Empty;
            Configuration.FrontEndUrl = builder.Configuration.GetValue<string>("FrontEndUrl") ?? string.Empty;
            ApiConfiguration.StripeApiKey = builder.Configuration.GetValue<string>("StripeApiKey") ?? string.Empty;

            //configurando a chave da stripe
            StripeConfiguration.ApiKey = ApiConfiguration.StripeApiKey;
        }

        public static void AddDocumentation(this WebApplicationBuilder builder) 
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                x.CustomSchemaIds(n => n.FullName);
            });
        }

        public static void AddSecurity(this WebApplicationBuilder builder) 
        {
            //adicionando autenticação via cookies
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();

            //adicionando autorização
            builder.Services.AddAuthorization();
        }

        public static void AddDataContexts(this WebApplicationBuilder builder) 
        {
            builder.Services.AddDbContext<AppDbContext>(x
                => x.UseSqlServer(Configuration.ConnectionString));

            //Adicionando o suporte do Identity e configurando a relação do mesmo com o EF
            builder.Services.AddIdentityCore<User>() //adiciona um usuário customizado
                .AddRoles<IdentityRole<long>>() //adiciona roles personalizados
                .AddEntityFrameworkStores<AppDbContext>() //adiciona o contexto
                .AddApiEndpoints(); //adiciona suporte para API (gera as apis para login, logout, etc.)
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
            builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
            builder.Services.AddTransient<IReportHandler, ReportHandler>();
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                .WithOrigins([
                    Configuration.BackEndUrl,
                    Configuration.FrontEndUrl
                    ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                ));
        }
    }
}
