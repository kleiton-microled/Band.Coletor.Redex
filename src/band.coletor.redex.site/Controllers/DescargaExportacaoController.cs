using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Site.Models.DescargaExportacao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly IArmazemBusiness _armazemBusiness;
        private readonly IRegistroBusiness _registroBusiness;
        //

        private ITalieBusiness _talieBusiness;
        public DescargaExportacaoController(IDescargaExportacaoBusiness descargaExportacaoBusiness,
                                            IEquipeBusiness equipeBusiness,
                                            IConferenteBusiness conferenteBusiness,
                                            IOperacaoBusiness operacaoBusiness,
                                            ITalieBusiness talieBusiness,
                                            IArmazemBusiness armazemBusiness,
                                            IRegistroBusiness registroBusines)
        {
            _descargaExportacaoBusiness = descargaExportacaoBusiness;
            _equipeBusiness = equipeBusiness;
            _conferenteBusiness = conferenteBusiness;
            _operacaoBusiness = operacaoBusiness;
            _talieBusiness = talieBusiness;
            _armazemBusiness = armazemBusiness;
            _registroBusiness = registroBusines;
        }
        public async Task<ActionResult> Index()
        {
            var registroData = TempData["DescargaExportacaoData"] as RegistroViewModel;

            var viewModel = new DescargaExportacaoViewModel
            {
                Registro = registroData ?? new RegistroViewModel(),
                Conferentes = await _conferenteBusiness.ListAll(),
                Equipes = await _equipeBusiness.ListAll(),
                Armazems = await _armazemBusiness.ListAll(),
                Operacoes = OperacaoViewModel.Create()
            };
            if(registroData != null)
                TempData["DescargaExportacaoData"] = registroData;

            return View(viewModel);
        }
        public async Task<ActionResult> DescargaExportacaoItens()
        {

            var registroData = TempData["DescargaExportacaoData"] as RegistroViewModel;

            var viewModel = new DescargaExportacaoViewModel
            {
                Registro = registroData ?? new RegistroViewModel(),
                Conferentes = await _conferenteBusiness.ListAll(),
                Equipes = await _equipeBusiness.ListAll(),
                Operacoes = OperacaoViewModel.Create(),
                Itens = null
            };
            TempData["DescargaExportacaoData"] = registroData;
            return View("_descargaExportacaoItens", viewModel);
        }

        public async Task<ActionResult> DescargaExportacaoMarcantes()
        {

            var registroData = TempData["DescargaExportacaoData"] as RegistroViewModel;

            var viewModel = new DescargaExportacaoViewModel
            {
                Registro = registroData ?? new RegistroViewModel(),
                Conferentes = await _conferenteBusiness.ListAll(),
                Equipes = await _equipeBusiness.ListAll(),
                Operacoes = OperacaoViewModel.Create(),
                Itens = null
            };
            TempData["DescargaExportacaoData"] = registroData;
            return View("_descargaExportacaoMarcante", viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> CarregarRegistro(int codigoRegistro)
        {
            var registro = await _registroBusiness.CarregarRegistro(codigoRegistro);

            // Retorna o objeto "talie" como JSON
            return Json(registro, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GravarTalie(RegistroViewModel formModel)
        {
            TempData["DescargaExportacaoData"] = formModel;

            var _serviceResultSave = new ServiceResult<int>();

            try
            {
                var valid = ValidarDados(formModel);
                if (valid.Result)
                {
                    _serviceResultSave = _registroBusiness.SaveOrUpdate(formModel);
                    if (_serviceResultSave.Status)
                    {
                        if (_serviceResultSave.Result > 0)
                        {
                            _registroBusiness.GeraDescargaAutomatica(formModel.CodigoRegistro, _serviceResultSave.Result);
                        }
                        return Json(new { sucesso = true, mensagem = _serviceResultSave.Mensagens.FirstOrDefault() });
                    }
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = valid.Mensagens.FirstOrDefault() });
                }

                return Json(new { sucesso = false, mensagem = "Erro no processamento dos dados. Contate o suporte." });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Ocorreu um erro inesperado: {ex.Message}" });
            }
        }


        [HttpPost]
        public async Task<JsonResult> BuscarNotaFiscal(string numeroNotaFiscal, string codigoBooking, string codigoRegistro)
        {
            try
            {
                var serviceResult = _talieBusiness.ObterNotaFiscal(numeroNotaFiscal, codigoBooking, codigoRegistro);

                if (serviceResult.Result == 0)
                {
                    return Json(new
                    {
                        sucesso = false,
                        mensagem = serviceResult.Mensagens.FirstOrDefault()
                    });
                }

                var itensNf = await _talieBusiness.ObterItensNotaFiscal(numeroNotaFiscal, codigoRegistro);

                return Json(new
                {
                    sucesso = true,
                    data = itensNf
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    sucesso = false,
                    mensagem = "Ocorreu um erro ao buscar a nota fiscal.",
                    detalheErro = ex.Message
                });
            }
        }

        private ServiceResult<bool> ValidarDados(RegistroViewModel formModel)
        {
            var _serviceResult = new ServiceResult<bool>
            {
                Result = true // Inicialmente assume que os dados são válidos
            };

            // Validação de DANFE
            var danfe = _registroBusiness.ValidarDanfe(formModel.CodigoRegistro);
            if (danfe == 1) //TODO - verificar com Valdemir
            {
                _serviceResult.Result = false;
                _serviceResult.Mensagens.Add("Encontrado registro de entrada sem número de DANFE informado. Acesse a tela de registro e verifique os dados da nota fiscal.");
            }

            var notaValida = _registroBusiness.ValidarNotaCadastrada(formModel.CodigoRegistro);

            if (!notaValida) //TODO - verificar com Valdemir
            {
                _serviceResult.Result = false;
                _serviceResult.Mensagens.Add("Registro com NF não cadastrada.");
            }

            //// Validação do conferente
            //if (formModel.Conferente == 0)
            //{
            //    _serviceResult.Result = false;
            //    _serviceResult.Mensagens.Add("O conferente é obrigatório. Por favor, preencha este campo.");
            //}

            //// Validação da equipe
            //if (formModel.Equipe == 0)
            //{
            //    _serviceResult.Result = false;
            //    _serviceResult.Mensagens.Add("A equipe é obrigatória. Por favor, preencha este campo.");
            //}

            //// Validação da operação
            //if (formModel.Operacao == 0)
            //{
            //    _serviceResult.Result = false;
            //    _serviceResult.Mensagens.Add("A operação é obrigatória. Por favor, preencha este campo.");
            //}

            //// Caso todas as validações sejam bem-sucedidas
            //if (_serviceResult.Result)
            //{
            //    _serviceResult.Mensagens.Add("Todos os dados foram validados com sucesso.");
            //}

            return _serviceResult;
        }


        //buscar os armazens
        [HttpGet]
        public async Task<JsonResult> GetArmazens()
        {
            var armazens = await ListarArmazens();
            return Json(armazens, JsonRequestBehavior.AllowGet);
        }

        private async Task<List<ArmazemViewModel>> ListarArmazens()
        {
            var armazens = await _armazemBusiness.ListAll();

            return armazens.ToList();
        }

        //ITENS
        #region ITENS
        [HttpGet]
        public async Task<JsonResult> CarregarDescarga(long talie)
        {
            var descargas = await _talieBusiness.ListarDescargas(talie);
            return Json(descargas.Result, JsonRequestBehavior.AllowGet);
        }

        //Observacao
        [HttpPost]
        public JsonResult GravarObservacao(string observacao, long talie)
        {
            _registroBusiness.GravarObservacao(observacao, talie);
            return Json("Observação Registrada com sucesso!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditarItemModal(int id)
        {
            // Simule a obtenção de dados do banco para edição
            var model = _talieBusiness.BuscarTalieItem(id); //TODO - verificar se esta trazendo o item da TB_TALIE_ITEM

            if (model == null)
                return HttpNotFound("Item não encontrado.");

            return Json(model.Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarAlteracoesItem(TalieItemViewModel itemAlterado)
        {
            try
            {
                if (itemAlterado.QuantidadeDescarga <= 0)
                {
                    return Json(new { sucesso = false, mensagem = "Quantidade deve ser maior que zero." });
                }

                var itemOriginal = _talieBusiness.BuscarTalieItem(itemAlterado.Id);

                if (itemOriginal == null)
                {
                    return Json(new { sucesso = false, mensagem = "Item não encontrado." });
                }

                if (itemAlterado.Quantidade > itemOriginal.Result.Quantidade)
                {
                    return Json(new { sucesso = false, mensagem = "A quantidade deve ser menor que a original." });
                }

                // Atualizar o registro original com a quantidade restante
                if (itemOriginal.Result.Quantidade != itemAlterado.QuantidadeDescarga)
                {
                    itemOriginal.Result.Quantidade -= itemAlterado.Quantidade;
                    _talieBusiness.UpdateTalieItem(itemOriginal.Result);

                    // Inserir o novo item com a nova quantidade
                    var novoItem = new TalieItemViewModel
                    {
                        Id = itemOriginal.Result.Id,
                        Quantidade = itemAlterado.Quantidade,
                        NotaFiscal = itemAlterado.NotaFiscal,
                        Embalagem = itemAlterado.Embalagem,
                        Peso = itemAlterado.Peso,
                        Comprimento = itemAlterado.Comprimento,
                        // Adicionar outros campos relevantes
                    };
                }
                else
                {
                    var result =  _talieBusiness.UpdateTalieItem(itemAlterado).Result;
                    if (!result.Status)
                    {
                        return Json(new { sucesso = true, mensagem = "Falha ao executar a operação, tente novamente!" });
                    }
                }
                return Json(new { sucesso = true, mensagem = "Alterações salvas com sucesso!" });

            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = "Erro ao salvar alterações: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> RecarregarTabelaItens(int talieId)
        {
            // Simule a obtenção de dados do banco para edição
            var model = await _talieBusiness.BuscarItensDoTalie(talieId);

            return Json(model.Result, JsonRequestBehavior.AllowGet);
        }

        #endregion ITENS

    }
}
