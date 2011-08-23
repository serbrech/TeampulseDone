using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TeampulseReporting.Data;

namespace TeampulseReporting
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static TeamPulseEntities CurrentContext
        {
            get { return HttpContext.Current.Items["EFSession"] as TeamPulseEntities; }
            set { HttpContext.Current.Items["EFSession"] = value; }
        }

        public MvcApplication()
        {
            BeginRequest += (sender, args) =>
            {
                CurrentContext = new TeamPulseEntities();
            };
            EndRequest += (o, eventArgs) =>
            {
                var context = CurrentContext;
                if (context != null)
                {
                    context.Dispose();
                }
            };

        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}