using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace review_v2.Models
{
    public class SentimentDb
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int Value { get; set; }
    }
}