using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using drDotnet.Services.Identity.IntegrationTests.Common;

namespace drDotnet.Services.Identity.IntegrationTests.Mocks
{
    public static class UserMocks
    {
        public static string UserPassword = "Pa$$word123";
        public static string AntiForgeryTokenKey = "__RequestVerificationToken";

        public static Dictionary<string, string> GenerateRegisterData()
        {
            return new Dictionary<string, string>
            {
                { "Email", $"{Guid.NewGuid().ToString()}@{Guid.NewGuid().ToString()}.com" },
                { "Password", UserPassword },
                { "ConfirmPassword", UserPassword }
            };
        }

        public static async Task<HttpResponseMessage> RegisterNewUserAsync(HttpClient client, Dictionary<string, string> registerDataForm)
        {
            const string accountRegisterAction = "/Account/Register";

            var registerResponse = await client.GetAsync(accountRegisterAction);
            var antiForgeryToken = await registerResponse.ExtractAntiForgeryToken();

            registerDataForm.Remove(AntiForgeryTokenKey);
            registerDataForm.Add(AntiForgeryTokenKey, antiForgeryToken);

            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountRegisterAction, registerDataForm, registerResponse);
            var responseMessage = await client.SendAsync(requestMessage);

            return responseMessage;
        }

        internal static Dictionary<string, string> GenerateLoginData(string email, string password, string antiForgeryToken)
        {
            var loginDataForm = new Dictionary<string, string>
            {
                {"Email", email},
                {"Password", password},
                {AntiForgeryTokenKey, antiForgeryToken}
            };

            return loginDataForm;
        }
    }
}