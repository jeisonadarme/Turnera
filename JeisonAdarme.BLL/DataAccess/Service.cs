using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using JeisonAdarme.BLL.Model.WSI;

namespace JeisonAdarme.BLL.DataAccess
{
    public class Service
    {
        public static bool RegisterService(Servicio Service, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {

                        db.Servicio.Add(Service);
                        db.SaveChanges();

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        error.Error = true;
                        error.Mensaje = ex.Message;
                        return false;
                    }
                }
            }
        }

        public static bool UpdateService(Servicio Service, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(Service).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        error.Error = true;
                        error.Mensaje = ex.Message;

                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        public static Servicio GetServiceForId(string serviceId, SystemFail error)
        {
            try
            {
                Guid guid = Guid.Parse(serviceId);

                using (var db = new TurneraEntities())
                {
                    return (from s in db.Servicio where s.GUID == guid select s).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Servicio();
            }
        }

        //public static bool UpdateService(UpdateServiceBindingModel Service, SystemFail error)
        //{
        //    using (var db = new TurneraEntities())
        //    {
        //        using (var tran = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var NewService = GetServiceForId(Service.ServiceId, error);

        //                switch (Service.IdEstadoServicio)
        //                {
        //                    case (int)Enums.ServiceStates.Timetaken:
        //                        NewService.FechaTomado = DateTime.Now;
        //                        break;
        //                    case (int)Enums.ServiceStates.TimeEnd:
        //                        NewService.FechaFinalizado = DateTime.Now;
        //                        break;
        //                     // case (int)Enums.ServiceStates.Timetaken:
        //                     //   NewService.FechaTomado = DateTime.Now;
        //                     //   break;
        //                    default:
        //                        return false;
        //                }

        //                NewService.IdEstadoServicio = Service.IdEstadoServicio;
        //                NewService.NivelSatisfaccion = Service.NivelSatisfaccion;

        //                db.Entry(NewService).State = System.Data.Entity.EntityState.Modified;
        //                //db.Servicio.Add(Service);
        //                db.SaveChanges();

        //                tran.Commit();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                tran.Rollback();
        //                error.Error = true;
        //                error.Mensaje = ex.Message;
        //                return false;
        //            }
        //        }
        //    }
        //}

        public static List<Servicio> GetServicesForEmployeeId(string employeeId, int idServiceState, SystemFail error)
        {
            try
            {
                Guid guid = Guid.Parse(employeeId);

                using (var db = new TurneraEntities())
                {
                    return (from e in db.Empleado
                            join se in db.ServicioEmpleado on e.GUID equals se.IdEmpleado
                            join s in db.Servicio on se.IdTipoServicio equals s.IdTipoServicio
                            where e.GUID == guid && s.IdEstadoServicio == idServiceState && (s.FechaPeticion.Value.Day == DateTime.Now.Day && s.FechaPeticion.Value.Year == DateTime.Now.Year)
                            select s).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Servicio>();
            }
        }

        public static List<Servicio> GetServicesToTakeForEmployeeUserName(string userName, int idServiceState, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario
                            join e in db.Empleado on u.Id equals e.IdUsuario
                            join se in db.ServicioEmpleado on e.GUID equals se.IdEmpleado
                            join s in db.Servicio on se.IdTipoServicio equals s.IdTipoServicio
                            where u.UserName == userName && s.IdEstadoServicio == idServiceState && (s.FechaPeticion.Value.Day == DateTime.Now.Day && s.FechaPeticion.Value.Year == DateTime.Now.Year)
                            && (s.IdEmpleado == null || s.IdEmpleado == e.GUID)
                            select s).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Servicio>();
            }
        }

        public static List<Servicio> GetServicesToTakeForCompanieUserName(string userName, int idServiceState, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario
                            join e in db.Empresa on u.Id equals e.IdUsuario
                            join s in db.Servicio on e.GUID equals s.IdEmpresa
                            where u.UserName == userName && s.IdEstadoServicio == idServiceState && (s.FechaPeticion.Value.Day == DateTime.Now.Day && s.FechaPeticion.Value.Year == DateTime.Now.Year)
                            select s).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Servicio>();
            }
        }



        public static List<Servicio> GetAllServices( SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from s in db.Servicio orderby s.FechaPeticion descending select s).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Servicio>();
            }
        }

        public static List<EstadoServicio> GetServicesStates(SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from e in db.EstadoServicio select e).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<EstadoServicio>();
            }
        }


        public static Empleado GetEmployeeForService(string userId, int idTypeService, SystemFail error)
        {
            try
            {
                Guid guid = Guid.Parse(userId);
                using(var db = new TurneraEntities())
                {
                    List<Empleado> Employees = (from e in db.Empleado.Include("Servicio")
                                     join ts in db.ServicioEmpleado on e.GUID equals ts.IdEmpleado
                                     where ts.IdEmpleado == guid && ts.IdTipoServicio == idTypeService
                                     select e).ToList();
                    return Employees.FirstOrDefault();
                }

            }
            catch(Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Empleado();
            }
        }

        public static TipoServicio GetTipeServiceWithServicesForTpeServiceId(int TypeServiceId, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from ts in db.TipoServicio.Include("tblServicio")
                            where ts.IdTipoServicio == TypeServiceId
                            select ts).FirstOrDefault(); 
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new TipoServicio();
            }
        }

        public static Servicio GetServiceForTypeServiceIdAndCustomerIdAndState(int typeServiceId,string customerId, Enums.ServiceStates state, SystemFail error)
        {
            try
            {
                Guid guid = new Guid(customerId);
                using (var db = new TurneraEntities())
                {
                    return (from s in db.Servicio
                                where s.IdTipoServicio == typeServiceId && s.IdCliente == guid && s.IdEstadoServicio == (int)state && (s.FechaPeticion.Value.Year == DateTime.Now.Year && s.FechaPeticion.Value.Day == DateTime.Now.Day)
                                select s).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Servicio();
            }
        }

       

    }
}
