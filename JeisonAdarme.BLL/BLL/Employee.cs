using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeisonAdarme.BLL.Model.WSI;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using data = JeisonAdarme.BLL.DataAccess;

namespace JeisonAdarme.BLL.BLL
{
    public class Employee
    {
        public static bool RegisterEmployee(RegisterEmployeeBindingModel model, string userId, string userName, SystemFail error)
        {
            if (IsValidCreateEmployee(model, error))
            {
                var companie = data.Companie.GetCompanieForUserName(userName, error);

                Empleado Employee = BuildEmployee(model, userId, companie.GUID);

                return data.Employee.InsertEmployee(Employee, error);
            }

            return false;
        }

        private static bool IsValidCreateEmployee(RegisterEmployeeBindingModel model, SystemFail error)
        {
            if (ExistEmployeeForEmail(model.Email, error))
            {
                error.Error = true;
                error.Mensaje = "Ya se encuentra alguien registrado con ese email.";
                return false;
            }


            if (error.Error)
                return false;

            return true;
        }

        private static Empleado BuildEmployee(RegisterEmployeeBindingModel model, string userId, Guid CompanieId)
        {
            Empleado Employee = new Empleado
            {
                GUID = Guid.NewGuid(),
                IdUsuario = userId,
                IdEmpresa = CompanieId,
                Nombre = model.Name,
                EsActivo = true,
                NumeroCelular = model.PhoneNumber
            };

            return Employee;
        }


        public static bool ExistEmployeeForEmail(string strEmployeeEmail, SystemFail error)
        {
            var Employee = data.Employee.GetEmployeeForUserName(strEmployeeEmail, error);

            if (Employee == null)
                return false;

            return true;
        }

        public static List<Empleado> GetEmployeeListForCompanieUsername(string userName, SystemFail error)
        {
            return data.Employee.GetEmployeeListForCompanieUsername(userName, error);
        }


        public static Empleado GetEmployeeForUserName(string userName, SystemFail error)
        {
            return data.Employee.GetEmployeeForUserName(userName, error);
        }

        public static List<Empleado> GetAllEmployees(SystemFail error)
        {
            return data.Employee.GetAllEmployees(error);
        }

        public static List<Empleado> GetListEmployeeForCompanieUniqueToken(string UniqueToken, SystemFail error)
        {
            return data.Employee.GetListEmployeeForCompanieUniqueToken(UniqueToken, error);
        }
    }
}