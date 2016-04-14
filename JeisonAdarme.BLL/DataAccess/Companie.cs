using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.DataAccess
{
    public class Companie
    {
        public static bool InsertCompanie(Empresa Companie, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Empresa.Add(Companie);
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


        public static bool InsertEmployee(Empresa Companie, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Empresa.Add(Companie);
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

        public static Empresa GetCompanieForUserName(string userName, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario
                            join e in db.Empresa on u.Id equals e.IdUsuario
                            where u.UserName == userName
                            select e).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Empresa();
            }
        }

        public static Empresa GetCompanieForUniqueToken(string uniqueToekn, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from e in db.Empresa 
                            where e.TokenUnico == uniqueToekn
                            select e).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Empresa();
            }
        }
    }
}
