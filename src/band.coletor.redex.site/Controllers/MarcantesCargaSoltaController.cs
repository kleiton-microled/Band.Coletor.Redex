using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Site.Models.DescargaExportacao;
using Band.Coletor.Redex.Site.Models.MarcanteCargaSolta;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class MarcantesCargaSoltaController : Controller
    {
        private readonly IConteinerBusiness _conteinerBusiness;
        public MarcantesCargaSoltaController(IConteinerBusiness conteinerBusiness)
        {
            _conteinerBusiness = conteinerBusiness;
        }
        public ActionResult Index()
        {
            var viewModel = new MarcanteCargaSoltaViewModel
            {
                Containers = new List<ConteinerViewModel>
                {
                    new ConteinerViewModel { Id = 1, Descricao = "Container A" },
                    new ConteinerViewModel { Id = 2, Descricao = "Container B" }
                },
            };
            return View(viewModel);
        }

        public async Task<JsonResult> CarregarContainers(string lote)
        {
            // Simule dados baseados no número do lote
            var viewModel = new MarcanteCargaSoltaViewModel();
            var containers = _conteinerBusiness.ObterContainersMarcantes(lote, 1);

            if (containers.Result.Any())
            {
                var dadosContainer = _conteinerBusiness.CarregarDadosContainer(lote);
                viewModel.QuantidadeMarcantes = dadosContainer.Result.Quantidade;
                viewModel.Volume = dadosContainer.Result.Embalagem;
                viewModel.Containers = containers.Result;
            }

            

            //se tiver resultado busca os dados completos

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
