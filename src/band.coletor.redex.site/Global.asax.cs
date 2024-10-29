using Band.Coletor.Redex.Site.App_Start;
using Band.Coletor.Redex.Site.Helpers;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Band.Coletor.Redex.Site
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(
              typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(
                typeof(decimal?), new DecimalModelBinder());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimsIdentity.DefaultNameClaimType;
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            Identity.Autenticar();
        }
    }
}