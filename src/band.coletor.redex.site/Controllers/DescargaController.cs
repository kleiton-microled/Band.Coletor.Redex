using Band.Coletor.Redex.Business.Enums;
using Band.Coletor.Redex.Business.Extensions;
using Band.Coletor.Redex.Business.Helpers;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Site.Extensions;
using Band.Coletor.Redex.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class DescargaController : BaseController
    {
        private readonly ITalieRepositorio _talieRepositorio;
        private readonly IRegistroRepositorio _registroRepositorio;
        private readonly IReservaRepositorio _reservaRepositorio;

        public DescargaController(ITalieRepositorio talieRepositorio, IRegistroRepositorio registroRepositorio, IReservaRepositorio reservaRepositorio)
        {
            _talieRepositorio = talieRepositorio;
            _registroRepositorio = registroRepositorio;
            _reservaRepositorio = reservaRepositorio;
        }

        [HttpGet]
        public ActionResult Registro(int? id)
        {
            var conferentes = _talieRepositorio.ObterConferentes(User.ObterId());

            var equipes = _talieRepositorio.ObterEquipes().ToList();

            var operacoes = _talieRepositorio.ObterOperacoes().ToList();

            var tipoDescarga = Enum.GetValues(typeof(TipoDescarga)).Cast<TipoDescarga>();

            if (id.HasValue)
            {
                var registroBusca = _registroRepositorio.ObterRegistroPorLote(id.Value);
                var conteineres = _talieRepositorio.ObterTaliesConteinersPorId(id.Value).ToList();

                return View(new RegistroViewModel
                {
                    GateId = registroBusca.GateId,
                    Placa = registroBusca.Placa,
                    ExportadorId = registroBusca.ExportadorId,
                    Exportador = registroBusca.Exportador,
                    Referencia = registroBusca.Referencia,
                    Reserva = registroBusca.Reserva,
                    Lote = registroBusca.Lote,
                    Patio = registroBusca.Patio,
                    Conferentes = conferentes,
                    Equipes = equipes,
                    Operacoes = operacoes,
                    Talies = conteineres

                });
            }

            return View(new RegistroViewModel
            {

                Conferentes = conferentes,
                Equipes = equipes,
                Operacoes = operacoes

            });
        }

        [HttpGet]
        public ActionResult RegistroOld(int? id)
        {
           
            var conferentes = _talieRepositorio.ObterConferentes(User.ObterId());

            var equipes = _talieRepositorio.ObterEquipes().ToList();

            var operacoes = _talieRepositorio.ObterOperacoes().ToList();

            if (id != null)
            {
                var talieBusca = _talieRepositorio.ObterTaliePorId(id.Value);
                var talies = _talieRepositorio.ObterTaliesConteinersPorId(id.Value).ToList();


                if (talieBusca == null)
                    throw new Exception("Talie não encontrado");

                if (talieBusca.EquipeId == 0)
                    talieBusca.EquipeId = ObterCodigoEquipeDefault(talieBusca);

                if (talieBusca.OperacaoId == null || talieBusca.OperacaoId != "M")
                    talieBusca.OperacaoId = "A";

                if (talieBusca.ConferenteId == 0)
                    talieBusca.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);

                return View(new TalieViewModel
                {
                    TalieNumero = talieBusca.Id,
                    Conteiners = talies,
                    TalieId = talieBusca.Id,
                    Registro = talieBusca.RegistroId.ToString(),
                    Inicio = talieBusca.Inicio,
                    Termino = talieBusca.Termino,
                    Placa = talieBusca.Placa,
                    BookingId = talieBusca.BookingId,
                    GateId = talieBusca.GateId,
                    Observacoes = talieBusca.Observacoes,
                    CrossDocking = talieBusca.CrossDocking,
                    Conferentes = conferentes,
                    Equipes = equipes,
                    Operacoes = operacoes,
                    ConferenteId = talieBusca.ConferenteId,
                    EquipeId = talieBusca.EquipeId,
                    OperacaoId = talieBusca.OperacaoId
                });
            }

            return View(new TalieViewModel
            {
                Conferentes = conferentes,
                Equipes = equipes,
                Operacoes = operacoes
            });
        }

        [HttpGet]
        public ActionResult RegistroItem(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");

            var talieBusca = _talieRepositorio.ObterTaliePorId(id.Value);

            if (talieBusca == null)
                throw new Exception("Talie não encontrado");

            var itens = _talieRepositorio.ObterItens(talieBusca.Id).ToList();

            var NFs = _talieRepositorio.ObterNFs(talieBusca.Id).ToList();

            foreach(var nf in NFs)
            {
                nf.Itens = _talieRepositorio.ObterItensPorNF(nf.Id).ToList();
            }



            return View(new TalieItemViewModel
            {
                TalieId = talieBusca.Id,
                RegistroId = talieBusca.RegistroId,
                BookingId = talieBusca.BookingId,
                Patio = talieBusca.Patio,
                PatioId = talieBusca.Patio,
                ConteinerId = talieBusca.ConteinerId,
                Itens = itens,
                NFs = NFs.ToList(),
                
            });
        }

        [HttpGet]
        public ActionResult ObterDadosNF(int idNF)
        {
            var dadosNF = _talieRepositorio.ObterNFPorId(idNF);

            if (dadosNF == null)
                return RetornarErro("Dados da nota não localizados");


            dadosNF.Itens = _talieRepositorio.ObterItensPorNF(dadosNF.Id).ToList();
            var produtos = _talieRepositorio.ObterProdutosPorNF(dadosNF.Id).ToList();
            var embalagens = _talieRepositorio.ObterEmbalagensPorNF(dadosNF.Id).ToList();
          
            //return View(new TalieItemViewModel
            //{
            //    Produtos = produtos,
            //    Embalagens = embalagens,
            //    QuantidadeNF = dadosNF.QuantidadeItens
            //});

            return Json(new
            {
                produtos,
                embalagens,
                dadosNF.QuantidadeItens
            }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult ObterConteinersPorRegistro(int lote)
        {
            var registroBusca = _registroRepositorio.ObterRegistroPorLote(lote);
            var conteineres = _talieRepositorio.ObterTaliesConteinersPorId(lote).ToList();
            var conferentes = _talieRepositorio.ObterConferentes(User.ObterId());
            var equipes = _talieRepositorio.ObterEquipes().ToList();
            var operacoes = _talieRepositorio.ObterOperacoes().ToList();

            if (conteineres.Count > 0)
            {
                return Json(new
                {
                    conteineres,
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {

                     registroBusca.GateId,
                     registroBusca.Placa,
                     registroBusca.ExportadorId,
                     registroBusca.Exportador,
                     registroBusca.Referencia,
                     registroBusca.Reserva,
                     registroBusca.Lote,
                     registroBusca.Patio,
                     Inicio = registroBusca.Inicio.DataHoraFormatada(),
                     conferentes,
                     equipes,
                     operacoes,
                }, JsonRequestBehavior.AllowGet);
            }

            return View(new RegistroViewModel
            {
                GateId = registroBusca.GateId,
                Placa = registroBusca.Placa,
                ExportadorId = registroBusca.ExportadorId,
                Exportador = registroBusca.Exportador,
                Referencia = registroBusca.Referencia,
                Reserva = registroBusca.Reserva,
                Lote = registroBusca.Lote,
                Patio = registroBusca.Patio,
                Conferentes = conferentes,
                Equipes = equipes,
                Operacoes = operacoes,
                Talies = conteineres

            });
            //if (dadosTalie != null)
            //{
            //    if (dadosTalie.ConferenteId == 0)
            //        dadosTalie.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);

            //    if (dadosTalie.EquipeId == 0)
            //        dadosTalie.EquipeId = ObterCodigoEquipeDefault(dadosTalie);

            //    if (dadosTalie.OperacaoId == null || dadosTalie.OperacaoId != "M")
            //        dadosTalie.OperacaoId = "A";


            //    return Json(new
            //    {
            //        dadosTalie.Id,
            //        dadosTalie.RegistroId,
            //        dadosTalie.PatioId,
            //        dadosTalie.Placa,
            //        dadosTalie.Descarga,
            //        dadosTalie.Estufagem,
            //        dadosTalie.CrossDocking,
            //        dadosTalie.Carregamento,
            //        dadosTalie.Fechado,
            //        dadosTalie.Pacotes,
            //        dadosTalie.Completo,
            //        dadosTalie.EmailEnviado,
            //        dadosTalie.Reference,
            //        dadosTalie.Cliente,
            //        dadosTalie.ConferenteId,
            //        dadosTalie.EquipeId,
            //        dadosTalie.BookingId,
            //        dadosTalie.OperacaoId,
            //        dadosTalie.GateId,
            //        dadosTalie.Observacoes,
            //        dadosTalie.RomaneiroId,
            //        dadosTalie.Audit225,
            //        dadosTalie.AnoTermio,
            //        dadosTalie.Termo,
            //        dadosTalie.AlertaEtiqueta,
            //        Inicio = dadosTalie.Inicio.DataHoraFormatada(),
            //        Termino = dadosTalie.Termino.DataHoraFormatada(),
            //        DataTermo = dadosTalie.DataTermo.DataFormatada(),
            //    }, JsonRequestBehavior.AllowGet);
            //}

            //var dadosRegistro = _talieRepositorio.ObterDadosRegistro(registro);

            //if (dadosRegistro == null)
            //{
            //    return RetornarErro("Registro não encontrado");
            //}

            //if (!string.IsNullOrEmpty(inicio))
            //{
            //    if (!DateTimeHelpers.IsDate(inicio))
            //        return RetornarErro("Data de início inválida");

            //    dadosRegistro.Inicio = Convert.ToDateTime(inicio);
            //}
            //else
            //{
            //    dadosRegistro.Inicio = DateTime.Now;
            //}

            //if (dadosRegistro == null)
            //    return RetornarErro("Gate In não localizado");

            //dadosRegistro.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);
            //dadosRegistro.EquipeId = ObterCodigoEquipeDefault(dadosTalie);

            //return Json(new
            //{
            //    Registro = registro,
            //    dadosRegistro.DataGateIn,
            //    dadosRegistro.BookingId,
            //    dadosRegistro.Cliente,
            //    dadosRegistro.ClienteId,
            //    dadosRegistro.Placa,
            //    dadosRegistro.Reference,
            //    dadosRegistro.ConferenteId,
            //    dadosRegistro.EquipeId,
            //    dadosRegistro.GateId,
            //    Inicio = dadosRegistro.Inicio.DataHoraFormatada()
            //}, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult ObterDadosRegistroTalie(int lote, int talie)
        //{
        //    var dadosTalie = _talieRepositorio.ObterDadosTaliePorId(talie);
        //    if (dadosTalie != null)
        //    {
        //        if (dadosTalie.ConferenteId == 0)
        //            dadosTalie.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);

        //        if (dadosTalie.EquipeId == 0)
        //            dadosTalie.EquipeId = ObterCodigoEquipeDefault(dadosTalie);

        //        if (dadosTalie.OperacaoId == null || dadosTalie.OperacaoId != "M")
        //            dadosTalie.OperacaoId = "A";


        //        return Json(new
        //        {
        //            dadosTalie.Id,
        //            dadosTalie.RegistroId,
        //            dadosTalie.PatioId,
        //            dadosTalie.Placa,
        //            dadosTalie.Descarga,
        //            dadosTalie.Estufagem,
        //            dadosTalie.CrossDocking,
        //            dadosTalie.Carregamento,
        //            dadosTalie.Fechado,
        //            dadosTalie.Pacotes,
        //            dadosTalie.Completo,
        //            dadosTalie.EmailEnviado,
        //            dadosTalie.Reference,
        //            dadosTalie.Cliente,
        //            dadosTalie.ConferenteId,
        //            dadosTalie.EquipeId,
        //            dadosTalie.BookingId,
        //            dadosTalie.OperacaoId,
        //            dadosTalie.GateId,
        //            dadosTalie.Observacoes,
        //            dadosTalie.RomaneiroId,
        //            dadosTalie.Audit225,
        //            dadosTalie.AnoTermio,
        //            dadosTalie.Termo,
        //            dadosTalie.AlertaEtiqueta,
        //            Inicio = dadosTalie.Inicio.DataHoraFormatada(),
        //            Termino = dadosTalie.Termino.DataHoraFormatada(),
        //            DataTermo = dadosTalie.DataTermo.DataFormatada(),
        //        }, JsonRequestBehavior.AllowGet);
        //    }

        //    var dadosRegistro = _talieRepositorio.ObterDadosRegistro(lote);

        //    if (dadosRegistro == null)
        //    {
        //        return RetornarErro("Registro não encontrado");
        //    }



        //    if (dadosRegistro == null)
        //        return RetornarErro("Gate In não localizado");

        //    dadosRegistro.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);
        //    dadosRegistro.EquipeId = ObterCodigoEquipeDefault(dadosTalie);

        //    return Json(new
        //    {
        //        Registro = lote,
        //        dadosRegistro.DataGateIn,
        //        dadosRegistro.BookingId,
        //        dadosRegistro.Cliente,
        //        dadosRegistro.ClienteId,
        //        dadosRegistro.Placa,
        //        dadosRegistro.Reference,
        //        dadosRegistro.ConferenteId,
        //        dadosRegistro.EquipeId,
        //        dadosRegistro.GateId,
        //        Inicio = dadosRegistro.Inicio.DataHoraFormatada()
        //    }, JsonRequestBehavior.AllowGet);
        //}

    
        [HttpGet]
        public ActionResult ObterDadosTaliePorRegistro(int registro, string inicio)
        {
            var dadosTalie = _talieRepositorio.ObterDadosTaliePorRegistro(registro);
            var talies = _talieRepositorio.ObterTaliesConteinersPorId(registro).ToList();

            if (dadosTalie != null)
            {
                if (dadosTalie.ConferenteId == 0)
                    dadosTalie.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);

                if (dadosTalie.EquipeId == 0)
                    dadosTalie.EquipeId = ObterCodigoEquipeDefault(dadosTalie);

                if (dadosTalie.OperacaoId == null || dadosTalie.OperacaoId != "M")
                    dadosTalie.OperacaoId = "A";


                return Json(new
                {
                    dadosTalie.Id,
                    dadosTalie.RegistroId,
                    //dadosTalie.PatioId,
                    //dadosTalie.Placa,
                    //dadosTalie.Descarga,
                    //dadosTalie.Estufagem,
                    //dadosTalie.CrossDocking,
                    //dadosTalie.Carregamento,
                    //dadosTalie.Fechado,
                    //dadosTalie.Pacotes,
                    //dadosTalie.Completo,
                    //dadosTalie.EmailEnviado,
                    //dadosTalie.Reference,
                    //dadosTalie.Cliente,
                    //dadosTalie.ConferenteId,
                    //dadosTalie.EquipeId,
                    //dadosTalie.BookingId,
                    //dadosTalie.OperacaoId,
                    //dadosTalie.GateId,
                    //dadosTalie.Observacoes,
                    //dadosTalie.RomaneiroId,
                    //dadosTalie.Audit225,
                    //dadosTalie.AnoTermio,
                    //dadosTalie.Termo,
                    //dadosTalie.AlertaEtiqueta,
                    //Inicio = dadosTalie.Inicio.DataHoraFormatada(),
                    //Termino = dadosTalie.Termino.DataHoraFormatada(),
                    //DataTermo = dadosTalie.DataTermo.DataFormatada(),
                    talies
                }, JsonRequestBehavior.AllowGet);
            }

            var dadosRegistro = _talieRepositorio.ObterDadosRegistro(registro);

            if (dadosRegistro == null)
            {
                return RetornarErro("Registro não encontrado");
            }

            if (!string.IsNullOrEmpty(inicio))
            {
                if (!DateTimeHelpers.IsDate(inicio))
                    return RetornarErro("Data de início inválida");

                dadosRegistro.Inicio = Convert.ToDateTime(inicio);
            }
            else
            {
                dadosRegistro.Inicio = DateTime.Now;
            }

            if (dadosRegistro == null)
                return RetornarErro("Gate In não localizado");

            dadosRegistro.ConferenteId = _talieRepositorio.ObterCodigoConferentePorLogin(201);
            dadosRegistro.EquipeId = ObterCodigoEquipeDefault(dadosTalie);

            return Json(new
            {
                Registro = registro,
                //dadosRegistro.DataGateIn,
                //dadosRegistro.BookingId,
                //dadosRegistro.Cliente,
                //dadosRegistro.ClienteId,
                //dadosRegistro.Placa,
                //dadosRegistro.Reference,
                dadosRegistro.ConferenteId,
                dadosRegistro.EquipeId,
                dadosRegistro.GateId,
                Inicio = dadosRegistro.Inicio.DataHoraFormatada()
            }, JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
        public ActionResult ObterDadosConteiner(string conteiner,int reserva)
        {
            var dadosConteiner = _reservaRepositorio.ObterDadosReservaPorConteiner(conteiner);
            var parceiroId = _reservaRepositorio.ObterParceiroPorIdReserva(reserva);


            if (dadosConteiner == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Contêiner não encontrado no pátio");

            if (parceiroId != dadosConteiner.ParceiroId)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Contêiner pertence a outro cliente");

            return Json(new
                {
                    dadosConteiner.PatioId,
                    dadosConteiner.ReservaId
                }, JsonRequestBehavior.AllowGet);
            
        }

        private int ObterCodigoEquipeDefault(Talie dadosTalie)
        {
            var agora = DateTime.Now;

            if (agora >= new DateTime(agora.Year, agora.Month, agora.Day, 8, 0, 0) && agora <= new DateTime(agora.Year, agora.Month, agora.Day, 15, 59, 59))
                return 62;

            if (agora >= new DateTime(agora.Year, agora.Month, agora.Day, 16, 0, 0) && agora <= new DateTime(agora.Year, agora.Month, agora.Day, 23, 59, 59))
                return 101;

            if (agora >= new DateTime(agora.Year, agora.Month, agora.Day, 0, 0, 0) && agora <= new DateTime(agora.Year, agora.Month, agora.Day, 07, 59, 59))
                return 270;

            return 0;
        }

        [HttpPost]
        public ActionResult GravarTalie([Bind(Include = "GateId,Reserva,Lote,TalieId,CrossDocking,OperacaoId,ExportadorId, Inicio, Patio, Placa, Conteiner, ConferenteId, EquipeId, Observacoes")] RegistroViewModel viewModel)
        {
            if (viewModel.TalieId==0)
            {
                var talie = new Talie(
                    viewModel.Lote,
                    viewModel.Placa,
                    viewModel.Inicio,
                    viewModel.CrossDocking,
                    viewModel.ConferenteId,
                    viewModel.EquipeId,
                    viewModel.Reserva,
                    viewModel.OperacaoId,
                    viewModel.GateId,
                    viewModel.Observacoes);

                if (!Validar(talie))
                    return RetornarErros();

                var id = _talieRepositorio.CadastrarTalie(talie);

                //return RedirectToAction(nameof(RegistroItem), new { id = id });
                return Json(new
                {
                    RedirectUrl = Url.Action(nameof(RegistroItem), new { id=id })
                });
            }
            else
            {
                var talieBusca = _talieRepositorio.ObterTaliePorId(viewModel.TalieId);

                if (talieBusca != null)
                {
                    talieBusca.Alterar(
                        new Talie
                        {
                            Inicio = viewModel.Inicio,
                            ConferenteId = viewModel.ConferenteId,
                            EquipeId = viewModel.EquipeId,
                            OperacaoId = viewModel.OperacaoId,
                            Observacoes = viewModel.Observacoes,
                            CrossDocking = viewModel.CrossDocking,
                            Id = viewModel.TalieId
                        });

                    if (!Validar(talieBusca))
                        return RetornarErros();

                    _talieRepositorio.AtualizarTalie(talieBusca);

                    return Json(new
                    {
                        RedirectUrl = Url.Action(nameof(RegistroItem), new { id = talieBusca.Id })
                    });
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult ExcluirTalie(int? id)
        {
            if (id == null)
                return RetornarErro("Talie não informado");

            var talieBusca = _talieRepositorio.ObterTaliePorId(id.Value);

            //if (talieBusca.Fechado)
            //    return RetornarErro("Talie Fechado. Operação não permitida");

            if (talieBusca != null)
            {
                _talieRepositorio.ExcluirTalie(id.Value);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public ActionResult FinalizarTalie(int? id)
        {
            if (id == null)
                return RetornarErro("Talie não informado");

            var talieBusca = _talieRepositorio.ObterTaliePorId(id.Value);

            if (talieBusca != null)
            {
                //if (talieBusca.Fechado)
                //    return RetornarErro("Já consta fechamento para este Talie");

                if (!_talieRepositorio.ExisteCargaCadastrada(talieBusca.Id))
                    return RetornarErro("Finalize o talie somente após o lançamento da descarga");

                var qtdeDescarga = _talieRepositorio.ObterQuantidadeDescarga(talieBusca.Id);

                var qtdeRegistro = _talieRepositorio.ObterQuantidadeRegistro(talieBusca.RegistroId);

                if (qtdeDescarga != qtdeRegistro)
                    return RetornarErro("Quantidade descarregada diverge da quantidade registrada");

                if (_talieRepositorio.ObrigatorioDescargaYard())
                {
                    if (_talieRepositorio.ExistemItensSemPosicao(talieBusca.Id))
                        return RetornarErro("Existem itens sem posição de armazém cadastrada");
                }

                //if (!_talieRepositorio.ExistemEtiquetas(talieBusca.Id))
                //    return RetornarErro("Não consta geração de etiquetas deste registro");

                _talieRepositorio.GerarAlertaEtiqueta(talieBusca.Id, 1);

                //if (_talieRepositorio.ExistemEtiquetasComPendencia(talieBusca.Id) || _talieRepositorio.ExistemMarcantesComPendencia(talieBusca.Id))
                //    return RetornarErro("Constam pendencias de emissão de etiquetas/marcantes deste registro");

                _talieRepositorio.GerarAlertaEtiqueta(talieBusca.Id, 2);

                var balanca = _talieRepositorio.ObterPesoBruto(talieBusca.GateId);

                try
                {
                    _talieRepositorio.FinalizarTalie(talieBusca.Id, talieBusca.Inicio.Value, balanca, talieBusca.BookingId);
                }
                catch
                {
                    return RetornarErro("Erro durante processo de Fechamento");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public ActionResult ObterItemPorNF(int registroId, string nf)
        {
            var dadosItem = _talieRepositorio.ObterItemNF(registroId, nf);

            if (dadosItem == null)
                return RetornarErro("Nota Fiscal não identificada no registro");

            return DadosItem(dadosItem);
        }

        [HttpGet]
        public ActionResult ObterItemPorId(int id)
        {
            var dadosItem = _talieRepositorio.ObterItemTaliePorId(id);

            if (dadosItem == null)
                return RetornarErro("Item não localizado");

            return DadosItem(dadosItem);
        }

        public ActionResult DadosItem(TalieItem dadosItem)
        {
            var produtos = _talieRepositorio.ObterProdutosPorNF(dadosItem.NotaFiscalId).ToList();
            var embalagens = _talieRepositorio.ObterEmbalagensPorNF(dadosItem.NotaFiscalId).ToList();
            return Json(new
            {
                dadosItem.Id,
                dadosItem.TalieId,
                dadosItem.RegistroCsId,
                dadosItem.NotaFiscalId,
                dadosItem.NotaFiscal,
                dadosItem.Quantidade,
                dadosItem.QuantidadeDescarga,
                dadosItem.Remonte,
                dadosItem.Fumigacao,
                dadosItem.EmbalagemId,
                dadosItem.EmbalagemSigla,
                dadosItem.Embalagem,
                dadosItem.IMO1,
                dadosItem.IMO2,
                dadosItem.IMO3,
                dadosItem.IMO4,
                dadosItem.UNO1,
                dadosItem.UNO2,
                dadosItem.UNO3,
                dadosItem.UNO4,
                dadosItem.Yard,
                Peso = dadosItem.Peso.ToString("n2"),
                Comprimento = dadosItem.Comprimento.GetValueOrDefault().ToNumero(),
                Largura = dadosItem.Largura.GetValueOrDefault().ToNumero(),
                Altura = dadosItem.Altura.GetValueOrDefault().ToNumero(),
                dadosItem.Fragil,
                dadosItem.Madeira,
                dadosItem.Avariado,
                Descarga = dadosItem.QuantidadeDescarga,//$"{dadosItem.QuantidadeDescarga} / {dadosItem.Quantidade}",
                FimNota = (dadosItem.QuantidadeDescarga == dadosItem.Quantidade),
                produtos,
                embalagens
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CalcularPeso(int talieId, string quantidade, string notafiscal)
        {
            var talie = _talieRepositorio.ObterTaliePorId(talieId);

            if (talie == null)
                return RetornarErro("Talie não encontrado ou já excluído");

            if (!Decimal.TryParse(quantidade, out decimal convertido))
                return RetornarErro("Quantidade inválida");

            var pesoNf = _talieRepositorio.ObterQuantidadeNF(talie.RegistroId, notafiscal);
            var resumo = _talieRepositorio.ObterResumoQuantidadeDescarga(talie.Id, notafiscal);

            if ((convertido + resumo.QuantidadeDescarga) > resumo.QuantidadeManifestada)
                return RetornarErro("A quantidade não pode ser maior que a quantidade manifestada");

            return Json(new
            {
                Peso = String.Format("{0:N3}", (pesoNf * convertido))
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObterEmbalagemPorSigla(string sigla)
        {
            var embalagem = _talieRepositorio.ObterEmbalagemPorSigla(sigla);

            return Json(new
            {
                Embalagem = embalagem
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObterItens(int talieId)
        {
            var itens = _talieRepositorio.ObterItens(talieId).ToList()
                .Select(c => new
                {
                    c.Id,
                    c.Descricao
                });

            return Json(itens, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GravarItemTalie([Bind(Include = "TalieId, TalieItemId, RegistroId, RegistroCsId,Descarga, QuantidadeDescarga, PatioId, Comprimento, Largura, Altura, Peso, Remonte, Fumigacao, Fragil, Madeira, Avariado, Yard, NFId, NotaFiscal, IMO1, IMO2, IMO3, IMO4, UNO1, UNO2, UNO3, UNO4, CodigoEmbalagem, EmbalagemId")] TalieItemViewModel viewModel)
        {
            if (viewModel.TalieItemId == 0)
            {
                var talieBusca = _talieRepositorio.ObterTaliePorId(viewModel.TalieId);

                if (talieBusca == null)
                    return RetornarErro("Talie não encontrado ou já excluído");

                var browser = Request.Browser;

                var browserInfo = new BrowserInfo
                {
                    Nome = browser.Browser,
                    Versao = browser.Version,
                    Mobile = browser.IsMobileDevice,
                    Fabricante = browser.MobileDeviceManufacturer,
                    Modelo = browser.MobileDeviceModel,
                    IP = Request.UserHostAddress
                };

                var dadosNF = _talieRepositorio.ObterNFPorId(viewModel.NFId);

                var regCS = _talieRepositorio.ObterRegCSIdPorEmbalagem(viewModel.EmbalagemId, viewModel.NFId);

                var quantidadeDescarregada = _talieRepositorio.ObterResumoQuantidadeDescarga(viewModel.TalieId, dadosNF.NumNF);

                if (quantidadeDescarregada.QuantidadeManifestada == quantidadeDescarregada.QuantidadeDescarga)
                    return RetornarErro("Todos os itens de Nota Fiscal já foram lançados");

                if (!string.IsNullOrEmpty(viewModel.IMO1))
                {
                    if (!_talieRepositorio.IMOValido(viewModel.IMO1))
                        return RetornarErro($"IMO1 {viewModel.IMO1} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.IMO2))
                {
                    if (!_talieRepositorio.IMOValido(viewModel.IMO2))
                        return RetornarErro($"IMO2 {viewModel.IMO2} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.IMO3))
                {
                    if (!_talieRepositorio.IMOValido(viewModel.IMO3))
                        return RetornarErro($"IMO3 {viewModel.IMO3} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.IMO4))
                {
                    if (!_talieRepositorio.IMOValido(viewModel.IMO4))
                        return RetornarErro($"IMO4 {viewModel.IMO4} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.UNO1))
                {
                    if (!_talieRepositorio.UNOValido(viewModel.UNO1))
                        return RetornarErro($"UNO1 {viewModel.UNO1} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.UNO2))
                {
                    if (!_talieRepositorio.UNOValido(viewModel.UNO2))
                        return RetornarErro($"UNO2 {viewModel.UNO2} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.UNO3))
                {
                    if (!_talieRepositorio.UNOValido(viewModel.UNO3))
                        return RetornarErro($"UNO3 {viewModel.UNO3} não cadastrado");
                }

                if (!string.IsNullOrEmpty(viewModel.UNO4))
                {
                    if (!_talieRepositorio.UNOValido(viewModel.UNO4))
                        return RetornarErro($"UNO4 {viewModel.UNO4} não cadastrado");
                }

                //if (talieBusca.Fechado)
                //    return RetornarErro($"Talie Fechado. Operação não permitida");

 

                var item = new TalieItem(
                    viewModel.TalieId,
                    regCS,
                    decimal.Parse( viewModel.Descarga),
                    viewModel.PatioId,
                    viewModel.Comprimento,
                    viewModel.Largura,
                    viewModel.Altura,
                    viewModel.Peso,
                    viewModel.Remonte,
                    viewModel.Fumigacao,
                    viewModel.Fragil,
                    viewModel.Madeira,
                    viewModel.Avariado,
                    viewModel.Yard,
                    viewModel.NFId,
                    dadosNF.NumNF,
                    viewModel.IMO1,
                    viewModel.IMO2,
                    viewModel.IMO3,
                    viewModel.IMO4,
                    viewModel.UNO1,
                    viewModel.UNO2,
                    viewModel.UNO3,
                    viewModel.UNO4,
                    viewModel.EmbalagemId);

                if (!Validar(item))
                    return RetornarErros();

                var resumo = _talieRepositorio.ObterResumoQuantidadeDescarga(talieBusca.Id, viewModel.NotaFiscal);

                //if ((viewModel.Quantidade + resumo.QuantidadeDescarga) > resumo.QuantidadeManifestada)
                //    return RetornarErro($"A quantidade não pode ser maior que a quantidade manifestada");

                var id = _talieRepositorio.CadastrarItemTalie(item, browserInfo);

                var resumoDescarga = _talieRepositorio.ObterResumoQuantidadeDescarga(viewModel.TalieId, viewModel.NotaFiscal);

                return Json(new
                {
                   // Descarga = $"{resumoDescarga.QuantidadeDescarga} / {resumoDescarga.QuantidadeManifestada}"
                });
            }
            else
            {
                var talieItemBusca = _talieRepositorio.ObterItemTaliePorId(viewModel.TalieItemId);

                var dadosNF = _talieRepositorio.ObterNFPorId(viewModel.NFId);

               // var regCS = _talieRepositorio.ObterRegCSIdPorEmbalagem(viewModel.EmbalagemId, viewModel.NFId);

                if (talieItemBusca != null)
                {
                    talieItemBusca.Alterar(
                        new TalieItem
                        {
                            Quantidade = viewModel.Quantidade,
                            PatioId = viewModel.PatioId,
                            Comprimento = viewModel.Comprimento,
                            Largura = viewModel.Largura,
                            Altura = viewModel.Altura,
                            Peso = viewModel.Peso,
                            Remonte = viewModel.Remonte,
                            Fumigacao = viewModel.Fumigacao,
                            Fragil = viewModel.Fragil,
                            Madeira = viewModel.Madeira,
                            Avariado = viewModel.Avariado,
                            Yard = viewModel.Yard,
                            NotaFiscalId = dadosNF.Id,
                            NotaFiscal = dadosNF.NumNF,
                            IMO1 = viewModel.IMO1,
                            IMO2 = viewModel.IMO2,
                            IMO3 = viewModel.IMO3,
                            IMO4 = viewModel.IMO4,
                            UNO1 = viewModel.UNO1,
                            UNO2 = viewModel.UNO2,
                            UNO3 = viewModel.UNO3,
                            UNO4 = viewModel.UNO4,
                            EmbalagemId = viewModel.EmbalagemId
                        });

                    if (!Validar(talieItemBusca))
                        return RetornarErros();

                    _talieRepositorio.AtualizarItemTalie(talieItemBusca);

                    var resumoDescarga = _talieRepositorio.ObterResumoQuantidadeDescarga(viewModel.TalieId, dadosNF.NumNF);

                    return Json(new
                    {
                        Descarga = $"{resumoDescarga.QuantidadeDescarga} / {resumoDescarga.QuantidadeManifestada}"
                    });
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public ActionResult ExcluirTalieItem(int? id)
        {
            if (id == null)
                return RetornarErro($"Talie não informado");

            var item = _talieRepositorio.ObterItemTaliePorId(id.Value);

            if (item == null)
                return RetornarErro($"Item não localizado");

            var talie = _talieRepositorio.ObterTaliePorId(item.TalieId);

            if (talie == null)
                return RetornarErro($"Talie não localizado");

            //if (talie.Fechado)
            //    return RetornarErro($"Talie Fechado. Operação não permitida");

            _talieRepositorio.ExcluirTalieItem(item.Id);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public ActionResult Balanco(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "Home");

            var talieBusca = _talieRepositorio.ObterTaliePorId(id);

            if (talieBusca == null)
                throw new Exception("Talie não encontrado");

            var balanco = _talieRepositorio
                .ObterBalancoTalie(talieBusca.RegistroId)
                .ToList();

            var volumesDescarregados = balanco
                .Where(c => c.Descarregado == true).ToList();

            var volumesNaoDescarregados = balanco
                .Where(c => c.Descarregado == false).ToList();

            return View(new TalieBalancoViewModel
            {
                TalieId = talieBusca.Id,
                QuantidadeNF = balanco.Count(),
                Descarregados = volumesDescarregados.Count(),
                Mercadoria = balanco.Select(c => c.Mercadoria).Distinct().FirstOrDefault(),
                Embalagem = balanco.Select(c => c.Embalagem).Distinct().FirstOrDefault(),
                VolumesDescarregados = volumesDescarregados,
                VolumesNaoDescarregados = volumesNaoDescarregados
            });
        }

        [HttpGet]
        public ActionResult Marcantes(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Home");

            var talieBusca = _talieRepositorio.ObterTaliePorId(id.Value);

            if (talieBusca == null)
                throw new Exception("Talie não encontrado");

            var armazens = _talieRepositorio
                .ObterArmazens(talieBusca.Patio).ToList();

            var marcantes = _talieRepositorio
                .ObterMarcantes(talieBusca.BookingId, talieBusca.Id).ToList();

            return View(new TalieMarcantesViewModel
            {
                TalieId = talieBusca.Id,
                PatioId = talieBusca.Patio,
                Armazens = armazens,
                Marcantes = marcantes
            });
        }

        [HttpGet]
        public ActionResult ObterQuadras(int armazemId)
        {
            var detalhes = _talieRepositorio.ObterDetalhesArmazem(armazemId).ToList();

            var quadras = detalhes.Where(c => c.Quadra != null)
                .Select(c => c.Quadra)
                .Distinct()
                .ToList();

            var ruas = detalhes.Where(c => c.Rua != null)
                .Select(c => c.Rua)
                .Distinct()
                .ToList();

            var fiadas = detalhes.Where(c => c.Fiada != null)
                .Select(c => c.Fiada)
                .Distinct()
                .ToList();

            var alturas = detalhes.Where(c => c.Altura != null)
                .Select(c => c.Altura)
                .Distinct()
                .ToList();

            return Json(new
            {
                Quadras = quadras,
                Ruas = ruas,
                Fiadas = fiadas,
                Alturas = alturas
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObterQuantidadeDescarga(int? id)
        {
            var talieBusca = _talieRepositorio.ObterTaliePorId(id.Value);

            if (talieBusca == null)
                return RetornarErro($"Talie não encontrado");

            var quantidadeDescarregada = _talieRepositorio.ObterQuantidadeDescarga(talieBusca.Id);

            var quantidadeAssociada = _talieRepositorio.ObterQuantidadeAssociada(talieBusca.Id, talieBusca.BookingId);

            return Json(new
            {
                QuantidadeDescarregada = quantidadeDescarregada,
                QuantidadeAssociada = quantidadeAssociada
            }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult GravarMarcantes([Bind(Include = "TalieId, CodigoMarcante, ArmazemId, Quadra, Quantidade, QuantidadeAssociada, QuantidadeDescarregada")] TalieMarcantesViewModel viewModel)
        //{
        //    var talieBusca = _talieRepositorio.ObterTaliePorId(viewModel.TalieId);

        //    if (talieBusca == null)
        //        return RetornarErro($"Talie não encontrado");

        //    Marcante marcanteBusca = _talieRepositorio.ObterMarcantePorId(viewModel.CodigoMarcante.ToInt());

        //    if (marcanteBusca == null)
        //        return RetornarErro($"Marcante não encontrado");

        //    if (viewModel.ArmazemId > 0)
        //    {
        //        var posicaoBusca = _talieRepositorio.ObterPosicaoPatio(viewModel.ArmazemId, viewModel.Quadra);

        //        if (posicaoBusca == null)
        //            return RetornarErro($"Posição não encontrada");

        //        if (posicaoBusca.PatioId != User.ObterPatioColetorId())
        //            return RetornarErro($"Armazem não pertence ao patio do usuário.");
        //    }

        //    var marcante = new Marcante(
        //        talieBusca.Id,
        //        viewModel.Quantidade,
        //        marcanteBusca.Volumes,
        //        viewModel.Quadra,
        //        viewModel.QuantidadeAssociada,
        //        viewModel.QuantidadeDescarregada,
        //        marcanteBusca.Registro,
        //        viewModel.CodigoMarcante,
        //        viewModel.ArmazemId);

        //    if (!Validar(marcante))
        //        return RetornarErros();

        //    _talieRepositorio.GravarMarcante(marcante);

        //    var marcantes = _talieRepositorio
        //        .ObterMarcantes(talieBusca.BookingId, talieBusca.Id).ToList();

        //    return PartialView("_ConsultaMarcantes", marcantes);
        //}

        //[HttpPost]
        //public ActionResult ExcluirMarcante(int? id)
        //{
        //    if (id == null)
        //        return RetornarErro($"Marcante não informado");

        //    var marcanteBusca = _talieRepositorio.ObterMarcantePorId(id.Value);

        //    if (marcanteBusca == null)
        //        return RetornarErro($"Marcante não encontrado ou já excluído");

        //    _talieRepositorio.ExcluirMarcante(marcanteBusca.CodigoMarcante);

        //    var marcantes = _talieRepositorio
        //        .ObterMarcantes(marcanteBusca.BookingId, marcanteBusca.TalieId).ToList();

        //    return PartialView("_ConsultaMarcantes", marcantes);
        //}
    }
}