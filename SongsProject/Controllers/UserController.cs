using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongsProject.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SongsProject.Controllers
{

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repo)
        {
            _repository = repo;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User loginModel)
        {
            var user = await _repository.Users.FirstOrDefaultAsync(p => p.Name == loginModel.Name);

            if (user != null)
                if (user.Password == loginModel.Password)
                    return Redirect("/Admin/Index");

            ModelState.AddModelError("", "Invalid name or password");
            return View(loginModel);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User() { Name = loginModel.Name, Password = loginModel.Password };
                _repository.SaveUser(user);
                return RedirectToAction("Login");
            }
            TempData["message"] = $"{loginModel.Name} has been created";
            ModelState.AddModelError("", "Invalid name or password");
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction("ListCountry", "Home");
        }
    }
}