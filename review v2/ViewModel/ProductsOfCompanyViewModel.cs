using review_v2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using review_v2.Resources;

namespace review_v2.ViewModel
{
    public class ProductsOfCompanyViewModel
    {

        public ProductsOfCompanyViewModel()
        {
            this.CompanyProducts = new List<Product>();
        }

        public string CompanyId { get; set; }

        [Display(Name = "TotalRate", ResourceType = typeof(MyTexts))]
        public decimal TotalRate { get; set; }

        [Display(Name = "CompanyName", ResourceType = typeof(MyTexts))]
        public string CompanyName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string CompanyEmail { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "ProductName", ResourceType = typeof(MyTexts))]
        public string ProductName { get; set; }

        [Display(Name = "Price", ResourceType = typeof(MyTexts))]
        public float ProductPrice { get; set; }

        [Display(Name = "ProductImage", ResourceType = typeof(MyTexts))]
        public string ProductImage { get; set; }

        [Display(Name = "Feature", ResourceType = typeof(MyTexts))]
        public string Features { get; set; }

        public string CompanyImageURL { get; set; }

        public List<Product> CompanyProducts { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

    }
}