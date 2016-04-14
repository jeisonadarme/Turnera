using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.Model.WSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using business = JeisonAdarme.BLL.BLL;
using JeisonAdarme.BLL.DataBase;

namespace JeisonAdarme.WSIntegration.Controllers
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        /// <summary>
        /// Inserta en base de datos, al cliente.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("RegisterCustomer")]
        public ApiResponse RegisterCustomer(RegisterCustomerBindingModel model)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "Se registro el cliente." };
            SystemFail error = new SystemFail();

            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            bool bitSave = false;
            bitSave = business.Customer.RegisterCustomer(model, error);

            if (!bitSave || error.Error)
            {
                response.Error = true;
                response.ErrorMessage = error.Mensaje;
            }

            return response;
        }

        /// <summary>
        /// Retorna toda la lista de clientes registrados en base de datos.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetAllCustomers")]
        public List<ResponseCustomer> GetAllCustomers()
        {
            SystemFail error = new SystemFail();
            List<ResponseCustomer> ListCustomers = new List<ResponseCustomer>();

            var List = business.Customer.GetAllCustomers(error);

            foreach (var item in List)
            {
                ListCustomers.Add(CastObjects.CastObject<ResponseCustomer, Cliente>(item));
            }

            return ListCustomers;
        }

        /// <summary>
        /// Valida si un cliente existe. se debe enviar uno de los dos parametros
        /// </summary>
        /// <param name="UniquePhoneNumer"></param>
        /// <param name="Email"></param>
        /// <returns>
        /// retorna un apiresponse error.error = falso si no existe y error.error = true si existe.
        /// </returns>
        [Route("ExistCustomer")]
        public ApiResponse ExistCustomer(string Email)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "No existe el cliente." };
            SystemFail error = new SystemFail();

            if(business.Customer.ExistCustomer(Email, error))
            {
                response.Error = true;
                error.Mensaje = "El cliente ya se encuentra registrado.";
            }

            return response;
        }


        /// <summary>
        /// retorna el cliente para el email.
        /// </summary>
        /// <param name="UniquePhoneNumber"></param>
        /// <returns></returns>
        [Route("GetCustomerForEmail")]
        public ResponseCustomer GetCustomerForEmail(string Email)
        {
            SystemFail error = new SystemFail();
            ResponseCustomer Customer = new ResponseCustomer();

            if (!string.IsNullOrEmpty(Email))
            {
                var customer = business.Customer.GetCustomerForEmail(Email, error);
                Customer = CastObjects.CastObject<ResponseCustomer, Cliente>(customer);
            }

            return Customer;
        }

        
        /// <summary>
        /// retorna el cliente para el push id.
        /// </summary>
        /// <param name="UniquePhoneNumber"></param>
        /// <returns></returns>
        [Route("GetCustomerForPushRegistrationId")]
        public ResponseCustomer GetCustomerForPushRegistrationId(string PushRegistrationId)
        {
            SystemFail error = new SystemFail();
            ResponseCustomer Customer = new ResponseCustomer();

            if (!string.IsNullOrEmpty(PushRegistrationId))
            {
                var customer = business.Customer.GetCustomerForPushRegistrationId(PushRegistrationId, error);
                Customer = CastObjects.CastObject<ResponseCustomer, Cliente>(customer);
            }

            return Customer;
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
    }
}
