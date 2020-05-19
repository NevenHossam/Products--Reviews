using review_v2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using review_v2.Resources;

namespace review_v2.ViewModel
{
    public class UserReviewsViewModel
    {
        public UserReviewsViewModel()
        {
            this.UserReviews = new List<Review>();
        }

        public string UserId { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(MyTexts))]
        public string Username { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(MyTexts))]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }

        public int ReviewId { get; set; }

        [Display(Name = "ProductName", ResourceType = typeof(MyTexts))]
        public string ProductName { get; set; }

        [Display(Name = "Rate", ResourceType = typeof(MyTexts))]
        public float Rate { get; set; }

        [Display(Name = "Comment", ResourceType = typeof(MyTexts))]
        public string Comment { get; set; }

        [Display(Name = "Date", ResourceType = typeof(MyTexts))]
        public DateTime Date { get; set; }

        [Display(Name = "Time", ResourceType = typeof(MyTexts))]
        public DateTime Time { get; set; }

        public int ProductId { get; set; }

        public string UserImageURL { get; set; }

        public List<Review> UserReviews { get; set; }

    }
}