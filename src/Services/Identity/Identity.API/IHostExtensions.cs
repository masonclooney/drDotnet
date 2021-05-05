using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Hosting
{
    public static class IHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost webHost) where TContext : DbContext
        {
            using(var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<TContext>();

                try
                {
                    context.Database.Migrate();
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }

            return webHost;
        }
    }
}