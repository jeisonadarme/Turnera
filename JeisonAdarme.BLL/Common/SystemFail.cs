using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.Common
{
    public interface ISystemFail
    {
        #region Atributos

        bool Error { get; set; }

        string Titulo { get; set; }

        string Mensaje { get; set; }

        string Detalle { get; set; }

        Exception Excepcion { get; set; }

        #endregion
    }

    public class SystemFail : ISystemFail
    {
        #region Constructor
        public SystemFail()
        {
            Error = false;
        }
        #endregion

        #region Atributos

        public bool Error { get; set; }

        public string Titulo { get; set; }

        public string Mensaje { get; set; }

        public string Detalle { get; set; }

        public Exception Excepcion { get; set; }

        #endregion
    }
}
