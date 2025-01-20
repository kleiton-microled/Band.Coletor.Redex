namespace Band.Coletor.Redex.Entity
{
    public class TalieItem
    {
        public static TalieItem CreateNew(int id, string nF, int codigoEmbalagem, string embalagem, decimal? comprimento, decimal? largura, decimal? altura, 
            decimal peso, string imo, string imo2, string imo3, string imo4, string imo5, string uno, string uno2, string uno3, string uno4, string uno5, int qtdNf, 
            int qtdDescarga)
        {
            var item = new TalieItem();
            item.Id = id;
            item.NotaFiscal = nF;
            item.CodigoEmbalagem = codigoEmbalagem;
            item.Embalagem = embalagem;
            item.Comprimento = comprimento;
            item.Largura = largura;
            item.Altura = altura;
            item.Peso = peso;
            item.Imo = imo;
            item.Imo2 = imo2;
            item.Imo3 = imo3;
            item.Imo4 = imo4;
            item.Imo5 = imo5;
            item.Uno = uno;
            item.Uno2 = uno2;
            item.Uno3 = uno3;
            item.Uno4 = uno4;
            item.Uno5 = uno5;
            item.QtdNf = qtdNf;
            item.QtdDescarga = qtdDescarga;

            return item;
        }

        public int Id { get; set; }
        public int TalieId { get; set; }
        public string NotaFiscal { get; set; }
        public int IdNotaFiscal { get; set; }
        public int CodigoEmbalagem { get; set; }
        public string Embalagem { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Altura { get; set; }
        public decimal Peso { get; set; }
        public string Imo { get; set; }
        public string Imo2 { get; set; }
        public string Imo3 { get; set; }
        public string Imo4 { get; set; }
        public string Imo5 { get; set; }
        public string Uno { get; set; }
        public string Uno2 { get; set; }
        public string Uno3 { get; set; }
        public string Uno4 { get; set; }
        public string Uno5 { get; set; }
        public int QtdNf { get; set; }
        public int QtdDescarga { get; set; }
        public bool FlagMadeira { get; set; }
        public bool FlagFragil { get; set; }
        public string Remonte { get; set; }
        public string Fumigacao { get; set; }
        public int RegistroCargaSolta { get; set; }
    }
}
