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
using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Site.Extensions;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class TalieColetorDescargaController : DefaultController
    {
        private readonly ITalieRepositorio _talieRepositorio;
        private readonly ITalieColetorDescargaRepositorio _talieColetorDescargaRepositorio;
        private readonly IUsuarioLoginRepositorio _usuarioLoginRepositorio;

        public TalieColetorDescargaController(ITalieRepositorio talieRepositorio, IUsuarioLoginRepositorio usuarioLoginRepositorio, ITalieColetorDescargaRepositorio talieColetorDescargaRepositorio)
        {
            _usuarioLoginRepositorio = usuarioLoginRepositorio;
            _talieRepositorio = talieRepositorio;
            _talieColetorDescargaRepositorio = talieColetorDescargaRepositorio;
        }

        public ActionResult Index(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {

                if (id == "dd")
                {
                    ViewBag.DD = 1;
                    ViewBag.CD = 0;
                    ViewBag.TipoDescarga = Band.Coletor.Redex.Business.Enums.OpcoesDescarga.DA;
                }
                else
                {
                    ViewBag.DD = 0;
                    ViewBag.CD = 1;
                    ViewBag.TipoDescarga = Band.Coletor.Redex.Business.Enums.OpcoesDescarga.CD;
                }
                ViewBag.Equipes = _talieRepositorio.ObterEquipes();
                ViewBag.Conferente = _talieRepositorio.ObterConferentes(User.ObterId());

                return View();
            }
        }
        public JsonResult GetAllDadosTalie(string talie, string registro, string tipoDescarga)
        {
            try
            {
                var query = _talieColetorDescargaRepositorio.GetAllDadosTalie(talie, registro, tipoDescarga);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados de talie não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllDadosTalieItens(int id)
        {
            try
            {
                var query = _talieColetorDescargaRepositorio.GetAllDadosTalieItens(id);
                var nf = "";
                if (query.Count() > 0)
                {
                  foreach(var i in query)
                    {
                        if (i.num_nf != nf)
                        {
                            i.Peso = i.PesoBruto;
                            nf = i.num_nf;
                        }
                        else
                        {
                            i.Peso = 0;
                        }
                  }

                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }


                retornoJson.Mensagem = "Não foram encontrados dados para o Talie selecionado";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados de talie não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ObterTaliePorIdConteiner(int id, string conteiner)
        {
            try
            {
                var query = _talieColetorDescargaRepositorio.GetTalieByIdConteiner(id, conteiner);

                if (query != null)
                {                    
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.possuiDados = true;  
                }
                else 
                {
                    query = _talieColetorDescargaRepositorio.GetTalieByRegistro(id);

                    if (query != null)
                    {
                       
                        if(query.AUTONUM_TALIE == 0)
                        {
                            query.AUTONUM_TALIE = id;
                        }


                        retornoJson.Mensagem = "";
                        retornoJson.objetoRetorno = query;
                        retornoJson.possuiDados = true;
                    }
                    else
                    {
                        retornoJson.Mensagem = "Não foram encontrados registros para a sua consulta";
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                    }
                }

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Não foram encontrados dados para sua pesquisa";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ObterTaliePorId(int id)
        {
            try
            {
                    var query = _talieColetorDescargaRepositorio.GetTalieById(id);

                if (query != null)
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.possuiDados = true;
                }
                else
                {
                    query = _talieColetorDescargaRepositorio.GetTalieByRegistro(id);

                    if (query != null)
                    {
                        retornoJson.Mensagem = "";
                        retornoJson.objetoRetorno = query;
                        retornoJson.possuiDados = true;
                    }
                    else
                    {
                        retornoJson.Mensagem = "Não foram encontrados registros para a sua consulta";
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                    }
                }

                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult CadastrarTalie(Talie obj)
        {
            try
            {
                var id = obj.AUTONUM_TALIE;
                var idReg = obj.AUTONUM_REG;
                var boo = obj.BookingId;     
                var retorno = 0;
                DescargaAutomaticaDTO dsc = new DescargaAutomaticaDTO();

                string conteiner = obj.ConteinerId;

                int IDconteiner = _talieColetorDescargaRepositorio.getValidaConteiner(conteiner);
                
                obj.AUTONUM_PATIO = IDconteiner;

                if (conteiner != null)
                {
                    if (IDconteiner == 0)
                    {
                        retornoJson.Mensagem = "Conteiner Inválido";
                        retornoJson.objetoRetorno = "";
                        retornoJson.possuiDados = true;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }
                }

                if (id == 0)
                {
                retorno = _talieColetorDescargaRepositorio.CadastrarTalie(obj);
                }
                else
                {
                    _talieColetorDescargaRepositorio.AtualizarTalie(obj);
                }

                if (id == 0)
                {
                  //  id = Convert.ToInt32(_talieColetorDescargaRepositorio.GetTalieCarregamentoId());

                    var query = _talieColetorDescargaRepositorio.GetDadosDescargaAutomatica(idReg);


                    foreach (var item in query)
                    {
                        string nf = item.NF;
                        var idNF = _talieColetorDescargaRepositorio.GetMaxNotaFiscal(nf);

                        double pesoBruto = _talieColetorDescargaRepositorio.GetPesoBruto(idNF);

                      

                        dsc.AUTONUM_TALIE = retorno;

                        dsc.AUTONUM_REGCS = item.AUTONUM_REGCS;
                        dsc.QUANTIDADE = item.QUANTIDADE;
                        dsc.AUTONUM_NF = idNF;
                        dsc.Peso = Convert.ToDecimal(pesoBruto);
                        dsc.NF = item.NF;
                        dsc.qtde_manifestada = item.qtde_manifestada;
                        dsc.peso_manifestado = item.peso_manifestado;
                        dsc.imo = item.imo;
                        dsc.imo2 = item.imo2;
                        dsc.imo3 = item.imo3;
                        dsc.imo4 = item.imo4;
                        dsc.uno = item.uno;
                        dsc.uno2 = item.uno2;
                        dsc.uno3 = item.uno3;
                        dsc.uno4 = item.uno4;
                        dsc.AUTONUM_PRO = item.AUTONUM_PRO;
                        dsc.AUTONUM_EMB = item.AUTONUM_EMB;


                        _talieColetorDescargaRepositorio.InserirTalieItemDescargaAutomatica(dsc);
                    }
                }               

                if (retorno > 0) {
                    retornoJson.Mensagem = "Talie gravada com sucesso";
                    retornoJson.objetoRetorno = dsc;
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";
                } else
                {
                    retornoJson.Mensagem = "Talie atualizada com sucesso";
                    retornoJson.objetoRetorno = "";
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";
                }

       

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Talie não foi cadastrado";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetFinalizarTalie(
            int id, 
            string inicio, 
            string tipoDescarga, 
            int estufagemCompleta, 
            string conteiner, 
            int atualizarAlertaEtiqueta1,
            int atualizarAlertaEtiqueta2, 
            int etiquetas, 
            int pendencias, 
            int totalItens,
            string idContainer
            )
        {
            try
            {
                int countTalies = _talieColetorDescargaRepositorio.GetCountTalie(id);
                string tipoDescargaDa = Band.Coletor.Redex.Business.Enums.OpcoesDescarga.DA.ToString();
                int boo = 0;

                if (countTalies != 0) 
                {
                    if (tipoDescarga == tipoDescargaDa)
                    {
                        if (etiquetas == 0)
                        {
                            if (atualizarAlertaEtiqueta1 == 0)
                            {
                                retornoJson.Mensagem = "";
                                retornoJson.possuiDados = true;
                                retornoJson.statusRetorno = "200";
                                retornoJson.objetoRetorno = null;

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }
                            _talieColetorDescargaRepositorio.GetUpdateEtiqueta(id);
                        }

                        if (pendencias != 0)
                        {
                            if (atualizarAlertaEtiqueta2 == 0)
                            {
                                retornoJson.Mensagem = "";
                                retornoJson.possuiDados = false;
                                retornoJson.statusRetorno = "500";
                                retornoJson.objetoRetorno = null;

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }
                            _talieColetorDescargaRepositorio.GetUpdateEtiqueta2(id);
                        }
                    }
                }
                
                var query = _talieColetorDescargaRepositorio.GetDadosFinalizarTalie(id);

                if (query != null)
                {
                    string dataRegistro = DateTime.Now.ToString("dd-MM-yyyy hh:mm");

                    int gate = _talieColetorDescargaRepositorio.getAutonumGate(id);

                    var bruto = _talieColetorDescargaRepositorio.getBrutoByTbNewGate(gate);
                    
                    
                    if (dataRegistro == null)
                    {
                        retornoJson.Mensagem = "Data de termino invalida ou estava em branco quando o talie foi carregado. Grave a data de termino e carregue o talie novamente";
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = "500";
                        retornoJson.objetoRetorno = null;

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }
                }
                double pesonf = _talieColetorDescargaRepositorio.getBrutoByTalieId(id);

                foreach (var item in query)
                {

                    _talieColetorDescargaRepositorio.insertDataSEQPATIOCS();

                    var patioCSId = _talieRepositorio.ObterProximoIdPatioCS();

                    //var patioCSId = 32001;

                    TalieDTO talie = new TalieDTO();

                    talie.AUTONUM_BOO = item.AUTONUM_BOO;
                    talie.AUTONUM_PCS = patioCSId;
                    talie.DT_PRIM_ENTRADA = Convert.ToDateTime(item.Inicio);
                    talie.BookingId = item.BookingId;
                    talie.QuantidadeDescarga = item.QuantidadeDescarga;
                    talie.EmbalagemId = item.EmbalagemId;
                    talie.AUTONUM_EMB = item.EmbalagemId;
                    talie.ProdutoId = item.ProdutoId;
                    talie.MARCA = item.MARCA;
                    talie.COMPRIMENTO = item.COMPRIMENTO;
                    talie.LARGURA = item.LARGURA;
                    talie.ALTURA = item.ALTURA;
                    talie.Peso = item.Peso;
                    talie.RegistroCsId = item.RegistroCsId;
                    talie.TalieItemId = item.TalieItemId;
                    talie.QuantidadeEstufagem = item.QuantidadeEstufagem;
                    talie.YARD = item.YARD;
                    talie.ARMAZEM = item.ARMAZEM;
                    talie.PatioId = item.PatioId;
                    talie.IMO = item.IMO;
                    talie.IMO2 = item.IMO2;
                    talie.IMO3 = item.IMO3;
                    talie.IMO4 = item.IMO4;
                    talie.UNO = item.UNO;
                    talie.UNO2 = item.UNO2;
                    talie.UNO3 = item.UNO3;
                    talie.UNO4 = item.UNO4;
                    talie.Inicio = item.Inicio;
                    talie.CodigoProduto = item.CodigoProduto;
                    talie.AUTONUM_TALIE = id;
                    talie.CODIGO_CARGA = item.CODIGO_CARGA;
                    talie.AUTONUM_BCG = item.BookingCargaId;
                    talie.AUTONUM_PRO = item.ProdutoId;
                    talie.VOLUME_DECLARADO = item.VOLUME_DECLARADO;
                    talie.BRUTONF = pesonf;
                    talie.FLAG_HISTORICO = item.FLAG_HISTORICO;
                    talie.AUTONUM_REGCS = item.RegistroCsId;
                    talie.AUTONUM_NF = item.NotaFiscalId;
                    talie.AUTONUM_PATIOS = item.PatioId;                    
                    talie.AUTONUM_TI = item.TalieItemId;
                    talie.PATIO = item.PatioId;
                    talie.CODPRODUTO = item.CodigoProduto;
                    talie.Peso = item.Peso;
                    talie.QTDE_ESTUFAGEM = item.QuantidadeEstufagem;
                    talie.autonum_gate = item.autonum_gate;
                    talie.QTDE_DESCARGA = Convert.ToInt32(item.QuantidadeDescarga);
                    talie.QTDE_ENTRADA = Convert.ToInt32(item.QuantidadeDescarga);


                    _talieColetorDescargaRepositorio.InsertTaliePatioCS(talie);

                    pesonf = 0;

                    int yardId = _talieRepositorio.ObterProximoIdCargaSoltaYard();
                    //int yardId = 3454;
                    int talieId = id;
                    int armazem = item.ARMAZEM;

                    _talieColetorDescargaRepositorio.InsertCargaSolta(patioCSId, yardId, armazem);

                    string IMO = item.IMO;

                    if (IMO != null)
                    {
                        _talieColetorDescargaRepositorio.UpdateTbBookingCarga(talie.AUTONUM_BCG, talie.IMO);
                    }

                    _talieColetorDescargaRepositorio.insertDataSEQAMRGate();

                    _talieColetorDescargaRepositorio.UpdateArmGate(talie);

                    _talieColetorDescargaRepositorio.UpdatePatioPaiCS(patioCSId);




                    boo = item.AUTONUM_BOO;

                }

                var totalItensPatio = _talieColetorDescargaRepositorio.countItensPatio(id);                    

                    if (totalItensPatio == 0)
                    {
                        retornoJson.Mensagem = "Falha no processo de fechamento, carga não transferida para o estoque, contate o TI assim que possível";
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = "500";
                        retornoJson.objetoRetorno = null;

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                    _talieColetorDescargaRepositorio.UpdateFlagFechadoByTalieID(id);

                    int QR = _talieColetorDescargaRepositorio.GetCargaEntrada(boo);
                    int QE = _talieColetorDescargaRepositorio.GetQuantidadeEntrada(boo);

                    if ((QR == QE) && QR > 0)
                    {
                        _talieColetorDescargaRepositorio.UpdateFlagFechadoByBoo(boo);
                    }
                    _talieColetorDescargaRepositorio.UpdateFlagFechadoByReserva(boo);

                    string enumTipoDescarga = Band.Coletor.Redex.Business.Enums.OpcoesDescarga.CD.ToString();
                    
                    //Falta testar esses codigos

                    if (enumTipoDescarga == tipoDescarga)
                    {
                        int containerId = _talieColetorDescargaRepositorio.GetContainerId(id);

                        _talieColetorDescargaRepositorio.UpdatePatioEF(containerId);

                        long reservaCC = _talieColetorDescargaRepositorio.GetReservaCC(containerId);

                        

                        long romaneioId = _talieColetorDescargaRepositorio.GetRomaneioId(containerId);
                         long idCurrentRomaneio = 0;

                        int usuarioID = Convert.ToInt32(Session["USUARIO"]);

                        if (romaneioId == 0)
                        {
                            _talieColetorDescargaRepositorio.insertDataSEQRomaneioID();
                            idCurrentRomaneio = _talieColetorDescargaRepositorio.GetCurrentIdRomaneio();
                            _talieColetorDescargaRepositorio.InsertRomaneio(usuarioID, containerId, reservaCC, idCurrentRomaneio);
                        }
                        var patiosCS = _talieColetorDescargaRepositorio.GetDadosPatioCS(id);



                        foreach (var patio in patiosCS)
                        {
                            _talieColetorDescargaRepositorio.InsertRomaneioCS(patio.AUTONUM_PCS, idCurrentRomaneio, patio.QTDE_ENTRADA);
                        }



                        string dtInicio = _talieColetorDescargaRepositorio.GetDataInicioTalie(containerId);
                        string dtFim = _talieColetorDescargaRepositorio.GetDataTerminoTalie(containerId);
                        int equipeID = _talieColetorDescargaRepositorio.GetEquipeTalie(containerId);
                        int conferenteID = _talieColetorDescargaRepositorio.GetConferenteTalie(containerId);
                        string modoID = _talieColetorDescargaRepositorio.GetFormaOperacaoTalie(containerId);
                        int countTalieByFlagCarregamento = _talieColetorDescargaRepositorio.countFlagCarregamentoTalie(containerId);
                        long TalieCarregamento = 0 ;

                        if (countTalieByFlagCarregamento == 0)
                        {
                            _talieColetorDescargaRepositorio.InserirTalieFechamento(containerId, dtInicio, dtFim, reservaCC, modoID, conferenteID, equipeID, idCurrentRomaneio);

                            TalieCarregamento = _talieColetorDescargaRepositorio.GetTalieCarregamentoId();

                            _talieColetorDescargaRepositorio.UpdateRomaneioIdTalie(TalieCarregamento, idCurrentRomaneio);
                        }
                        else
                        {
                            _talieColetorDescargaRepositorio.UpdateTalieByInicioTermino(containerId, dtInicio, dtFim);
                        }

                        foreach (var patioSaidaCarga in patiosCS)
                        {
                            _talieColetorDescargaRepositorio.InsertAMRNFSaida(containerId, patioSaidaCarga.AUTONUM_NF, patioSaidaCarga.QTDE_ENTRADA);

                            long idCs = _talieColetorDescargaRepositorio.GetSaidaCargaId();

                            _talieColetorDescargaRepositorio.InsertSaidaCarga(
                                idCs,
                                patioSaidaCarga.AUTONUM_PCS,
                                patioSaidaCarga.QTDE_ENTRADA,
                                patioSaidaCarga.AUTONUM_EMB,
                                patioSaidaCarga.BRUTO,
                                patioSaidaCarga.ALTURA,
                                patioSaidaCarga.COMPRIMENTO,
                                patioSaidaCarga.LARGURA,
                                patioSaidaCarga.VOLUME_DECLARADO,
                                containerId,
                                idContainer,
                                dtInicio,
                                patioSaidaCarga.AUTONUM_NF,
                                Convert.ToInt32(TalieCarregamento),
                                Convert.ToInt32(idCurrentRomaneio)
                                );

                            long QS = _talieColetorDescargaRepositorio.GetSomaQuantidadeSaida(patioSaidaCarga.AUTONUM_PCS);

                            if (QS >= patioSaidaCarga.QTDE_ENTRADA)
                            {
                                _talieColetorDescargaRepositorio.UpdatePatioHistorico(patioSaidaCarga.AUTONUM_PCS);
                            }
                        }
                    }

                    //Falta testar esses codigos    

                retornoJson.Mensagem = "Carga Transferida para o estoque";
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";
                retornoJson.objetoRetorno = null;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "A talie não foi fechada, por favor tente novamente";
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";
                retornoJson.objetoRetorno = null;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CadastrarTalieItem(TalieItemDTO obj)
        {
            try
            {
                var talieId = obj.AUTONUM_TALIE;
                var talieItem = obj.AUTONUM_TI;
                long quantidade = Convert.ToInt64(obj.Quantidade);
                long qtde = 0;
                long qtdeNf = 0;
                long dif = 0;
                int nfID = obj.AUTONUM_NF;
                double peso = 0;
                int countPesoTalieItem = 0;

                countPesoTalieItem = _talieColetorDescargaRepositorio.GetCountPesoBruto(nfID);
                peso = _talieColetorDescargaRepositorio.GetPesoBruto(nfID);

                if (countPesoTalieItem > 0)
                {
                    obj.Peso = 0;
                }
                else
                {
                    obj.Peso = Convert.ToDecimal(peso);
                }

                if (talieItem > 0)
                {
                    qtdeNf = _talieColetorDescargaRepositorio.countQuantidadeDescarga(talieItem);
                    qtde = _talieColetorDescargaRepositorio.countQuantidadeTotalDescarga(nfID);
                    qtde = qtde + quantidade;
                   

                    if (quantidade > qtdeNf)
                    {
                        retornoJson.Mensagem = "Qtde descarregada não pode ser superior ao saldo (" + qtdeNf + ") existente ";
                        retornoJson.possuiDados = true;
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                   
                    _talieColetorDescargaRepositorio.UpdateTalieItem(obj);

                    if (quantidade < qtdeNf)
                    {
                        dif =  Convert.ToInt64(qtdeNf) - Convert.ToInt64(quantidade) ;

                        _talieColetorDescargaRepositorio.InsertTalieItem(dif, talieItem);

                        retornoJson.Mensagem = "Itens da talie inseridos com sucesso";
                        retornoJson.possuiDados = true;
                        retornoJson.objetoRetorno = null;
                        retornoJson.statusRetorno = "200";
                    }
                }

                retornoJson.Mensagem = "Itens da talie alterados com sucesso";
                retornoJson.possuiDados = true;
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "O item da talie não foi cadastrado, por favor tente novamente";
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";
                retornoJson.objetoRetorno = null;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ObterTotalItensPorTalie(int id)
        {
            try
            {
                int count = _talieColetorDescargaRepositorio.GetCountTalie(id);

                retornoJson.Mensagem = "";
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";
                retornoJson.objetoRetorno = count;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "";
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";
                retornoJson.objetoRetorno = null;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDadosTalieItemByID(int id)
        {
            try
            {
                var query = _talieColetorDescargaRepositorio.GetDadosItemTalieId(id);
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetExcluirTalieItem(int id, int talieId, int quantidade)
        {
            try
            {
                _talieColetorDescargaRepositorio.UpdateQuantidades(talieId, quantidade);

                _talieColetorDescargaRepositorio.ExcluirTalieITem(id);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Item excluído com sucesso";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetExcluirTalie(int id, int talieId, int quantidade)
        {
            try
            {
                _talieColetorDescargaRepositorio.ExcluirTalie(id);

                retornoJson.Mensagem = "Dados excluídos com sucesso";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram excluídos";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDadosNF(int id)
        {
            try
            {
                var query = _talieColetorDescargaRepositorio.GetDadosNF(id);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "As Nfs da talie não foram listadas";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetValidarFecharTalie(int talieId, string tipoDescarga, string conteinerId)
        {
            try
             {
                int countTalie = _talieColetorDescargaRepositorio.GetCountTalie(talieId);

                if (talieId == 0)
                {
                    retornoJson.Mensagem = "Finalize somente após o lançamento do talie";
                    retornoJson.objetoRetorno = "";
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (countTalie == 0)
                {
                    retornoJson.Mensagem = "Finalize somente após o lançamento da descarga";
                    retornoJson.objetoRetorno = "";
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (tipoDescarga == Band.Coletor.Redex.Business.Enums.OpcoesDescarga.CD.ToString())
                {
                    if (conteinerId == null)
                    {
                        retornoJson.Mensagem = "Descarga com Unitização é obrigatório informar contêiner";
                        retornoJson.objetoRetorno = "";
                        retornoJson.possuiDados = true;
                        retornoJson.statusRetorno = "200";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                    int talieNF = _talieColetorDescargaRepositorio.GetCountTalieNF(talieId);

                    if (talieNF != 0)
                    {
                        retornoJson.Mensagem = "Consta item sem vínculo de NF. Favor providenciar o cadastro da NF.";
                        retornoJson.objetoRetorno = "";
                        retornoJson.possuiDados = true;
                        retornoJson.statusRetorno = "200";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }
                }
                    string wNFs = "";

                    var nfLoop = _talieColetorDescargaRepositorio.GetNFByTalieId(talieId);

                    foreach (var item in nfLoop)
                    {
                        if (wNFs != "")
                        {
                            wNFs = wNFs + ",";
                        }
                    }

                    if (wNFs != "")
                    {

                        retornoJson.Mensagem = "A(s) NF(s) abaixo estão sem o vinculo do registro de entrada e isso gera impacto no processo de automatização da DUE <br/>  '" + wNFs + "' ";
                        retornoJson.objetoRetorno = "";
                        retornoJson.possuiDados = true;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                    int totalItens = 0;
                    int pendencias = 0;
                    int etiquetas = 0;

                    totalItens = _talieColetorDescargaRepositorio.GetCountTalie(talieId);

                    if (totalItens != 0)
                    {
                        etiquetas = _talieColetorDescargaRepositorio.countEtiquetas(talieId);
                        pendencias = _talieColetorDescargaRepositorio.countPendencias(talieId);
                    }

                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = new
                    {

                        totalItens = totalItens,
                        etiquetas = etiquetas,
                        pendencias = pendencias
                    };
                    retornoJson.possuiDados = true;
                    retornoJson.statusRetorno = "200";                    
                
                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "A talie não pode ser validada";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetComboEmbalagens()
        {
            try
            {
                var query = _talieColetorDescargaRepositorio.GetListarEmbalagens();

                retornoJson.objetoRetorno = query;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200'";
                retornoJson.Mensagem = "";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500'";
                retornoJson.Mensagem = "Os dados das embalagens não foram carregados";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
    }
}