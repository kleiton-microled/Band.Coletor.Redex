using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Band.Coletor.Redex.Site.Extensions;
using Band.Coletor.Redex.Business.DTO;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class MovimentacaoContainerController : DefaultController
    {
        private readonly IMovimentacaoContainerRepositorio _movimentacaoContainerRepositorio;
        private readonly IMotivosRepositorio _motivosRepositorio;
        private readonly IArmazensRepositorio _armazensRepositorio;
        private readonly IVeiculosRepositorio _veiculosRepositorio;
        private readonly IAutorizaSaidaRepository _autorizaSaidaRepository;

        public MovimentacaoContainerController(IMovimentacaoContainerRepositorio movimentacaoContainerRepositorio, IMotivosRepositorio motivosRepositorio, IArmazensRepositorio armazensRepositorio, IVeiculosRepositorio veiculosRepositorio, IAutorizaSaidaRepository autorizaSaidaRepository)
        {
            _movimentacaoContainerRepositorio = movimentacaoContainerRepositorio;
            _motivosRepositorio = motivosRepositorio;
            _armazensRepositorio = armazensRepositorio;
            _veiculosRepositorio = veiculosRepositorio;
            _autorizaSaidaRepository = autorizaSaidaRepository;
        }

        public ActionResult Index()
        {
            int id = User.ObterId();

            if (id == 0)
                return RedirectToAction("Login", "Home");

            int patio = User.ObterPatioColetorId();

            ViewBag.Motivos = _motivosRepositorio.GetComboMotivos();
            ViewBag.Armazens = _armazensRepositorio.GetComboArmazens(patio);
            ViewBag.CarregarVeiculos = _veiculosRepositorio.GetCarregarVeiculos();
            ViewBag.DescarregarVeiculos = _veiculosRepositorio.GetDescarregarVeiculos();

            return View();
        }


        public JsonResult GetBuscaCntr(MovimentacaoContainerDTO movimentacaoContainerDTO)
        {
            try
            {
                movimentacaoContainerDTO.PATIO = User.ObterPatioColetorId();

                if (movimentacaoContainerDTO.AUTONUM_CNTR != 0 && string.IsNullOrEmpty(movimentacaoContainerDTO.ID_CONTEINER))
                {

                    if (movimentacaoContainerDTO.ID_CONTEINER.Replace("-", "").Length != 4)
                    {
                        retornoJson.Mensagem = messages.wrongCntr();
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = codes.emptyInput();

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }


                    var countCntr = _movimentacaoContainerRepositorio.countCntr(movimentacaoContainerDTO.PATIO, movimentacaoContainerDTO.ID_CONTEINER);

                    if (countCntr != null)
                    {
                        if (countCntr.CCNTR == 0)
                        {
                            retornoJson.Mensagem = messages.noCntr();
                            retornoJson.objetoRetorno = null;
                            retornoJson.possuiDados = false;
                            retornoJson.statusRetorno = codes.emptyInput();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }

                        if (countCntr.CCNTR > 1)
                        {
                            retornoJson.Mensagem = messages.moreCntr();
                            retornoJson.objetoRetorno = null;
                            retornoJson.possuiDados = false;
                            retornoJson.statusRetorno = codes.emptyInput();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }

                        if (countCntr.CCNTR > 1)
                        {
                            retornoJson.Mensagem = messages.moreCntr();
                            retornoJson.objetoRetorno = null;
                            retornoJson.possuiDados = false;
                            retornoJson.statusRetorno = codes.emptyInput();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }

                        if (countCntr.CCNTR == 1)
                        {
                            int flag_truck = _movimentacaoContainerRepositorio.GetFlagTruckMovColetor(movimentacaoContainerDTO.PATIO);


                            var dadosCntr = _movimentacaoContainerRepositorio.GetDadosCntr(movimentacaoContainerDTO.ID_CONTEINER, movimentacaoContainerDTO.PATIO);

                            dadosCntr.CAMERA = _movimentacaoContainerRepositorio.GetQualCam(movimentacaoContainerDTO.PATIO);


                            retornoJson.Mensagem = string.Empty;
                            retornoJson.objetoRetorno = dadosCntr;
                            retornoJson.possuiDados = true;
                            retornoJson.statusRetorno = codes.Ok();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else if (movimentacaoContainerDTO.AUTONUM_CNTR == 0 && !string.IsNullOrEmpty(movimentacaoContainerDTO.ID_CONTEINER))
                {
                    var countCntr = _movimentacaoContainerRepositorio.countCntr(movimentacaoContainerDTO.PATIO, movimentacaoContainerDTO.ID_CONTEINER);

                    if (countCntr != null)
                    {
                        if (countCntr.CCNTR == 0)
                        {
                            retornoJson.Mensagem = messages.noCntr2();
                            retornoJson.objetoRetorno = null;
                            retornoJson.possuiDados = false;
                            retornoJson.statusRetorno = codes.emptyInput();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }

                        if (countCntr.CCNTR == 1)
                        {
                            int flag_truck = _movimentacaoContainerRepositorio.GetFlagTruckMovColetor(movimentacaoContainerDTO.PATIO);


                            var dadosCntr = _movimentacaoContainerRepositorio.GetDadosCntr(countCntr.ID_CONTEINER, movimentacaoContainerDTO.PATIO);

                            dadosCntr.CAMERA = _movimentacaoContainerRepositorio.GetQualCam(movimentacaoContainerDTO.PATIO);

                            retornoJson.Mensagem = string.Empty;
                            retornoJson.objetoRetorno = dadosCntr;
                            retornoJson.possuiDados = true;
                            retornoJson.statusRetorno = codes.Ok();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }

                        if (countCntr.CCNTR > 1)
                        {
                            retornoJson.Mensagem = "Mais de um conteiner com este final, digite a sigla completa";
                            retornoJson.objetoRetorno = null;
                            retornoJson.possuiDados = true;
                            retornoJson.statusRetorno = codes.Ok();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                retornoJson.Mensagem = string.Empty;
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.notFound();

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = messages.loadDataFailed();
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest() + ex.Message;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInserir(MovimentacaoContainerDTO movimentacaoContainerDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(movimentacaoContainerDTO.YARD_DESTINO))
                {
                    retornoJson.Mensagem = messages.EqualsYard();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }



                if (movimentacaoContainerDTO.ID_MOTIVO == 0)
                {
                    retornoJson.Mensagem = messages.motivoMovimento();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                bool temcaminhao = false;
                string placa = "";


                if (movimentacaoContainerDTO.YARD_DESTINO.Length == 7 && movimentacaoContainerDTO.ID_MOTIVO == 8)
                {
                    placa = movimentacaoContainerDTO.YARD_DESTINO.Substring(0, 3) + "-" + movimentacaoContainerDTO.YARD_DESTINO.Substring(3, 4);


                }
                else
                {
                    placa = movimentacaoContainerDTO.YARD_DESTINO;
                }


                int gate = _autorizaSaidaRepository.GetAutonumGate(placa);

                if (gate != 0)
                {

                    movimentacaoContainerDTO.YARD_DESTINO = "SAIDA";
                    temcaminhao = true;

                }

                //Saida
                if (movimentacaoContainerDTO.YARD_DESTINO == "SAIDA")
                {
                    int cAutorizaSaida = _autorizaSaidaRepository.Verifica_Saida(movimentacaoContainerDTO.ID_CONTEINER);

                    if (cAutorizaSaida == 0)
                    {
                        retornoJson.Mensagem = messages.NoAuthorize();
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = codes.notFound();

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                }




                var cntr = new ConteinerDTO();

                cntr.PATIO = User.ObterPatioColetorId();
                cntr.YARD = movimentacaoContainerDTO.YARD;
                cntr.TAMANHO = movimentacaoContainerDTO.TAMANHO;
                cntr.Id = movimentacaoContainerDTO.AUTONUM_CNTR;

                string ret = string.Empty;

                //if (!string.IsNullOrEmpty(movimentacaoContainerDTO.IMO1.Trim()))
                //{
                //     ret = _movimentacaoContainerRepositorio.Valida_Aloca_Imo(cntr); 

                //    if (ret != "")
                //    {
                //        retornoJson.Mensagem = ret.ToString();
                //        retornoJson.objetoRetorno = null;
                //        retornoJson.possuiDados = false;
                //        retornoJson.statusRetorno = codes.BadRequest();

                //        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //    }
                //}
                //else
                //{
                //    ret = _movimentacaoContainerRepositorio.Valida_NImo(cntr);

                //    if (ret != "")
                //    {
                //        retornoJson.Mensagem = ret.ToString();
                //        retornoJson.objetoRetorno = null;
                //        retornoJson.possuiDados = false;
                //        retornoJson.statusRetorno = codes.BadRequest();

                //        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //    }
                //}

                int valida = _movimentacaoContainerRepositorio.GetCampoValida(movimentacaoContainerDTO.PATIO, movimentacaoContainerDTO.YARD);
                bool valida_b = false;

                string mensgPatio = string.Empty;

                if (valida == 1)
                {
                    valida_b = true;
                }

                //if (!valida_b)
                //{
                //    mensgPatio = _movimentacaoContainerRepositorio.ValidaRegrasPatio(movimentacaoContainerDTO.AUTONUM_CNTR.ToString(), movimentacaoContainerDTO.YARD);

                //    if (mensgPatio != "OK")
                //    {
                //        retornoJson.Mensagem = "Conflito com Regra de Pátio :" + mensgPatio;
                //        retornoJson.objetoRetorno = null;
                //        retornoJson.possuiDados = false;
                //        retornoJson.statusRetorno = codes.BadRequest();

                //        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //    }
                //}
                //else 
                //{
                //    string pilha1 = string.Empty;
                //    string pilha2 = string.Empty; 

                //    if (movimentacaoContainerDTO.TAMANHO == 20)
                //    {
                //        pilha1 = movimentacaoContainerDTO.YARD.ToUpper().Substring(0, 5);

                //        mensgPatio = _movimentacaoContainerRepositorio.ValidaRegrasPatio(movimentacaoContainerDTO.AUTONUM_CNTR.ToString(), pilha1);

                //        if (mensgPatio != "OK")
                //        {
                //            retornoJson.Mensagem = "Conflito com Regra de Pátio :" + mensgPatio;
                //            retornoJson.objetoRetorno = null;
                //            retornoJson.possuiDados = false;
                //            retornoJson.statusRetorno = codes.BadRequest();

                //            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //        }
                //    }
                //    else
                //    {
                //        pilha1 = movimentacaoContainerDTO.YARD.ToUpper().Substring(0, 1) + Convert.ToDecimal(movimentacaoContainerDTO.YARD.ToUpper().Substring(2, 1) + 1) + movimentacaoContainerDTO.YARD.ToUpper().Substring(4, 1);

                //        mensgPatio = _movimentacaoContainerRepositorio.ValidaRegrasPatio(movimentacaoContainerDTO.AUTONUM_CNTR.ToString(), pilha1);

                //        if (mensgPatio != "OK")
                //        {
                //            retornoJson.Mensagem = "Conflito com Regra de Pátio :" + mensgPatio;
                //            retornoJson.objetoRetorno = null;
                //            retornoJson.possuiDados = false;
                //            retornoJson.statusRetorno = codes.BadRequest();

                //            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //        }

                //        pilha2 = movimentacaoContainerDTO.YARD.ToUpper().Substring(0, 1) + Convert.ToDecimal(movimentacaoContainerDTO.YARD.ToUpper().Substring(2, 1) + 1) + movimentacaoContainerDTO.YARD.ToUpper().Substring(4, 1);

                //        mensgPatio = _movimentacaoContainerRepositorio.ValidaRegrasPatio(movimentacaoContainerDTO.AUTONUM_CNTR.ToString(), pilha1);

                //        if (mensgPatio != "OK")
                //        {
                //            retornoJson.Mensagem = "Conflito com Regra de Pátio :" + mensgPatio;
                //            retornoJson.objetoRetorno = null;
                //            retornoJson.possuiDados = false;
                //            retornoJson.statusRetorno = codes.BadRequest();

                //            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //        }
                //    }
                //}

                movimentacaoContainerDTO.PATIO = User.ObterPatioColetorId();
                movimentacaoContainerDTO.ID_USUARIO = User.ObterId();
                movimentacaoContainerDTO.AUTONUM_CNTR = cntr.Id;


                string ok = _movimentacaoContainerRepositorio.GetInserir(movimentacaoContainerDTO);

                if (ok != string.Empty)
                {

                    if (ok.Contains(codes.retOK()) || ok.Contains(codes.retAttOK()))
                    {
                        if (ok == codes.retOK() || ok == codes.retAttOK())
                        {
                            if (temcaminhao == true)
                            {
                                GetAutorizaSaidaGate(placa, movimentacaoContainerDTO.ID_CONTEINER);

                                retornoJson.Mensagem = ok;
                                retornoJson.objetoRetorno = new { saida = 0 };
                                retornoJson.possuiDados = false;
                                retornoJson.statusRetorno = codes.Ok();

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }

                            if (movimentacaoContainerDTO.YARD_DESTINO == "SAIDA")
                            {
                                retornoJson.Mensagem = ok;
                                retornoJson.objetoRetorno = new { saida = 0 };
                                retornoJson.possuiDados = false;
                                retornoJson.statusRetorno = codes.Ok();
                                return Json(retornoJson, JsonRequestBehavior.AllowGet);

                            }

                            retornoJson.Mensagem = ok;
                            retornoJson.objetoRetorno = new { saida = 0 };
                            retornoJson.possuiDados = false;
                            retornoJson.statusRetorno = codes.Ok();

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            retornoJson.Mensagem = ok;
                            retornoJson.objetoRetorno = new { saida = 0 };
                            retornoJson.possuiDados = false;
                            retornoJson.statusRetorno = codes.BadRequest();


                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        retornoJson.Mensagem = ok;
                        retornoJson.objetoRetorno = new { saida = 0 };
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = codes.BadRequest();


                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                }



                retornoJson.Mensagem = messages.inserirFailed();
                retornoJson.objetoRetorno = new { id = 0 };
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest();


                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = messages.inserirFailed();
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest() + ex.Message;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInfoLote(string id_conteiner)
        {
            try
            {
                if (string.IsNullOrEmpty(id_conteiner))
                {
                    retornoJson.Mensagem = messages.loadDataFailed();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.BadRequest();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                var infoLote = _movimentacaoContainerRepositorio.getInfoLote(id_conteiner);

                if (infoLote == null)
                {
                    retornoJson.Mensagem = messages.notFound();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.BadRequest();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                retornoJson.Mensagem = string.Empty;
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.Ok();

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = messages.loadDataFailed();
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest() + ex.Message;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetVerificaVeiculoSaidaCntr(string id_conteiner)
        {
            try
            {
                if (string.IsNullOrEmpty(id_conteiner))
                {
                    retornoJson.Mensagem = messages.NocntrName();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.notFound();


                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int cAutorizaSaida = _autorizaSaidaRepository.Verifica_Saida(id_conteiner);


                if (cAutorizaSaida == 0)
                {
                    retornoJson.Mensagem = messages.NoAuthorize();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.notFound();


                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                retornoJson.Mensagem = string.Empty;
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.Ok();


                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Conteiner não verificado";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest();


                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAutorizaSaidaGate(string placa, string cntr)
        {
            try
            {
                if (string.IsNullOrEmpty(placa))
                {
                    retornoJson.Mensagem = "Uma Placa deve ser informada";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.notFound();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int gate = _autorizaSaidaRepository.GetAutonumGate(placa);

                if (gate == 0)
                {
                    retornoJson.Mensagem = "Não foi encontrada entrada do veiculo vazio";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.notFound();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                var query_ro = _autorizaSaidaRepository.GetDadosVerificaSaidaFD(cntr);

                if (query_ro.autonum_ro == 0)
                {
                    retornoJson.Mensagem = "Carregamento/Estufagem nao encontrado";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.notFound();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int autonum_ro = query_ro.autonum_ro;

                var romaneio_by_id = _autorizaSaidaRepository.GetDadosRomaneioById(autonum_ro);

                int autonum_patio = romaneio_by_id.autonum_patio;

                //var query_autonum_reg = _autorizaSaidaRepository.GetAutonumReg(autonum_patio);
                var query_autonum_reg = _autorizaSaidaRepository.GetAutonumReg(autonum_ro);

                if (query_autonum_reg == null)
                {
                    retornoJson.Mensagem = "Nao consta registro de retirada para esta unidade";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.notFound();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int autonum_reg = query_autonum_reg.autonum_reg;

                var query_fe_ie = _autorizaSaidaRepository.GetDadosVerificaSaida(gate);

                _autorizaSaidaRepository.UpdatePatioGateById(gate, autonum_patio);

                _autorizaSaidaRepository.UpdateSaidaCargaByRomaneioId(autonum_ro);

                _autorizaSaidaRepository.UpdateRomaneioById(autonum_ro);


                var gateNewById = _autorizaSaidaRepository.GetDadosGateNewById(gate);

                if (gateNewById != null)
                {
                    var gateDTO = new GateDTO();

                    gateDTO.PLACA = gateNewById.PLACA;
                    gateDTO.CARRETA = gateNewById.CARRETA;
                    gateDTO.Id_Motorista = gateNewById.Id_Motorista;
                    gateDTO.ID_TRANSPORTADORA = gateNewById.ID_TRANSPORTADORA;
                    //gateDTO.AUTONUM_GATE = gateNewById.AUTONUM_GATE;
                    gateDTO.AUTONUM_GATE = gateNewById.AUTONUM;
                    gateDTO.TARA = gateNewById.TARA;
                    gateDTO.AUTONUM_REG = query_autonum_reg.autonum_reg;
                    gateDTO.UsuarioId = User.ObterId();
                    gateDTO.Peso_Entrada = gateNewById.TARA;
                    gateDTO.Peso_Saida = gateNewById.BRUTO;
                    gateDTO.AUTONUM = gate;

                    _autorizaSaidaRepository.UpdateDadosTbRegistro(gateDTO);


                    retornoJson.Mensagem = "Placa " + placa + " vinculada com sucesso ao conteiner " + cntr;
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.Ok();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                //Liberacao saida foi criada a Controller de mesmo nome onde irá ficava as rotinas Sai_Salva_204 e Sai_Salva_207 inicialmente que foram para outra tela                

                retornoJson.Mensagem = "Não foi possível gerar gate out automático. Favor verificar com TI ou solicitar saida manual pelo gate";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.notFound();

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Não foi possível gerar gate out automático. Favor verificar com TI ou solicitar saida manual pelo gate  ERRO: " + ex.Message.ToString();
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest();


                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
    }
}