using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Site.Models.DescargaExportacao;
using System;
using System.Globalization;
using System.Linq;
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
            var talieData = TempData["DescargaExportacaoData"] as TalieViewModel;

            var viewModel = new DescargaExportacaoViewModel
            {
                Talie = talieData ?? new TalieViewModel(), 
                Conferentes = await _conferenteBusiness.ListAll(), 
                Equipes = await _equipeBusiness.ListAll(), 
                Operacoes = OperacaoViewModel.Create() 
            };
            TempData["DescargaExportacaoData"] = talieData;
            return View(viewModel);
        }
        public async Task<ActionResult> DescargaExportacaoItens()
        {

            var talieData = TempData["DescargaExportacaoData"] as TalieViewModel;

            var viewModel = new DescargaExportacaoViewModel
            {
                Talie = talieData ?? new TalieViewModel(),
                Conferentes = await _conferenteBusiness.ListAll(),
                Equipes = await _equipeBusiness.ListAll(),
                Operacoes = OperacaoViewModel.Create(),
                Itens = null
            };
            TempData["DescargaExportacaoData"] = talieData;
            return View("_descargaExportacaoItens", viewModel); 
        }

        public async Task<ActionResult> DescargaExportacaoMarcantes()
        {

            var talieData = TempData["DescargaExportacaoData"] as TalieViewModel;

            var viewModel = new DescargaExportacaoViewModel
            {
                Talie = talieData ?? new TalieViewModel(),
                Conferentes = await _conferenteBusiness.ListAll(),
                Equipes = await _equipeBusiness.ListAll(),
                Operacoes = OperacaoViewModel.Create(),
                Itens = null
            };
            TempData["DescargaExportacaoData"] = talieData;
            return View("_descargaExportacaoMarcante", viewModel);
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
            TempData["DescargaExportacaoData"] = formModel;

            var _serviceResultSave = new ServiceResult<int>();
            var _serviceResultUpdate = new ServiceResult<bool>();
            try
            {
                var valid = ValidarDados(formModel);
                if (valid.Result)
                {
                    if (formModel.CodigoTalie == 0)
                    {
                        _serviceResultSave = _talieBusiness.Save(formModel).Result;
                        if (_serviceResultSave.Status)
                            return Json(new { sucesso = true, mensagem = _serviceResultSave.Mensagens.FirstOrDefault() });

                    }
                    else
                    {
                        _serviceResultUpdate = _talieBusiness.Update(formModel).Result;
                        if (_serviceResultUpdate.Status)
                            return Json(new { sucesso = false, mensagem = _serviceResultUpdate.Mensagens.FirstOrDefault() });
                    }

                }
                else
                {
                    return Json(new { sucesso = false, mensagem = valid.Mensagens.FirstOrDefault() });
                }


                return Json(new { sucesso = true, mensagem = "Erro no processamento" });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult BuscarNotaFiscal(string numeroNotaFiscal, string codigoBooking, string codigoRegistro)
        {
            //validacoes
            var _serviceResult = _talieBusiness.ObterNotaFiscal(numeroNotaFiscal, codigoBooking, codigoRegistro);
            return Json(new { sucesso = false, mensagem = _serviceResult.Mensagens.FirstOrDefault() });
        }
        private ServiceResult<bool> ValidarDados(TalieViewModel formModel)
        {
            var _serviceResult = new ServiceResult<bool>();

            if (formModel.StatusTalie == "TALIE FECHADO")
            {
                _serviceResult.Mensagens.Add("Talie Fechado - Operação cancelada");
                _serviceResult.Result = false;
                return _serviceResult;
            }
            //var data = DateTime.ParseExact(formModel.Inicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //if (data.Month > 12)
            //{
            //    _seriviceResult.Error = "Data inválida";
            //    return _seriviceResult;
            //}

            _serviceResult.Mensagens.Add("Dados validados");
            _serviceResult.Result = true;
            return _serviceResult;
        }


    }
}
