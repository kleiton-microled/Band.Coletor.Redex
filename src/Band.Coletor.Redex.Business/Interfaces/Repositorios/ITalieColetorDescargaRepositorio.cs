using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface ITalieColetorDescargaRepositorio
    {
        (IEnumerable<TalieDTO>, int TotalRecords) GetAllDadosTalie(string talie, string registro, string tipoDescarga, int pageNumber, int pageSize);
        TalieDTO GetTalieById(int id);
        TalieDTO GetTalieByRegistro(int id);
        IEnumerable<TalieDTO> GetAllDadosTalieItens(int id);
        int CadastrarTalie(Talie obj);
        Talie AtualizarTalie(Talie obj);
        TalieDTO getRegistroBusca(string id);
        TalieDTO GetDadosItemTalieId(int id);
        IEnumerable<ArmazensDTO> GetDadosNF(int id);
        #region Finalizar Talie
        int GetCountTalie(int id);
        Talie GetUpdateEtiqueta(int talie);
        Talie GetUpdateEtiqueta2(int talie);
        int getAutonumGate(int talie);
        int GetCountTalieNF(int id);
        IEnumerable<TalieDTO> GetDadosFinalizarTalie(int id);
        TalieDTO UpdateTalieAlertaEtiqueta(int alertaId, int talieId);
        string getBrutoByTbNewGate(int id);
        TalieDTO InsertTaliePatioCS(TalieDTO obj);
        TalieDTO UpdateTalieFlagFechado(int id);
        TalieDTO UpdateTalieFlagFinalizado(int idBoo);
        TalieDTO UpdateTalieStatusReserva(int idBoo);
        TalieDTO InsertCargaSolta(int autonum, int yard, int armazem);
        TalieDTO UpdateTbBookingCarga(int id, string IMO);
        TalieDTO insertDataSEQPATIOCS();
        TalieDTO insertDataSEQAMRGate();
        TalieDTO insertDataSEQRomaneioID();
        TalieDTO UpdateArmGate(TalieDTO obj);
        TalieDTO UpdatePatioPaiCS(int id);
        int countItensPatio(int id);
        TalieDTO UpdateFlagFechadoByTalieID(int id);
        TalieDTO UpdateFlagFechadoByBoo(int id);
        int GetCargaEntrada(int id);
        int GetQuantidadeEntrada(int id);        
        TalieDTO UpdateImoBookingCarga(string imo, int id);
        TalieDTO UpdateFlagFechadoByReserva(int id);
        int countTotalItensPatio(int id);
        int GetContainerId(int id);
        IEnumerable<TalieDTO> GetDadosPatioCS(int id);
        TalieDTO UpdatePatioEF(int id);
        TalieDTO UpdatePatioE(int id);
        long GetReservaCC(int id);
        long GetRomaneioId(int id);
        long GetCurrentIdRomaneio();
        TalieDTO InsertRomaneio(int usuarioId, int conteinerId, long reservaCC, long idRomaneio);
        TalieDTO InsertRomaneioCS(int autonum_pcs, long autonum_ro, int quantidade);
        string GetDataInicioTalie(int id);
        string GetDataTerminoTalie(int id);
        int GetEquipeTalie(int id);
        int GetConferenteTalie(int id);
        string GetFormaOperacaoTalie(int id);
        int countFlagCarregamentoTalie(int id);
        TalieDTO InserirTalieFechamento(int conteinerId, string dtInicio, string dtFim, long reserva, string modo, int conferenteId, int equipeId, long idRomaneio);
        long GetTalieCarregamentoId();
        TalieDTO UpdateRomaneioIdTalie(long talieId, long romaneioID);
        TalieDTO UpdateTalieByInicioTermino(int id, string inicio, string termino);
        TalieDTO InsertAMRNFSaida(int id, int nf, int quantidade);
        TalieDTO UpdateQuantidadeEstufada(int quantidade, int id);
        long GetSaidaCargaId();
        TalieDTO InsertSaidaCarga(long idCS, int autonumPCS, int quantidade, int autonumEmb, int pBruto, decimal altura, decimal comprimento, decimal largura, int volume, int conteinerId, string conteiner, string dtEstufagem, int autonumNF, int talieCarregamento, int autonum_ro);
        int GetSomaQuantidadeSaida(int id);
        TalieDTO UpdatePatioHistorico(int id);
        #endregion
        #region Talie Item
        long countQuantidadeDescarga(int id);
        TalieItem InsertTalieItem(long dif, int id);
        TalieItemDTO UpdateTalieItem(TalieItemDTO obj);
        #endregion
        #region descarga automática 
        IEnumerable<DescargaAutomaticaDTO> GetDadosDescargaAutomatica(int id);
        int GetMaxNotaFiscal(string id);
        double GetMaxPesoBruto(int id);
        double getBrutoByTalieId(int id);
        DescargaAutomaticaDTO InserirTalieItemDescargaAutomatica(DescargaAutomaticaDTO obj);
        #endregion 
        Talie ExcluirTalie(int id);
        TalieItem ExcluirTalieITem(int id);
        TalieItem UpdateQuantidades(int talieId, int quantidade);

        IEnumerable<TalieDTO> GetNFByTalieId(int id);
        int countEtiquetas(int id);
        int countPendencias(int id);
        int GetValidaConteiner(string id);
        TalieDTO GetTalieByIdConteiner(int id, string conteiner);
        IEnumerable<Embalagem> GetListarEmbalagens();
        double GetPesoBruto(int id);
        int GetCountPesoBruto(int id);
        long countQuantidadeTotalDescarga(int id);
    }
}
