using System.Net.Http;
using drDotnet.Services.Identity.API.Configuration.Test;
using drDotnet.Services.Identity.IntegrationTests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace drDotnet.Services.Identity.IntegrationTests.Tests.Base
{
    public class BaseClassFixture : IClassFixture<WebApplicationFactory<StartupTest>>
    {
        protected readonly HttpClient Client;

        public BaseClassFixture(WebApplicationFactory<StartupTest> factory)
        {
            Client = factory.SetupClient();
        }
    }
}