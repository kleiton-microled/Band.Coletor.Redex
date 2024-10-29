using System;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Models
{
    public class Registro
    {
        public int GateId { get; set; }

        public string Placa { get; set; }

        public int ExportadorId { get; set; }

        public string Exportador { get; set; }

        public string Referencia { get; set; }

        public int Reserva { get; set; }

        public int Lote { get; set; }

        public int Patio { get; set; }

        public DateTime Inicio { get; set; }

        public IEnumerable<TalieConteiner> Conteineres { get; set; }
    }
}