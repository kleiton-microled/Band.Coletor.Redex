using Band.Coletor.Redex.Site.Filtros;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}