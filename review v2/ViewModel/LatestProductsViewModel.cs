using review_v2.Models;
using review_v2.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace review_v2.ViewModel
{
    public class LatestProductsViewModel
    {
        public LatestProductsViewModel()
        {
            this.latestFeaturedProducts = new List<Product>();
            this.latestProducts = new List<Product>();
            this.latestProducts = new List<Product>();
        }

        public List<Product> latestProducts { get; set; }

        public List<Product> latestFeaturedProducts { get; set; }

        public List<Product> topRatedProducts { get; set; }

        public int Id { get; set; }

        [Required]
        [Display(Name = "ProductName", ResourceType = typeof(MyTexts))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Type", ResourceType = typeof(MyTexts))]
        public string Type { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public int ProductCategoryId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Display(Name = "TotalPercentageRate", ResourceType = typeof(MyTexts))]
        public decimal TotalPercentageRate { get; set; }

        [Required]
        [Display(Name = "Price", ResourceType = typeof(MyTexts))]
        public float Price { get; set; }

        [Required]
        [Display(Name = "Feature", ResourceType = typeof(MyTexts))]
        public string Features { get; set; }

        public string ProductImageUrl { get; set; }

    }
}