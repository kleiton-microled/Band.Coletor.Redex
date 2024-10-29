using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Enums;
using Band.Coletor.Redex.Business.Models;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface ISaidaCaminhaoRepositorio
    {
        void RegistrarSaida(int preRegistroId, LocalPatio localPati);
        
        RegistroSaidaCaminhaoDTO ObterDadosCaminhao(string protocolo, string ano, string placa, string placaCarreta, LocalPatio local);

        RegistroSaidaCaminhaoDTO ObterDadosCaminhaoPorPreRegistroId(int preRegistroId);

        Agendamento PendenciaDeSaida(string placa);

        Agendamento PendenciaSaida(string placa);
    }
}
