using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Band.Coletor.Redex.Site.Models
{
    public class TalieItemViewModel
    {
        public TalieItemViewModel()
        {
            NotasFiscais = new List<NotaFiscal>();
            Itens = new List<TalieItem>();
            Comprimento = .7m;
            Largura = .55m;
            Altura = .15m;
            Peso = 50.152m;
            NFs = new List<NF>();
            Produtos = new List<Produto>();
            Embalagens = new List<Embalagem>();
        }

        public int TalieId { get; set; }

        [Display(Name = "Item")]
        public int TalieItemId { get; set; }

        public int RegistroId { get; set; }

        public int RegistroCsId { get; set; }

        [Display(Name = "Patio")]
        public int Patio { get; set; }

        [Display(Name = "Contêiner")]
        public int PatioId { get; set; }

        [Display(Name = "Contêiner")]
        public string ConteinerId { get; set; }

        public int BookingId { get; set; }

        [Display(Name = "Notas Fiscais")]
        public int NotaFiscalId { get; set; }

        [Display(Name = "Nota Fiscal")]
        public string NotaFiscal { get; set; }

        public decimal? Quantidade { get; set; }

        // public int EmbalagemId { get; set; }

        [Display(Name = "Emb.")]
        public string CodigoEmbalagem { get; set; }

        [Display(Name = "Descrição Embalagem")]
        public string DescricaoEmbalagem { get; set; }

        [Display(Name = "Comp.")]
        public decimal? Comprimento { get; set; }

        public decimal? Largura { get; set; }

        public decimal? Altura { get; set; }

        public decimal Peso { get; set; }

        public string IMO1 { get; set; }

        public string IMO2 { get; set; }

        public string IMO3 { get; set; }

        public string IMO4 { get; set; }

        public string UNO1 { get; set; }

        public string UNO2 { get; set; }

        public string UNO3 { get; set; }

        public string UNO4 { get; set; }

        public string Remonte { get; set; }

        public string Yard { get; set; }

        [Display(Name = "Fumigação")]
        public string Fumigacao { get; set; }

        [Display(Name = "Frágil")]
        public bool Fragil { get; set; }

        public bool Madeira { get; set; }

        public bool Avariado { get; set; }

        public string Descarga { get; set; }

        public bool EhCrossDocking { get; set; }

        [Display(Name = "Contêiner")]
        public string Conteiner { get; set; }

        [Display(Name = "NF")]
        public int NFId { get; set; }

        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }

        [Display(Name = "Qtde. NF")]
        public int QuantidadeNF { get; set; }

        [Display(Name = "Qtde. Descarga")]
        public int QuantidadeDescarga { get; set; }

        public string Placa { get; set; }

        [Display(Name = "Embalagem")]
        public int EmbalagemId { get; set; }

        public List<NotaFiscal> NotasFiscais { get; set; }

        public List<TalieItem> Itens { get; set; }

        public List<NF> NFs { get; set; }

        public List<Produto> Produtos { get; set; }

        public List<Embalagem> Embalagens { get; set; }
    }
}