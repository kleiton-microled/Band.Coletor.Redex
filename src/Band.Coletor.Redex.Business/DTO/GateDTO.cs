using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class GateDTO
    {
        public int AUTONUM { get; set; }
        public string PLACA { get; set; }
        public string CARRETA { get; set; }
        public int Id_Motorista { get; set; }
        public int ID_TRANSPORTADORA { get; set; }
        public decimal TARA { get; set; }
        public int AUTONUM_PATIO { get; set; }
        public int AUTONUM_GATE { get; set; }
        public int AUTONUM_REG { get; set; }
        public int AUTONUM_CNTR { get; set; }
        public int AUTONUM_BOO { get; set; }
        public int AUTONUM_PCS { get; set; }
        public string funcao_gate_entrada { get; set; }
        public decimal BRUTO { get; set; }
        public DateTime DT_GATE_IN { get; set; }
        public DateTime DT_GATE_OUT { get; set; }
        public DateTime DATE { get; set; }
        public int UsuarioId { get; set; }
        public decimal Peso_Entrada { get; set; }
        public decimal Peso_Saida { get; set; }
        public int funcao_gate { get; set; }
        public int flag_historico { get; set; }
        public decimal PESO { get; set; }
        public string id_conteiner { get; set; }

        public string EF_SAIDA { get; set; }
        public int usuario { get; set; }

    }
}
