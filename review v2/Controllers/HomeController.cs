using review_v2.Models;
using review_v2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace review_v2.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var latestProductsInDb = _context.Products.OrderByDescending(p => p.Id).Take(4).ToList();
            var products = _context.Products.ToList();
            List<Product> latestFeaturedProductsInDb = new List<Product>();
            List<Product> topRatedProductsInDb = new List<Product>();

            var maxRateOfPro = products.OrderByDescending(p => p.TotalPercentageRate).Take(4);
            foreach (var pro in maxRateOfPro)
            {
                topRatedProductsInDb.Add(pro);
            }

            var viewModel = new LatestProductsViewModel()
            {
                latestProducts = latestProductsInDb,
                topRatedProducts = topRatedProductsInDb,
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}