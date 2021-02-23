using System;
using Microsoft.AspNetCore.Http;

namespace drDotnet.Services.Identity.API.Helpers
{
    public class AbsoluteUrlFactory : IAbsoluteUrlFactory
    {
        public IHttpContextAccessor ContextAccessor { get; }

        public AbsoluteUrlFactory(IHttpContextAccessor contextAccessor)
        {
            ContextAccessor = contextAccessor;
        }

        public string GetAbsoluteUrl(string path)
        {
            var (process, result) = ShouldProcessPath(path);
            if (!process)
            {
                return result;
            }

            if (ContextAccessor.HttpContext?.Request == null)
            {
                throw new InvalidOperationException("The request is not currently available. This service can only be used within the context of an existing HTTP request.");
            }

            return GetAbsoluteUrl(ContextAccessor.HttpContext, path);
        }

        public string GetAbsoluteUrl(HttpContext context, string path)
        {
            var (process, result) = ShouldProcessPath(path);
            if(!process)
            {
                return result;
            }

            var request = context.Request;
            return $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}{path}";
        }

        private (bool, string) ShouldProcessPath(string path)
        {
            if(path == null || !Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                return (false, null);
            }

            if(Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                return (false, path);
            }

            return (true, path);
        }
    }
}