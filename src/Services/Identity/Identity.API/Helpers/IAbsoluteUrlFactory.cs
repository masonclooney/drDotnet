using Microsoft.AspNetCore.Http;

namespace drDotnet.Services.Identity.API.Helpers
{
    public interface IAbsoluteUrlFactory
    {
        string GetAbsoluteUrl(string path);
        string GetAbsoluteUrl(HttpContext context, string path);
    }
}