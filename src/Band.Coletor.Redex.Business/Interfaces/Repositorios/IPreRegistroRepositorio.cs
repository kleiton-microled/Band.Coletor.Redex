using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IPreRegistroRepositorio
    {
        int Cadastrar(PreRegistro preRegistro);
        Agendamento ObterDadosAgendamento(string protocolo, string ano, string placa, string placaCarreta, string sistema);
        Agendamento PendenciaDeSaida(string placa);
        Agendamento PendenciaDeSaidaEstacionamento(string placa);
        Agendamento PendenciaDeSaidaPatio(string placa);
        int PendenciaEntrada(string placa);
        void AtualizarDataChegada(int id);
        IEnumerable<PreRegistro> GetDadosPatio();
        int GetPendenciasSaidaPlaca(string placa);
        Agendamento GetDadosAgendamento(PreRegistro preRegistro);
    }
}
