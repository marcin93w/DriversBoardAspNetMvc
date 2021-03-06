﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Driver.WebSite.Controllers
{
    public static class ControllersHelper
    {
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