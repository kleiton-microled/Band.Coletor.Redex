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

namespace Band.Coletor.Redex.Site.Controllers
{
    public class EstufagemController : DefaultController
    {
        private readonly IConferenteRepositorio _conferenteRepositorio;
        private readonly IEquipeRepositorio _equipesRepositorio;
        private readonly IOperacaoRepositorio _operacaoRepositorio;
        private readonly IEstufagemRepositorio _estufagemRepositorio;
        private readonly IConteinerRepositorio _conteinerRepositorio;

        public EstufagemController(IConferenteRepositorio conferenteRepositorio, IEquipeRepositorio equipesRepositorio, IOperacaoRepositorio operacaoRepositorio, IEstufagemRepositorio estufagemRepositorio, IConteinerRepositorio conteinerRepositorio)
        {
            _conferenteRepositorio = conferenteRepositorio;
            _equipesRepositorio = equipesRepositorio;
            _operacaoRepositorio = operacaoRepositorio;
            _estufagemRepositorio = estufagemRepositorio;
            _conteinerRepositorio = conteinerRepositorio; 
        }

        
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            else 
            {
             
                ViewBag.Conferentes = _conferenteRepositorio.ObterConferentes(User.ObterId());
                ViewBag.Equipes = _equipesRepositorio.ObterEquipes();
                ViewBag.Operacoes = _operacaoRepositorio.ObterOperacoes();


                return View();
            }
        }
        
