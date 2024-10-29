using Band.Coletor.Redex.Business.Enums;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Site.Extensions;
using Band.Coletor.Redex.Site.Models;
using System;
using System.Net;
using System.Web.Mvc;
using System.Linq;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class PreRegistroController : DefaultController
    {
        private readonly IPreRegistroRepositorio _preRegistroRepositorio;

        public PreRegistroController(IPreRegistroRepositorio preRegistroRepositorio)
        {
            _preRegistroRepositorio = preRegistroRepositorio;
        }

        [HttpGet]
        public ActionResult Cadastrar(int? id)
        {
            var patio = _preRegistroRepositorio.GetDadosPatio().ToList() ;

            return View(new PreRegistroViewModel
            {
                PatioDestino = patio,
            });

        }

        [HttpPost]
        public ActionResult Registrar([Bind(Include = "Protocolo,Placa,PlacaCarreta,Ticket")] PreRegistroViewModel viewModel)
        {
            if (viewModel.Placa.IsNullOrEmptyOrWhiteSpace() || viewModel.PlacaCarreta.IsNullOrEmptyOrWhiteSpace())
                return RetornarErro("Informe as Placas");
         

            try
            {
                if (User.ObterLocalPatio() == LocalPatio.Patio)
                {
                    var id = _preRegistroRepositorio.PendenciaEntrada(viewModel.Placa);

                    if (id > 0)
                    {
                        _preRegistroRepositorio.AtualizarDataChegada(id);
                    }
                    else
                    {
                        _preRegistroRepositorio.Cadastrar(
                          new PreRegistro
                          {
                              Protocolo = viewModel.Protocolo,
                              Placa = viewModel.Placa,
                              PlacaCarreta = viewModel.PlacaCarreta,
                              Ticket = viewModel.Ticket,
                              LocalPatio = LocalPatio.Patio,
                              DataChegada = DateTime.Now,
                              DataChegadaDeicPatio = null,
                              FlagDeicPatio = false,
                              FinalidadeId = viewModel.FinalidadeId, 
                              PatioDestinoId = viewModel.PatioDestinoId, 
                          });
                    }
                }

                if (User.ObterLocalPatio() == LocalPatio.Estacionamento)
                {
                    var id = _preRegistroRepositorio.Cadastrar(
                       new PreRegistro
                       {
                           Protocolo = viewModel.Protocolo,
                           Placa = viewModel.Placa,
                           PlacaCarreta = viewModel.PlacaCarreta,
                           Ticket = viewModel.Ticket,
                           LocalPatio = LocalPatio.Estacionamento,
                           DataChegada = null,
                           DataChegadaDeicPatio = DateTime.Now,
                           FlagDeicPatio = true,
                           FinalidadeId = viewModel.FinalidadeId,
                           PatioDestinoId = viewModel.PatioDestinoId,
                       });
                }
            }
            catch
            {
                return RetornarErro("Erro durante o registro do caminhão");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpGet]
        public JsonResult GetDadosAgendamento(PreRegistro preRegistro)
        {
            try
            {

                if (string.IsNullOrEmpty(preRegistro.Placa))
                {
                    retornoJson.Mensagem = "Preencha o numero da placa";
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = "500";

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }
                

                if (preRegistro == null) 
                {
                    retornoJson.Mensagem = "Não foram encontrados dados para o agendamento para a placa " + preRegistro.Placa;
                    retornoJson.objetoRetorno = null;
                    retornoJson.possuiDados = false;
                    retornoJson.statusRetorno = "404";

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }


                if (User.ObterLocalPatio() == LocalPatio.Patio)
                {
                    var pendenciaDeSaida = _preRegistroRepositorio.PendenciaDeSaidaEstacionamento(preRegistro.Placa);

                    if (pendenciaDeSaida != null)
                    {
                        retornoJson.Mensagem = "Existe pendencia de saída no Estacionamento para a placa " + preRegistro.Placa;
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);
                    }

                    var pendenciaDeSaidaPatio = _preRegistroRepositorio.PendenciaDeSaidaPatio(preRegistro.Placa);

                    if (pendenciaDeSaidaPatio != null) 
                    {
                        retornoJson.Mensagem = "Existe pendência de saída no Pátio para a placa " + preRegistro.Placa;
                        retornoJson.objetoRetorno = null;
                        retornoJson.possuiDados = false;
                        retornoJson.statusRetorno = "500";

                        return Json(retornoJson, JsonRequestBehavior.AllowGet);

                    }

                    if (User.ObterLocalPatio() == LocalPatio.Estacionamento)
                    {
                        pendenciaDeSaida = _preRegistroRepositorio.PendenciaDeSaida(preRegistro.Placa);

                        if (pendenciaDeSaida != null)
                        {
                            if (pendenciaDeSaida.DataChegadaDeicPatio != null && pendenciaDeSaida.DataSaidaDeicPatio == null)
                            {
                                retornoJson.Mensagem = "Existe pendência de saída no Estacionamento para a placa " + preRegistro.Placa;
                                retornoJson.objetoRetorno = null;
                                retornoJson.possuiDados = false;
                                retornoJson.statusRetorno = "500";

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }

                            if (pendenciaDeSaida.DataChegadaPatio != null && pendenciaDeSaida.DataSaidaPatio == null)
                            {
                                retornoJson.Mensagem  = "Existe pendência de saída no Pátio para a placa " + preRegistro.Placa;
                                retornoJson.objetoRetorno = null;
                                retornoJson.possuiDados = false;
                                retornoJson.statusRetorno = "500";

                                return Json(retornoJson, JsonRequestBehavior.AllowGet);
                            }   
                        }
                    }
                }

                var dadosAgendamento = _preRegistroRepositorio.GetDadosAgendamento(preRegistro);

                if (dadosAgendamento == null)
                {
                    retornoJson.Mensagem = "Não foram encontrados agendamentos para placa " + preRegistro.Placa;
                    retornoJson.objetoRetorno = null;
                    retornoJson.statusRetorno = "404";
                    retornoJson.possuiDados = false;

                    return Json(retornoJson, JsonRequestBehavior.AllowGet);
                }

                retornoJson.Mensagem = string.Empty;
                retornoJson.objetoRetorno = dadosAgendamento;
                retornoJson.statusRetorno = "200";
                retornoJson.possuiDados = true;

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) 
            {
                retornoJson.Mensagem = "Não foi possível consultar os dados ERRO " + ex.Message.ToString();
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ConfirmarEntradaSemAgendamento(string placaCavalo, string placaCarreta, string Ticketent, string finalidadeId, int patioId)
        {
            if (string.IsNullOrEmpty(placaCavalo))
                return RetornarErro("Informe a placa cavalo");

            //if (string.IsNullOrEmpty(placaCarreta))
            //    return RetornarErro("Informe a placa carreta");
            
            if(string.IsNullOrEmpty(finalidadeId))
                return RetornarErro("Selecione uma finalidade");

            if(patioId == 0)
                return RetornarErro("Selecione um pátio de destino");

            if (User.ObterLocalPatio() == LocalPatio.Patio)
            {
                var pendenciaDeSaida = _preRegistroRepositorio.PendenciaDeSaidaEstacionamento(placaCavalo);

                if (pendenciaDeSaida != null)
                    return RetornarErro("Existe pendência de saída no Estacionamento");

                var pendenciaDeSaidaPatio = _preRegistroRepositorio.PendenciaDeSaidaPatio(placaCavalo);

                if (pendenciaDeSaidaPatio != null)
                    return RetornarErro("Existe pendência de saída no Pátio");
            }

            if (User.ObterLocalPatio() == LocalPatio.Estacionamento)
            {
                var pendenciaDeSaida = _preRegistroRepositorio.PendenciaDeSaida(placaCavalo);

                if (pendenciaDeSaida != null)
                {
                    if (pendenciaDeSaida.DataChegadaDeicPatio != null && pendenciaDeSaida.DataSaidaDeicPatio == null)
                        return RetornarErro("Existe pendência de saída no Estacionamento");

                    if (pendenciaDeSaida.DataChegadaPatio != null && pendenciaDeSaida.DataSaidaPatio == null)
                        return RetornarErro("Existe pendência de saída no Pátio");
                }
            }

            try
            {
                if (User.ObterLocalPatio() == LocalPatio.Patio)
                {

                    var id1 = _preRegistroRepositorio.Cadastrar(
                          new PreRegistro
                          {
                              Placa = placaCavalo,
                              PlacaCarreta = placaCarreta,
                              Ticket = Ticketent,
                              LocalPatio = LocalPatio.Patio,
                              DataChegada = DateTime.Now,
                              DataChegadaDeicPatio = null,
                              FlagDeicPatio = false,
                              FinalidadeId = finalidadeId,
                              PatioDestinoId = patioId,
                          });
               }
               

                if (User.ObterLocalPatio() == LocalPatio.Estacionamento)
                {
                    var id2 = _preRegistroRepositorio.Cadastrar(
                       new PreRegistro
                       {

                           Placa = placaCavalo,
                           PlacaCarreta = placaCarreta,
                           Ticket = Ticketent,
                           LocalPatio = LocalPatio.Estacionamento,
                           DataChegada = null,
                           DataChegadaDeicPatio = DateTime.Now,
                           FlagDeicPatio = true,
                           FinalidadeId = finalidadeId,
                           PatioDestinoId = patioId,
                       });
                }
                 
            }
            catch
            {
                return RetornarErro("Erro durante o registro do caminhão");
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
        public JsonResult GetVerificaSaidaPlaca(string placa)
        {
            try
            {
                int countPendenciaSaida = _preRegistroRepositorio.GetPendenciasSaidaPlaca(placa);

                if (countPendenciaSaida == 0)
                {
                    retornoJson.Mensagem = "";
                }
                else
                {
                    retornoJson.Mensagem = "Por favor regularize a saída da movimentação anterior";
                }

                retornoJson.objetoRetorno = new
                {
                    pendenciaSaida = countPendenciaSaida
                };
                retornoJson.possuiDados = true;
                retornoJson.statusRetorno = "200";


                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                retornoJson.Mensagem = "Os dados não foram verificados";
                retornoJson.objetoRetorno = null;
                retornoJson.possuiDados = false;
                retornoJson.statusRetorno = "500";

                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
        }
    }
}