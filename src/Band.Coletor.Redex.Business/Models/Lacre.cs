using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Lacre
    {
        public Lacre()
        {
        }

        public Lacre(string idConteiner, string lacreDescricao)
        {
            IdConteiner = idConteiner;
            LacreDescricao = lacreDescricao;
        }

        public Lacre(string idConteiner, int patio, string lacreDescricao, DateTime data, int validado)
        {
            IdConteiner = idConteiner;
            Patio = patio;
            LacreDescricao = lacreDescricao;
            Data = data;
            Validado = validado;
        }

        public string IdConteiner { get; set; }

        public string LacreDescricao { get; set; }

        public int Patio { get; set; }

        public string Usuario { get; set; }

        public DateTime Data { get; set; }

        public int Validado { get; set; }
    }
}