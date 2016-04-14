using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.Model.WSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JeisonAdarme.BLL.Model.WSI;
using business = JeisonAdarme.BLL.BLL;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;

namespace JeisonAdarme.WSIntegration.Controllers
{
    /// <summary>
    /// se encarga de todo el funcionamiento de la asignacion de turnos, tomarlos, cancelarlos.
    /// </summary>
    [RoutePrefix("api/Service")]
    public class ServiceController : ApiController
    {
        /// <summary>
        /// Registra el servicio en db con el estado de pendiente. //----//
        /// retorna todos los datos del servicio si y solo si se registra, pero tambien tiene los campos de error y mensaje de error para ser validados.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// retorna todos los datos del servicio si y solo si se registra, pero tambien tiene los campos de error y mensaje de error para ser validados.
        /// </returns>
        [AllowAnonymous]
        [Route("Register")]
        public ResponseService Register(RegisterServiceBindingModel model)
        {
            ResponseService response = new ResponseService { Error = false, ErrorMessage = "Registro exitoso." };
            SystemFail error = new SystemFail();

            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            var Service = business.Service.RegisterService(model, error);

            if (error.Error)
            {
                response.Error = true;
                response.ErrorMessage = error.Mensaje;
            }
            else
            {
                response = CastObjects.CastObject<ResponseService, Servicio>(Service);
                response.ErrorMessage = "Registro exitoso.";
            }

            return response;
        }

        private void ManageModelErrors(ResponseService response)
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
        /// obtiene todos los servicios registrados
        /// </summary>
        /// <returns></returns>
        [Route("GetAllServices")]
        public List<ResponseService> GetAllServices()
        {
            SystemFail error = new SystemFail();
            List<ResponseService> ListServices = new List<ResponseService>();


            var List = business.Service.GetAllServices(error);

            foreach (var item in List)
            {
                ListServices.Add(CastObjects.CastObject<ResponseService, Servicio>(item));
            }

            return ListServices;
        }


        /// <summary>
        /// Retorna la lista de servicios que el empleado puede tomar. o los que ya ha tomado.//--//
        /// Los Servicios son del dia y año en curso. //--//
        /// El estado del servicio si no se envia se toma por defecto pendiente //--//
        /// Se debe estar autenticado con rol de Empleado.
        /// </summary>
        /// <param name="idServiceState"></param>
        /// <returns></returns>
        [Authorize(Roles = "Empleado")]
        [Route("GetServicesForEmployee")]
        public List<ResponseService> GetServicesForEmployee(int idServiceState = 1)
        {
            SystemFail error = new SystemFail();
            List<ResponseService> ListServices = new List<ResponseService>();

            var List = business.Service.GetServicesForEmployeeUserName(User.Identity.Name, idServiceState, error);

            foreach (var item in List)
            {
                ListServices.Add(CastObjects.CastObject<ResponseService, Servicio>(item));
            }

            return ListServices;
        }

        /// <summary>
        /// Retorna la lista de servicios que la empresa tiene //--//
        /// Los Servicios son del dia y año en curso. //--//
        /// El estado del servicio si no se envia se toma por defecto pendiente //--//
        /// Se debe estar autenticado con rol de Empresa.
        /// </summary>
        /// <param name="idServiceState"></param>
        /// <returns></returns>
        [Authorize(Roles = "Empresa")]
        [Route("GetServicesForCompanie")]
        public List<ResponseService> GetServicesForCompanie(int idServiceState = 1)
        {
            SystemFail error = new SystemFail();
            List<ResponseService> ListServices = new List<ResponseService>();

            var List = business.Service.GetServicesToTakeForCompanieUserName(User.Identity.Name, idServiceState, error);

            foreach (var item in List)
            {
                ListServices.Add(CastObjects.CastObject<ResponseService, Servicio>(item));
            }

            return ListServices;
        }


        /// <summary>
        ///  obtine los estados que se le asignan a los servicios //--//
        ///  Pendiente: Estado en el que inician cuando se registra un nuevo servicio o turno. //--//
        ///  Tomado: cuando el empleado lo busca y se lo asigna //--//
        ///  Finalizado: cuando se termina el servicio, es el empleado quien lo finaliza. //--//
        ///  Cancelado: cuando el cliente lo cancela o puede ser el empleado
        /// </summary>
        /// <returns>
        /// </returns>
        [Route("GetAllStatesServices")]
        public List<ResponseStates> GetAllStatesServices()
        {
            SystemFail error = new SystemFail();
            List<ResponseStates> ListStates = new List<ResponseStates>();


            var List = business.Service.GetServicesStates(error);
            foreach (var item in List)
            {
                ListStates.Add(CastObjects.CastObject<ResponseStates, EstadoServicio>(item));
            }

            return ListStates;

        }

        /// <summary>
        /// No es necesario estar autenticado, porque el cliente podria cancelar el servicio. //--//
        /// Actualiza el estado del servicio segun el estado que se envie. //--//
        /// Si hay un empleado autenticado y el servicio aun no ha sido asignado a ningun empleado entonces:
        /// 1. se actualiza el estado del servicio 
        /// 2. se le asigna a el empleado autenticado.
        /// 3. se envia el mensaje push para que se le informe a el cliente que es su turno. (Aun falta implementar) //--// ESTADOS QUE SE PUEDEN ENVIAR--
        ///  Pendiente: Estado en el que inician cuando se registra un nuevo servicio o turno. //--//
        ///  Tomado: cuando el empleado lo busca y se lo asigna //--//
        ///  Finalizado: cuando se termina el servicio, es el empleado quien lo finaliza. //--//
        ///  Cancelado: cuando el cliente lo cancela o puede ser el empleado.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateService")]
        public ResponseService UpdateService(UpdateServiceBindingModel model)
        {
            ResponseService response = new ResponseService { Error = false, ErrorMessage = "Registro exitoso." };
            SystemFail error = new SystemFail();

            string userNameEmployee = string.Empty;

            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            if (User.Identity.IsAuthenticated && User.IsInRole(Enums.UserRoles.Empleado.ToString()))
            {
                userNameEmployee = User.Identity.Name;
            }

            var service = business.Service.UpdateService(model, userNameEmployee, error);

            if (error.Error)
            {
                response.Error = true;
                response.ErrorMessage = error.Mensaje;
            }
            else
            {
                response = CastObjects.CastObject<ResponseService, Servicio>(service);
                response.ErrorMessage = "Actualizacion existosa";
            }

            return response;
        }
    }
}
