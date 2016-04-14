using JeisonAdarme.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeisonAdarme.BLL.DataBase;

namespace JeisonAdarme.BLL.DataAccess
{
    public class UserAccount
    {
        public static Usuario GetUserForUserName(string userName, SystemFail error)
        {
            try
            {
                using(var db = new TurneraEntities())
                {
                    return (from u in db.Usuario where u.UserName == userName select u).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Usuario();
            }
        }

        public static Usuario GetUserForUserId(string userId, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario where u.Id == userId select u).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Usuario();
            }
        }


        public static List<Usuario> GetAllUsers(SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from u in db.Usuario  select u).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Usuario>();
            }
        }
    }
}
