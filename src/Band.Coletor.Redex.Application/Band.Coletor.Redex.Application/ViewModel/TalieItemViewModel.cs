namespace Band.Coletor.Redex.Application.ViewModel
{
    public class TalieItemViewModel
    {
        public static TalieItemViewModel CreateNew(string codigoEmbalagem, string embalagem, string uno, string uno2, string uno3, string uno4, string uno5,
                                                   string imo, string imo2, string imo3, string imo4, string imo5, decimal pesoBruto, int quantidade, 
                                                   int codigoRegCs, string resumo
                                                   )
        {
            var view = new TalieItemViewModel();
            view.EmbalagemSigla = codigoEmbalagem;
            view.Embalagem = embalagem;
            view.UNO = uno;
            view.UNO2 = uno2;
            view.UNO3 = uno3;
            view.UNO4 = uno4;
            view.UNO5 = uno5;
            view.IMO = imo;
            view.IMO2 = imo2;
            view.IMO3 = imo3;
            view.IMO4 = imo4;
            view.IMO5 = imo5;
            view.Peso = pesoBruto;
            view.Quantidade = quantidade;
            view.RegistroCsId = codigoRegCs;
            view.Resumo = resumo;

            return view;
        }


        public int Id { get; set; }
        public int TalieId { get; set; }
        public int RegistroCsId { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public int PatioId { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Altura { get; set; }
        public decimal Peso { get; set; }
        public string Remonte { get; set; }
        public string Fumigacao { get; set; }
        public bool Fragil { get; set; }
        public bool Madeira { get; set; }
        public bool Avariado { get; set; }
        public string Yard { get; set; }
        public int NotaFiscalId { get; set; }
        public string NotaFiscal { get; set; }
        public string IMO { get; set; }
        public string IMO2 { get; set; }
        public string IMO3 { get; set; }
        public string IMO4 { get; set; }
        public string IMO5 { get; set; }
        public string UNO { get; set; }
        public string UNO2 { get; set; }
        public string UNO3 { get; set; }
        public string UNO4 { get; set; }
        public string UNO5 { get; set; }
        public string EmbalagemSigla { get; set; }
        public int CodigoEmbalagem { get; set; }
        public string Embalagem { get; set; }
        public int QtdDescarga { get; set; }
        public string Resumo { get; set; }
    }
}
