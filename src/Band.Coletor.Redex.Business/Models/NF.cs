using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Models
{
    public class NF
    {
        public int Id { get; set; }

        public string NumNF { get; set; }

        public IEnumerable<ItemNF> Itens { get; set; }

        public IEnumerable<Produto> Produtos { get; set; }

        public int QuantidadeItens { get; set; }
    }
}