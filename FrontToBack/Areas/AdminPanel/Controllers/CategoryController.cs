using FrontToBack.Data;
using FrontToBack.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = RoleConstants.AdminRole)]

    
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;
        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var categpries = await _dbContext.Categories.ToListAsync();
            return View(categpries);
        }
       public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)

                return NotFound();

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
