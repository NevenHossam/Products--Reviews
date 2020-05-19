using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace review_v2.Models
{
    public class SentimentSpecial
    {
        public int Id { get; set; }
        public string SpecialWord { get; set; }
        public int Value { get; set; }
        public int IsBefore { get; set; }
    }
}