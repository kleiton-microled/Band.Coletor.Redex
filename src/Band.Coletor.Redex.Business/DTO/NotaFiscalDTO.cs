namespace Band.Coletor.Redex.Business.DTO
{
    public class NotaFiscalDTO
    {
        public string NumeroNF { get; set; }

        public string Mercadoria { get; set; }

        public string Embalagem { get; set; }

        public bool Descarregado { get; set; }

        public int Total { get; set; }
    }
}