using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Models.ViewModels;

namespace SongsProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //UserManager<IdentityUser> class contains the required methods to manage users in the underlying data store. 
        //For example, this class has methods like CreateAsync, DeleteAsync, UpdateAsync to create, delete and update users.
        private readonly UserManager<IdentityUser> _userManager;
        //SignInManager<IdentityUser> class contains the required methods for users signin. 
        //For example, this class has methods like SignInAsync, SignOutAsync to signin and signout a user.
        private readonly SignInManager<IdentityUser> _signInManager;

        //Both UserManager and SignInManager services are injected into the AccountController using constructor injection
        //Both these services accept a generic parameter.We use the generic parameter to specify the User class that these services should work with.
        //At the moment, we are using the built-in IdentityUser class as the argument for the generic parameter.
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                //jQuery validation issues AJAX Call to method IsEmailInUse 
                //and he excepts to returning json object - here no validation errors
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
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
                // Copy data from RegisterViewModel to IdentityUser:
                // UserManger uses IdentityUser for CreateAsync
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user
                // isPersistent : false - created session cookie of user in browser
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Admin");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("ListCountry", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // checks returnURL through model binding and access if the Sign in is correct
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Cancel()
        {
            return RedirectToAction("ListCountry", "Home");
        }
    }
}