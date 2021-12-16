using FrontToBack.Data;
using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = RoleConstants.AdminRole)]
    public class ExpertsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
      
        public ExpertsController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var flowerExpertsImages = await _dbContext.FlowerExpertsImages.ToListAsync();

            return View(flowerExpertsImages);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var flowerExpertsImage = await _dbContext.FlowerExpertsImages.FindAsync(id);
            if (flowerExpertsImage == null)
                return NotFound();

            return View(flowerExpertsImage);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlowerExpertsImage flowerExpertsImage)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isExist = await _dbContext.FlowerExpertsImages.AnyAsync(x => x.Name.ToLower() == flowerExpertsImage.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu ad artiq movcuddur");
                return View();
            }

            if (!flowerExpertsImage.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Photo", "Yalniz shekil");
                return View();
            }

            if (flowerExpertsImage.Photo.Length > 1024 * 1000)
            {
                ModelState.AddModelError("Photo", "olchu boyukdur");
                return View();
            }

            var webRootPath = _environment.WebRootPath;
            var fileName = Guid.NewGuid().ToString() + " - " + flowerExpertsImage.Photo.FileName;
            var path = Path.Combine(webRootPath, "img", fileName);

            var fileStream = new FileStream(path, FileMode.CreateNew);
            await flowerExpertsImage.Photo.CopyToAsync(fileStream);

            flowerExpertsImage.Image = fileName;

            await _dbContext.FlowerExpertsImages.AddAsync(flowerExpertsImage);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();

            var flowerExpertsImage = await _dbContext.FlowerExpertsImages.FindAsync(id);
            if (flowerExpertsImage == null)
                return NotFound();

            return View(flowerExpertsImage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FlowerExpertsImage flowerExpertsImage)
        {
            if (id == null)
                return NotFound();

            if (id != flowerExpertsImage.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View();

            var dbflowerExpertsImage = await _dbContext.FlowerExpertsImages.FindAsync(id);
            if (dbflowerExpertsImage == null)
                return NotFound();


            var isExist = await _dbContext.FlowerExpertsImages.AnyAsync(x => x.Name.ToLower() == flowerExpertsImage.Name.ToLower() &&
            x.Id!=flowerExpertsImage.Id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu ad artiq movcuddur");
                return View();
            }
            dbflowerExpertsImage.Name = flowerExpertsImage.Name;
            dbflowerExpertsImage.Experts = flowerExpertsImage.Experts;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
                   
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var flowerExpertsImage = await _dbContext.FlowerExpertsImages.FindAsync(id);
            if (flowerExpertsImage == null)
                return NotFound();

            return View(flowerExpertsImage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
                return NotFound();

            var flowerExpertsImage = await _dbContext.FlowerExpertsImages.FindAsync(id);
            if (flowerExpertsImage == null)
                return NotFound();

            _dbContext.FlowerExpertsImages.Remove(flowerExpertsImage);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
