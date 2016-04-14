using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using JeisonAdarme.BLL.Model.UI;
using JeisonAdarme.BLL.Model.WSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using business = JeisonAdarme.BLL.BLL;

namespace JeisonAdarme.UI.Controllers.BLL
{
    public class TypeService
    {
        public static List<TipoServicio> GetListTypeServiceForUserName(string userName, SystemFail error)
        {
            return business.ServiceType.GetListTypeServiceForCompanieUserName(userName, error);
        }

        public static bool DisableTypeServiceForIdTypeService(int IdTypeService, SystemFail error)
        {
            return business.ServiceType.DisableTypeServiceForIdTypeService(IdTypeService, error);
        }

        public static bool RegisterTypeService(TypeServiceModelView model,string userName, SystemFail error)
        {
            RegisterTyepeServiceBindingModel TypeService = new RegisterTyepeServiceBindingModel()
            {
                NombreTipoServicio = model.NombreTipoServicio
            };

            return business.ServiceType.RegisterTypeService(TypeService, userName, error);
        }
    }
}