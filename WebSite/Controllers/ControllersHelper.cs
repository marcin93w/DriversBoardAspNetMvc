using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Driver.WebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Driver.WebSite.Controllers
{
    public static class ControllersHelper
    {
        private static async Task<ApplicationUser> GetCurrentUserAsync(
            ApplicationDbContext context, IPrincipal user)
        {
            var store = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(store);
            return await userManager.FindByNameAsync(user.Identity.Name);
        }

        public static async Task<ApplicationUser> GetCurrentUserAsync(this ApiController controller,
            ApplicationDbContext context)
        {
            return await GetCurrentUserAsync(context, controller.User);
        }

        public static async Task<ApplicationUser> GetCurrentUserAsync(this Controller controller,
            ApplicationDbContext context)
        {
            return await GetCurrentUserAsync(context, controller.User);
        }

        private static string GetCurrentUserId(IPrincipal user)
        {
            return user.Identity?.GetUserId();
        }

        public static string GetCurrentUserId(this ApiController controller)
        {
            return GetCurrentUserId(controller.User);
        }

        public static string GetCurrentUserId(this Controller controller)
        {
            return GetCurrentUserId(controller.User);
        }
    }
}