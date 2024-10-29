using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.DTO;
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
    public class IventarioCNTRController : DefaultController
    {
        private readonly IIventarioCNTRRepositorio _iventarioCNTRRepositorio;
        private readonly IPatiosRepositorio _patiosRepositorio;

        public IventarioCNTRController(IIventarioCNTRRepositorio iventarioCNTRRepositorio, IPatiosRepositorio patiosRepositorio)
        {
            _iventarioCNTRRepositorio = iventarioCNTRRepositorio;           
            _patiosRepositorio = patiosRepositorio;
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Login");
            }
            else
            {
                int patioId = Convert.ToInt32(Session["AUTONUMPATIO"]);
                ViewBag.ListarDadosMotivos = _iventarioCNTRRepositorio.GetListarMotivos();
                ViewBag.ListarEmpilhadeiras = _iventarioCNTRRepositorio.GetListarEmpilhadeiras(patioId);
                ViewBag.TruckMovColetor = _iventarioCNTRRepositorio.GetFlagTruckMovColetor(patioId);

                return View();
            }            
        }

        public JsonResult GetDadosBusca(string conteiner, string ctnr)
        {
            try
            {
                int patio = Convert.ToInt32(Session["AUTONUMPATIO"]);
                
                

                if (conteiner != "" && ctnr == "")
                {
                    var getIDConteinerSubs = _iventarioCNTRRepositorio.GetIDContainerSubstring(patio, conteiner);
                    int Q = getIDConteinerSubs.COUNT_CONTEINER;
                    string id_Conteiner = getIDConteinerSubs.ID_CONTEINER;

                    if (Q == 0)

                    {
                        retornoJson.Mensagem = "Nenhum Conteiner encontrado neste patio com este final";
                        retornoJson.possuiDados = false;
                        
                    }
                    else if (Q > 1)
                    {
                        retornoJson.Mensagem = "Mais de um Conteiner encontrado neste patio com este final. Informe a sigla completa";
                        retornoJson.possuiDados = false;
                        
                    }
                    else if(Q == 1)
                    {
                        var query = new
                        {
                            container = id_Conteiner,
                        };

                        retornoJson.objetoRetorno = query;
                        retornoJson.Mensagem = "";
                        retornoJson.possuiDados = false;

                    }
                    _iventarioCNTRRepositorio.GetBuscaCTNR(id_Conteiner);
                }
                else if (conteiner == "" && ctnr != "")
                {
                    var getIDContainer = _iventarioCNTRRepositorio.GetIDContainer(patio, ctnr);
                    var Z = getIDContainer.COUNT_CONTEINER;


                    if (Z == 0)
                    {
                        retornoJson.Mensagem = "Conteiner nao encontrado";
                        retornoJson.possuiDados = false;
                        retornoJson.objetoRetorno = null;
                    }
                    else if (Z == 1)
                    {
                        conteiner = getIDContainer.ID_CONTEINER;
                        ctnr = "";
                        var query = _iventarioCNTRRepositorio.GetBuscaCTNR(conteiner);

                        retornoJson.Mensagem = "";
                        retornoJson.possuiDados = true;
                        retornoJson.objetoRetorno = query;
                    }

                }        
                
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram carregados";
                retornoJson.possuiDados = false;
                retornoJson.objetoRetorno = null;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetInserirCTNR(IventarioCNTRDTO obj)
        {
            try
            {
                //colocar os metodos de validacao Valida_Aloca_Imo e Valida_NImo
                string sistema = obj.SISTEMA;
                string imo = obj.IMO1;
                int autonum = obj.AUTONUM;
                string yard = obj.YARD;
                int patio = Convert.ToInt32(Session["AUTONUMPATIO"]);
                string RetSeg = "";

                //_iventarioCNTRRepositorio.InserirCTNR(obj);


                if (sistema == "I")
                {
                    _iventarioCNTRRepositorio.UpdateCTNRBL(yard, patio);
                }
                else if (sistema == "R")
                {
                    _iventarioCNTRRepositorio.UpdatePatioByYard(yard, patio);
                }
                else if (sistema == "Z" || sistema == "A")
                {
                    _iventarioCNTRRepositorio.GetUpdateArmazens(yard, patio);
                }

                obj.CNTR = obj.AUTONUM;
                obj.ORIGEM = obj.YARD_ATUAL;
                obj.DESTINO = obj.YARD;
                //obj.MOTIVO = obj.DESCR_MOTIVO_POSIC, 


                _iventarioCNTRRepositorio.Insert_HIST_SHIFITING(obj);

                if (imo != "") 
                {
                    //Colocar o metodo do vbnet Verifica_Regras_Seg_Imo_Delta

                    if (RetSeg != "")
                    {
                        if (sistema == "I")
                        {
                            _iventarioCNTRRepositorio.UpdateCTNRBL(yard, patio);
                        }
                        else if (sistema == "O")
                        {
                            _iventarioCNTRRepositorio.UpdatePatioById(yard, patio);
                        }
                        else if (sistema == "R")
                        { 
                            _iventarioCNTRRepositorio.UpdatePatioByYard(yard, patio);
                        }
                        else if (sistema == "A")
                        {
                            _iventarioCNTRRepositorio.GetUpdateArmazens(yard, patio);
                        }
                    }
                }


                retornoJson.Mensagem = "Dados inseridos com sucesso";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                retornoJson.Mensagem = "Os dados não foram inseridos";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBuscaByYard(string yard)
        {
            try
            {
                int patio = Convert.ToInt32(Session["AUTONUMPATIO"]);
                
                var query = _iventarioCNTRRepositorio.CountYardByID(patio, yard);

                retornoJson.Mensagem = "";
                retornoJson.objetoRetorno = query;
                retornoJson.possuiDados = true;
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
    }
}