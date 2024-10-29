using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class EstufagemDTO
    {
        public int TalieId { get; set; }

        public string Reserva { get; set; }

        public int ClienteId { get; set; }

        public int PatioId { get; set; }

        public int ConteinerId { get; set; }

        public string Conteiner { get; set; }

        public DateTime? Inicio { get; set; }

        public DateTime? Termino { get; set; }

        public int ConferenteId { get; set; }

        public int EquipeId { get; set; }

        public string OperacaoId { get; set; }

        public string Marcante { get; set; }

        public int Quantidade { get; set; }

        public int itemNF { get; set; }

        public int NF { get; set; }

        public string Lote { get; set; }
        public bool CargaSuzano { get; set; }
        public bool CrossDocking { get; set; }

        public int BookingId { get; set; }
        
    }
}
