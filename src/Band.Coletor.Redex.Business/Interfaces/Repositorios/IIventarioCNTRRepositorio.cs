using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using Band.Coletor.Redex.Business.DTO;


namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IIventarioCNTRRepositorio
    {
        IEnumerable<MotivosDTO> GetListarMotivos();
        IEnumerable<VeiculosDTO> GetListarVeiculos(int id);
        IEnumerable<EmpilhadeirasDTO> GetListarEmpilhadeiras(int id);
        int GetFlagTruckMovColetor(int id);
        IventarioCNTRDTO GetIDContainerSubstring(int patio, string container);
        IventarioCNTRDTO GetIDContainer(int patio, string container);
        IventarioCNTRDTO GetBuscaCTNR(string id);
        IventarioCNTRDTO Insert_HIST_SHIFITING(IventarioCNTRDTO obj);
        ParametrosDTO GetEspacoMin();
        IEnumerable<IventarioCNTRDTO> GetDadosDistDelta(int idPatio, string idConteiner);
        YardDTO GetValidaYard(int idPatio, string Yard);
        IventarioCNTRDTO InserirCTNR(IventarioCNTRDTO obj);
        IventarioCNTRDTO UpdateCTNRBL(string yard, int id);
        IventarioCNTRDTO UpdatePatioByYard(string yard, int id);
        IventarioCNTRDTO UpdatePatioById(string yard, int id);
        IventarioCNTRDTO UpdateArmazemIPAByYard(string yard, int id);
        IventarioCNTRDTO GetInventSistemasXYZ(string id);
        IventarioCNTRDTO GetRegrasImoQuadra(int idPatio, string Yard);
        IventarioCNTRDTO GetRegrasImoPilhas(int idPatio, string Yard, int tamanho);
        IventarioCNTRDTO GetRegrasConteinerRua(int idPatio, string Yard);
        IventarioCNTRDTO getUpdateCNTR_BL(string yard, int id);
        IventarioCNTRDTO getUpdatePatio(string yard, int id);
        IventarioCNTRDTO GetUpdateArmazens(string yard, int id);
        IventarioCNTRDTO CountYardByID(int idPatio, string Yard);
    }
}
