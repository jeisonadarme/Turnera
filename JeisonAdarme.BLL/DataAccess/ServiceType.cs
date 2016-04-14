using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.DataAccess
{
    public class ServiceType
    {
        public static TipoServicio GetTypeServiceforName(string nameService, SystemFail error)
        {
            try
            {
                using(var db = new TurneraEntities())
                {
                    return (from ts in db.TipoServicio where ts.NombreTipoServicio.ToLower() == nameService.ToLower() select ts).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new TipoServicio();
            }
        }

        public static TipoServicio GetTypeServiceforId(int idTypeService, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from ts in db.TipoServicio where ts.IdTipoServicio == idTypeService select ts).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new TipoServicio();
            }
        }

        public static List<TipoServicio> GetTypeServiceListForCompanieUserName(string userName, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario 
                            join e in db.Empresa on u.Id equals e.IdUsuario
                            join ts in db.TipoServicio on e.GUID equals ts.IdEmpresa
                            where u.UserName == userName && ts.EsActivo == true
                            select ts).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<TipoServicio>();
            }
        }

        public static List<TipoServicio> GetListTypeServiceForUniqueTokenCompanie(string uniqueToken, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from ts in db.TipoServicio 
                            join e in db.Empresa on ts.IdEmpresa equals e.GUID
                            where e.TokenUnico == uniqueToken && ts.EsActivo == true
                            select ts).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<TipoServicio>();
            }
        }

        public static List<TipoServicio> GetListTypeServicesForEmployeeUserName(string userName, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario
                            join e in db.Empleado on u.Id equals e.IdUsuario
                            join es in db.ServicioEmpleado on e.GUID equals es.IdEmpleado
                            join s in db.TipoServicio on es.IdTipoServicio equals s.IdTipoServicio
                            where u.UserName == userName && s.EsActivo == true
                            select s).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<TipoServicio>();
            }
        }


        public static bool InsertTypeService(TipoServicio TypeService, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.TipoServicio.Add(TypeService);
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

        public static bool DisableTypeServiceForIdTypeService(int IdTypeService, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var TypeService = GetTypeServiceforId(IdTypeService, error);
                        TypeService.EsActivo = false;
                        db.Entry(TypeService).State = System.Data.Entity.EntityState.Modified;
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



        public static List<TipoServicio> GetAllTypeServices(SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from ts in db.TipoServicio select ts).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<TipoServicio>();
            }
        }
    }
}
