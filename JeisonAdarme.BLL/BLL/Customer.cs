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
    public class Customer
    {
        public static bool RegisterCustomer(RegisterCustomerBindingModel model, SystemFail error)
        {
            if(IsValidRegisterCustomer(model,error))
            {
                var Customer = BuildCustomer(model);

              return data.Customer.InsertCustomer(Customer, error);
            }

            return false;
        }

        private static bool IsValidRegisterCustomer(RegisterCustomerBindingModel model, SystemFail error)
        {
            if(ExistCustomer(model.Email, error))
            {
                error.Error = true;
                error.Mensaje = "El cliente ya se encuentra registrado.";
                return false;
            }

            if (error.Error)
                return false;

            return true;
        }

        private static Cliente BuildCustomer(RegisterCustomerBindingModel model)
        {
            Cliente Customer = new Cliente()
            {
                GUID = Guid.NewGuid(),
                Nombre = model.Nombre,
                Email = model.Email,
                PushRegistrationId = model.PushRegistrationId
            };

            return Customer;
        }

        public static bool ExistCustomer(string Email, SystemFail error)
        {
            bool Exist = false;

            var Customer = data.Customer.GetCustomerForEmail(Email, error);

            if(Customer != null)
            {
                Exist = true;
            }

            return Exist;
        }

        public static List<Cliente> GetAllCustomers(SystemFail error)
        {
            return data.Customer.GetAllCustomers(error);
        }

        public static Cliente GetCustomerForEmail(string Email, SystemFail error)
        {
            return data.Customer.GetCustomerForEmail(Email, error);
        }

        public static Cliente GetCustomerForPushRegistrationId(string PushRegistrationId, SystemFail error)
        {
            return data.Customer.GetCustomerForPushRegistrationId(PushRegistrationId, error);
        }
    }
}
