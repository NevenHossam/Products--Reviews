using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace review_v2.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change(string languageAbbreviation)
        {
            if (languageAbbreviation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageAbbreviation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageAbbreviation);
            }
            HttpCookie Cookie = new HttpCookie("Language");
            Cookie.Value = languageAbbreviation;
            Response.Cookies.Add(Cookie);
            return RedirectToAction("Index", "Home");
            
        }
    }
}