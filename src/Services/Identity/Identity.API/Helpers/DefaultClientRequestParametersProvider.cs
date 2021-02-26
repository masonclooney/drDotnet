using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;

namespace drDotnet.Services.Identity.API.Helpers
{
    public class DefaultClientRequestParametersProvider : IClientRequestParametersProvider
    {
        private readonly IClientStore _clientStore;

        private readonly IAbsoluteUrlFactory _urlFactory;

        public DefaultClientRequestParametersProvider(IClientStore clientStore, IAbsoluteUrlFactory urlFactory)
        {
            _clientStore = clientStore;
            _urlFactory = urlFactory;
        }

        public async Task<IDictionary<string, string>> GetClientParameters(HttpContext context, string clientId)
        {
            var authority = context.GetIdentityServerIssuerUri();
            var client = await _clientStore.FindClientByIdAsync(clientId);

            if (client == null) return new Dictionary<string, string>();

            return new Dictionary<string, string>
            {
                ["authority"] = authority,
                ["client_id"] = client.ClientId,
                ["redirect_uri"] = _urlFactory.GetAbsoluteUrl(context, client.RedirectUris.First()),
                ["post_logout_redirect_uri"] = _urlFactory.GetAbsoluteUrl(context, client.PostLogoutRedirectUris.First()),
                ["response_type"] = "code",
                ["scope"] = string.Join(" ", client.AllowedScopes)
            };
        }
    }
}