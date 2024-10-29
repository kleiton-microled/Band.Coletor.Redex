using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class RegistroSaidaCaminhaoDTO
    {
        public int PreRegistroId { get; set; }

        public string Placa { get; set; }

        public string PlacaCarreta { get; set; }

        public string Protocolo { get; set; }

        public int PesoBruto { get; set; }

        public bool GateIn { get; set; }

        public bool GateOut { get; set; }
        public string Ticket { get; set; }
    }
}
