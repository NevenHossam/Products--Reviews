using review_v2.Models;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using review_v2.ViewModel;

namespace review_v2.Controllers
{
    [AllowAnonymous]
    public class ProductController : Controller
    {
        private ApplicationDbContext _context;

        public ProductController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Product
        public ActionResult Index(int? type)
        {
            if (type == null)
            {
                var productsInDb = _context.Products.Include(p => p.Reviews).ToList();
                return View(productsInDb);
            }
            else
            {
                var productsInDb = _context.Products.Include(p => p.Reviews).Include(p => p.ProductCategory).Where(x => x.ProductCategoryId == type).ToList();
                return View(productsInDb);
            }
        }

        [Authorize(Roles = RoleName.company)]
        public ActionResult Create()
        {
            var catList = _context.ProductCategories.ToList();
            TempData["ProductCategory"] =
                new SelectList(catList, "Id", "Category", catList.FirstOrDefault()?.Id);

            if (User.IsInRole(RoleName.company))
                return View();
            else if (User.Identity.Name != null && User.IsInRole(RoleName.user))
                return View();
            else
                return RedirectToAction("Login", "Account");

        }

        [Authorize(Roles = RoleName.company)]
        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase ProductImageUrl)
        {
            try
            {
                if (ProductImageUrl != null)
                {
                    var imageFilePath = Server.MapPath("~/ProductsImages");
                    ProductImageUrl.SaveAs(imageFilePath + "/" + ProductImageUrl.FileName);
                    product.ProductImageUrl = ProductImageUrl.FileName;
                }

                var cat = _context.ProductCategories.SingleOrDefault(c => c.Id == product.ProductCategoryId);
                var brand = _context.Users.FirstOrDefault(b => b.UserName.Equals(User.Identity.Name));
                product.CompanyName = brand.UserName;
                product.Type = "General";
                product.Name = product.Name;
                product.ProductCategory = cat;
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index", "Product");
            }
            catch
            {

                return View();
            }
        }
        //public ActionResult Edit(int id)
        //{
        //    try
        //    {
        //        //if (User.IsInRole(RoleName.company))
        //        //{

        //        var productInDb = _context.Products.Single(p => p.Id == id);

        //        if (productInDb == null)
        //            return HttpNotFound();
        //        else
        //            return View("Edit", productInDb);
        //        //}
        //        //else if (User.Identity.Name != null && User.IsInRole(RoleName.user))
        //        //    return View();
        //        //else
        //        //    return RedirectToAction("Login", "Account");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //[HttpPost]
        //public ActionResult Edit(int id, Product product, HttpPostedFileBase ProductImageUrl)
        //{
        //    try
        //    {
        //        //if (User.IsInRole(RoleName.company))
        //        //{
        //        if (ProductImageUrl != null)
        //        {
        //            var imageFilePath = Server.MapPath("~/ProductsImages");
        //            ProductImageUrl.SaveAs(imageFilePath + "/" + ProductImageUrl.FileName);
        //            product.ProductImageUrl = ProductImageUrl.FileName;
        //        }

        //        var productInDb = _context.Products.SingleOrDefault(p => p.Id == id);

        //        productInDb.Name = product.Name;
        //        productInDb.CompanyName = User.Identity.Name;
        //        productInDb.Price = product.Price;
        //        productInDb.Features = product.Features;
        //        productInDb.ProductImageUrl = product.ProductImageUrl;

        //        _context.SaveChanges();

        //        return RedirectToAction("Index");
        //        //}
        //        //else if (User.Identity.Name != null && User.IsInRole(RoleName.user))
        //        //    return View();
        //        //else
        //        //    return RedirectToAction("Login", "Account");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        public ActionResult Delete(int id)
        {
            try
            {
                if (User.IsInRole(RoleName.company))
                {
                    var productInDb = _context.Products.Single(p => p.Id == id);
                    if (productInDb == null)
                        return HttpNotFound();
                    else
                        return View("Delete", productInDb);
                }
                else if (User.Identity.Name != null && User.IsInRole(RoleName.user))
                    return View();
                else
                    return RedirectToAction("Login", "Account");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                if (User.IsInRole(RoleName.company))
                {
                    var productInDb = _context.Products.SingleOrDefault(p => p.Id == id);

                    _context.Products.Remove(productInDb);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else if (User.Identity.Name != null && User.IsInRole(RoleName.user))
                    return View();
                else
                    return RedirectToAction("Login", "Account");
            }
            catch
            {
                return View();
            }
        }

        // search for product
        public async System.Threading.Tasks.Task<ActionResult> Search(string searchQuery)
        {
            var splitiedQuery = searchQuery.Split(' ');

            var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var role = await _roleManager.Roles.SingleAsync(r => r.Name == RoleName.company);

            var companiesInDb = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId.Equals(role.Id)));
            var comList = new List<ApplicationUser>();

