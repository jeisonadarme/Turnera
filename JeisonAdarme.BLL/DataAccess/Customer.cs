using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeisonAdarme.BLL.DataBase;
using JeisonAdarme.BLL.Common;

namespace JeisonAdarme.BLL.DataAccess
{
    public class Customer
    {
        public static bool InsertCustomer(Cliente Customer, SystemFail error)
        {
            using (var db = new TurneraEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Cliente.Add(Customer);
                        tran.Commit();
                        db.SaveChanges();

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

        public static Cliente GetCustomerForPushRegistrationId(string PushRegistrationId, SystemFail error)
        {
            try
            {
                using(var db = new TurneraEntities())
                {
                    return (from c in db.Cliente where c.PushRegistrationId == PushRegistrationId select c).FirstOrDefault();
                }
            }
            catch(Exception  ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Cliente();
            }
        }



        public static Cliente GetCustomerForCustomerId(string CustomerId, SystemFail error)
        {
            try
            {
                Guid guid = new Guid(CustomerId);
                using (var db = new TurneraEntities())
                {
                   return (from c in db.Cliente where c.GUID == guid select c).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Cliente();
            }
        }

        public static Cliente GetCustomerForEmail(string Email, SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from c in db.Cliente where c.Email == Email select c).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new Cliente();
            }
        }

        public static List<Cliente> GetAllCustomers(SystemFail error)
        {
            try
            {
                using (var db = new TurneraEntities())
                {
                    return (from c in db.Cliente select c).ToList();
                }
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return new List<Cliente>();
            }
        }
    }
}
