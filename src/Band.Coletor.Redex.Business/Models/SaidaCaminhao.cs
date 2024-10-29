using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Models
{
    public class SaidaCaminhao
    {
        public int Id { get; set; }

        public string Protocolo { get; set; }

        public string Placa { get; set; }

        public string PlacaCarreta { get; set; }

        public DateTime DataChegada { get; set; }

        public int IdReg { get; set; }
    }
}
