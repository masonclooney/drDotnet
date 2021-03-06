using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace drDotnet.Services.Identity.API.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> Resources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "web",
                    ClientName = "drDotnet Web SPA",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris = { "https://localhost:6001/authentication/login-callback" },
                    AllowedCorsOrigins = { "https://localhost:6001" },

                    PostLogoutRedirectUris = { "https://localhost:6001/logout-callback" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                }
            };
    }
}