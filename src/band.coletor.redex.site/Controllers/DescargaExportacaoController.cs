using Band.Coletor.Redex.Site.Models.CarregamentoCargaSolta;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class DescargaExportacaoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObterConferentes()
        {
            // Dados mockados para teste
            var mockData = new List<OrdemCarregamento>
            {
                new OrdemCarregamento { NumOc = "3972588", Lote = "2435638", Qtde = 5, Carreg = 0, Embalagem = "CAIXA DE MADEIRA" }
            };

            return Json(mockData, JsonRequestBehavior.AllowGet);
        }
    }
}
