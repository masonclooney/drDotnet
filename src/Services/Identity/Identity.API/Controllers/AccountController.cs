using System;
using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.Identity.API.IntegrationEvents;
using drDotnet.Services.Identity.API.IntegrationEvents.Events;
using drDotnet.Services.Identity.API.Models;
using drDotnet.Services.Identity.API.Models.AccountViewModels;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.Identity.API.Controllers
{
    public class AccountController: Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;

        public AccountController(UserManager<AppIdentityUser> userManager, ILogger<AccountController> logger, SignInManager<AppIdentityUser> signInManager, IIdentityServerInteractionService interaction, IIdentityIntegrationEventService identityIntegrationEventService)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _interaction = interaction;
            _identityIntegrationEventService = identityIntegrationEventService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implmented!");
            }

            var vm = BuildLoginViewModel(returnUrl, context);
            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var props = new AuthenticationProperties
                    {
                    };

                    await _signInManager.SignInAsync(user, props);

                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            var vm = await BuildLoginViewModelAsync(model);
            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        private LoginViewModel BuildLoginViewModel(string returnUrl, AuthorizationRequest context)
        {
            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Email = context?.LoginHint
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = BuildLoginViewModel(model.ReturnUrl, context);
            vm.Email = model.Email;
            return vm;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            await _signInManager.SignOutAsync();
            return Content("sign out");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new AppIdentityUser { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Errors.Count() > 0)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View(model);
                    }
                }
                else
                {
                    var userRegisteredEvent = new UserRegisteredIntegrationEvent(user.Id, user.UserName, user.Email);
                    _identityIntegrationEventService.PublishThroughEventBusAsync(userRegisteredEvent);
                }
            }

            if (returnUrl != null)
            {
                if (ModelState.IsValid)
                    return RedirectToAction("login", "account", new { returnUrl = returnUrl });
                else
                    return View(model);
            }

            return RedirectToAction("index", "home");
        }
    }
}