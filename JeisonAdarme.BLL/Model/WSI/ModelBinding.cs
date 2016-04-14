using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.Model.WSI
{
    public class ModelBinding
    {
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

    public class RegisterEmployeeBindingModel
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


        [Required(ErrorMessage = "{0} es requerido.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }
    }

    public class RegisterTyepeServiceBindingModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} es requerido.")]
        public string NombreTipoServicio { get; set; }

        public Nullable<int> Complejidad { get; set; }

        public Nullable<int> TiempoEstimado { get; set; }

        [DataType(DataType.Currency)]
        public Nullable<decimal> Precio { get; set; }
    }

    public class RegisterTypeServiceToEmployeeBindingModel
    {
        [Required(ErrorMessage="{0} es requerido.")]
        public string IdTypeService { get; set; }
    }


    public class RegisterServiceBindingModel
    {
        [Required(ErrorMessage = "{0} es requerido.")]
        public string QrUser { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        public int IdTipoServicio { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        public string IdCliente { get; set; }

    }

    //public class UpdateServiceBindingModel
    //{
    //    [Required(ErrorMessage="{0} es requerido.")]
    //    public string ServiceId { get; set; }

    //    public string EmployeeId { get; set; }
    //    public string EmployeeUniqueNumber { get; set; }
    //    public string EmployeeEmail { get; set; }

    //    [Required(ErrorMessage = "{0} es requerido.")]
    //    public Nullable<int> IdEstadoServicio { get; set; }

    //    public Nullable<int> NivelSatisfaccion { get; set; }
    //}

    public class RegisterCustomerBindingModel
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PushRegistrationId { get; set; }
    }

    public class UpdateServiceBindingModel
    {
        public string IdService { get; set; }
        public int IdServiceNewState { get; set; }
        //public int LevelSatisfaction { get; set; }
    }
}
