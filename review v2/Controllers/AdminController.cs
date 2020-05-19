using review_v2.Models;
using review_v2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace review_v2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Look up the role
            string roleName = RoleName.admin;
            var role = await _roleManager.Roles.SingleAsync(r => r.Name == roleName);

            // Find the users in that role
            var roleUsers = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id.ToString()));

            return View(roleUsers);

            //var adminsInDb = _context.Admins.ToList();
            //return View(adminsInDb);
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            var adminsInDb = _context.Users.SingleOrDefault(a => a.Id == id);
            return View(adminsInDb);
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleName.admin)]
        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(Person admin)
        {
            try
            {
                var adminObj = new ApplicationUser()
                {
                    Email = admin.Email,
                    PasswordHash = admin.Password,
                    PhoneNumber = admin.PhoneNumber,
                    UserName = admin.Username,
                    PhoneNumberConfirmed = true,
                };

                _context.Users.Add(adminObj);
                _context.SaveChanges();

                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(_context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.AddToRole(adminObj.Id, RoleName.admin);
                userManager.CreateAsync(adminObj, admin.Password);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Admin/Delete/5
        public ActionResult Delete(string id)
        {
            var adminInDb = _context.Users.SingleOrDefault(a => a.Id == id);
            return View(adminInDb);
        }

        [Authorize(Roles = RoleName.admin)]
        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Person admin)
        {
            try
            {
                var adminInDb = _context.Users.SingleOrDefault(a => a.Id == id);

                _context.Users.Remove(adminInDb);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult IndexProfile()
        {
            try
            {
                var adminEmail = User.Identity.Name;
                if (adminEmail != null)
                {
                    var adminReviewsList = _context.Reviews.Include(r => r.product).Where(r => r.ReviewOwner == adminEmail).ToList();
                    var adminInDb = _context.Users.SingleOrDefault(a => a.UserName == adminEmail);

                    if(adminInDb != null)
                    {
                        var viewModel = new UserReviewsViewModel()
                        {
                            UserReviews = adminReviewsList,
                            Username = adminInDb.UserName,
                            PhoneNumber = adminInDb.PhoneNumber,
                            Email = adminInDb.Email,
                            UserId = adminInDb.Id,
                            UserImageURL = adminInDb.UserImageURL
                        };

                        return View(viewModel);
                    }
                    else
                    {
                        return Content("Failed");
                    }
                }
                else if (User.Identity.Name != null && User.IsInRole(RoleName.admin))
                    return View();
                else
                    return RedirectToAction("Login", "Account");
            }
            catch
            {
                return View();
            }
        }

        // Get: Admin/EditProfile/3
        [HttpGet]
        public ActionResult EditProfile(string id)
        {
            try
            {
                var adminInfo = _context.Users.FirstOrDefault(a => a.Id.Equals(id));

                if (adminInfo != null && User.Identity.Name == adminInfo.UserName)
                    return View("EditProfile", adminInfo);
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        // Post: Admin/EditProfile/3
        [HttpPost]
        public ActionResult EditProfile(string id, Person adminObj, HttpPostedFileBase UserImageURL)
        {
            try
            {
                var adminInDb = _context.Users.SingleOrDefault(a => a.Id.Equals(id));

                if (UserImageURL != null)
                {
                    var imageFilePath = Server.MapPath("~/ProductsImages");
                    UserImageURL.SaveAs(imageFilePath + "/" + UserImageURL.FileName);
                    adminInDb.UserImageURL = UserImageURL.FileName;
                }

                //adminInDb.UserName = adminObj.Username;
                //adminInDb.Email = adminObj.Email;
                //adminInDb.PhoneNumber = adminObj.PhoneNumber;

                _context.SaveChanges();

                return RedirectToAction("IndexProfile", "Admin");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BlockUser(string id, string QueryString)
        {
            var userInDb = _context.Users.SingleOrDefault(c => c.Id == id);
            if (userInDb.LockoutEnabled)
            {
                userInDb.LockoutEnabled = false;
                _context.SaveChanges();
            }

            switch (QueryString)
            {
                case "Company":
                    return RedirectToAction("Index", "Company");
                case "Customer":
                    return RedirectToAction("Index", "Customer");
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                default:
                    return RedirectToAction("Index", "Customer");
            }
        }

        public ActionResult ReActivateUser(string id, string QueryString)
        {
            var userInDb = _context.Users.SingleOrDefault(c => c.Id == id);
            if (!userInDb.LockoutEnabled)
            {
                userInDb.LockoutEnabled = true;
                _context.SaveChanges();
            }
            switch (QueryString)
            {
                case "Company":
                    return RedirectToAction("Index", "Company");
                case "Customer":
                    return RedirectToAction("Index", "Customer");
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                default:
                    return RedirectToAction("Index", "Customer");
            }
        }
        public ActionResult ProductsSummary()
        {
            var productsview = _context.ProductsSummaryVw.ToList();
            return View(productsview);
        }
    }
}