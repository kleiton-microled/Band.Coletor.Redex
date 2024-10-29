using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IMovimentacaoContainerRepositorio
    {
        int GetFlagTruckMovColetor(int patio);
        MovimentacaoContainerDTO GetDadosCntr(string id_conteiner, int patio);
        string GetQualCam(int patio);
        IEnumerable<ConteinerDTO> GetDadosConteiner(string id_conteiner);
        string Valida_Aloca_Imo(ConteinerDTO conteiner);
        string Valida_NImo(ConteinerDTO conteiner);
        string ValidaRegrasPatio(string id_conteiner, string pilha);
        string Verifica_Regras_Seg_Imo_Delta(ConteinerDTO conteiner);
        string GetInserir(MovimentacaoContainerDTO conteiner);
        int GetCampoValida(int patio, string yard);
        int BuscaCntrPorVeiculo(int valor);
        ConteinerDTO countCntr(int patio, string id_conteiner);
        IEnumerable<ConteinerDTO> getInfoLote(string id_conteiner);
        int GetAutonum(string id_conteiner);
        bool Placa_Pendente(string placa);

        Reserva ConsultarReserva(string reserva);

        Reserva ConsultarCarga20(string Boo);
        Reserva ConsultarCarga40(string Boo);
        int ConsultaCNTREstoque(string cntr);

        int ConsultaBooking(int Boo);
        int CadastrarOS(string ano);

        int AtualizarBooking(string os, string ano, string boo);
        int Cadastrar(RegistrosViewModel viewModel);

        int UpdateRegistro(string boo, string IdConteiner, string Tam, string BCG, string TPC, string placa);
        string ConsultarMotorista(int id);
    }
}

