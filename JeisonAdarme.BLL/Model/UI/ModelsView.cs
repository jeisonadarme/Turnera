using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.Model.UI
{
    public class TypeServiceModelView
    {
        public int IdTipoServicio { get; set; }

        [Required(ErrorMessage="El Nombre del servicio es requerido.")]
        public string NombreTipoServicio { get; set; }
        public Nullable<int> Complejidad { get; set; }
        public Nullable<int> TiempoEstimado { get; set; }
        public Nullable<bool> EsActivo { get; set; }
        public Nullable<decimal> Precio { get; set; }
    }

    public class QrCodesModelView
    {
        public string CodeEmploye { get; set; }
        public string CodeService { get; set; }
    }


    public class RegisterBindingModel
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        public string NumeroTelefono { get; set; }


        public string NumeroCelular { get; set; }

        public string Direccion { get; set; }

        public Nullable<decimal> Longitud { get; set; }

        public Nullable<decimal> Latitud { get; set; }

        public string Descripcion { get; set; }

    }
}
