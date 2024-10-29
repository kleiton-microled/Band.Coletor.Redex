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
    public class MovimentacaoCargaSoltaController : DefaultController
    {
        private readonly IMovimentacaoCargaSoltaRepositorio _movimentacaoCargaSoltaRepositorio;
        private readonly IArmazensRepositorio _armazensRepositorio;
        private readonly IMotivosRepositorio _motivosRepositorio;

        public MovimentacaoCargaSoltaController(IMovimentacaoCargaSoltaRepositorio movimentacaoCargaSoltaRepositorio, IArmazensRepositorio armazensRepositorio, IMotivosRepositorio motivosRepositorio)
        {
            _movimentacaoCargaSoltaRepositorio = movimentacaoCargaSoltaRepositorio;
            _armazensRepositorio = armazensRepositorio;
            _motivosRepositorio = motivosRepositorio;
        }
        [HttpGet]
        public ActionResult Index()
        {
            int id = User.ObterId();

            if (id == 0)
                return RedirectToAction("Login", "Home");

            ViewBag.Armazem = _armazensRepositorio.GetComboArmazens(User.ObterPatioColetorId());
            ViewBag.Motivos = _motivosRepositorio.GetComboMotivos();

            return View();
        }
        [HttpGet]
        public ActionResult MovimentacaoCargaSoltaSemMC()
        {
            ViewBag.Armazem = _armazensRepositorio.GetComboArmazens(User.ObterPatioColetorId());
            ViewBag.Motivos = _motivosRepositorio.GetComboMotivos();

            return View();
        }
        #region metodos json Index (movimentacao carga solta)        
        public JsonResult GetDadosByMarcante(string marcante)
        {
            try
            {
                int patio = User.ObterPatioColetorId();

                if (string.IsNullOrEmpty(marcante))
                {
                    retornoJson.Mensagem = messages.emptyInput("marcante");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.BadRequest();
                }

                if (marcante.Length >= 12)
                {
                    string autonum_CS = _movimentacaoCargaSoltaRepositorio.GetLoteArmazemMarcante(marcante, patio);

                    if (!string.IsNullOrEmpty(autonum_CS))
                    {
                        var query = _movimentacaoCargaSoltaRepositorio.GetDadosInventArmazemCol_P(marcante);

                        query.AUTONUM_CS = Convert.ToInt32(autonum_CS);

                        retornoJson.Mensagem = string.Empty;
                        retornoJson.objetoRetorno = query;
                        retornoJson.possuiDados = true;
                        retornoJson.statusRetorno = codes.Ok();

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }
                }

                if (marcante.Length == 7)
                {
                    marcante = marcante.PadLeft(12, Convert.ToChar("0"));

                    string lote = _movimentacaoCargaSoltaRepositorio.GetLoteArmazemMarcante(marcante, patio);

                    if (!string.IsNullOrEmpty(lote))
                    {
                        var query = _movimentacaoCargaSoltaRepositorio.GetDadosInventArmazemCol_P(lote);


                        retornoJson.Mensagem = string.Empty;
                        retornoJson.objetoRetorno = query;
                        retornoJson.possuiDados = true;
                        retornoJson.statusRetorno = codes.Ok();

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }
                }


                retornoJson.Mensagem = messages.noCarga();
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = codes.BadRequest();

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
        public JsonResult GetDadosByEtiqueta(string etiqueta, int armazem, string local)
        {
            try
            {
                int amr = 0;
                string pos = string.Empty;

                pos = etiqueta.Substring(2, etiqueta.Length - 2);

                amr = Convert.ToInt32(etiqueta.Substring(0, 1));

                int countPos = _movimentacaoCargaSoltaRepositorio.getAutonumByEtiqueta(pos);

                if (countPos == 0)
                {
                    retornoJson.Mensagem = messages.YardnotFound();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.BadRequest();
                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                retornoJson.Mensagem = string.Empty;
                retornoJson.objetoRetorno = new
                {
                    pos = pos,
                    idamr = amr,
                };
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "200";


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
        public JsonResult GetInserirMovimentacaoCS(InserirNovaMovimentacaoCargaSoltaDTO movimentacaoCargaSolta)
        {
            int autonum_cs = 0;
            int autonum_cy = 0;

            try
            {

                if (movimentacaoCargaSolta.AUTONUM_CS.ToString() == "0")
                {
                    retornoJson.Mensagem = messages.emptyInput("MARCANTE");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrEmpty(movimentacaoCargaSolta.ETIQUETA))
                {
                    retornoJson.Mensagem = messages.emptyInput("Localizacao");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }


                //if (string.IsNullOrEmpty(movimentacaoCargaSolta.QUANTIDADE))
                //{
                //    retornoJson.Mensagem = messages.emptyInput("quantidade");
                //    retornoJson.objetoRetorno = null;
                //    retornoJson.possuiDados = false;
                //    retornoJson.statusRetorno = codes.emptyInput();

                //    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //}

                //if (Convert.ToInt32(movimentacaoCargaSolta.QUANTIDADE_POS) > Convert.ToInt32(movimentacaoCargaSolta.QUANTIDADE_POS))
                //{
                //    retornoJson.Mensagem = messages.emptyIndisponivel();
                //    retornoJson.objetoRetorno = null;
                //    retornoJson.possuiDados = false;
                //    retornoJson.statusRetorno = codes.emptyInput();

                //    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //}

                //if (string.IsNullOrEmpty(movimentacaoCargaSolta.QUANTIDADE_POS) || Convert.ToInt32(movimentacaoCargaSolta.QUANTIDADE_POS) > 0)
                //{
                //    retornoJson.Mensagem = messages.emptyInput("quantidade");
                //    retornoJson.objetoRetorno = null;
                //    retornoJson.possuiDados = false;
                //    retornoJson.statusRetorno = codes.emptyInput();

                //    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //}

                //if (movimentacaoCargaSolta.ARMADOR == 0)
                //{
                //    retornoJson.Mensagem = messages.emptySelect("armador");
                //    retornoJson.objetoRetorno = null;
                //    retornoJson.possuiDados = false;
                //    retornoJson.statusRetorno = codes.emptyInput();

                //    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //}

                if (movimentacaoCargaSolta.MOTIVO_COL == 0)
                {
                    retornoJson.Mensagem = messages.emptySelect("Motivo");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                //if (_movimentacaoCargaSoltaRepositorio.GetFlagCT(Convert.ToInt32(movimentacaoCargaSolta.ARMAZEM)) == 1)
                //{
                //    if (string.IsNullOrEmpty(movimentacaoCargaSolta.LOCAL_POS))
                //    {
                //        retornoJson.Mensagem = messages.armazemCT();
                //        retornoJson.objetoRetorno = null;
                //        retornoJson.possuiDados = false;
                //        retornoJson.statusRetorno = codes.emptyInput();

                //        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //    }
                //}

                //if (_movimentacaoCargaSoltaRepositorio.GetFlagCT(Convert.ToInt32(movimentacaoCargaSolta.ARMAZEM)) == 0)
                //{
                //    if (string.IsNullOrEmpty(movimentacaoCargaSolta.LOCAL_POS))
                //    {
                //        retornoJson.Mensagem = messages.posicInput();
                //        retornoJson.objetoRetorno = null;
                //        retornoJson.possuiDados = false;
                //        retornoJson.statusRetorno = codes.emptyInput();

                //        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //    }
                //    else
                //    {
                //        var yard = _movimentacaoCargaSoltaRepositorio.GetDadosYardBloqueio(Convert.ToInt32(movimentacaoCargaSolta.ARMAZEM), movimentacaoCargaSolta.YARD);

                //        if (yard == null)
                //        {
                //            retornoJson.Mensagem = messages.posicInvalid();
                //            retornoJson.objetoRetorno = null;
                //            retornoJson.possuiDados = false;
                //            retornoJson.statusRetorno = codes.emptyInput();

                //            return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //        }
                //        else
                //        {
                //            if (yard.FLAG_BLOQUEIO == 1)
                //            {
                //                retornoJson.Mensagem = messages.posicBloq();
                //                retornoJson.objetoRetorno = null;
                //                retornoJson.possuiDados = false;
                //                retornoJson.statusRetorno = codes.emptyInput();

                //                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                //            }
                //        }
                //    }
                //}


                //if (movimentacaoCargaSolta.TIPO == "C")
                //{
                //    autonum_cs = Convert.ToInt32(movimentacaoCargaSolta.TIPO);
                //}
                //else if(movimentacaoCargaSolta.TIPO == "Y")
                //{
                //    autonum_cy = Convert.ToInt32(movimentacaoCargaSolta.TIPO);


                //    autonum_cs = _movimentacaoCargaSoltaRepositorio.GetAutonumPatioCS(autonum_cy);
                //    if (movimentacaoCargaSolta.SISTEMA == "R")
                //    {
                //        if (autonum_cs == 0)
                //        {
                //            if (movimentacaoCargaSolta.ID_MARCANTE > 0)
                //            {
                //                autonum_cs = _movimentacaoCargaSoltaRepositorio.GetAutonumCS(movimentacaoCargaSolta.ID_MARCANTE.ToString();                               
                //            }
                //        }
                //    }
                //}

                //string mensPatio = _movimentacaoCargaSoltaRepositorio.ValidaRegrasArmazem(autonum_cs, movimentacaoCargaSolta.YARD, movimentacaoCargaSolta.ARMADOR); 


                //if (mensPatio != "OK")
                //{
                //    retornoJson.Mensagem = messages.conflitoArmazem(mensPatio);
                //    retornoJson.objetoRetorno = new { id = movimentacaoCargaSolta.AUTONUM };
                //    retornoJson.possuiDados = false;
                //    retornoJson.statusRetorno = codes.BadRequest();
                //}

                string pos = string.Empty;



                pos = movimentacaoCargaSolta.ETIQUETA.Substring(2, movimentacaoCargaSolta.ETIQUETA.Length - 2);



                int countPos = _movimentacaoCargaSoltaRepositorio.getAutonumByEtiqueta(pos);

                if (countPos == 0)
                {
                    retornoJson.Mensagem = messages.YardnotFound();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.BadRequest();
                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }


                var inserirMovimentacaoCS = new InserirNovaMovimentacaoCargaSoltaDTO();

                inserirMovimentacaoCS.AUTONUM_CS = movimentacaoCargaSolta.AUTONUM_CS;

                inserirMovimentacaoCS.ARMAZEM = movimentacaoCargaSolta.ETIQUETA.Substring(0, 1);

                inserirMovimentacaoCS.LOCAL_POS = movimentacaoCargaSolta.ETIQUETA.Substring(2, 8);
                //inserirMovimentacaoCS.YARD = movimentacaoCargaSolta.YARD.ToString();
                inserirMovimentacaoCS.YARD = "";
                inserirMovimentacaoCS.QUANTIDADE = movimentacaoCargaSolta.QUANTIDADE_POS;
                inserirMovimentacaoCS.QUANTIDADE = "1";
                inserirMovimentacaoCS.MOTIVO_COL = movimentacaoCargaSolta.MOTIVO_COL;
                inserirMovimentacaoCS.ID_USUARIO = User.ObterId();

                // inserirMovimentacaoCS.ETIQUETA = inserirMovimentacaoCS.ARMAZEM + "-" + inserirMovimentacaoCS.YARD.Substring(2, inserirMovimentacaoCS.YARD.Length);


                var newID = _movimentacaoCargaSoltaRepositorio.InsertCargaSoltaYard(inserirMovimentacaoCS);

                if (newID > 0)
                {
                    var idhist = _movimentacaoCargaSoltaRepositorio.InsertCargaSoltaYardHist(inserirMovimentacaoCS);

                    retornoJson.Mensagem = messages.inserirSucess();
                    retornoJson.objetoRetorno = new { id = movimentacaoCargaSolta.AUTONUM };
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.Ok();
                }
                else
                {

                    retornoJson.Mensagem = messages.inserirFailed();
                    retornoJson.objetoRetorno = new { id = 0 };
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.BadRequest();
                }

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
        #endregion
        #region metodos Json MovimentacaoCargaSoltaSemMC
        public JsonResult GetInserirMovimentacaoCS_Sem_Marcante(InserirNovaMovimentacaoCargaSoltaDTO movimentacaoCargaSolta)
        {
            try
            {

                if (!string.IsNullOrEmpty(movimentacaoCargaSolta.LOTE))
                {
                    retornoJson.Mensagem = messages.emptyInput("Lote");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (movimentacaoCargaSolta.ITEM_CS == 0)
                {
                    retornoJson.Mensagem = messages.emptySelect("Item");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(movimentacaoCargaSolta.QUANTIDADE))
                {
                    retornoJson.Mensagem = messages.emptyInput("quantidade");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (Convert.ToInt32(movimentacaoCargaSolta.QUANTIDADE_POS) > Convert.ToInt32(movimentacaoCargaSolta.QUANTIDADE_POS))
                {
                    retornoJson.Mensagem = messages.emptyIndisponivel();
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(movimentacaoCargaSolta.QUANTIDADE_POS) || Convert.ToInt32(movimentacaoCargaSolta.QUANTIDADE_POS) > 0)
                {
                    retornoJson.Mensagem = messages.emptyInput("quantidade");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (movimentacaoCargaSolta.ARMADOR == 0)
                {
                    retornoJson.Mensagem = messages.emptySelect("armador");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (movimentacaoCargaSolta.MOTIVO_COL == 0)
                {
                    retornoJson.Mensagem = messages.emptySelect("armador");
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = codes.emptyInput();

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                if (_movimentacaoCargaSoltaRepositorio.GetFlagCT(Convert.ToInt32(movimentacaoCargaSolta.ARMAZEM)) == 1)
                {
                    if (string.IsNullOrEmpty(movimentacaoCargaSolta.LOCAL_POS))
                    {
                        retornoJson.Mensagem = messages.armazemCT();
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = codes.emptyInput();

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
        #endregion
    }
}