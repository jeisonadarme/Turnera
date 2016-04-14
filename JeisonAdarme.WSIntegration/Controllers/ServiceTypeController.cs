using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.Model.WSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using business = JeisonAdarme.BLL.BLL;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace JeisonAdarme.WSIntegration.Controllers
{
    [RoutePrefix("api/ServiceType")]
    public class ServiceTypeController : ApiController
    {
        /// <summary>
        /// Registra el servicio en base de datos, es necesario estar autenticado con rol de empresa!!
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Register")]
        [Authorize(Roles="Empresa")]
        public ApiResponse Register(RegisterTyepeServiceBindingModel model)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "Se registro el servicio." };
            SystemFail error = new SystemFail();

            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            bool bitSave = false;
            bitSave = business.ServiceType.RegisterTypeService(model,User.Identity.Name, error);

            if (!bitSave || error.Error)
            {
                response.Error = true;
                response.ErrorMessage = error.Mensaje;
            }

            return response;
        }

        private void ManageModelErrors(ApiResponse response)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    response.ErrorMessage = string.Format("{0} - {1}", response.ErrorMessage, error.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// retorna la lista de los tipos de servicios para el usuario en sesion.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetListTypeService")]
        [Authorize]
        public List<ReponseTypeService> GetListTypeService()
        {
            SystemFail error = new SystemFail();
            List<ReponseTypeService> ListTypeService = new List<ReponseTypeService>();
            var List = new List<TipoServicio>();

            if (User.IsInRole(Enums.UserRoles.Empresa.ToString()))
            {
                List = business.ServiceType.GetListTypeServiceForCompanieUserName(User.Identity.Name, error);
            }
            else if (User.IsInRole(Enums.UserRoles.Empleado.ToString()))
            {
                List = business.ServiceType.GetListTypeServicesEmployeeUserName(User.Identity.Name, error);
            }

            foreach (var item in List)
            {
                ListTypeService.Add(CastObjects.CastObject<ReponseTypeService, TipoServicio>(item));
            }

            return ListTypeService;
        }

        /// <summary>
        /// retorna la lista de los tipos de servicios para el token unico. es el codigo que se escanea en el QR
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetListTypeServiceForCompanieUniqueToken")]
        public List<ReponseTypeService> GetListTypeServiceForCompanieUniqueToken(string uniqueToken)
        {
            SystemFail error = new SystemFail();
            List<ReponseTypeService> ListTypeService = new List<ReponseTypeService>();

            if (!string.IsNullOrEmpty(uniqueToken))
            {
                var List = business.ServiceType.GetListTypeServiceForCompanieUniqueToken(uniqueToken, error);

                foreach (var item in List)
                {
                    ListTypeService.Add(CastObjects.CastObject<ReponseTypeService, TipoServicio>(item));
                }
            }

            return ListTypeService;
        }

        /// <summary>
        /// retorna la lista de los tipos de servicios registrados en base de datos.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetAllTypeServices")]
        public List<ReponseTypeService> GetAllTypeServices()
        {
            SystemFail error = new SystemFail();
            List<ReponseTypeService> ListTypeService = new List<ReponseTypeService>();

            var List = business.ServiceType.GetAllTypeServices(error);

            foreach (var item in List)
            {
                ListTypeService.Add(CastObjects.CastObject<ReponseTypeService, TipoServicio>(item));
            }

            return ListTypeService;
        }

    }
}
