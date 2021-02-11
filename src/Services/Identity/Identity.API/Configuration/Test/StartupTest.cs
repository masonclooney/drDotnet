using drDotnet.Services.Identity.API.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace drDotnet.Services.Identity.API.Configuration.Test
{
    public class StartupTest : Startup
    {
        public StartupTest(IConfiguration configuration) : base(configuration)
        {
        }

        public override void RegisterDbContexts(IServiceCollection services)
        {
            services.RegisterDbContextsStaging();
        }
    }
}