using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Band.Coletor.Redex.Site.Filtros
{
    public class AutorizacaoFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Object usuario = filterContext.HttpContext.Session["usuarioLogado"];

            if (usuario == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary(
                                        new { controller = "Home", action = "Index" }));
            }
        }
    }
}