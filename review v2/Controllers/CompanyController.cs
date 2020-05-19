using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using review_v2.Models;
using review_v2.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace review_v2.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        ApplicationDbContext _context;

        public CompanyController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Company
        public async Task<ActionResult> Index()
        {
            var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            // Look up the role
            string roleName = RoleName.company;
            var role = await _roleManager.Roles.SingleAsync(r => r.Name == roleName);
            // Find the users in that role
            var roleUsers = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));

            return View(roleUsers);
        }

        // GET: Company/Details/5
        public ActionResult Details(string id)
        {
            var companiesInDb = _context.Users.SingleOrDefault(c => c.Id == id);
            return View(companiesInDb);
        }

        [Authorize(Roles = RoleName.admin)]
        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleName.admin)]
        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(Person company)
        {
            try
            {
                var companyObj = new ApplicationUser()
                {
                    Email = company.Email,
                    PasswordHash = company.Password,
                    UserName = company.Username,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = true,
                };
                _context.Users.Add(companyObj);
                _context.SaveChanges();
                
                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(_context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.AddToRole(companyObj.Id, "Company");
                userManager.CreateAsync(companyObj, company.Password);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        [Authorize(Roles = RoleName.admin)]
        // GET: Company/Delete/5
        public ActionResult Delete(string id)
        {
            var companyInDb = _context.Users.SingleOrDefault(c => c.Id == id);
            return View(companyInDb);
        }

        [Authorize(Roles = RoleName.admin)]
        // POST: Company/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Person company)
        {
            try
            {
                var companyInDb = _context.Users.SingleOrDefault(c => c.Id == id);

                _context.Users.Remove(companyInDb);
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
                var companyEmail = User.Identity.Name;
                if (companyEmail != null)
                {
                    var companyInDb = _context.Users.SingleOrDefault(c => c.UserName == companyEmail);
                    var companyProductsList = _context.Products.Where(p => p.CompanyName == companyEmail).ToList();

                    if(companyInDb != null)
                    {
                        var viewModel = new ProductsOfCompanyViewModel()
                        {
                            CompanyProducts = companyProductsList,
                            CompanyName = companyInDb.UserName,
                            CompanyId = companyInDb.Id,
                            CompanyEmail = companyInDb.Email,
                            CompanyImageURL = companyInDb.UserImageURL,
                        };

                        return View(viewModel);
                    }
                    else
                    {
                        return Content("Failed");
                    }
                }
                else if (User.Identity.Name != null && User.IsInRole(RoleName.company))
                    return View();
                else
                    return RedirectToAction("Login", "Account");
            }
            catch
            {
                return View();
            }
        }

        // Get: Company/EditProfile/3
        [HttpGet]
        public ActionResult EditProfile(string id)
        {
            try
            {
                var companyInfo = _context.Users.FirstOrDefault(c => c.Id.Equals(id));

                if (User.Identity.Name == companyInfo.UserName)
                    return View("EditProfile", companyInfo);
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
        public ActionResult EditProfile(string id, Person companyObj, HttpPostedFileBase UserImageURL)
        {
            try
            {
                var companyInDb = _context.Users.SingleOrDefault(c => c.Id.Equals(id));

                if (UserImageURL != null)
                {
                    var imageFilePath = Server.MapPath("~/ProductsImages");
                    UserImageURL.SaveAs(imageFilePath + "/" + UserImageURL.FileName);
                    companyInDb.UserImageURL = UserImageURL.FileName;
                }

                //companyInDb.UserName = companyObj.Username;
                //companyInDb.Email = companyObj.Email;

                _context.SaveChanges();

                return RedirectToAction("IndexProfile", "Company", RoleName.company);
            }
            catch
            {
                return View();
            }
        }
    }
}