using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace drDotnet.Services.Identity.API.Helpers
{
    public interface IClientRequestParametersProvider
    {
        Task<IDictionary<string, string>> GetClientParameters(HttpContext context, string clientId);
    }
}