using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using JeisonAdarme.WSIntegration.Models;
using JeisonAdarme.WSIntegration.Providers;
using JeisonAdarme.WSIntegration.Results;
using JeisonAdarme.BLL.Model.WSI;
using business = JeisonAdarme.BLL.BLL;
using JeisonAdarme.BLL.Common;
using JeisonAdarme.BLL.DataBase;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;

namespace JeisonAdarme.WSIntegration.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        /// <summary>
        /// retorna toda la informacion del usuario autenticado.
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            SystemFail error = new SystemFail();
            var UserData = business.UserAccount.GetUserForUserName(User.Identity.GetUserName(), error);

            return new UserInfoViewModel
            {
                Id = UserData.Id,
                UserName = UserData.UserName,
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        /// <summary>
        /// Retorna toda la lista de usaurios registrados en base de datos.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetAllUsers")]
        public List<ResponseUser> GetAllUsers()
        {
            SystemFail error = new SystemFail();
            List<ResponseUser> ListUsers = new List<ResponseUser>();


            var List = business.UserAccount.GetAllUser(error);

            foreach (var item in List)
            {
                ListUsers.Add(CastObjects.CastObject<ResponseUser, Usuario>(item));
            }

            return ListUsers;
        }

        /// <summary>
        /// obtiene el rol del usurio en sesion
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("GetRoleForUser")]
        public string GetRoleForUser()
        {
            if (User.IsInRole(Enums.UserRoles.Empleado.ToString()))
                return Enums.UserRoles.Empleado.ToString();

            if (User.IsInRole(Enums.UserRoles.Empresa.ToString()))
                return Enums.UserRoles.Empresa.ToString();

            return "";
        }

        /// <summary>
        /// desautentica a el usaurio.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins
                ,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        /// <summary>
        /// cambia la contraseña para el suario.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// asigna una contrasena a el usuario.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        /// <summary>
        /// Inserta en base de datos la empresa.
        /// y crea la cuenta de usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("RegisterCompanie")]
        public async Task<ApiResponse> RegisterCompanie(RegisterBindingModel model)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "Se registro el usuario." };
            SystemFail error = new SystemFail();
            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            try
            {
                string validationErrors = ValidateRegisterUser(model.Email);

                if (!string.IsNullOrEmpty(validationErrors))
                {
                    response.Error = true;
                    response.ErrorMessage = validationErrors;

                    return response;
                }

                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FechaRegistro = DateTime.Now,
                    EmailConfirmed = false
                };

                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 4,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var rerulstError = GetErrorResult(result);
                    response.Error = true;

                    response.ErrorMessage = "";

                    foreach (var item in result.Errors)
                    {
                        response.ErrorMessage += " " + item;
                    }

                    return response;
                }
                else
                {
                    bool bitSave = false;
                    bitSave = business.Companie.RegisterCompanie(model, user.Id, error);

                    if (!bitSave || error.Error)
                    {
                        response.Error = true;
                        response.ErrorMessage = error.Mensaje;
                    }
                    else
                    {
                        //lo agregamos al rol
                        UserManager.AddToRole(user.Id, Enums.UserRoles.Empresa.ToString());
                        //confirmat email --string userId, string code
                        string code = GenerateEmailComfirmationToken(user.Id);
                        // await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);  
                        string callbackUrl = string.Format("http://turnera1-001-site1.atempurl.com/Account/ConfirmEmail?userId={0}&code={1}", user.Id, HttpUtility.UrlEncode(code));
                        sendemail(model.Email, callbackUrl);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
                return response;
            }
        }



        /// <summary>
        /// Registra el empleado en base de datos, es necesario estar autenticado con rol de empresa para poder registrar empleados
        /// y crea la cuenta de usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("RegisterEmployee")]
        [Authorize(Roles = "Empresa")]
        public ApiResponse RegisterEmployee(RegisterEmployeeBindingModel model)
        {
            ApiResponse response = new ApiResponse { Error = false, ErrorMessage = "Se registro el empleado." };
            SystemFail error = new SystemFail();

            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.ErrorMessage = string.Empty;

                ManageModelErrors(response);

                return response;
            }

            try
            {
                string validationErrors = ValidateRegisterUser(model.Email);

                if (!string.IsNullOrEmpty(validationErrors))
                {
                    response.Error = true;
                    response.ErrorMessage = validationErrors;

                    return response;
                }

                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FechaRegistro = DateTime.Now,
                    EmailConfirmed = true
                };

                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 4,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };

                IdentityResult result = UserManager.Create(user, model.Password);

                if (!result.Succeeded)
                {
                    var resultError = GetErrorResult(result);
                    response.Error = true;

                    response.ErrorMessage = "";

                    foreach (var item in result.Errors)
                    {
                        response.ErrorMessage += " " + item;
                    }

                    return response;
                }
                else
                {

                    bool bitSave = false;
                    bitSave = business.Employee.RegisterEmployee(model, user.Id, User.Identity.Name, error);

                    if (!bitSave || error.Error)
                    {
                        response.Error = true;
                        response.ErrorMessage = error.Mensaje;
                    }
                    else //lo agregamos al rol
                        UserManager.AddToRole(user.Id, Enums.UserRoles.Empleado.ToString());

                }

                return response;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
                return response;
            }
        }

        private string GenerateEmailComfirmationToken(string userId)
        {
            string html = string.Empty;
            string url = @"http://localhost:52281/Account/GenerateCodeEmail?userId=" + userId;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        private bool sendemail(string strEmail, string callBackUrl)
        {
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(strEmail));
            email.From = new MailAddress("jh.12@outlook.com");
            email.Subject = "Asunto ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            email.Body = "Por favor, para confirmar tu cuenta haz click <a href=\"" + callBackUrl + "\">aquí</a>";
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.live.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("jh.12@outlook.com", "8970127293*12");


            try
            {
                ServicePointManager.ServerCertificateValidationCallback =

               delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
               {
                   return true;
               };

                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private string ValidateRegisterUser(string userName)
        {
            SystemFail error = new SystemFail();

            string MessageError = string.Empty;
            var user = business.UserAccount.GetUserForUserName(userName, error);
            if (user != null)
            {
                MessageError = "Ya existe un usuario con esta cuenta de correo.";
            }

            if (error.Error)
                MessageError = error.Mensaje;

            return MessageError;
        }

        private void ManageModelErrors(ApiResponse response)
        {
            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    response.ErrorMessage = string.Format("{0} - {1}", response.ErrorMessage, error.ErrorMessage);
                }
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
