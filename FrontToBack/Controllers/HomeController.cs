using FrontToBack.DataAccessLayer;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            //HttpContext.Session.SetString("demo", "Hello P318!");
            //HttpContext.Session.SetString("sessionId", Guid.NewGuid().ToString());

            //Response.Cookies.Append("demo2", "Bye P318!");

            var sliderImages = _dbContext.SliderImages.ToList();
            //var slider = _dbContext.Sliders.FirstOrDefault();
            var slider = _dbContext.Sliders.SingleOrDefault();
            var categories = _dbContext.Categories.ToList();
            //var products = _dbContext.Products.Include(x => x.Category).OrderBy(x => x.Price).ToList();
            //var products = _dbContext.Products.Include(x => x.Category).Where(x => x.Price > 200).ToList();
            //var products = _dbContext.Products.Include(x => x.Category).OrderByDescending(x => x.Price).ToList();
            //var products = _dbContext.Products.Where(x => x.Price > 200).ToList();

            var flowerExpertsImage = _dbContext.FlowerExpertsImages.ToList();
            var flowerExpertsTitle = _dbContext.FlowerExpertsTitles.SingleOrDefault();

            return View(new HomeViewModel 
            {
                Slider = slider,
                SliderImages = sliderImages,
                Categories = categories,
                //Products = products
                FlowerExpertsImage = flowerExpertsImage,
                FlowerExpertsTitle = flowerExpertsTitle,
            });
        }

        public async Task<IActionResult> Basket()
        {
            //var session = HttpContext.Session.GetString("demo");
            //var session2 = HttpContext.Session.GetString("sessionId");
            //var cookie = Request.Cookies["demo2"];

            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Content("Empty");
            }

            var newBasketViewModels = new List<BasketViewModel>();
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            foreach (var item in basketViewModels)
            {
                var existProduct = await _dbContext.Products.FindAsync(item.Id);
                if (existProduct == null)
                    continue;

                newBasketViewModels.Add(new BasketViewModel
                {
                    Id = item.Id,
                    Image = existProduct.Image,
                    Name = existProduct.Name,
                    Price = existProduct.Price,
                    Count = item.Count
                });
            }

            basket = JsonConvert.SerializeObject(newBasketViewModels);
            Response.Cookies.Append("basket", basket);

            return Content(basket);
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null)
                return Content("Error");

            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return Content("Not found");

            List<BasketViewModel> basketViewModels;
            var existBasket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(existBasket))
            {
                basketViewModels = new List<BasketViewModel>();
            }
            else
            {
                basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(existBasket);
            }

            var existProductInBasket = basketViewModels.Find(x => x.Id == product.Id);
            if (existProductInBasket == null)
            {
                existProductInBasket = new BasketViewModel
                {
                    Id = product.Id                    
                };
                basketViewModels.Add(existProductInBasket);
            }
            else
            {
                existProductInBasket.Count++;
            }

            var basket = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("basket", basket);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index2()
        {
            var sliderImages = _dbContext.SliderImages.ToListAsync();
            var slider = _dbContext.Sliders.SingleOrDefaultAsync();
            var categories = _dbContext.Categories.ToListAsync();
            var products = _dbContext.Products.Include(x => x.Category).OrderByDescending(x => x.Price).ToListAsync();

            return Json(new HomeViewModel
            {
                Slider = await slider,
                SliderImages = await sliderImages,
                Categories = await categories,
                Products = await products
            });
        }

        public IActionResult Search(string searchedProduct)
        {
            if (string.IsNullOrEmpty(searchedProduct.Trim()))
            {
                return NoContent();
            }

            var products = _dbContext.Products.Where(x => x.Name.Contains(searchedProduct)).ToList();

            return PartialView("_SearchedProductPartial", products);
        }
    }
}
