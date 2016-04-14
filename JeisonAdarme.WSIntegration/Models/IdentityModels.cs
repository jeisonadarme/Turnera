using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using JeisonAdarme.BLL.DataBase;
using System;

namespace JeisonAdarme.WSIntegration.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public System.DateTime FechaRegistro { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<ApplicationUser>().ToTable("tblUsuario");
            //modelBuilder.Entity<IdentityRole>().ToTable("Role");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}