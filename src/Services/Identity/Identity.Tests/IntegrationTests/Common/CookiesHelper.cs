using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Net.Http.Headers;

namespace drDotnet.Services.Identity.IntegrationTests.Common
{
    public static class CookiesHelper
    {
        internal static HttpRequestMessage CopyCookiesFromResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            return PutCookiesOnRequest(request, ExtractCookiesFromResponse(response));
        }

        private static HttpRequestMessage PutCookiesOnRequest(HttpRequestMessage request, IDictionary<string, string> cookies)
        {
            cookies.Keys.ToList().ForEach(key =>
            {
                request.Headers.Add("cookie", new CookieHeaderValue(key, cookies[key]).ToString());
            });

            return request;
        }

        private static IDictionary<string, string> ExtractCookiesFromResponse(HttpResponseMessage response)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                SetCookieHeaderValue.ParseList(values.ToList()).ToList().ForEach(cookie =>
                {
                    result.Add(cookie.Name.Value, cookie.Value.Value);
                });
            }

            return result;
        }

        public static bool ExistsCookie(HttpResponseMessage responseMessage, string identityCookieName)
        {
            var existsCookie = false;
            const string cookieHeader = "Set-Cookie";

            if (responseMessage.Headers.TryGetValues(cookieHeader, out var cookies))
            {
                existsCookie = cookies.Any(x => x.Contains(identityCookieName));
            }

            return existsCookie;
        }
    }
}