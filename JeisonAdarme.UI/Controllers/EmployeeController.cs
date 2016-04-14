using JeisonAdarme.BLL.Common;
using JeisonAdarme.UI.Controllers.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JeisonAdarme.UI.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            SystemFail error = new SystemFail();

            var lisEmployees = Employee.GetListEmployeeForUserName(User.Identity.Name, error);

            return PartialView("_EmployeeTablePartial", lisEmployees);
        }
    }
}