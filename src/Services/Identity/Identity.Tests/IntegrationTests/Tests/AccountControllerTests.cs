using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using drDotnet.Services.Identity.API.Configuration.Test;
using drDotnet.Services.Identity.IntegrationTests.Common;
using drDotnet.Services.Identity.IntegrationTests.Mocks;
using drDotnet.Services.Identity.IntegrationTests.Tests.Base;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace drDotnet.Services.Identity.IntegrationTests.Tests
{
    public class AccountControllerTests : BaseClassFixture
    {
        public AccountControllerTests(WebApplicationFactory<StartupTest> factory) : base(factory)
        {
        }

        [Fact]
        public async Task UserIsAbleToRegister()
        {
            Client.DefaultRequestHeaders.Clear();

            var registerFormData = UserMocks.GenerateRegisterData();
            var registerResponse = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            registerResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);

            registerResponse.Headers.Location.ToString().Should().Be("/");
        }

        [Fact]
        public async Task UserIsNotAbleToRegisterWithSameUserName()
        {
            Client.DefaultRequestHeaders.Clear();

            var registerFormData = UserMocks.GenerateRegisterData();
            var registerResponseFirst = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            registerResponseFirst.StatusCode.Should().Be(HttpStatusCode.Redirect);
            registerResponseFirst.Headers.Location.ToString().Should().Be("/");

            var registerResponseSecond = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            registerResponseSecond.StatusCode.Should().Be(HttpStatusCode.OK);

            var contentWithErrorMessage = await registerResponseSecond.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(contentWithErrorMessage);

            var errorNodes = doc.DocumentNode
                .SelectNodes("//div[contains(@class, 'validation-summary-errors')]/ul/li");

            errorNodes.Should().HaveCount(1);

            var expectedErrorMessages = new List<string>
            {
                $"Username &#x27;{registerFormData["Email"]}&#x27; is already taken."
            };

            var containErrors = errorNodes.Select(x => x.InnerText).ToList().SequenceEqual(expectedErrorMessages);

            containErrors.Should().BeTrue();
        }

        [Fact]
        public async Task UserIsAbleToLogin()
        {
            Client.DefaultRequestHeaders.Clear();

            var registerFormData = UserMocks.GenerateRegisterData();
            await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
            var loginResponse = await Client.GetAsync(accountLoginAction);
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();

            var loginDataForm = UserMocks.GenerateLoginData(registerFormData["Email"], registerFormData["Password"], antiForgeryToken);

            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountLoginAction, loginDataForm, loginResponse);
            var responseMessage = await Client.SendAsync(requestMessage);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.Redirect);

            responseMessage.Headers.Location.ToString().Should().Be("/");

            const string identityCookieName = ".AspNetCore.Identity.Application";
            var existsCookie = CookiesHelper.ExistsCookie(responseMessage, identityCookieName);

            existsCookie.Should().BeTrue();
        }

        [Fact]
        public async Task UserIsNotAbleToLoginWithIncorrectPassword()
        {
            Client.DefaultRequestHeaders.Clear();

            var registerFormData = UserMocks.GenerateRegisterData();
            await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
            var loginResponse = await Client.GetAsync(accountLoginAction);
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();

            var loginDataForm = UserMocks.GenerateLoginData(registerFormData["Email"], Guid.NewGuid().ToString(), antiForgeryToken);

            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountLoginAction, loginDataForm, loginResponse);
            var responseMessage = await Client.SendAsync(requestMessage);

            var contentWithErrorMessage = await responseMessage.Content.ReadAsStringAsync();

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var doc = new HtmlDocument();
            doc.LoadHtml(contentWithErrorMessage);

            var errorNodes = doc.DocumentNode
                .SelectNodes("//div[contains(@class, 'validation-summary-errors')]/ul/li");

            errorNodes.Should().HaveCount(1);

            var expectedErrorMessages = new List<string>
            {
                "Invalid username or password."
            };

            var containErrors = errorNodes.Select(x => x.InnerText).ToList().SequenceEqual(expectedErrorMessages);

            containErrors.Should().BeTrue();

            const string identityCookieName = ".AspNetCore.Identity.Application";
            var existsCookie = CookiesHelper.ExistsCookie(responseMessage, identityCookieName);

            existsCookie.Should().BeFalse();
        }
    }
}