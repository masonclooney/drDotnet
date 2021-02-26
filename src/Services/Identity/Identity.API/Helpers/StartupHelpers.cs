using System;
using drDotnet.Services.Identity.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace drDotnet.Services.Identity.API.Helpers
{
    public static class StartupHelpers
    {
        static readonly string CorsName = "drDotnetWebCors";

        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsName, builder =>
                {
                    builder.WithOrigins("https://localhost:6001").AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public static void UseCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors(CorsName);
        }

        public static void AddOidcConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IClientRequestParametersProvider, DefaultClientRequestParametersProvider>();
            services.AddSingleton<IAbsoluteUrlFactory, AbsoluteUrlFactory>();
        }

        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(optionBuilder => optionBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void RegisterDbContextsStaging(this IServiceCollection services)
        {
            var identityDatabaseName = Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(optionBuilder => optionBuilder.UseInMemoryDatabase(identityDatabaseName));
        }
    }
}