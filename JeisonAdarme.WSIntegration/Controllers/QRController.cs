using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JeisonAdarme.BLL.Model.WSI;
using JeisonAdarme.WSIntegration.Models;
using JeisonAdarme.WSIntegration.Providers;
using JeisonAdarme.WSIntegration.Results;
using business = JeisonAdarme.BLL.BLL;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;

namespace JeisonAdarme.WSIntegration.Controllers
{
    [RoutePrefix("api/QRController")]
    public class QRController : ApiController
    {
        /// <summary>
        /// retorna los codigos para generar el QR //--//
        /// Este codigo es el token unico de la empresa. //--//
        /// Se bede estar autenticado con rol de empresa
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Empresa")]
        [Route("GetQrCodes")]
        public ResponseQrCodes GetQrCodes()
        {
            ResponseQrCodes Codes = new ResponseQrCodes();
            SystemFail error = new SystemFail();
            var Companie = business.Companie.GetCompanieForUserName(User.Identity.Name, error);

            if (Companie != null)
            {
                Codes.CodeService = Companie.TokenUnico;
                Codes.CodeEmploye = Companie.TokenUnico + "Employe";
            }

            return Codes;
        }
    }
}
