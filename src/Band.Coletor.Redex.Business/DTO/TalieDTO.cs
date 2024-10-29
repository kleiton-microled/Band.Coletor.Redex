using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class TalieDTO
    {
        public int RegistroId { get; set; }

        public int Patio { get; set; }
        public DateTime? Inicio { get; set; }

        public DateTime? Termino { get; set; }

        public bool CrossDocking { get; set; }

        public int ConferenteId { get; set; }

        public int EquipeId { get; set; }

        public string ConteinerId { get; set; }

        public int BookingId { get; set; }

        public string OperacaoId { get; set; }
        public string Placa { get; set; }
        public string Reference { get; set; }
        public int GateId { get; set; }
        public bool EhEstufagem { get; set; }
        public string Observacoes { get; set; }

        public string Lote { get; set; }

        public string obs { get; set; }

        //

        //talies itens 

        public int TalieId { get; set; }

        public int RegistroCsId { get; set; }

        public string Descricao { get; set; }

        public decimal? Quantidade { get; set; }

        public int ProdutoId { get; set; }

        public int PatioId { get; set; }

        //public decimal? Comprimento { get; set; }

        //public decimal? Largura { get; set; }

        //public decimal? Altura { get; set; }

        public decimal COMPRIMENTO { get; set; }
        public decimal LARGURA { get; set; }
        public decimal ALTURA { get; set; }

        public decimal Peso { get; set; }
        public decimal PesoBruto { get; set; }
        public int TalieItemId { get; set; }
        public int QuantidadeEstufagem { get; set; }
        public string Remonte { get; set; }

        public string Fumigacao { get; set; }

        public bool Fragil { get; set; }

        public bool Madeira { get; set; }

        public bool Avariado { get; set; }

        public string Yard { get; set; }

        public int NotaFiscalId { get; set; }

        public string NotaFiscal { get; set; }

        public string IMO1 { get; set; }

        public string IMO2 { get; set; }

        public string IMO3 { get; set; }

        public string IMO4 { get; set; }

        public string UNO1 { get; set; }

        public string UNO2 { get; set; }

        public string UNO3 { get; set; }

        public string UNO4 { get; set; }

        public string CodigoProduto { get; set; }

        public string EmbalagemSigla { get; set; }

        public int EmbalagemId { get; set; }

        public string Embalagem { get; set; }

        public decimal QuantidadeDescarga { get; set; }

        //Grid Talies
        public int autonum_reg { get; set; }
        public int Id { get; set; }
        public DateTime INICIO { get; set; }
        public DateTime TERMINO { get; set; }
        public string id_conteiner { get; set; }
        public string reference { get; set; }
        public string instrucao { get; set; }
        public string fantasia { get; set; }

        //Finalizar TAlie
        public int AUTONUM_PCS { get; set; }
        public int AUTONUM_BCG { get; set; }
        public int QTDE_ENTRADA { get; set; }
        public int AUTONUM_EMB { get; set; }
        public int AUTONUM_PRO { get; set; }
        public int AUTONUM_BOO { get; set; }
        public string MARCA { get; set; }
        public int VOLUME_DECLARADO { get; set; }

        public string IMO { get; set; }
        public int BRUTO { get; set; }
        public double BRUTONF { get; set; }
        public DateTime DT_PRIM_ENTRADA { get; set; }
        public int FLAG_HISTORICO { get; set; }
        public int AUTONUM_REGCS { get; set; }
        public int AUTONUM_NF { get; set; }
        public int TALIE_DESCARGA { get; set; }
        public int QTDE_ESTUFAGEM { get; set; }
        public string YARD { get; set; }
        public int ARMAZEM { get; set; }
        public int AUTONUM_PATIO { get; set; }
        public int AUTONUM_PATIOS { get; set; }
        public int PATIO { get; set; }
        public string UNO { get; set; }
        public string CODPRODUTO { get; set; }
        public string FORMA_OPERACAO { get; set; }
        public int AUTONUM_TALIE { get; set; }
        public Int64 REGISTRO_BUSCA { get; set; }
        public int AMRIDGate { get; set; }
        public int AUDIT_94 { get; set; }
        public int FL_FRENTE { get; set; }
        public int FL_FUNDO { get; set; }
        public int FL_LE { get; set; }
        public int FL_LD { get; set; }
        public int AUTONUM_TI { get; set; }
        public int QTDE_DESCARGA { get; set; }
        public int AUTONUM_NFI { get; set; }
        public DateTime DATA_ESTUFAGEM { get; set; }
        public int AUTONUM_RO { get; set; }
        public string Cliente { get; set; }
        public int Fechado { get; set; }
        public int ClienteId { get; set; }
        public int Estufagem { get; set; }
        public string descricao_emb { get; set; }
        public int qtde { get; set; }
        public int qtde_estufagem { get; set; }
        public string num_nf { get; set; }
        public string serie_nf { get; set; }
        public string desc_produto { get; set; }
        public string tipo_descarga { get; set; }
        public int diferenca { get; set; }
        public int ITEM { get; set; }
        public int AUTONUM_PRODUTO { get; set; }
        public int AUTONUM_EMBALAGEM { get; set; }
        public int ItemTalie { get; set; }
        public int Carimbo { get; set; }
        public int Avaria { get; set; }
        public int ExportadorId { get; set; }
        public int Descarga { get; set; }
        public int Carregamento { get; set; }
        public string Conferente { get; set; }
        public string Equipe { get; set; }
        public int FLAG_REMONTE { get; set; }
        public int REMONTE { get; set; }
        public string CODIGO_CARGA { get; set; }
        public int autonum_gate { get; set; }
        public int BookingCargaId { get; set; }
        public int emb_reserva { get; set; }
        public string fcl_lcl { get; set; }
        public string marcacao { get; set; }
        public string observacao { get; set; }
        public int flag_numerada { get; set; }
    }
}
