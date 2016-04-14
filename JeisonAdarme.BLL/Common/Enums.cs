using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.Common
{
    public class Enums
    {
        public enum ServiceStates
        {
            TimeRegister = 1,
            Timetaken = 2,
            TimeEnd = 3,
            TimeCanceled = 4
        }

        public enum UserRoles
        {
            Empresa,
            Empleado
        }
    }
}
