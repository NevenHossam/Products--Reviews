using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using review_v2.Resources;

namespace review_v2.Models
{

    
    public class Person
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "UserName", ResourceType = typeof(MyTexts))]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD", ResourceType = typeof(MyTexts))]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }

        [StringLength(11, ErrorMessage = "The minimum length must be at least 11 number.")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(MyTexts))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "The minimum length must be at least 11 number.")]
        public string PhoneNumber { get; set; }

        public string UserImageUrl { get; set; }

        public bool? Active { get; set; }

    }
}