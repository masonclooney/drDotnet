using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.Identity.API.Models;
using drDotnet.Services.Identity.API.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace drDotnet.Services.Identity.API.Controllers
{
    public class AccountController: Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppIdentityUser> userManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;
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

                return Content("Successfully Registered");
            }

            return View(model);
        }
    }
}