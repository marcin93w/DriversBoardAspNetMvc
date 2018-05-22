using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Driver.WebSite.DAL;
using Driver.WebSite.Migrations;
using Driver.WebSite.Source.Security;

namespace Driver.WebSite
{
    public class MvcApplication : HttpApplication
    {
        private MaliciousRequestsDetector MaliciousRequestsDetector
        {
            get
            {
                var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
                return (MaliciousRequestsDetector)
                    dependencyResolver.GetService(typeof(MaliciousRequestsDetector));
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.CreateMappings();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        protected void Application_BeginRequest()
        {
            MaliciousRequestsDetector.InspectRequest(Context.Request);
        }
    }
}
