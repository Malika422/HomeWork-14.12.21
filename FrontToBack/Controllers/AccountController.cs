using FrontToBack.Data;
using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
                return View();
            var existUser = await _userManager.FindByEmailAsync(login.Email);
            if (existUser == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, login.Password, login.RememberMe, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }



            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
                return View();

            var dbUser = await _userManager.FindByNameAsync(register.Username);
            if (dbUser != null)
            {
                ModelState.AddModelError("Username", "Istifadechi mivcuddur");
                return View();
            }
            var user = new User
            {
                UserName = register.Username,
                Email = register.Email,
                Fullname = register.Fullname
            };
            var identityResult = await _userManager.CreateAsync(user, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, RoleConstants.MembereRole);
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
