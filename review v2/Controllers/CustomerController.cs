using review_v2.ViewModel;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using review_v2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace review_v2.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Customer
        public ActionResult Index()
        {
            try
            {
                if (User.IsInRole(RoleName.admin))
                {
                    try
                    {
                        var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                        var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                        // Look up the role
                        //var role = await _roleManager.Roles.SingleAsync(r => r.Name == RoleName.user);

                        // Find the users in that role
                        var roleUsers = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == "3"));

                        return View(roleUsers);
                    }
                    catch
                    {
                        return Content("Failed");
                    }
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

        [Authorize(Roles = RoleName.admin)]
        // GET: Customer/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                if (User.IsInRole(RoleName.admin))
                {
                    var customerInDb = _context.Users.FirstOrDefault(c => c.Id == id);
                    return View(customerInDb);
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

        [Authorize(Roles = RoleName.admin)]
        // GET: Customer/Create
        public ActionResult Create()
        {
            if (User.IsInRole(RoleName.admin))
                return View();
            else if (User.Identity.Name != null && User.IsInRole(RoleName.user))
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = RoleName.admin)]
        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Person customer)
        {
            try
            {
                if (User.IsInRole(RoleName.admin))
                {
                    var customerObj = new ApplicationUser()
                    {
                        Email = customer.Email,
                        PasswordHash = customer.Password,
                        UserName = customer.Username,
                        PhoneNumber = customer.PhoneNumber,
                        PhoneNumberConfirmed = true,
                    };
                    _context.Users.Add(customerObj);
                    _context.SaveChanges();

                    var roleStore = new RoleStore<IdentityRole>(_context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var userStore = new UserStore<ApplicationUser>(_context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(customerObj.Id, RoleName.user);
                    userManager.CreateAsync(customerObj, customer.Password);

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

        //[Authorize(Roles = RoleName.admin)]
        //// GET: Customer/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    var customerInDb = _context.Users.SingleOrDefault(c => c.Id == id);
        //    return View(customerInDb);
        //}

        //[Authorize(Roles = RoleName.admin)]
        //// POST: Customer/Delete/5
        //[HttpPost]
        //public ActionResult Delete(string id, Person customer)
        //{
        //    try
        //    {
        //        var customerInDb = _context.Users.SingleOrDefault(c => c.Id == id);

        //        _context.Users.Remove(customerInDb);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public ActionResult IndexProfile()
        {
            try
            {
                var customerEmail = User.Identity.Name;
                if (customerEmail != null)
                {
                    var customerReviewsList = _context.Reviews.Include(r => r.product).Where(r => r.ReviewOwner == customerEmail).ToList();
                    var customerInDb = _context.Users.SingleOrDefault(c => c.UserName == customerEmail);

                    if(customerInDb != null)
                    {
                        var viewModel = new UserReviewsViewModel()
                        {
                            UserReviews = customerReviewsList,
                            Username = customerInDb.UserName,
                            PhoneNumber = customerInDb.PhoneNumber,
                            Email = customerEmail,
                            UserId = customerInDb.Id,
                            UserImageURL = customerInDb.UserImageURL

                        };

                        return View(viewModel);
                    }
                    else
                    {
                        return Content("Failed");
                    }
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

        // Get: Customer/EditProfile/3
        [HttpGet]
        public ActionResult EditProfile(string id)
        {
            try
            {
                var customerInfo = _context.Users.SingleOrDefault(c => c.Id == id);

                if (User.Identity.Name == customerInfo.UserName)
                    return View("EditProfile", customerInfo);
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        // Post: Customer/EditProfile/3
        [HttpPost]
        public ActionResult EditProfile(string id, Person CustomerObj, HttpPostedFileBase UserImageURL)
        {
            try
            {
                var customerInDb = _context.Users.SingleOrDefault(c => c.Id == id);

                if (UserImageURL != null)
                {
                    var imageFilePath = Server.MapPath("~/ProductsImages");
                    UserImageURL.SaveAs(imageFilePath + "/" + UserImageURL.FileName);
                    customerInDb.UserImageURL = UserImageURL.FileName;
                }

                //customerInDb.UserName = CustomerObj.Username;
                //customerInDb.Email = CustomerObj.Email;
                //customerInDb.PhoneNumber = CustomerObj.PhoneNumber;

                _context.SaveChanges();

                return RedirectToAction("IndexProfile", "Customer");
            }
            catch
            {
                return View();
            }
        }

    }
}