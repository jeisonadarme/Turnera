using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using data = JeisonAdarme.BLL.DataAccess;
using JeisonAdarme.BLL.Model.WSI;

namespace JeisonAdarme.BLL.BLL
{
    public class ServiceType
    {
        public static bool RegisterTypeService(RegisterTyepeServiceBindingModel model, string userName, SystemFail error)
        {
            if (IsValidInRegisterTypeService(model, error))
            {
                var companie = data.Companie.GetCompanieForUserName(userName, error);

                TipoServicio TypeService = BuildTypeService(model, companie.GUID);

                return data.ServiceType.InsertTypeService(TypeService, error);
            }

            return false;
        }

        private static bool IsValidInRegisterTypeService(RegisterTyepeServiceBindingModel model, SystemFail error)
        {
            var TyperService = data.ServiceType.GetTypeServiceforName(model.NombreTipoServicio, error);

            if (TyperService != null)
            {
                error.Error = true;
                error.Mensaje = string.Format("Ya existe un servicio con este nombre: {0}", model.NombreTipoServicio);
                return false;
            }

            if (error.Error)
                return false;

            return true;
        }

        private static TipoServicio BuildTypeService(RegisterTyepeServiceBindingModel model, Guid companieId)
        {
            TipoServicio TypeService = new TipoServicio
            {
                NombreTipoServicio = model.NombreTipoServicio,
                IdEmpresa = companieId,
                Complejidad = model.Complejidad,
                TiempoEstimado = model.TiempoEstimado,
                Precio = model.Precio,
                EsActivo = true
            };

            return TypeService;
        }

        public static List<TipoServicio> GetListTypeServiceForCompanieUserName(string userName, SystemFail error)
        {
            return data.ServiceType.GetTypeServiceListForCompanieUserName(userName, error);
        }

        public static List<TipoServicio> GetListTypeServiceForCompanieUniqueToken(string uniqueToken, SystemFail error)
        {
            return data.ServiceType.GetListTypeServiceForUniqueTokenCompanie(uniqueToken, error);
        }

        public static List<TipoServicio> GetListTypeServicesEmployeeUserName(string userName, SystemFail error)
        {
            return data.ServiceType.GetListTypeServicesForEmployeeUserName(userName, error);
        }

        public static List<TipoServicio> GetAllTypeServices(SystemFail error)
        {
            return data.ServiceType.GetAllTypeServices(error);
        }

        public static bool DisableTypeServiceForIdTypeService(int IdTypeService, SystemFail error)
        {
            return data.ServiceType.DisableTypeServiceForIdTypeService(IdTypeService, error);
        }
    }
}
