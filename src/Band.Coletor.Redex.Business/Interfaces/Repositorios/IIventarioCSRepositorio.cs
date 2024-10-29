using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.DTO;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IIventarioCSRepositorio
    {
        IventarioCSDTO GetDadosPopulaLote(string Lote, int patio, bool redex);
        IEnumerable<ArmazensDTO> GetConsultaArmazens(string patio);
        IventarioCSDTO GetPopulaItem(string id);
        IventarioCSDTO GetSetaOcupacao(Int64 id);
        IEnumerable<IventarioCSDTO> GetDadosGravacao(string id, string UV);
        int getDadosCargaSoltaYard(long id);
        int getDadosCargaSoltaYardByIDPatio(long id);
        int getDadosQuantidadeAlocada(long id);
        int getDadosQuantidadeEstoque(long id);
        int getIdCargaSolta(long autonumCs, int armazem, string yard, string banco);
        IventarioCSDTO InserirCargaSoltaYARD(IventarioCSDTO obj, string sistema);
        IventarioCSDTO InserirCargaSoltaPatioCSYARD(IventarioCSDTO obj, string sistema);
        IventarioCSDTO UpdateCargaSoltaYARD(IventarioCSDTO obj, string schema, string soma);
        IventarioCSDTO UpdateArmazemIPA(string ocupacao, int id);       
        IventarioCSDTO GetArmazemYARD(string id);
        int countYard(string id);
        IEnumerable<ArmazensDTO> GetConsultarItensLote(string lote, int patio);
    }
}
