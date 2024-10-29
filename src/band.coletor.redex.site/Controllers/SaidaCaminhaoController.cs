using Band.Coletor.Redex.Business.Enums;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Site.Extensions;
using Band.Coletor.Redex.Site.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace Band.Coletor.Redex.Site.Controllers
{
    public class SaidaCaminhaoController : BaseController
    {
        private readonly ISaidaCaminhaoRepositorio _saidaCaminhaoRepositorio;

        public SaidaCaminhaoController(ISaidaCaminhaoRepositorio saidaCaminhaoRepositorio)
        {
            _saidaCaminhaoRepositorio = saidaCaminhaoRepositorio;
        }

        [HttpGet]
        public ActionResult Index(int? id) => View();

        [HttpPost]
        public ActionResult RegistrarSaida([Bind(Include = "PreRegistroId")] SaidaCaminhaoViewModel viewModel)
        {
            try
            {
                if (viewModel.PreRegistroId == 0)
                    return RetornarErro("Ocorreu um erro ao selecionar registro");
             
                _saidaCaminhaoRepositorio.RegistrarSaida(viewModel.PreRegistroId, User.ObterLocalPatio());
            }
            catch(Exception ex)
            {
                return RetornarErro("Erro durante o registro da saída do caminhão");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult ObterDadosCaminhao(string protocolo, string ano, string placa, string placaCarreta)
        {
            var dadosAgendamento = _saidaCaminhaoRepositorio.ObterDadosCaminhao(protocolo, ano, placa, placaCarreta, User.ObterLocalPatio() );

            if (dadosAgendamento == null)
            {
                return RetornarErro("Registro do caminhão não encontrado para os dados informados");
            }
            if (User.ObterLocalPatio() == LocalPatio.Patio)
            {
                if (!dadosAgendamento.GateOut)
                {
                    return RetornarErro("Caminhão sem registro de pesagem. É necessário pesar antes de sair!");
                }
            }
            if (string.IsNullOrWhiteSpace(placa))
            {
                placa = dadosAgendamento.Placa;
            }

            if (string.IsNullOrWhiteSpace(placaCarreta))
            {
                placaCarreta = dadosAgendamento.PlacaCarreta;
            }
                       

            if (User.ObterLocalPatio() == LocalPatio.Patio)
            {
                if (_saidaCaminhaoRepositorio.PendenciaSaida(placa) == null)
                {
                    return RetornarErro("Não foi localizado Registro de entrada para o Veículo");
                }
            }

            if (User.ObterLocalPatio() == LocalPatio.Estacionamento)
            {
                var pendenciaDeSaida = _saidaCaminhaoRepositorio.PendenciaDeSaida(placa);

                if (pendenciaDeSaida == null)
                    return RetornarErro("Não foi localizado Registro com entrada do Veículo");
            }

            return Json(new
            {
                dadosAgendamento.PreRegistroId,
                dadosAgendamento.Placa,
                dadosAgendamento.PlacaCarreta,
                dadosAgendamento.Ticket,
                dadosAgendamento.Protocolo,
                dadosAgendamento.PesoBruto,
                dadosAgendamento.GateIn,
                dadosAgendamento.GateOut
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
