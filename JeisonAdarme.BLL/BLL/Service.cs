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
    public class Service
    {
        public static Servicio  RegisterService(RegisterServiceBindingModel model, SystemFail error)
        {
            if (IsValidRegisterService(model, error))
            {
                var Service = BuildService(model, error);

                if (!error.Error)
                {
                    if (data.Service.RegisterService(Service, error))
                    {
                        return Service;
                    }
                }
            }

            return new Servicio();
        }

        public static Servicio UpdateService(UpdateServiceBindingModel model, string UserNameEmployee, SystemFail error)
        {
            var service = data.Service.GetServiceForId(model.IdService, error);

            if (error.Error)
                return service;

            bool SendPushNotification = false;

            if (service == null)
            {
                error.Error = true;
                error.Mensaje = "No se encuentra el servicio";
            }



            switch (model.IdServiceNewState)
            {
                case (int)Enums.ServiceStates.Timetaken:
                    //si no tiene id de empleado y el estado es tomado se lo registro a el empleado.
                    if (string.IsNullOrEmpty(service.IdEmpleado.ToString()) && !string.IsNullOrEmpty(UserNameEmployee))
                    {
                        var employee = data.Employee.GetEmployeeForUserName(UserNameEmployee, error);

                        if (error.Error || employee == null)
                            return service;

                        service.IdEmpleado = employee.GUID;
                        service.FechaTomado = DateTime.Now;
                        SendPushNotification = true;
                    }
                    else
                    {
                        error.Error = true;
                        error.Mensaje = !string.IsNullOrEmpty(service.IdEmpleado.ToString()) ? "Alguien ya se esta encargando de este servicio" : "para asignar el servicio a un empleado debe estar autenticado.";
                        return service;
                    }

                    break;
                case (int)Enums.ServiceStates.TimeEnd:
                    service.FechaFinalizado = DateTime.Now;
                    break;
                case (int) Enums.ServiceStates.TimeCanceled:
                    service.FechaFinalizado = DateTime.Now;
                    break;
                default:
                    error.Error = true;
                    error.Mensaje = "No se encuentra el estado del servicio.";
                    return service;
            }


           // if (model.LevelSatisfaction != 0) service.NivelSatisfaccion = model.LevelSatisfaction;

            service.IdEstadoServicio = model.IdServiceNewState;

            //actualizo
            if(data.Service.UpdateService(service, error))
            {
                //si debo enviar la notificacion
                if(SendPushNotification)
                {
                    //TODO
                }
            }

            return service;
        }

        private static Servicio BuildService(RegisterServiceBindingModel model, SystemFail error)
        {
            var Companie = data.Companie.GetCompanieForUniqueToken(model.QrUser, error);
            int typeServiceId = int.Parse(model.IdTipoServicio.ToString());

            string serviceCode = BuildServiceCode(typeServiceId, error);

            if (error.Error)
                return new Servicio();

            Servicio Service = new Servicio() { 
                GUID = Guid.NewGuid(),
                IdCliente = Guid.Parse(model.IdCliente),
                IdTipoServicio = typeServiceId,
                IdEmpresa = Companie.GUID,
                IdEstadoServicio = (int)Enums.ServiceStates.TimeRegister,
                CodigoServicio = serviceCode,
                FechaPeticion = DateTime.Now
            };

            return Service;
        }


        private static bool IsValidRegisterService(RegisterServiceBindingModel model, SystemFail error)
        {
            var customer = data.Customer.GetCustomerForCustomerId(model.IdCliente, error);
            if(customer == null)
            {
                error.Error = true;
                error.Mensaje = "Debes estar registrado para poder pedir tu turno.";
                return false;
            }

            var states = data.ServiceType.GetListTypeServiceForUniqueTokenCompanie(model.QrUser, error);
            if(states == null)
            {
                error.Error = true;
                error.Mensaje = "No se encuentra el servicio.";
                return false;
            }

            var service = data.Service.GetServiceForTypeServiceIdAndCustomerIdAndState(model.IdTipoServicio, model.IdCliente, Enums.ServiceStates.TimeRegister, error);
            if(service != null)
            {
                error.Error = true;
                error.Mensaje = "Ya solicitaste un turno para este servicio.";
                return false;
            }

            return true;    
        }

        private static string BuildServiceCode(int TypeServiceId, SystemFail error)
        {
            var TypeService = data.Service.GetTipeServiceWithServicesForTpeServiceId(TypeServiceId, error);
            if (error.Error )
                return "";

            if(TypeService == null)
            {
                error.Error = true;
                error.Mensaje = "No se logra encontrar el servicio.";
                return "";
            }

            return string.Format("{0}{1}", TypeService.NombreTipoServicio.Substring(0, 2), (TypeService.tblServicio.Count() + 1));
        }

        public static List<Servicio> GetServicesForEmployeeId(string employeeId, int idServiceState, SystemFail error)
        {
            return data.Service.GetServicesForEmployeeId(employeeId, idServiceState, error);
        }

        public static List<Servicio> GetServicesForEmployeeUserName(string userName, int idServiceState, SystemFail error)
        {
            return data.Service.GetServicesToTakeForEmployeeUserName(userName, idServiceState, error);
        }

        public static List<Servicio> GetServicesToTakeForCompanieUserName(string userName, int idServiceState, SystemFail error)
        {
            return data.Service.GetServicesToTakeForCompanieUserName(userName, idServiceState, error);
        }

        public static List<EstadoServicio> GetServicesStates(SystemFail error)
        {
            return data.Service.GetServicesStates(error);
        }

        public static List<Servicio> GetAllServices(SystemFail error)
        {
            return data.Service.GetAllServices(error);
        }
    }
}