            foreach (var comp in companiesInDb)
            {
                comList.Add(comp);
            }

            var catList = _context.ProductCategories.ToList();
            var proList = _context.Products.ToList();

            var searchProResult = new List<Product>();
            var searchComResult = new List<ApplicationUser>();
            var searchCatResult = new ProductCategory();
            var FinalResult = new List<Product>();

            var ePro = false;
            var eCat = false;
            var eCom = false;

            foreach (var item in splitiedQuery)
            {
                ePro = proList.Exists(x => x.Name.ToLower() == item.ToLower());
                eCat = catList.Exists(x => x.Category.ToLower() == item.ToLower());
                eCom = comList.Exists(x => x.UserName.ToLower() == item.ToLower());

                if (ePro)
                {
                    searchProResult = proList.Where(p => p.Name.ToLower() == item.ToLower()).ToList();
                }
                if (eCat)
                {
                    searchCatResult = catList.SingleOrDefault(p => p.Category.ToLower() == item.ToLower());
                }
                else
                {
                    searchCatResult = null;
                }
                if (eCom)
                {
                    foreach (var y in comList)
                    {
                        if (y.UserName.ToLower().Equals(item.ToLower()))
                        {
                            searchComResult.Add(y);
                        }
                    }
                }

            }

            if (searchProResult.Count != 0)
            {
                if (searchCatResult != null)
                {
                    //kolo mawgod
                    if (searchComResult.Count != 0)
                    {
                        foreach (var t in searchProResult)
                        {
                            foreach (var o in searchComResult)
                            {
                                if ((t.ProductCategory.Category.ToLower().Equals(searchCatResult.Category.ToLower())) && (t.CompanyName.ToLower() == o.UserName.ToLower()))
                                {
                                    FinalResult.Add(t);
                                }
                            }
                        }
                    }
                    //pro w cat bs
                    else
                    {
                        foreach (var t in searchProResult)
                        {
                            if (t.ProductCategory.Category.ToLower().Equals(searchCatResult.Category.ToLower()))
                            {
                                FinalResult.Add(t);
                            }
                        }
                    }

                }
                // pro w com bs
                else if (searchComResult.Count != 0 && searchCatResult == null)
                {
                    foreach (var o in searchComResult)
                    {
                        var result = searchProResult.Where(p => p.CompanyName.ToLower() == o.UserName.ToLower());
                        foreach (var i in result)
                        {
                            FinalResult.Add(i);
                        }
                    }
                }
                // pro bs
                else
                {
                    foreach (var t in searchProResult)
                    {
                        FinalResult.Add(t);
                    }
                }
            }
            else if (searchProResult.Count == 0)
            {
                if (searchCatResult != null)
                {
                    // cat w com bs
                    if (searchComResult.Count != 0)
                    {
                        foreach (var o in proList)
                        {
                            foreach (var u in searchComResult)
                            {
                                if (o.ProductCategory.Category.ToLower().Equals(searchCatResult.Category.ToLower()) && o.CompanyName.ToLower().Equals(u.UserName.ToLower()))
                                {
                                    FinalResult.Add(o);
                                }
                            }
                        }
                    }
                    // cat bs
                    else
                    {
                        foreach (var t in proList)
                        {
                            if (t.ProductCategory.Category.ToLower().Equals(searchCatResult.Category.ToLower()))
                            {
                                FinalResult.Add(t);
                            }

                        }

                    }
                }
                else if (searchCatResult == null)
                {
                    // com bs
                    if (searchComResult != null)
                    {
                        foreach (var t in proList)
                        {
                            foreach (var p in searchComResult)
                            {
                                if (p.UserName.ToLower().Equals(t.CompanyName.ToLower()))
                                {
                                    FinalResult.Add(t);
                                }
                            }
                        }
                    }
                    // else mfesh 7aga 5als
                }
            }
            return View("Index", FinalResult);
        }

        [HttpGet]
        public ActionResult GetProductSummary(int productID)
        {
            var product = _context.Products.Include(x => x.Reviews).FirstOrDefault(x => x.Id == productID);
            var reviewAVGperMonth = new List<double>();
            for (int i = 0; i < 12; i++)
            {
                // 12 Months
                reviewAVGperMonth.Add(0);
            }
            if (product != null)
            {
                var reviewsPerMonth = product.Reviews.GroupBy(x => x.Date.Month);
                foreach (var reviews in reviewsPerMonth)
                {
                    double average = reviews.Average(x => x.Rate);
                    reviewAVGperMonth[reviews.Key - 1] = average;
                }


                return Json(new
                {
                    title = product.Name,
                    data = reviewAVGperMonth
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new object());
            }
        }


    }
}

