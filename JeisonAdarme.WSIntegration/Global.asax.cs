using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using JeisonAdarme.WSIntegration.Models;

namespace JeisonAdarme.WSIntegration
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationDbContext db = new ApplicationDbContext();

            CreateRoles(db);

            db.Dispose();
        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                
            if (!roleManager.RoleExists("Empresa"))
                roleManager.Create(new IdentityRole("Empresa"));

            if (!roleManager.RoleExists("Empleado"))
                roleManager.Create(new IdentityRole("Empleado"));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));
        }
    }
}
