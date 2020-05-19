using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using review_v2.Models;

namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<review_v2.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            SeedUsers(context);
            SeedCategories(context);
            SeedProducts(context);
        }

        private void SeedCategories(ApplicationDbContext context)
        {
            if (!context.ProductCategories.Any())
            {
                var category1 = new ProductCategory()
                {
                    Category = "Mobiles"
                };
                var category2 = new ProductCategory()
                {
                    Category = "LapTops"
                };
                context.ProductCategories.Add(category1);
                context.ProductCategories.Add(category2);
                context.SaveChanges();
            }
        }

        private void SeedProducts(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                var Products = new List<Product>()
                {
                    { new Product()
                    {
                        ProductCategoryId = context.ProductCategories.FirstOrDefault(x=>x.Category == "Mobiles").Id,
                        CompanyName =  "SuperAdmin",
                        Name = "Samsung A10",
                        Features = "Camera, Bluetooth, Wifi and touch screen",
                        Price = 5000,
                        Type = "Mobile",
                        TotalPercentageRate = 0,
                    }},
                    { new Product()
                    {
                        ProductCategoryId = context.ProductCategories.FirstOrDefault(x=>x.Category == "Mobiles").Id,
                        CompanyName =  "SuperAdmin",
                        Name = "Samsung A70",
                        Features = "Camera, Bluetooth, Wifi and touch screen",
                        Price = 8000,
                        Type = "Mobile",
                        TotalPercentageRate = 0,
                    }},
                    { new Product()
                    {
                        ProductCategoryId = context.ProductCategories.FirstOrDefault(x=>x.Category == "LapTops").Id,
                        CompanyName =  "SuperAdmin",
                        Name = "Lenovo 5070",
                        Features = "6 Gb Ram, 15.7 in screen, intel i7 8th generation",
                        Price = 12500,
                        Type = "LapTops",
                        TotalPercentageRate = 0,
                    }},
                    { new Product()
                    {
                        ProductCategoryId = context.ProductCategories.FirstOrDefault(x=>x.Category == "LapTops").Id,
                        CompanyName =  "SuperAdmin",
                        Name = "Lenovo 5090",
                        Features = "8 Gb Ram, 15.7 in screen, intel i7 8th generation, GPU 2Gb nvidia 1080 GTX",
                        Price = 20000,
                        Type = "LapTops",
                        TotalPercentageRate = 0,
                    }},
                };
                context.Products.AddRange(Products);
                context.SaveChanges();
            }
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Company" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Customer" });
            context.SaveChanges();
            if (!context.Users.Any(x=>x.UserName == "SuperAdmin"))
            {
            var user = new ApplicationUser()
            {
                UserName = "SuperAdmin",
                Email = "SuperAdmin@reviewry.com",
                PhoneNumber = "123456",
                EmailConfirmed = true
            };
            userManager.Create(user, "123456");
            userManager.AddToRole(user.Id, "Admin");
            userManager.AddToRole(user.Id, "Company");
            }
        }
    }
}
