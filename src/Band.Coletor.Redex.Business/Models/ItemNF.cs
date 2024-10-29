namespace Band.Coletor.Redex.Business.Models
{
    public class ItemNF
    {
        public int Id { get; set; }

        public int NFId { get; set; }

        public string Item { get; set; }

        public int ProdutoId { get; set; }
        public int EmbalagemId { get; set; }
    }
}