using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class ContatosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }
    }
}