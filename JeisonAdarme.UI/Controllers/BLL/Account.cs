using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using business = JeisonAdarme.BLL.BLL;

namespace JeisonAdarme.UI.Controllers.BLL
{
    public class Account
    {
        public static Usuario GetUserForUserName(string userName, SystemFail error)
        {
            return business.UserAccount.GetUserForUserName(userName, error);
        }
    }
}