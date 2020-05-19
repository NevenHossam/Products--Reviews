using review_v2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace review_v2.ViewModel
{
    public class ProductReviewViewModel
    {
        public ProductReviewViewModel()
        {
            this.ProductReviewsPositiveList = new List<Review>();
            this.ProductReviewsNegativeList = new List<Review>();
            this.ProductReviewsList = new List<Review>();
            this.LatestProductsList = new List<Product>();
        }

        public int ProductId { get; set; }

        public int ReviewId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Total Rate")]
        public decimal TotalPercentageRate { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public int ProductCategoryId { get; set; }

        public string CategoryName { get; set; }

        public decimal Ratio { get; set; }

        [Display(Name = "Features")]
        public string ProductFeatures { get; set; }

        [Display(Name = "Price")]
        public float ProdcutPrice { get; set; }

        [Display(Name = "Type")]
        public string ProductType { get; set; }

        [Display(Name = "Product Image")]
        public string Image { get; set; }

        public string ImageURL { get; set; }

        public float Rate { get; set; }

        public string Comment { get; set; }

        [Display(Name = "Review Owner")]
        public string ReviewOwner { get; set; }

        public DateTime Time { get; set; }

        public DateTime Date { get; set; }

        public string UserImageUrl { get; set; }

        public List<Review> ProductReviewsPositiveList { get; set; }

        public List<Review> ProductReviewsNegativeList { get; set; }

        public List<Review> ProductReviewsList { get; set; }

        public List<Product> LatestProductsList { get; set; }



    }
}