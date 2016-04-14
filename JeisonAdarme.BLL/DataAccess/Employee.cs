using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeisonAdarme.BLL.DataBase;
using JeisonAdarme.BLL.Common;

namespace JeisonAdarme.BLL.DataAccess
{
    public class Employee
    {
        public static Empleado GetEmployeeForUserName(string userName, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario
                            join e in db.Empleado on u.Id equals e.IdUsuario
                            where u.UserName == userName
                            select e).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Empleado();
            }
        }

        public static Empleado GetEmployeeForId(string employeeId, SystemFail error)
        {
            try
            {
                Guid guid = Guid.Parse(employeeId);
                using (var db = new TurneraEntities())
                {
                    return (from e in db.Empleado where e.GUID == guid select e).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Empleado();
            }
        }

        public static bool InsertEmployee(Empleado Employee, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Empleado.Add(Employee);
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

        public static List<Empleado> GetEmployeeListForCompanieUsername(string userName, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario
                            join em in db.Empresa on u.Id equals em.IdUsuario
                            join e in db.Empleado on em.GUID equals e.IdEmpresa
                              
                            where u.UserName == userName select e).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Empleado>();
            }
        }


        public static List<Empleado> GetListEmployeeForCompanieUniqueToken(string UniqueToken, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from em in db.Empresa
                            join e in db.Empleado on em.GUID equals e.IdEmpresa
                            where em.TokenUnico == UniqueToken
                            select e).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Empleado>();
            }
        }

        public static List<Empleado> GetAllEmployees(SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from e in db.Empleado select e).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Empleado>();
            }
        }
    }
}
