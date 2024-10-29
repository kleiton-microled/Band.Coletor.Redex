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

namespace Band.Coletor.Redex.Site.Controllers
{
    public class IventarioCSController : DefaultController
    {
        private readonly IIventarioCNTRRepositorio _iventarioCNTRRepositorio;
        private readonly IIventarioCSRepositorio _iventarioCSRepositorio;
        private readonly IPatiosRepositorio _patiosRepositorio;
        private readonly IUsuarioLoginRepositorio _usuarioLoginRepositorio;

        public IventarioCSController(IIventarioCSRepositorio iventarioCSRepositorio, IIventarioCNTRRepositorio iventarioCNTRRepositorio, 
            IPatiosRepositorio patiosRepositorio, IUsuarioLoginRepositorio usuarioLoginRepositorio)
        {
            _iventarioCNTRRepositorio = iventarioCNTRRepositorio;
            _iventarioCSRepositorio = iventarioCSRepositorio;
            _patiosRepositorio = patiosRepositorio;
            _usuarioLoginRepositorio = usuarioLoginRepositorio;
        }
        [Authorize]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string patio = String.Empty;
                ViewBag.GetAutonumPatio = Convert.ToInt32(Session["AUTONUMPATIO"]);
                ViewBag.GetCarregaMotivos = _iventarioCNTRRepositorio.GetListarMotivos();
                ViewBag.GetCarregaArmazens = _iventarioCSRepositorio.GetConsultaArmazens(patio);

