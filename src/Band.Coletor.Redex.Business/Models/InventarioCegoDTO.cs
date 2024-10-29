using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Models
{
    public class InventarioCegoDTO
    {
        public int AUTONUM { get; set; }
        public int AUTONUM_INV { get; set; }
        public int ID_ARMAZEM { get; set; }
        public string DESCR { get; set; }
        public string DESCRICAO { get; set; }
        public string PRATELEIRA { get; set; }
        public string HEAP { get; set; }
        public int PATIO { get; set; }
        public string MARCANTE { get; set; }
        public string YARD { get; set; }
        public int USUARIO { get; set; }
        public int AUTONUM_YARD { get; set; }
    }
}
