using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminUserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUserController(IUserRepository userRepository, UserManager<IdentityUser>userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAll();

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<Models.ViewModels.User>();
            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAdress = user.Email
                });
            }
            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult>List (UserViewModel newUser)
        {
            var identityUser = new IdentityUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, newUser.Password);

            if(identityResult is not null) 
            { 
                if(identityResult.Succeeded)
                {
                    var roles = new List<string> { "User"};

                    if(newUser.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }

                    await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUser");
                    }
                }
            }
            return View() ;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUser");
                }

            }
            return View();
        }
    }
}
