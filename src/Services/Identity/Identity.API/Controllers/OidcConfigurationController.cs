using System.Threading.Tasks;
using drDotnet.Services.Identity.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.Identity.API.Controllers
{
    public class OidcConfigurationController : Controller
    {
        private readonly ILogger<OidcConfigurationController> _logger;
        private readonly IClientRequestParametersProvider _clientRequestParametersProvider;


        public OidcConfigurationController(ILogger<OidcConfigurationController> logger, IClientRequestParametersProvider clientRequestParametersProvider)
        {
            _logger = logger;
            _clientRequestParametersProvider = clientRequestParametersProvider;
        }

        [HttpGet("_configuration/{clientId}")]
        public async Task<IActionResult> GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = await _clientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}