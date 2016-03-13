using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Driver.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AddItem",
                url: "dodaj",
                defaults: new { controller = "Home", action = "AddItem", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Item",
                url: "wpis/{id}",
                defaults: new { controller = "Home", action = "ItemPage", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "HomeIndexPaged",
                url: "strona/{page}",
                defaults: new { controller = "Home", action = "Index", page = 1, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "HomePaged",
                url: "{action}/strona/{page}",
                defaults: new { controller = "Home", action = "Index", page = 1, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
