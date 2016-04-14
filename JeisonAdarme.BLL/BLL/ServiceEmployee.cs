using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using JeisonAdarme.BLL.Model.WSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data = JeisonAdarme.BLL.DataAccess;

namespace JeisonAdarme.BLL.BLL
{
    public class ServiceEmployee
    {
        public static bool InsertTypeServiceToEmployee(RegisterTypeServiceToEmployeeBindingModel model, string userName, SystemFail error)
        {
            if (IsValidInsertTypeserviceToEmployee(model, error))
            {
                var employee = data.Employee.GetEmployeeForUserName(userName, error);

                ServicioEmpleado ServiceEmployee = BuildTyperServiceToEmployee(model, employee.GUID);

                return data.ServiceEmployee.InsertTypeServiceToEmployee(ServiceEmployee, error);
            }

            return false;
        }

        public static bool InsertTypeServiceToEmployee(List<RegisterTypeServiceToEmployeeBindingModel> model, string userName, SystemFail error)
        {
            List<ServicioEmpleado> ServiceToEmployee = new List<ServicioEmpleado>();
            var employee = data.Employee.GetEmployeeForUserName(userName, error);

            foreach (var item in model)
            {
                if (IsValidInsertTypeserviceToEmployee(item, error))
                {
                    ServiceToEmployee.Add(BuildTyperServiceToEmployee(item, employee.GUID));
                }
                else
                    break;
            }

            if (data.ServiceEmployee.InsertTypeServiceToEmployee(ServiceToEmployee, error))
                return true;

            return false;
        }

        private static bool IsValidInsertTypeserviceToEmployee(RegisterTypeServiceToEmployeeBindingModel model, SystemFail error)
        {
            var TypeService = data.ServiceType.GetTypeServiceforId(int.Parse(model.IdTypeService), error);

            if (TypeService == null)
            {
                error.Error = true;
                error.Mensaje = string.Format("El servico no se encuentra registrado.");
                return false;
            }

            if (error.Error)
                return false;

            return true;
        }

        private static ServicioEmpleado BuildTyperServiceToEmployee(RegisterTypeServiceToEmployeeBindingModel model, Guid EmployeeId)
        {
            ServicioEmpleado ServiceToEmployee = new ServicioEmpleado
            {
                GUID = Guid.NewGuid(),
                IdEmpleado = EmployeeId,
                IdTipoServicio = int.Parse(model.IdTypeService),
                EsActivo = true
            };

            return ServiceToEmployee;
        }
    }
}