                return View();
            }
        }
        public JsonResult GetDadosLote(string lote)
        {
            try
            {
                //int patio = Convert.ToInt32(Session["AUTONUMPATIO"]);
                int patio = 2;
                bool redex = true;
                var query = _iventarioCSRepositorio.GetDadosPopulaLote(lote, patio, redex);

                if (query == null)
                {
                    retornoJson.Mensagem = "Não foram encontrados registros";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                }
                else
                {
                    retornoJson.Mensagem = "";
                    retornoJson.objetoRetorno = query;
                    retornoJson.possuiDados = true;
                }


                retornoJson.statusRetorno = "200";
                

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
               
                retornoJson.Mensagem = "Os dados não foram carregados, tente novamente";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetConsultarItensLote(string lote, int patio)
        {
            try
            {
                var query = _iventarioCSRepositorio.GetConsultarItensLote(lote, patio);

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
        public JsonResult GetDadosItem(string lote)
        {
            try
            {
                var query = _iventarioCSRepositorio.GetPopulaItem(lote);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)            
            {
                retornoJson.Mensagem = "Os itens não foram carregados, tente novamente";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SalvarDados(Band.Coletor.Redex.Business.DTO.IventarioCSDTO obj)
        {
            try
            {
                string id_gravacao = obj.ID_GRAVACAO;
                string RI = id_gravacao.Substring(0, 1);
                string UV = id_gravacao.Substring(1, 1);
                string tipo = id_gravacao.Substring(2, 1);
                long AutonumCs = 0;
                long AutonumCy = 0;
                int QtdeM = 0;
                string localPos = obj.LOCAL_POS;
                string yard = obj.YARD;
               



                var gravacao = _iventarioCSRepositorio.GetDadosGravacao(id_gravacao, UV);

                int quantidade = obj.QUANTIDADE;
                string sistema = obj.FINALITY;
                int cargaSolta = 0;
                int armazem = obj.AUTONUM_ARMAZEM;
                int idCargaSolta = 0;
                string ocupacao_CT = obj.OCUPACAO_CT;
                

                foreach (var item in gravacao)
                {
                    string ID_gravacao = item.ID_GRAVACAO;

                    if (QtdeM < quantidade)
                    {

                        if (tipo == "C")
                        {
                            AutonumCs = Convert.ToInt64(ID_gravacao.Substring(4, 10));                            
                        }
                        else if (tipo == "Y")
                        {
                            //AutonumCs = Convert.ToInt32(ID_gravacao.Substring(4, 7));
                            AutonumCs = 2222832;

                            if (sistema != "RDX")
                            {
                                cargaSolta = _iventarioCSRepositorio.getDadosCargaSoltaYard(AutonumCy);
                            }
                            else
                            {
                                AutonumCy = 2222832;
                                cargaSolta = _iventarioCSRepositorio.getDadosCargaSoltaYardByIDPatio(AutonumCy);
                            }
                            AutonumCs = Convert.ToInt64(cargaSolta);

                            int quantidadeAlocada = _iventarioCSRepositorio.getDadosQuantidadeAlocada(AutonumCs);
                            //int quantidadeEstoque = _iventarioCSRepositorio.getDadosQuantidadeEstoque(AutonumCs);
                            int quantidadeEstoque = 80;

                            int quantidadePOS = obj.QUANTIDADE;
                            int quantidadeSoma = quantidadeAlocada + quantidadePOS;

                            if (quantidadeSoma > quantidadeEstoque && (quantidadeAlocada != quantidadePOS))
                            {
                                retornoJson.Mensagem = "Item não informado";
                                retornoJson.objetoRetorno = null;
                                retornoJson.statusRetorno = "200";
                                retornoJson.possuiDados = true;

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }

                            if (sistema != "RDX")
                            {
                                string banco = "SGIPA";
                                if (localPos != "")
                                {

                                    idCargaSolta = _iventarioCSRepositorio.getIdCargaSolta(AutonumCs, armazem, yard, banco);
                                }
                                else
                                {
                                    yard = null;

                                    idCargaSolta = _iventarioCSRepositorio.getIdCargaSolta(AutonumCs, armazem, yard, banco);
                                }
                            }
                            else
                            {
                                string banco = "REDEX";
                                if (localPos != "")
                                {
                                    idCargaSolta = _iventarioCSRepositorio.getIdCargaSolta(AutonumCs, armazem, yard, banco);
                                }
                                else
                                {
                                    yard = null;
                                    idCargaSolta = _iventarioCSRepositorio.getIdCargaSolta(AutonumCs, armazem, yard, banco);
                                }
                            }
                            string soma = "+";

                            if (idCargaSolta == 0)
                            {
                                if (quantidadePOS > 0)
                                {
                                    if (localPos != "" && quantidadePOS > 0)
                                    {
                                        obj.AUTONUM_CS = Convert.ToInt32(AutonumCs);

                                        if (sistema == "RDX")
                                        {
                                            _iventarioCSRepositorio.InserirCargaSoltaYARD(obj, "REDEX");
                                        }
                                        else
                                        {
                                            if (UV != "U")
                                            {
                                                //obj.QUANT_POS = item.QTDE;
                                                obj.QUANT_POS = QtdeM;
                                            }
                                            _iventarioCSRepositorio.InserirCargaSoltaPatioCSYARD(obj, sistema);
                                        }
                                    }
                                    else
                                    {
                                        yard = null;
                                        if (sistema == "RDX")
                                        {
                                            _iventarioCSRepositorio.InserirCargaSoltaPatioCSYARD(obj, sistema);
                                        }
                                        else
                                        {
                                            if (UV != "U")
                                            {
                                                //obj.QUANT_POS = item.QTDE;
                                                obj.QUANT_POS = QtdeM;
                                            }

                                            _iventarioCSRepositorio.InserirCargaSoltaPatioCSYARD(obj, sistema);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (quantidadePOS > 0)
                                {

                                    if (sistema == "RDX")
                                    {
                                        obj.AUTONUM = idCargaSolta;
                                        _iventarioCSRepositorio.UpdateCargaSoltaYARD(obj, sistema, soma);
                                    }
                                    else
                                    {

                                        if (UV == "U")
                                        {
                                            obj.AUTONUM = idCargaSolta;
                                        }
                                        else
                                        {
                                            obj.AUTONUM = idCargaSolta;
                                            //obj.QUANT_POS = item.QTDE;
                                            obj.QUANT_POS = QtdeM;
                                        }
                                        _iventarioCSRepositorio.UpdateCargaSoltaYARD(obj, sistema, soma);
                                    }
                                }
                            }

                            if (tipo == "Y" && sistema != "RDX")
                            {
                                soma = "-";
                                obj.AUTONUM = Convert.ToInt32(AutonumCy);

                                _iventarioCSRepositorio.UpdateCargaSoltaYARD(obj, sistema, soma);
                            }

                            if (tipo == "Y" && sistema == "RDX")
                            {
                                if (UV == "U")
                                {
                                    soma = "-";
                                    obj.AUTONUM = Convert.ToInt32(AutonumCy);
                                    //obj.QUANT_POS = obj.OCU
                                    _iventarioCSRepositorio.UpdateCargaSoltaYARD(obj, "REDEX", soma);
                                }
                                else
                                {

                                }
                            }
                            int flagCT = obj.FLAG_CT;

                            if (flagCT == 1)
                            {
                                 _iventarioCSRepositorio.UpdateArmazemIPA(ocupacao_CT, armazem);
                            }
                        }
                    }
                }

                


                retornoJson.Mensagem = "Dados salvos com sucesso";
                retornoJson.objetoRetorno =  null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson = new ResponseJson();

                retornoJson.Mensagem = "Os dados não foram salvos";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetGravarRepetir(Band.Coletor.Redex.Business.DTO.IventarioCSDTO obj)
        {
            try
            {
                string id_gravacao = obj.ID_GRAVACAO;
                string RI = id_gravacao.Substring(1, 1);
                string UV = id_gravacao.Substring(2, 1);
                string tipo = id_gravacao.Substring(3, 1);
                long AutonumCs = 0;
                long AutonumCy = 0;
                int QtdeM = 0;
                string localPos = obj.LOCAL_POS;
                string yard = obj.YARD;
                string sistema = obj.FINALITY;
                int qtdePOS = obj.QUANT_POS;
                string schemaBD = "";
                string soma = "";

                if (sistema == "RDX")
                {
                    schemaBD = "REDEX";
                }
                else
                {
                    schemaBD = "SGIPA";
                }

                if (tipo == "C")
                {
                    AutonumCs = Convert.ToInt64(id_gravacao.Substring(2));
                }
                else if (tipo == "Y")
                {
                    AutonumCy = Convert.ToInt64(id_gravacao.Substring(2));
                    int idCS = 0;

                    if (sistema == "RDX")
                    {
                        idCS = _iventarioCSRepositorio.getDadosCargaSoltaYard(AutonumCy);
                    }
                    else
                    {
                        idCS = _iventarioCSRepositorio.getDadosCargaSoltaYardByIDPatio(AutonumCy);
                    }
                    
                    if (qtdePOS == 0)
                    {
                        if (sistema != "RDX")
                        {
                            
                            obj.ORIGEM = null;
                        }
                        else
                        {
                            
                            obj.ORIGEM = "I";
                        }

                        _iventarioCSRepositorio.InserirCargaSoltaYARD(obj, schemaBD);
                    }
                    else
                    {
                        obj.AUTONUM = idCS;
                        soma = "+";
                        _iventarioCSRepositorio.UpdateCargaSoltaYARD(obj, schemaBD, soma);
                    }

                }

                if (tipo == "Y")
                {
                    soma = "-";
                    obj.AUTONUM = Convert.ToInt32(AutonumCy);

                    _iventarioCSRepositorio.UpdateCargaSoltaYARD(obj, schemaBD, soma);
                }

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                retornoJson.Mensagem = "Os dados não foram gravados, por favor tente novamente";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLocalPos(string localPos)
        {
            try
            {
                var query =_iventarioCSRepositorio.GetArmazemYARD(localPos);
                
                retornoJson = new ResponseJson();

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados, tente novamente";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult countYard(string localPos)
        {
            try
            {
                int count = _iventarioCSRepositorio.countYard(localPos);

                var query = new
                {
                    count =  count,
                };

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
        public JsonResult GetSetaOcupacao(int id)
        {
            try
            {
                var query = _iventarioCSRepositorio.GetSetaOcupacao(Convert.ToInt64(id));

                retornoJson = new ResponseJson();

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";
                retornoJson.possuiDados = false;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
    }
}