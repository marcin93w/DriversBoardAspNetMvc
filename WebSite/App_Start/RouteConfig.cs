using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

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

            AddPagedRoutes(routes, "Top", "top");
            AddPagedRoutes(routes, "WaitingItems", "poczekalnia");

            routes.MapRoute(
                name: "DriverId",
                url: "kierowca/id/{driverId}",
                defaults: new { controller = "Home", action = "DriverItemsById", driverId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DriverIdPaged",
                url: "kierowca/id/{driverId}/strona/{page}",
                defaults: new { controller = "Home", action = "DriverItemsById", page = 1, driverId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Driver",
                url: "kierowca/{plate}",
                defaults: new { controller = "Home", action = "DriverItems", plate = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DriverPaged",
                url: "kierowca/{plate}/strona/{page}",
                defaults: new { controller = "Home", action = "DriverItems", page = 1, plate = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Index",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "HomeIndexPaged",
                url: "strona/{page}",
                defaults: new { controller = "Home", action = "Index", page = 1, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private static void AddPagedRoutes(RouteCollection routes, string action, string url)
        {
            routes.MapRoute(
                name: action,
                url: url,
                defaults: new { controller = "Home", action = action, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: $"{action}Paged",
                url: url + "/strona/{page}",
                defaults: new { controller = "Home", action = action, page = 1, id = UrlParameter.Optional }
            );
        }
    }
}
