using FrontToBack.DataAccessLayer;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public const int a = 10;

        public HeaderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int basketCount = 0;
            var basket = Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basket))
            {
                var basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
                //basketCount = basketItems.Count;
                //basketCount = basketItems.Count();
                //var ids = basketItems.Select(x => x.Id).ToList();
                
                //foreach (var item in basketItems)
                //{
                //    basketCount += item.Count;
                //}
                basketCount = basketItems.Select(x => x.Count)
                    .Aggregate(0, (count, item) => count + item);
            }
            ViewBag.BasketCount = basketCount;

            var bio = await _dbContext.Bios.SingleOrDefaultAsync();

            return View(bio);
        }
    }
}
