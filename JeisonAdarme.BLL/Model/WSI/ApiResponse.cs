using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeisonAdarme.BLL.Model.WSI
{

    public class ResponseStates
    {
        public int IdEstadoServicio { get; set; }
        public string NombreEstadoServicio { get; set; }
    }

    public class ApiResponse
    {
        public bool Error { get; set; }
        public string ErrorMessage {get; set; }
    }

    public class Response
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Idservicio { get; set; }
        public string NumeroTurno { get; set; }
    }

    public class ReponseTypeService
    {
        public int IdTipoServicio { get; set; }
        public string NombreTipoServicio { get; set; }
        public Nullable<int> Complejidad { get; set; }
        public Nullable<int> TiempoEstimado { get; set; }
        public Nullable<bool> EsActivo { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<System.Guid> IdEmpresa { get; set; }
    }

    public class ResponseEmployee
    {
        public System.Guid GUID { get; set; }
        public string IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string NumeroCelular { get; set; }
        public Nullable<bool> EsActivo { get; set; }
        public Nullable<System.Guid> IdEmpresa { get; set; }
    }

    public class ResponseCustomer
    {
        public System.Guid GUID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PushRegistrationId { get; set; }
    }

    public class ResponseUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    }

    public class ResponseQrCodes
    {
        public string CodeEmploye { get; set; }
        public string CodeService { get; set; }
    }

    public class ResponseService
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }

        public System.Guid GUID { get; set; }
        public Nullable<System.Guid> IdEmpleado { get; set; }
        public Nullable<int> IdTipoServicio { get; set; }
        public Nullable<int> IdEstadoServicio { get; set; }
        public Nullable<System.DateTime> FechaPeticion { get; set; }
        public Nullable<System.DateTime> FechaTomado { get; set; }
        public Nullable<System.DateTime> FechaFinalizado { get; set; }
        public Nullable<int> NivelSatisfaccion { get; set; }
        public string CodigoServicio { get; set; }
        public Nullable<System.Guid> IdCliente { get; set; }
        public Nullable<System.Guid> IdEmpresa { get; set; }
    }



}
