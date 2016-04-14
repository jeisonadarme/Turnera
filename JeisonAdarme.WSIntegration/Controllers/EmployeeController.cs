using JeisonAdarme.BLL.Model.WSI;
using JeisonAdarme.WSIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using JeisonAdarme.BLL.Model.WSI;
using business = JeisonAdarme.BLL.BLL;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;

namespace JeisonAdarme.WSIntegration.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        /// <summary>
        /// Retorna la lista de empleados para el usuario con rol de empresa que este en sesion
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetListEmployee")]
        [Authorize(Roles = "Empresa")]
        public List<ResponseEmployee> GetListEmployee()
        {
            SystemFail error = new SystemFail();
            List<ResponseEmployee> ListEmployees = new List<ResponseEmployee>();

            var List = business.Employee.GetEmployeeListForCompanieUsername(User.Identity.Name, error);

            foreach (var item in List)
            {
                ListEmployees.Add(CastObjects.CastObject<ResponseEmployee, Empleado>(item));
            }

            return ListEmployees;
        }

        ///// <summary>
        ///// Retorna la lista de empleados para el token unico de la empresa
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[Route("GetListEmployeeForCompanieUniqueToken")]
        //public List<ResponseEmployee> GetListEmployeeForCompanieUniqueToken(string uniqueToken)
        //{
        //    SystemFail error = new SystemFail();
        //    List<ResponseEmployee> ListEmployees = new List<ResponseEmployee>();

        //    if (!string.IsNullOrEmpty(uniqueToken))
        //    {
        //        var List = business.Employee.GetListEmployeeForCompanieUniqueToken(uniqueToken, error);

        //        foreach (var item in List)
        //        {
        //            ListEmployees.Add(CastObjects.CastObject<ResponseEmployee, Empleado>(item));
        //        }
        //    }

        //    return ListEmployees;
        //}

        /// <summary>
        /// Retorna toda la lista de empleados registrados en db.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetAllEmployees")]
        public List<ResponseEmployee> GetAllEmployees()
        {
            SystemFail error = new SystemFail();
            List<ResponseEmployee> ListEmployees = new List<ResponseEmployee>();

            var List = business.Employee.GetAllEmployees(error);

            foreach (var item in List)
            {
                ListEmployees.Add(CastObjects.CastObject<ResponseEmployee, Empleado>(item));
            }

            return ListEmployees;
        }


        /// <summary>
        /// Retorna el empleado para el username del empleado.
        /// El userName es el correo electronico del empleado.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetEmployeeForUniquePhoneNumber")]
        public static ResponseEmployee GetEmployeeForUserName(string userName)
        {
            SystemFail error = new SystemFail();
            ResponseEmployee Employee = new ResponseEmployee();

            if (!string.IsNullOrEmpty(userName))
            {
                var employee = business.Employee.GetEmployeeForUserName(userName, error);
                Employee = CastObjects.CastObject<ResponseEmployee, Empleado>(employee);
            }

            return Employee;
        }

        /// <summary>
        /// Inserta en base de datos, un servcio a el empleado. 
        /// se debe estar autenticado y ser de rol empleado.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles="Empleado")]
        [Route("InsertTypeServiceToEmployee")]
        public ApiResponse InsertTypeServiceToEmployee(RegisterTypeServiceToEmployeeBindingModel model)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "Se los servicos a el empleado correctamente." };
            SystemFail error = new SystemFail();



            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            bool bitSave = false;
            bitSave = business.ServiceEmployee.InsertTypeServiceToEmployee(model, User.Identity.Name, error);

            if (!bitSave || error.Error)
            {
                response.Error = true;
                response.ErrorMessage = error.Mensaje;
            }

            return response;
        }

        /// <summary>
        /// Inserta en base de datos, una lista servcios a el empleado. 
        /// se debe estar autenticado y ser de rol empleado.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Empleado")]
        [Route("InsertListTypeServiceToEmployee")]
        public ApiResponse InsertListTypeServiceToEmployee(List<RegisterTypeServiceToEmployeeBindingModel> model)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "Se los servicos a el empleado correctamente." };
            SystemFail error = new SystemFail();

            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            bool bitSave = false;
            bitSave = business.ServiceEmployee.InsertTypeServiceToEmployee(model, User.Identity.Name, error);

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
    }
}
