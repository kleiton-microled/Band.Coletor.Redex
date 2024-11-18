using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Models.Entities;
using Band.Coletor.Redex.Site.Models.CarregamentoCargaSolta;
using Band.Coletor.Redex.Site.Models.DescargaExportacao;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class DescargaExportacaoController : Controller
    {
        private readonly IDescargaExportacaoBusiness _descargaExportacaoBusiness;
        private readonly IEquipeBusiness _equipeBusiness;
        private readonly IConferenteBusiness _conferenteBusiness;
        private readonly IOperacaoBusiness _operacaoBusiness;
        //

        private ITalieBusiness _talieBusiness;
        public DescargaExportacaoController(IDescargaExportacaoBusiness descargaExportacaoBusiness,
                                            IEquipeBusiness equipeBusiness,
                                            IConferenteBusiness conferenteBusiness,
                                            IOperacaoBusiness operacaoBusiness,
                                            ITalieBusiness talieBusiness)
        {
            _descargaExportacaoBusiness = descargaExportacaoBusiness;
            _equipeBusiness = equipeBusiness;
            _conferenteBusiness = conferenteBusiness;
            _operacaoBusiness = operacaoBusiness;
            _talieBusiness = talieBusiness;
        }
        public async Task<ActionResult> Index()
        {
            var model = new DescargaExportacaoViewModel
            {
                Conferentes = await _conferenteBusiness.ListAll(),
                Equipes = await _equipeBusiness.ListAll(),
                Operacoes = OperacaoViewModel.Create(),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> ObterDadosTaliePorRegistro(int registro)
        {
            var talie = await _talieBusiness.ObterDadosTaliePorRegistro(registro);


            // Retorna o objeto "talie" como JSON
            return Json(talie, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GravarTalie(TalieViewModel formModel)
        {
            try
            {
                var result = _talieBusiness.Gravar(formModel);

                return Json(new { sucesso = true, mensagem = "Dados gravados com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = "Erro ao gravar os dados. Tente novamentedddd." });
            }
        }


        public ActionResult DescargaExportacaoItens()
        {

            var model = new DescargaExportacaoViewModel()
            {
                Itens = null
            };
            return View("_descargaExportacaoItens", model); // Certifique-se de passar o model, se necessário
        }


    }
}
