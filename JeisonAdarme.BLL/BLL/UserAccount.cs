using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data = JeisonAdarme.BLL.DataAccess;

namespace JeisonAdarme.BLL.BLL
{
    public class UserAccount
    {
        public static bool ExistUserForUserName(string userName, SystemFail error)
        {
            var user = data.UserAccount.GetUserForUserName(userName, error);

            if (error.Error)
                return false;

            if (user == null)
                return false;

            return true;
        }

        public static Usuario GetUserForUserName(string userName, SystemFail error)
        {
            return data.UserAccount.GetUserForUserName(userName, error);
        }

        public static List<Usuario> GetAllUser(SystemFail error)
        {
            return data.UserAccount.GetAllUsers(error);
        }
    }
}
