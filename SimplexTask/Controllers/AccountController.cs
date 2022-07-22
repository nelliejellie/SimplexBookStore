using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplexTask.DataAccess;
using SimplexTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _database;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager, AppDbContext database, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _database = database;
            _userManager = userManager;
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }


            var userWithEmail = _database.Users.FirstOrDefaultAsync(user => user.Email == registerModel.Email);

            if(registerModel.Password != registerModel.ConfirmPassword)
            {
                ModelState.AddModelError("", "Password doesn't match");
                return View(registerModel);
            }

            if(userWithEmail.Result == null)
            {
                var newUser = new AppUser
                {
                    Email = registerModel.Email,
                    UserName = registerModel.Email
                };

                try
                {
                    var result = await _userManager.CreateAsync(newUser, registerModel.Password);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "Registeration was Successfully";
                        return RedirectToAction("Index", "Home");
                    }
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Unable to create user");
                        return View(registerModel);
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);

                }
            }

            ModelState.AddModelError("", "User already exists");
            return View(registerModel);
            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
            if (result.Succeeded)
            {
                TempData["success"] = "Login Successful";
                return RedirectToAction("Index", "Home");
            }
            return View(loginModel);
        }
    }
}
