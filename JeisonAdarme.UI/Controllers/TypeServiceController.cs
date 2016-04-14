using JeisonAdarme.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JeisonAdarme.UI.Controllers.BLL;
using JeisonAdarme.BLL.DataBase;
using JeisonAdarme.BLL.Model.UI;

namespace JeisonAdarme.UI.Controllers
{
    public class TypeServiceController : Controller
    {
        [Authorize]
        public ActionResult CreateTypeService()
        {
            return PartialView("_CreateTypeServicePartial");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateTypeService(TypeServiceModelView model)
        {
            if (!string.IsNullOrEmpty(model.NombreTipoServicio))
            {
                SystemFail error = new SystemFail();

                if (!TypeService.RegisterTypeService(model, User.Identity.Name, error))
                    ViewBag.Error = "Mensaje";
            }

            return PartialView("_CreateTypeServicePartial");
        }

        public ActionResult GetTypesServices()
        {
            SystemFail error = new SystemFail();
            var TypesServices = TypeService.GetListTypeServiceForUserName(User.Identity.Name, error);

            return PartialView("_TypeServicesTablePartial", TypesServices);
        }

        public ActionResult Delete(int? id)
        {
            SystemFail error = new SystemFail();

            if (id != null)
            {
                TypeService.DisableTypeServiceForIdTypeService(int.Parse(id.ToString()), error);
            }

            return PartialView("_CreateTypeServicePartial");
        }
    }
}