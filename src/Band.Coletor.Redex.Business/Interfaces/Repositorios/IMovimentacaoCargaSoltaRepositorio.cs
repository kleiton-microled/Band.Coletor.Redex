using Band.Coletor.Redex.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IMovimentacaoCargaSoltaRepositorio
    {
        string GetLoteArmazemMarcante(string marcante, int patio);
        MovimentacaoCargaSoltaDTO GetDadosInventArmazemCol_P(string marcante);
        MovimentacaoCargaSoltaItemDTO GetDadosInventArmazemCol_P_Item(string marcante);
        IEnumerable<MovimentacaoCargaSoltaItemDTO> GetDadosInventArmazemCol_P_Item_Lote(string lote, string marcante);
        int GetIDCsIpa(int id);
        int GetIDCsRedex(int id);
        int GetAutonumCS(string marcante);
        int GetNextValIPA();
        int GetNextValRedex();
        int InsertCargaSoltaYard(InserirNovaMovimentacaoCargaSoltaDTO movimentacaoCargaSoltaDTO);
        void UpdateArmazemIPA(int ocupacaoID, int armazemID);
        int InsertCargaSoltaYardHist(InserirNovaMovimentacaoCargaSoltaDTO movimentacaoCargaSoltaDTO);
        bool GetValida_Quantidade(int autonum_cs, double quantidade);
        int GetFlagCT(int id_armazem);
        YardDTO GetDadosYardBloqueio(int armazem, string yard);
        int GetAutonumPatioCS(int id);
        int GetAutonumPatioCsStp(int marcante);
        string ValidaRegrasArmazem(long autonum_cs, string pilha, long autonum_armazem);
        void GetAtualizaMarcante(int idCsYard, int marcante);
        int getNextValHistShiftingCs();
        int getInserHistShiftingCS(HIST_SHIFTING_CS hist_);
        int getAutonumByEtiqueta(string etiqueta);
        IEnumerable<InventarioCegoDTO> GetDadosEtiquetaByCodBar(string cod_bar, string pos, string pos1);
    }
}
