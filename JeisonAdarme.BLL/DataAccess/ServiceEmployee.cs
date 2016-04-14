using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.DataAccess
{
    public class ServiceEmployee
    {
        public static bool InsertTypeServiceToEmployee(ServicioEmpleado ServiceToEmployee, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.ServicioEmpleado.Add(ServiceToEmployee);
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

        public static bool InsertTypeServiceToEmployee(List<ServicioEmpleado> ServiceToEmployee, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in ServiceToEmployee)
                        {
                            db.ServicioEmpleado.Add(item);
                            db.SaveChanges();
                        }

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

        public static ServicioEmpleado GetEmployeeServiceForIdTyepeServiceAndIdEmployee(string idEmployee, string idTypeService, SystemFail error)
        {
            try
            {
                Guid guidEmployee = Guid.Parse(idEmployee);
                int idService = int.Parse(idTypeService);
                using(var db = new TurneraEntities())
                {
                    return (from se in db.ServicioEmpleado
                                where se.IdEmpleado == guidEmployee && se.IdTipoServicio == idService
                                select se).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;

                return new ServicioEmpleado();
            }
        }
    }
}
