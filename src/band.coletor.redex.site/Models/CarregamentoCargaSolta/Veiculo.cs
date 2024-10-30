using System.Collections.Generic;

namespace Band.Coletor.Redex.Site.Models.CarregamentoCargaSolta
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public static Veiculo Create(int id, string descricao)
        {
            var veiculo = new Veiculo();
            veiculo.Id = id;
            veiculo.Descricao = descricao;
            return veiculo;
        }
    }
}
