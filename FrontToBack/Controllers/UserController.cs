using FrontToBack.Data;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles=RoleConstants.AdminRole)]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(x => new UserViewModel
            {
                Id = x.Id,
                Fullname = x.Fullname,
                UserName=x.UserName,
                Email = x.Email,
                User=x
            }).ToListAsync();

            foreach(var item in users)
            {
                item.Role = (await _userManager.GetRolesAsync(item.User))[0];
                item.User = null;
            }
            return View(users);
        }
        public async Task<IActionResult>ChangePassword(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
                return NotFound();

            return View(new ChangePasswordViewModel 
            { 
                Username=dbUser.UserName
            });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangePasswordViewModel(string id, ChangePasswordViewModel model)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return NotFound();
        //    if (!ModelState.IsValid)
        //        return View();
        //    var dbUser = await _userManager.FindByIdAsync(id);
        //    if (dbUser == null)
        //        return NotFound();
        //    if (!await _userManager.ChangePasswordAsync(dbUser, model.OldPasswor))
        //    {
        //        ModelState.AddModelError("","Old password isn't corrent");
        //        return View();
        //    }

        //}

    }
}
