using System;
using drDotnet.Services.Identity.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace drDotnet.Services.Identity.API.Helpers
{
    public static class StartupHelpers
    {
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