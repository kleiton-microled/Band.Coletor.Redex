

using Band.Coletor.Redex.Site.Models.CarregamentoCargaSolta;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class CarregamentoCargaSoltaController : Controller
    {
        // Ação principal da página de Carregamento Carga Solta
        public ActionResult Index()
        {

            var veiculos = new List<Veiculo>();
            veiculos.Add(Veiculo.Create(1, "Selecione um veiculo"));
            veiculos.Add(Veiculo.Create(1, "BX-9551 SCANIA"));

            return View(veiculos);
        }

        [HttpGet]
        public JsonResult ObterMarcante(string marcante)
        {
            // Dados mockados para simular a resposta do banco
            var resultado = new
            {
                Marcante = marcante,
                Local = "Local Exemplo",
                Lote = "Lote Exemplo"
            };

            // Retornar dados como JSON
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #region Listas
        [HttpGet]
        public JsonResult ObterVeiculos(int veiculoId)
        {
            // Dados mockados para teste
            var mockData = new List<OrdemCarregamento>
            {
                new OrdemCarregamento { NumOc = "3972588", Lote = "2435638", Qtde = 5, Carreg = 0, Embalagem = "CAIXA DE MADEIRA" }
            };

            return Json(mockData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
