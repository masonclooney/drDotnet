using System.Net.Http;
using drDotnet.Services.Identity.API.Configuration.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace drDotnet.Services.Identity.IntegrationTests.Common
{
    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient SetupClient(this WebApplicationFactory<StartupTest> fixutre)
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };

            return fixutre.WithWebHostBuilder(builder =>
            {
                builder.UseStartup<StartupTest>()
                    .ConfigureTestServices(services => { });
            }).CreateClient(options);
        }
    }
}