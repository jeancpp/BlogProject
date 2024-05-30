using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser>signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UserManager<IdentityUser> UserManager { get; }
        public SignInManager<IdentityUser> SignInManager { get; }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVIewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,

                };
                var identityResult = await UserManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded)
                {
                    var roleIdentityResult = await UserManager.AddToRoleAsync(identityUser, "User");
                    if (roleIdentityResult.Succeeded)
                    {
                        return RedirectToAction("Register");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnURL) 
         {
            var model = new LoginVIewModel { 
                ReturnURL = ReturnURL
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVIewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var signInResult = await SignInManager.PasswordSignInAsync(loginViewModel.Username,loginViewModel.Password, false, false);
            if (signInResult!= null && signInResult.Succeeded)
            {
                if(!string.IsNullOrWhiteSpace(loginViewModel.ReturnURL) )
                {
                    return Redirect(loginViewModel.ReturnURL);
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
