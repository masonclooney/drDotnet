using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.Identity.API.Models;
using drDotnet.Services.Identity.API.Models.AccountViewModels;
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

        public AccountController(UserManager<AppIdentityUser> userManager, ILogger<AccountController> logger, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
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

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
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

                return Redirect("~/");
            }

            return View(model);
        }
    }
}