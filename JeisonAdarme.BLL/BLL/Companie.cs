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
    public class Companie
    {
        public static Empresa GetCompanieForUserName(string userName, SystemFail error)
        {
            return data.Companie.GetCompanieForUserName(userName, error);
        }

        public static Empresa GetCompanieForUniqueToken(string uniqueToekn, SystemFail error)
        {
            return data.Companie.GetCompanieForUniqueToken(uniqueToekn, error);
        }

        public static bool RegisterCompanie(RegisterBindingModel model, string userId, SystemFail error)
        {
            try
            {
                if (data.Companie.InsertCompanie(BuildCompanie(model, userId), error))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                error.Error = true;
                error.Mensaje = ex.Message;
                return false;
            }
        }

        private static Empresa BuildCompanie(RegisterBindingModel model, string userId)
        {
            Empresa Companie = new Empresa()
            {
                GUID = Guid.NewGuid(),
                IdUsuario = userId,
                Nombre = model.Nombre,
                Latitud = model.Latitud,
                Longitud = model.Longitud,
                Direccion = model.Direccion,
                Descripcion = model.Descripcion,
                NumeroCelular = model.NumeroCelular,
                NumeroTelefono = model.NumeroTelefono,
                TokenUnico = Util.GetNewNumericGuid(),
                UrlImagen = ""
            };

            return Companie;
        }
    }
}
