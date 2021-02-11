using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace drDotnet.Services.Identity.IntegrationTests.Common
{
    public class RequestHelper
    {
        public static HttpRequestMessage CreatePostRequestWithCookies(string path, Dictionary<string, string> formPostBodyData, HttpResponseMessage response)
        {
            var httpRequestMessage = CreatePostRequest(path, formPostBodyData);

            return CookiesHelper.CopyCookiesFromResponse(httpRequestMessage, response);
        }

        private static HttpRequestMessage CreatePostRequest(string path, Dictionary<string, string> formPostBodyData)
        {
            var httpRequestMessage = new  HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new FormUrlEncodedContent(ToFormPostData(formPostBodyData))
            };

            return httpRequestMessage;  
        }

        private static List<KeyValuePair<string, string>> ToFormPostData(Dictionary<string, string> formPostBodyData)
        {
            var result = new List<KeyValuePair<string, string>>();

            formPostBodyData.Keys.ToList().ForEach(key =>
            {
                result.Add(new KeyValuePair<string, string>(key, formPostBodyData[key]));
            });

            return result;
        }
    }
}