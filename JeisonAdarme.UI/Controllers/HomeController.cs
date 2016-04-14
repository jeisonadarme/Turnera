using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.Model.UI;
using JeisonAdarme.UI.Controllers.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JeisonAdarme.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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


        [Authorize]
        public ActionResult GenerateQrCode()
        {
            QrCodesModelView code = new QrCodesModelView();
            SystemFail error = new SystemFail();
            string userToken = Companie.GetCompanieForUserName(User.Identity.Name, error).TokenUnico;

            code.CodeService = userToken;
            code.CodeEmploye = userToken + "-Employee";

            return PartialView("_QrCodePartial", model: code);
        }
        
    }
}