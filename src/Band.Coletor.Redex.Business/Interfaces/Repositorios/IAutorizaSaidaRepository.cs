using Band.Coletor.Redex.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IAutorizaSaidaRepository
    {
        int Verifica_Saida(string cntr);
        int GetAutonumGate(string placa);
        int GetIDFuncaoGateIN(string placa);
        IEnumerable<AutorizaSaidaDTO> GetDadosVerificaSaida(int gate);
        AutorizaSaidaDTO GetDadosVerificaSaidaFD(string gate);
        AutorizaSaidaDTO GetDadosRomaneioById(int id);
        AutorizaSaidaDTO GetAutonumReg(int id);
        void UpdatePatioGateById(int gate, int autonum_patio);
        void UpdateSaidaCargaByRomaneioId(int id);
        void UpdateRomaneioById(int id);
        GateDTO GetDadosGateNewById(int id);
        void UpdateDadosTbRegistro(GateDTO gateDTO);
        IEnumerable<GateDTO> GetDadosPatioBooCntrByIdGate(int id);
        void UpdateDadosTbGateNew(GateDTO gateDTO);
        long Amr_Gate_RDX(long Gate, long Cntr_Rdx, long Peso_Entrada, long Peso_Saida, string Data, long Booking, long IdReg, string Funcao_Gate, byte Historico);
        void UpdateTbPatio(GateDTO gateDTO);
        void UpdateTbPatio207(GateDTO gateDTO);
        void UpdateTbRegistroByPlaca(string placa);
        void UpdateTbRegistroByConteiner(string op, string unidades, string placa);
        IEnumerable<GateDTO> GetDadosPatioBooByIdGate(int id);
        decimal GetPesoTaraByGateNew(int id);
        void UpdateDadosTbGateNew207(GateDTO gateDTO);
        void UpdateTbTalieByIdPatio(int gate, int autonum_patio);
        void UpdateTbRomaneioByIdPatio(int gate, int autonum_patio);
        void UpdateTbSaidaCargaByIdPatio(int gate, string data_saida, int autonum_patio);
        IEnumerable<GateDTO> GetAutonumPcsByIdPatio(int id);
        void UpdateUltSaidaTbPatioCsById(int id, string data);
        void UpdatePrimSaidaTbPatioCsById(int id, string data);
        RomaneioDTO GetDadosRomaneioByPatioId(int id);
        long InsertNewTalie(RomaneioDTO romaneio);
        void UpdateTbRomaneioTalieById(int talie_id, int id);
        int GetQuantidadeEntrada(int id);
        int GetQuantidadesSaida(int id);
        void UpdateTbPatioFlagHistorico(int id);
        void UpdateTbPregistroByPlaca(string placa);
        void UpdateTbRegistroCntrSaida(string operacaoUnidades, string placa);
    }
}
