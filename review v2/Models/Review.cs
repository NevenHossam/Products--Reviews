using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace review_v2.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int Rate { get; set; }
        
        public decimal Ratio { get; set; }

        public string Category { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string ReviewOwner { get; set; }

        public Product product { get; set; }

        public int productId { get; set; }
    }
}