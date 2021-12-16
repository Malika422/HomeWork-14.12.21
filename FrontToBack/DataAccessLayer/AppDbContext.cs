using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.Models;
using FrontToBack.Migrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FrontToBack.DataAccessLayer
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
        {
        }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<SliderImage> SliderImages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Bio> Bios { get; set; }
        public DbSet<FlowerExpertsImage> FlowerExpertsImages { get; set; }
        public DbSet<FlowerExpertsTitle> FlowerExpertsTitles { get; set; }

    }
}
