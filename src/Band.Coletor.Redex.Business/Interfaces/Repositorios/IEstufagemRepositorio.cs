using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IEstufagemRepositorio
    {
        IEnumerable<ConteinerDTO> ObterConteiners();
        DateTime ObterDataEntradaPorConteinerId(int conteinerId);
        IEnumerable<ClienteDTO> ObterClientesPorReserva(string resrva);
        IEnumerable<ConteinerDTO> ObterConteineresPorReservaCliente(string reserva, int cliente);
        int ObterQuantidadeSaidaPorConteinerId(int conteinerId);
        int ObterQuantidadePackingPorConteinerId(int conteinerId);
        int ObterIdTaliePorConteinerId(int conteinerId);
        int Gravar(Talie talie);
        void Atualizar(Talie talie);
        int ObterBookingCargaIdPorConteinerId(int conteinerId);
        int ObterBookingPorBookingCargaId(int bookingCargaId);
        int ObterQuantidadeCargaSoltaPorReserva(int reservaId);
        int ObterBookingCargaIdPorReservaId(int reservaId);
        int ObterConteineresEstufar(int reservaId);
        int ObterConteineresFechados(int reservaId);
        int ObterQuantidadeEstufada(int reservaId);
        void TransferirCargasDaReservaOrigemParaRservaEmbarque(int reservaId, int bookingargaId);
        void AtualizarTalieFechado(DateTime termino, int talieId);
        void AtualizarConteinerFechado(int conteinerId, string tipo);
        IEnumerable<Estufagem> ObterItensEstufadosPorTalieConteinerId(int talieId, int conteinerId);
        IEnumerable<Estufagem> ObterItensPorProduto(int produtoId);
        void AtualizarNFs(int quantidade, int nfItemId);
        int GravarCargaSaida(CargaEstufagem cargaEstufagem);
        void AtualizarPatioCS(int patioCSId);
        void AtualizaIntegraCarga(string produto);
        void ReabrirEstufagem(int conteinerId);
        void Descarga(int autonumNFI, int scId, int patioId, int qtdeSaida, int produtoId, bool cargaSuzano);
        int GetFlagTalieFechado(int id);
        int getIDNF(int romaneioId, string os);
        int GetValidaSaldoByOsRo(int romaneioId, int nf);
        IEnumerable<CargaEstufagem> GetDadosInsertEstufagem(int romaneio, int nf);
        IEnumerable<Estufagem> GetDadosNFByOS(string conteiner, int romaneioId, string os);
        Estufagem GetDadosNF(int id);
        Estufagem GetCarregaDadosConteiner(string conteiner);
        //Estufagem GetTalieId(int patio);
        Estufagem GetTalieId(int patio, int romaneio);
        Estufagem GetDadosClienteByReserva(string reserva);
        int countTalieByCarregamento(int patio);
        Estufagem GetCarregaDadosTalieById(int id);
        IEnumerable<Estufagem> GetItensEstufadosPorTalieConteinerId(int talie, int container);
        Estufagem GetSaldoAtualizado(int id);
        Estufagem GetDescarga(int autonumNFI, int scId, int patioId, int qtdeSaida, string produtoId, bool cargaSuzano);
        RomaneioDTO GetDadosRomaneio(int id);
        int countTalieByRomaneio(int id);
        int countTalieByPatio(int id);
        int FlagOpFULL(int id);
        int GravarTalie(int patio, string inicio, int boo, string formaOperacao, int conferente, int equipe, int idRomaneio, int gate);
        string GetDataInicioTalie(int id);
        RomaneioDTO UpdateRomaneio(int talie, int romaneio);
        TalieDTO UpdateSaidaCarga(int talie, int romaneio, int patio);
        Talie UpdateTalie(string Inicio, string Termino, int TalieID, int boo, int Romaneio, int conferente, int equipe, int gate, int patio, string modo);
        Estufagem FecharEstufagem(int talie, int romaneio,int conferente,int equipe);
        int getFechamentoIDTalieByRomaneio(int romaneio);
        int getFechamentoIDRomaneioByTalie(int talie);
        int getFechamentoConsisteNF(int romaneio);
        Estufagem GetDadosValidaFechamento(int id);
        Estufagem GetDadosValidaFechamentoParte2(int id);
        string GetDadosLacre(int id);
        int countPatiosByTpc(int id);
        int sumQuantidadeSaida(int id);
        int sumQuantidadeRomaneio(int romaneio);
        int sumQuantidadeSaidasCargas(int patio, int talie);
        Estufagem GerarCancelamento(int talie, int romaneio);

        IEnumerable<Estufagem> GetDadosNFByLote(string lote,string cntr);
        Estufagem GetTalieIdSemRomaneio(int patio);

        int CargaBagagem(string lote);
    }
}
