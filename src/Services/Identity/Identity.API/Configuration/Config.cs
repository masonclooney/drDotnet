using System.Collections.Generic;
using IdentityServer4.Models;

namespace drDotnet.Services.Identity.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> Resources =>
            new List<IdentityResource>
            {

            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {

            };
    }
}