using JeisonAdarme.WSIntegration.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JeisonAdarme.WSIntegration.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    AccountController c = new AccountController();

        //    c.ConfirmEmail(userId, code);

        //    return View();
        //}
    }
}
