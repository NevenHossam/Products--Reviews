using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using review_v2.Resources;
using System.ComponentModel.DataAnnotations;

namespace review_v2.Models.DBViews
{
    public class ProductsSummaryView
    {
        public int Id { get; set; }
        [Display(Name = "CompanyName", ResourceType = typeof(MyTexts))]
        public string CompanyName { get; set; }

        [Display(Name = "ModelName", ResourceType = typeof(MyTexts))]
        public string Name { get; set; }

        [Display(Name = "TotalPercentageRate", ResourceType = typeof(MyTexts))]
        public decimal TotalPercentageRate { get; set; }

        [Display(Name = "Category", ResourceType = typeof(MyTexts))]
        public string Category { get; set; }

        [Display(Name = "AverageRate", ResourceType = typeof(MyTexts))]
        public int AverageRate { get; set; }

        [Display(Name = "ReviewsCount", ResourceType = typeof(MyTexts))]
        public int ReviewsCount { get; set; }
    }
}