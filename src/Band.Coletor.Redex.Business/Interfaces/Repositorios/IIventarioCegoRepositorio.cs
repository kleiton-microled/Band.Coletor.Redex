using Band.Coletor.Redex.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IIventarioCegoRepositorio
    {
        IEnumerable<InventarioCegoDTO> getDadosByIdPatio(int patio);
        IEnumerable<InventarioCegoDTO> getDadosById(int id, int patio);
        IEnumerable<InventarioCegoDTO> GetCarregaGridCarga(int id);
        void GetAtualizaDados(InventarioCegoDTO inventario);
        void GetInsertDados(InventarioCegoDTO inventario);
        int GetAutonumByTbArmazens(int armador);
        int GetAutonumByTbYard(int armazem, int yard);
        InventarioCegoDTO GetDadosInventarioAberto(int id);
        IEnumerable<InventarioCegoDTO> GetDadosInventario(string cod_bar, string pos, string pos1);
    }
}

