using System;

namespace Band.Coletor.Redex.Business.DTO
{
    public class ConteinerDTO
    {
        public int Id { get; set; }
        public int IdConteiner { get; set; }

        public string Conteiner { get; set; }

        public DateTime Deadline { get; set; }
        public string YARD { get; set; }
        public int TAMANHO { get; set; }
        public int PATIO { get; set; }
        public string IMO1 { get; set; }
        public string IMO2 { get; set; }
        public string IMO3 { get; set; }
        public string IMO4 { get; set; }
        public int SEGREGACAO { get; set; }
        public string ID_CONTEINER { get; set; }
        public string ID_CONTEINER_D { get; set; }
        public string YARD_D { get; set; }
        public string IMO1_D { get; set; }
        public string IMO2_D { get; set; }
        public string IMO3_D { get; set; }
        public string IMO4_D { get; set; }
        public int DIST_DELTA { get; set; }
        public int CCNTR { get; set; }
    }
}