        public JsonResult GetClientePorReserva(string reserva)
        {
            try
            {
                var query = _estufagemRepositorio.ObterClientesPorReserva(reserva);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCarregaDadosConteiner(string conteiner)
        {
            try
            {
                var query = _estufagemRepositorio.GetCarregaDadosConteiner(conteiner);
                //query.ConferenteId = User.ObterId();

                if (query == null)
                {
                    retornoJson.Mensagem = "Conteiner não encontrado";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false ;
                    retornoJson.statusRetorno = "500";
                }
                else
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";
                }

            
                

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetTalieId(int patio, int romaneio)
        {
            try
            {
                var query = _estufagemRepositorio.GetTalieId(patio, romaneio);

                if (query == null)
                {
                    query = _estufagemRepositorio.GetTalieIdSemRomaneio(patio);
                }
                if (query == null)
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "200";
                    retornoJson.possuiDados = false;
                }
                else
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.statusRetorno = "200";
                    retornoJson.possuiDados = true;
                }

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItensEstufadosPorTalieConteinerId(int talie, int conteiner)
        {
            try
            {
                var query = _estufagemRepositorio.GetItensEstufadosPorTalieConteinerId(talie,conteiner);
                var controle = 0;
                var soma = 0;
                var nf = "" ; 
                foreach (var item in query)
                {

                    soma = soma + item.Quantidade;
                    item.SomaQuant = soma;
                    if(nf == item.NF)
                    {
                        controle =  1;
                        nf = item.NF;
                    }
                    else
                    {
                        controle = 0;
                        nf = item.NF;

                    }

                    if (controle > 0)
                        item.Peso = "0";

                }

                
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }        
        public JsonResult GetGravaCarga(int romaneio, int nf, int quantidade, string data, int saldo)
        {
            try
            {
                var query = _estufagemRepositorio.GetDadosInsertEstufagem(romaneio, nf);


                int qtr = 0;                

                CargaEstufagem obj = new CargaEstufagem();

                foreach (var item in query)
                {                   

                    if (quantidade != 0)
                    {
                        qtr = item.Quantidade;

                        if (quantidade < qtr)
                        {
                            qtr = quantidade;
                        }

                        quantidade = quantidade - qtr;
                            
                        obj.PatioCSId = item.PatioCSId;
                        obj.AUTONUM_RCS = item.AUTONUM_RCS;
                        obj.autonum_ro = item.autonum_ro;
                        obj.Quantidade = qtr;
                        obj.EmbalagemId = item.EmbalagemId;
                        obj.Bruto = item.Bruto;
                        obj.Comprimento = item.Comprimento;
                        obj.Largura = item.Largura;
                        obj.Altura = item.Altura;
                        obj.VolumeTotal = item.VolumeTotal;
                        obj.Conteiner = item.Conteiner;
                        obj.PatioId = item.PatioId;
                        obj.ProdutoId = item.ProdutoId;
                        obj.NFItemId = item.NFItemId;
                        obj.TlieId = item.TlieId;
                        obj.Produto = item.Produto;
                        obj.Inicio = data;

                        _estufagemRepositorio.GravarCargaSaida(obj);
                        
                    }
                }               

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "A estufagem não foi gravada, por favor tente novamente";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNFByIdOsRo(int romaneio, string os)
        {
            try
            {
                int idNF = _estufagemRepositorio.getIDNF(romaneio, os);


                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = new
                {
                    NF = idNF
                };
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDadosByIdOsRo(int romaneio, int idNF)
        {
            try
            {
                int saldo = _estufagemRepositorio.GetValidaSaldoByOsRo(romaneio, idNF);
                
                /// retornar o autonumNF deixar aqui até validar
                /// 
                /// select para validar o saldo insuficiente 
                /// select sum(rcs.qtde - isnull(sc.qtde_saida,0)) from tb_romaneio_cs rcs
                //                inner join tb_patio_cs pcs on rcs.autonum_pcs = pcs.autonum_pcs
                //left join tb_saida_carga sc on rcs.autonum_pcs = sc.autonum_pcs
                //where rcs.autonum_ro = 84125 and pcs.autonum_nf = 169480


                retornoJson.Mensagem = "Estufagem gravado com sucesso";
                retornoJson.objetoRetorno = new { 
                    SALDO = saldo
                };
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNfLote(string lote, string cntr)
        {


            var query = _estufagemRepositorio.GetDadosNFByLote(lote, cntr);
            var bagagem = 0;
            if (query.Count() == 0)
            {
                bagagem = _estufagemRepositorio.CargaBagagem(lote);
            }

           if (bagagem == 0)
            { 
            retornoJson.objetoRetorno = query;
            retornoJson.statusRetorno = "200";
            retornoJson.possuiDados = true;

            return Json(retornoJson, JsonRequestBehavior.AllowGet);
           }
            else
            {
                retornoJson.objetoRetorno = bagagem;
                retornoJson.statusRetorno = "300";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetDadosByIdOs(string conteiner, int romaneio, string os)
        {
            try
            {                
                    var query = _estufagemRepositorio.GetDadosNFByOS(conteiner, romaneio, os);
                
                if (query.Count() >  0)
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.possuiDados = true;
                }
                else {
                    retornoJson.Mensagem = "Não foi encotrada nenhuma NF com o número pesquisado";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                }                
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDadosNF(int id)
        {
            try
            {
                var query = _estufagemRepositorio.GetDadosNF(id);

                if (query != null) 
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.statusRetorno = "200";
                    retornoJson.possuiDados = true;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }


                retornoJson.Mensagem = "Dados de NF não encotrados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "404";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDadosClienteByReserva(string reserva)
        {
            try
            {
                var query = _estufagemRepositorio.GetDadosClienteByReserva(reserva);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCarregaDadosTalieById(int id)
        {
            try
            {
                var query = _estufagemRepositorio.GetCarregaDadosTalieById(id);
              
                //query.CONFERENTE = User.ObterNome();
               

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSalvarDescarga(int nfi, int scId, int patio, int quantidade, string produto)
        {
            try
            {
                bool cargaSuzano = false;

                _estufagemRepositorio.GetDescarga(nfi, scId, patio, quantidade, produto, cargaSuzano);


                retornoJson.Mensagem = "Descarga realizada com sucesso";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram salvos";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetFlagFechado(int id)
        {
            try
            {
                var query = _estufagemRepositorio.GetFlagTalieFechado(id);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram salvos";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSaldoAtualizado(int id)
        {
            try
            {
                var query = _estufagemRepositorio.GetSaldoAtualizado(id);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SalvarEstufagem(Band.Coletor.Redex.Business.DTO.EstufagemDTO obj)
        {
            try
            {
                var dataEntrada = _estufagemRepositorio.ObterDataEntradaPorConteinerId(obj.PatioId);
                var inicio = obj.Inicio;
                var cargaSuzano =  false;
                var talieID = obj.TalieId;
                

                if (inicio < dataEntrada)
                {
                    retornoJson.Mensagem = "Início de estufagem não pode ser anterior a data de entrada do contêiner!";
                    retornoJson.possuiDados = false;
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";


                    return Json(retornoJson, JsonRequestBehavior.AllowGet);

                }
                if (cargaSuzano)
                {
                    var qtdeSaida = _estufagemRepositorio.ObterQuantidadeSaidaPorConteinerId(obj.ConferenteId);
                    var qtdePacking = _estufagemRepositorio.ObterQuantidadePackingPorConteinerId(obj.ConteinerId);

                    if (qtdePacking != qtdeSaida || qtdeSaida == 0)
                    {
                        retornoJson.Mensagem = "Quantidade Programada diverge da quantidade estufada para este contêiner <br />Fechamento de Talie não permitido <br />Operação Cancelada!";
                        retornoJson.possuiDados = false;
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "500";


                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }
                }

                var conteiner  = _conteinerRepositorio.ObterConteinerPorId(obj.PatioId);


                if (talieID == 0)
                {
                    var countTalie = _estufagemRepositorio.ObterIdTaliePorConteinerId(obj.PatioId);

                    if (countTalie > 0)
                    {
                        retornoJson.Mensagem = "Já existe talie para este contêiner!";
                        retornoJson.possuiDados = false;
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "500";


                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                    Talie tl = new Talie();
                    tl.Inicio = obj.Inicio;
                    tl.CrossDocking = obj.CrossDocking;
                    tl.BookingId = obj.BookingId;
                    tl.ConferenteId = obj.ConferenteId;
                    tl.EquipeId = obj.EquipeId;
                    tl.OperacaoId = obj.OperacaoId;

                    _estufagemRepositorio.Gravar(tl);


                }
                else
                {
                    Talie tl = new Talie();
                    tl.Inicio = obj.Inicio;
                    tl.CrossDocking = obj.CrossDocking;
                    tl.BookingId = obj.BookingId;
                    tl.ConferenteId = obj.ConferenteId;
                    tl.EquipeId = obj.EquipeId;
                    tl.OperacaoId = obj.OperacaoId;

                    _estufagemRepositorio.Atualizar(tl);
                }

                if (obj.Termino.HasValue)
                {
                    var idBookingCarga = _estufagemRepositorio.ObterBookingCargaIdPorConteinerId(obj.PatioId);
                    var idBooking = _estufagemRepositorio.ObterBookingPorBookingCargaId(idBookingCarga);

                    var qtdeTotal = _estufagemRepositorio.ObterQuantidadeCargaSoltaPorReserva(idBooking);

                    if (qtdeTotal == 0)
                    {
                        retornoJson.Mensagem = "Não foi encontrada carga cadastrada na reserva desta unidade!";

                        retornoJson.possuiDados = false;
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);

                    }


                    idBookingCarga = _estufagemRepositorio.ObterBookingCargaIdPorReservaId(idBooking);

                    if (idBooking == 0)
                    {
                        retornoJson.Mensagem = "Não foi encontrada carga solta cadastrada na reserva desta unidade!";
                        retornoJson.possuiDados = false;
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                    var conteineresEstufar = _estufagemRepositorio.ObterConteineresEstufar(idBooking);
                    var conteineresFechados = _estufagemRepositorio.ObterConteineresFechados(idBooking);

                    //fechamento do ultimo conteiner ou todos ja fechados
                    var qtdeEstufada = _estufagemRepositorio.ObterQuantidadeEstufada(idBooking);
                    if ((conteineresEstufar == conteineresFechados) || (conteineresEstufar == conteineresFechados + 1))
                    {

                        if (qtdeTotal != qtdeEstufada)
                        {
                            retornoJson.Mensagem = "Divergencia na quantidade programada e quantidade estufada!";
                            retornoJson.possuiDados = false;
                            retornoJson.objetoRetorno = null;
                            retornoJson.statusRetorno = "500";


                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }


                        //Transfere cargas da reserva de origem para a reserva de embarque
                        _estufagemRepositorio.TransferirCargasDaReservaOrigemParaRservaEmbarque(idBooking, idBookingCarga);

                    }
                    else
                    {
                        if (qtdeEstufada > qtdeTotal)
                        {
                            retornoJson.Mensagem = "Atenção - Quantidade estufada está maior que a programada \nFechamento Cancelado!";
                            retornoJson.possuiDados = false;
                            retornoJson.objetoRetorno = null;
                            retornoJson.statusRetorno = "500";


                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }
                        else 
                        {
                            retornoJson.Mensagem = "Dados salvos com sucesso";
                            retornoJson.possuiDados = false;
                            retornoJson.objetoRetorno = null;
                            
                        }
                    }


                    _estufagemRepositorio.AtualizarTalieFechado(obj.Termino.Value, obj.TalieId);
                    _estufagemRepositorio.AtualizarConteinerFechado(obj.PatioId, "F");
                }
                else
                {
                    _estufagemRepositorio.AtualizarConteinerFechado(obj.PatioId, "E");
                }

                
                retornoJson.statusRetorno = "200";
                

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson = new ResponseJson();

                retornoJson.Mensagem = "Os dados não foram inseridos";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Gravar(string inicio, string termino , int talie, int boo, int romaneio, int conferente, int equipe, int gate, int patio, string modo)
        {
            try
            {
                var query = _estufagemRepositorio.GetDadosRomaneio(romaneio);

                if (query != null)
                {
                    var talieid = query.autonum_talie;

                    if (talie == 0)
                    {
                        if (query.autonum_patio != 0)
                        {
                            int countTalieByPatio = _estufagemRepositorio.countTalieByRomaneio(query.autonum_patio);

                            if (countTalieByPatio != 0)
                            {
                                retornoJson.Mensagem = "Já existe um talie talie para este contêiner - Operação cancelada";
                                retornoJson.objetoRetorno = null;
                                retornoJson.statusRetorno = "500";
                                retornoJson.possuiDados = false;

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }

                            int countTalieByRomaneio = _estufagemRepositorio.countTalieByPatio(query.autonum_ro);

                            if (countTalieByRomaneio != 0)
                            {
                                retornoJson.Mensagem = "Foi encontrado um talie para este romaneio - Operação cancelada";
                                retornoJson.objetoRetorno = null;
                                retornoJson.statusRetorno = "500";
                                retornoJson.possuiDados = false;

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }

                            int talieID = _estufagemRepositorio.GravarTalie(
                                    query.autonum_patio,
                                    inicio,
                                    boo,
                                    modo,
                                    conferente,
                                    equipe,
                                    romaneio,
                                    query.AUTONUM_GATE_SAIDA
                                );

                            string dtInicio = _estufagemRepositorio.GetDataInicioTalie(talieID);

                            if (dtInicio != null)
                            {

                            }

                            _estufagemRepositorio.UpdateRomaneio(talieID, query.autonum_ro);

                            if (query.crossdocking == 1)
                            {
                                _estufagemRepositorio.UpdateSaidaCarga(talieID, query.autonum_ro, query.autonum_patio);
                            }

                            retornoJson.Mensagem = "Dados cadastrados com sucesso";
                            retornoJson.objetoRetorno = new { 
                                dtInicio = dtInicio, 
                                talieID = talieID, 
                            };
                            retornoJson.statusRetorno = "200";
                            retornoJson.possuiDados = false;
                        }                       
                    }
                    else
                    {
                        _estufagemRepositorio.UpdateTalie(inicio, termino, talieid, boo, romaneio, conferente, equipe, gate, patio, modo);

                        retornoJson.Mensagem = "Dados atualizados com sucesso";
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "200";
                        retornoJson.possuiDados = false;                        
                    }                    
                }
                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        #region validaFechamento
        public JsonResult ValidarFechamento(int talie, int romaneio)
        {
            try
            {
                if (talie == 0)
                {
                    retornoJson.Mensagem = "Fechamento disponível somente após o lançamento da estufagem!";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (romaneio == 0)
                {
                    retornoJson.Mensagem = "Falha na busca do planejamento";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int talieID = _estufagemRepositorio.getFechamentoIDTalieByRomaneio(romaneio);

                if (talie != talieID)
                {
                    retornoJson.Mensagem = "Já existe um talie para o romaneio selecionado - Operação cancelada";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }
                int romaneioID = _estufagemRepositorio.getFechamentoIDRomaneioByTalie(talie);

                if (romaneio != romaneioID)
                {
                    retornoJson.Mensagem = "Já existe um romaneio para o talie selecionado - Operação cancelada";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int countNF = _estufagemRepositorio.getFechamentoConsisteNF(romaneio);

                if (countNF != 0)
                {
                    retornoJson.Mensagem = "Carregamento / Estufagem possui NF sem item cadastrado";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                var dadosFechamento = _estufagemRepositorio.GetDadosValidaFechamento(talie);

                if (dadosFechamento == null)
                {
                    retornoJson.Mensagem = "Talie não Criado - Processo cancelado";
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "500";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                int flagFechado = dadosFechamento.flag_fechado;
            

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = new {
                    flagFechado = flagFechado
                };
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ValidarFechamentoParte2(int talie, int autonumRomaneio, int autonumPatio)
        {
            try
            {
                var query = _estufagemRepositorio.GetDadosValidaFechamentoParte2(talie);
                string lacre = "";
                int counttpc = 0;
                if (query != null)
                {                 
                    if (query.AUTONUM_PATIO != 0)
                    {
                        lacre = _estufagemRepositorio.GetDadosLacre(autonumPatio);
                       
                        if (lacre == "")
                        {
                            counttpc = _estufagemRepositorio.countPatiosByTpc(autonumPatio);
                            if (counttpc == 0)
                            {
                                retornoJson.Mensagem = "Não consta Lacre para o conteiner estufado - Verifique controle de lacres";
                                retornoJson.objetoRetorno = null;
                                retornoJson.statusRetorno = "500";
                                retornoJson.possuiDados = false;

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }
                            
                        }

                        int qtde = _estufagemRepositorio.sumQuantidadeSaida(autonumPatio);                        
                        int qtdeSaida = _estufagemRepositorio.sumQuantidadeRomaneio(autonumRomaneio);

                        if (qtde != qtdeSaida)
                        {
                            retornoJson.Mensagem = "Divergencia na quantidade planejada e quantidade estufada. Fechamento cancelado";
                            retornoJson.objetoRetorno = null;
                            retornoJson.statusRetorno = "500";
                            retornoJson.possuiDados = false;

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (query.AUTONUM_PATIO != 0 && query.flag_CARREGAMENTO == 1 && query.FLAG_ESTUFAGEM == 1)
                    {
                        int sumQtdeSaidas = _estufagemRepositorio.sumQuantidadeSaidasCargas(autonumPatio, talie);

                        if (sumQtdeSaidas == 0)
                        {
                            retornoJson.Mensagem = "Não foi encontrado registro de saida";
                            retornoJson.objetoRetorno = null;
                            retornoJson.statusRetorno = "500";
                            retornoJson.possuiDados = false;

                            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

              
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ConfirmaFechamento(int talie, int autonumRomaneio, int autonumPatio,int conferente,int equipe)
        {
            try
            {

                _estufagemRepositorio.AtualizarConteinerFechado(autonumPatio, "F");

                _estufagemRepositorio.FecharEstufagem(talie, autonumRomaneio,conferente,equipe);
                retornoJson.Mensagem = "Fechamento Realizado com Sucesso ";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Erro ao Fechar";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        public JsonResult GerarCancelamento(int talie, int autonumRomaneio)
        {
            try
            {
                _estufagemRepositorio.GerarCancelamento(talie, autonumRomaneio);

                retornoJson.Mensagem = "Cancelamento realizado com sucesso";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Erro ao cancelar registro";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

