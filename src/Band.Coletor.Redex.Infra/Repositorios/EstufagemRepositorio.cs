using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Extensions;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class EstufagemRepositorio : IEstufagemRepositorio
    {
        public void AtualizaIntegraCarga(string produto)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Produto", value: produto, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..tb_integra_carga
                        SET
                            ESTUFADO = 1
                    WHERE codbarra = @Produto", parametros);
            }
        }

        public void AtualizarConteinerFechado(int conteinerId, string tipo)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ConteinerId", value: conteinerId, direction: ParameterDirection.Input);
                parametros.Add(name: "Tipo", value: tipo, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..TB_PATIO
                        SET
                            ef = @Tipo
                    WHERE autonum_patio = @ConteinerId", parametros);
            }
        }

        public void AtualizarNFs(int quantidade, int nfItemId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Quantidade", value: quantidade, direction: ParameterDirection.Input);
                parametros.Add(name: "NFItem", value: nfItemId, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..TB_NOTAS_ITENS
                        SET
                            QTDE_ESTUFADA = NVL(QTDE_ESTUFADA,0) + @Quantidade
                    WHERE AUTONUM_NFI = @NFItem", parametros);
            }
        }

        public void AtualizarPatioCS(int patioCSId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "PatioCSId", value: patioCSId, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..tb_patio_cs
                        SET
                            FLAG_HISTORICO = 1
                    WHERE autonum_pcs = @PatioCSId", parametros);
            }
        }

        public void AtualizarTalieFechado(DateTime termino, int talieId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Termino", value: termino, direction: ParameterDirection.Input);
                parametros.Add(name: "DataFechamento", value: termino, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: talieId, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..TB_TALIE
                        SET
                            FLAG_FECHADO=1,
                            TERMINO = @Termino,
                            DT_FECHAMENTO = @Termino
                    WHERE AUTONUM_TALIE = @Id", parametros);
            }
        }

        public void Descarga(int autonumNFI, int scId, int patioId, int qtdeSaida, int produtoId, bool cargaSuzano)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    var parametros = new DynamicParameters();

                    parametros.Add(name: "NFIId", value: autonumNFI, direction: ParameterDirection.Input);
                    parametros.Add(name: "SCId", value: scId, direction: ParameterDirection.Input);
                    parametros.Add(name: "PatioId", value: patioId, direction: ParameterDirection.Input);
                    parametros.Add(name: "QtdeSaida", value: qtdeSaida, direction: ParameterDirection.Input);
                    parametros.Add(name: "ProdutoId", value: produtoId, direction: ParameterDirection.Input);

                    con.Execute(@"
                        DELETE
                            REDEX..TB_AMR_NF_SAIDA
                        WHERE 
                            autonum_nfi = @NFIId AND autonum_patio = @PatioId ", parametros, transaction);

                    con.Execute(@"
                        UPDATE
                            REDEX..TB_NOTAS_ITENS
                            SET
                                QTDE_ESTUFADA = qtde_estufada - @QtdeSaida
                            WHERE 
                                AUTONUM_NFI = @NFIId", parametros, transaction);

                    con.Execute(@"
                        DELETE
                            REDEX..tb_saida_carga
                        WHERE 
                            autonum_sc = @SCId ", parametros, transaction);

                    if (cargaSuzano)
                    {
                        con.Execute(@"
                        UPDATE
                            REDEX..tb_integra_carga
                            SET
                                estufado = 0
                            WHERE 
                                codbarra = @ProdutoId", parametros, transaction);
                    }


                    con.Execute(@"
                        UPDATE
                            REDEX..tb_patio
                            SET
                                ef = 'E'
                            WHERE 
                                autonum_patio = @PatioId", parametros, transaction);
                    transaction.Commit();
                }
            }
        }

        public int Gravar(Talie talie)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ConteinerId", value: talie.ConteinerId, direction: ParameterDirection.Input);
                parametros.Add(name: "Inicio", value: talie.Inicio, direction: ParameterDirection.Input);
                parametros.Add(name: "CrossDocking", value: talie.CrossDocking.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "BookingId", value: talie.BookingId, direction: ParameterDirection.Input);
                parametros.Add(name: "OperacaoId", value: talie.OperacaoId, direction: ParameterDirection.Input);
                parametros.Add(name: "ConferenteId", value: talie.ConferenteId, direction: ParameterDirection.Input);
                parametros.Add(name: "EquipeId", value: talie.EquipeId, direction: ParameterDirection.Input);
                parametros.Add(name: "RegistroId", value: talie.RegistroId, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                return con.Query<int>(@"
                    INSERT INTO
	                    REDEX..TB_TALIE
                            (
                                AUTONUM_PATIO,
                                INICIO,
                                FLAG_ESTUFAGEM,
                                CROSSDOCKING,
                                AUTONUM_BOO,
                                FORMA_OPERACAO,
                                CONFERENTE,
                                EQUIPE
                            ) VALUES (
                                @ConteinerId,
                                @Inicio,
                                1,
                                @CrossDocking,
                                @BookingId,
                                @OperacaoId,
                                @ConferenteId,
                                @EquipeId
                            ); SELECT AUTONUM_TALIE = @RegistroId FROM TB_TALIE;", parametros).FirstOrDefault();
            }
        }

        public void Atualizar(Talie talie)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ConteinerId", value: talie.ConteinerId, direction: ParameterDirection.Input);
                parametros.Add(name: "Inicio", value: talie.Inicio, direction: ParameterDirection.Input);
                parametros.Add(name: "CrossDocking", value: talie.CrossDocking.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "BookingId", value: talie.BookingId, direction: ParameterDirection.Input);
                parametros.Add(name: "OperacaoId", value: talie.OperacaoId, direction: ParameterDirection.Input);
                parametros.Add(name: "ConferenteId", value: talie.ConferenteId, direction: ParameterDirection.Input);
                parametros.Add(name: "EquipeId", value: talie.EquipeId, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: talie.Id, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..TB_TALIE
                        SET
                            AUTONUM_PATIO = @ConteinerId,
		                    INICIO = @Inicio,
		                    CROSSDOCKING = @CrossDocking,
                            AUTONUM_BOO = @BookingId,
		                    FORMA_OPERACAO = @OperacaoId,
		                    CONFERENTE = @ConferenteId,
		                    EQUIPE = @EquipeId
                    WHERE AUTONUM_TALIE = @Id", parametros);
            }
        }

        public int GravarCargaSaida(CargaEstufagem cargaEstufagem)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("  exec redex.dbo.sp_nextval 'seq_SAIDA_CARGA' ");

                int autonum_SC = con.Query<int>(sb.ToString()).FirstOrDefault();

                sb.Clear();               

                var parametros = new DynamicParameters();

                parametros.Add(name: "AutonumSC", value: autonum_SC, direction: ParameterDirection.Input);
                parametros.Add(name: "PatioCSId", value: cargaEstufagem.PatioCSId, direction: ParameterDirection.Input);
                parametros.Add(name: "Quantidade", value: cargaEstufagem.Quantidade, direction: ParameterDirection.Input);
                parametros.Add(name: "EmbalagemId", value: cargaEstufagem.EmbalagemId, direction: ParameterDirection.Input);
                parametros.Add(name: "Bruto", value: cargaEstufagem.Bruto, direction: ParameterDirection.Input);
                parametros.Add(name: "Altura", value: cargaEstufagem.Altura, direction: ParameterDirection.Input);
                parametros.Add(name: "Comprimento", value: cargaEstufagem.Comprimento, direction: ParameterDirection.Input);
                parametros.Add(name: "Largura", value: cargaEstufagem.Largura, direction: ParameterDirection.Input);
                parametros.Add(name: "Volume", value: cargaEstufagem.VolumeTotal, direction: ParameterDirection.Input);
                parametros.Add(name: "ConteinerId", value: cargaEstufagem.PatioId, direction: ParameterDirection.Input);
                parametros.Add(name: "Conteiner", value: cargaEstufagem.Conteiner, direction: ParameterDirection.Input);
                parametros.Add(name: "ProdutoId", value: cargaEstufagem.ProdutoId, direction: ParameterDirection.Input);
                parametros.Add(name: "Inicio", value: Convert.ToDateTime(cargaEstufagem.Inicio), direction: ParameterDirection.Input);                
                parametros.Add(name: "NFId", value: cargaEstufagem.NFItemId, direction: ParameterDirection.Input);
                parametros.Add(name: "TalieId", value: cargaEstufagem.TlieId, direction: ParameterDirection.Input);
                parametros.Add(name: "Produto", value: cargaEstufagem.Produto, direction: ParameterDirection.Input);
                parametros.Add(name: "AUTONUM_RCS", value: cargaEstufagem.AUTONUM_RCS, direction: ParameterDirection.Input);
                parametros.Add(name: "autonum_ro", value: cargaEstufagem.autonum_ro, direction: ParameterDirection.Input);


                return con.Query<int>(@"
                    INSERT INTO
	                    REDEX..TB_SAIDA_CARGA
                            (
                                AUTONUM_SC, 
                                AUTONUM_PCS,
                                AUTONUM_RCS,
                                autonum_ro, 
                                QTDE_SAIDA,
                                AUTONUM_EMB,
                                PESO_BRUTO,
                                ALTURA,
                                COMPRIMENTO,
                                LARGURA,
                                VOLUME,
                                autonum_patio,
                                ID_CONTEINER,
                                MERCADORIA,
                                DATA_ESTUFAGEM,
                                autonum_nfi,
                                autonum_talie,
                                CODPRODUTO
                            ) VALUES (
                                @AutonumSC,
                                @PatioCSId,
                                @AUTONUM_RCS,
                                @autonum_ro,
                                @Quantidade,
                                @EmbalagemId,
                                @Bruto,
                                @Altura,
                                @Comprimento,
                                @Largura,
                                @Volume,
                                @ConteinerId,
                                @Conteiner,
                                @ProdutoId,                                
                                @Inicio,
                                @NFId,
                                @TalieId,
                                @Produto
                            );", parametros).FirstOrDefault();
            }
        }

        public int ObterBookingCargaIdPorConteinerId(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "conteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                       AUTONUM_BCG
                    FROM
	                    REDEX..TB_PATIO 
                    WHERE
	                    autonum_patio = @conteinerId", parametros).FirstOrDefault();
            }
        }

        public int ObterBookingCargaIdPorReservaId(int reservaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ReservaId", value: reservaId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                       AUTONUM_BCG
                    FROM
	                    REDEX..TB_BOOKING_CARGA 
                    WHERE
	                    autonum_boo = @ReservaId AND FLAG_CS=1", parametros).FirstOrDefault();
            }
        }

        public int ObterBookingPorBookingCargaId(int bookingCargaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "BookingCargaId", value: bookingCargaId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                       AUTONUM_BOO
                    FROM
	                    REDEX..TB_BOOKING_CARGA 
                    WHERE
	                    AUTONUM_BCG = @BookingCargaId", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<ClienteDTO> ObterClientesPorReserva(string reserva)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "reserva", value: reserva, direction: ParameterDirection.Input);

                return con.Query<ClienteDTO>(@"
                    SELECT
                        AUTONUM as Id, 
                        FANTASIA as NomeFantasia
                    FROM
	                    REDEX..TB_BOOKING BOO
                    INNER JOIN
	                    REDEX..TB_CAD_PARCEIROS CP ON BOO.AUTONUM_PARCEIRO = CP.AUTONUM
                    WHERE
	                    BOO.REFERENCE = @reserva", parametros);
            }
        }

        public int ObterConteineresEstufar(int reservaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ReservaId", value: reservaId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                         count(*)
                    FROM
	                    REDEX..TB_PATIO CC
                    INNER JOIN
	                    REDEX..tb_booking_carga bcg on cc.autonum_bcg=bcg.autonum_bcg
                    WHERE
	                    bcg.autonum_boo = @ReservaId AND FLAG_CNTR=1 ", parametros).FirstOrDefault();
            }
        }

        public int ObterConteineresFechados(int reservaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ReservaId", value: reservaId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                         distinct count(autonum_patio)
                    FROM
	                    REDEX..tb_talie
                    WHERE
	                    autonum_boo= @ReservaId AND flag_estufagem=1 and flag_fechado=1 ", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<ConteinerDTO> ObterConteineresPorReservaCliente(string reserva, int cliente)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ConteinerDTO> ObterConteiners()
        {
            throw new NotImplementedException();
        }

        public DateTime ObterDataEntradaPorConteinerId(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "conteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<DateTime>(@"
                    SELECT
                        DT_ENTRADA
                    FROM
	                    REDEX..tb_patio 
                    WHERE
	                    autonum_patio = @conteinerId", parametros).FirstOrDefault();
            }
        }

        public int ObterIdTaliePorConteinerId(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "conteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                        autonum_talie
                    FROM
	                    REDEX..tb_talie 
                    WHERE
	                    autonum_patio = @conteinerId and nvl(crossdocking,0)=0", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<Estufagem> ObterItensEstufadosPorTalieConteinerId(int talieId, int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: talieId, direction: ParameterDirection.Input);
                parametros.Add(name: "ConteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<Estufagem>(@"
                    SELECT
                        cc.id_conteiner Conteiner,
                        D.REFERENCE Reserva, 
                        E.DESCRICAO_EMB Embalagem, 
                        F.DESC_PRODUTO Produto, 
                        A.QTDE_SAIDA Quantidade, 
						A.Mercadoria,
						A.PESO_BRUTO Peso,
						A.VOLUME Volume,
						A.autonum_sc AUTONUM_SC,
                        B.AUTONUM_PCS as PatioCargaId, 
                        G.RAZAO Cliente, 
                        H.DESCRICAO_NAV || '--' || I.NUM_VIAGEM AS NavioViagem,
                        NF.NUM_NF as NF
                    FROM
	                    REDEX..TB_SAIDA_CARGA A 
                    LEFT JOIN 
                        REDEX..TB_PATIO_CS B ON A.AUTONUM_PCS = B.AUTONUM_PCS
                    LEFT JOIN 
                        REDEX..TB_BOOKING_CARGA C ON B.AUTONUM_BCG = C.AUTONUM_BCG
                    LEFT JOIN 
                        REDEX..TB_BOOKING D ON C.AUTONUM_BOO = D.AUTONUM_BOO
                    LEFT OUTER JOIN 
                        REDEX..TB_CAD_EMBALAGENS E ON A.AUTONUM_EMB = E.AUTONUM_EMB
                    LEFT JOIN 
                        REDEX..TB_CAD_PRODUTOS F ON  B.AUTONUM_PRO = f.AUTONUM_PRO
                    LEFT JOIN 
                        REDEX..TB_CAD_PARCEIROS G ON D.AUTONUM_PARCEIRO = G.AUTONUM
                    LEFT JOIN 
                        REDEX..TB_VIAGENS I ON D.autonum_via = i.autonum_via
                    LEFT JOIN 
                        REDEX..TB_CAD_NAVIOS H ON i.autonum_nav = h.autonum_nav
                    LEFT JOIN REDEX..TB_NOTAS_FISCAIS NF ON a.AUTONUM_NFi = NF.AUTONUM_NF
                    LEFT OUTER JOIN 
                        REDEX..TB_PATIO CC ON A.AUTONUM_PATIO = CC.AUTONUM_PATIO
                    WHERE
	                    and a.autonum_patio = @ConteinerId AND a.autonum_talie = TalieId", parametros);
            }
        }

        public IEnumerable<Estufagem> ObterItensPorProduto(int produtoId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ProdutoId", value: produtoId, direction: ParameterDirection.Input);

                return con.Query<Estufagem>(@"
                    SELECT
                        a.instrucao,
                        a.reference, 
                        b.autonum_bcg, 
                        a.autonum_boo,
                        b.lote,
                        i.autonum_regcs, 
                        case
                            when (select 
                                    sum(qtde_saida) 
                                    from 
                                        REDEX..tb_saida_carga  
                                    where autonum_pcs = c.autonum_pcs) is null then 0 
                            Else (
                                    select 
                                        sum(qtde_saida) 
                                    from 
                                        REDEX..tb_saida_carga 
                                    where 
                                        autonum_pcs = c.autonum_pcs) end as qtde_saida, 
                        c.qtde_entrada,  
                        c.qtde_entrada - case when (
                                                    select 
                                                        sum(qtde_saida) 
                                                    from 
                                                        REDEX..tb_saida_carga 
                                                    where 
                                                        autonum_pcs = c.autonum_pcs) is null  then 0 
                                                Else (
                                                    select 
                                                        sum(qtde_saida) 
                                                    from 
                                                        REDEX..tb_saida_carga 
                                                    where 
                                                        autonum_pcs = c.autonum_pcs) end as saldo, 
                        d.razao as cliente, 
                        e.descricao_nav || '--' || f.num_viagem as navio_viagem, 
                        g.desc_produto as descricao_mer ,
                        h.autonum_emb, 
                        h.descricao_emb , 
                        b.marca, 
                        c.* ,
                        j.autonum_nf , 
                        j.num_nf,
                        I.AUTONUM_NFI
                    FROM
	                    REDEX..tb_booking A 
                    INNER JOIN
                        REDEX..tb_booking_carga b on a.autonum_boo = b.autonum_boo
                    INNER JOIN
                        REDEX..tb_patio_cs c on b.autonum_bcg = c.autonum_bcg
                    INNER JOIN
                        REDEX..tb_cad_parceiros d on a.autonum_parceiro = d.autonum
                    INNER JOIN
                        REDEX..tb_viagens f on a.autonum_via  = f.autonum_via
                    INNER JOIN
                        REDEX..tb_cad_navios e on f.autonum_nav = e.autonum_nav
                    INNER JOIN
                        REDEX..tb_cad_produtos g on c.AUTONUM_PRO = g.AUTONUM_PRO
                    INNER JOIN
                        REDEX..tb_cad_embalagens h on c.autonum_emb = h.autonum_emb
                    INNER JOIN REDEX..tb_notas_fiscais j on j.autonum_nf = c.autonum_nf
                    INNER JOIN REDEX..tb_notas_itens i on j.autonum_nf = i.autonum_nf
                    WHERE
	                    I.autonum_nfI = @ProdutoId", parametros);
            }
        }

        public int ObterQuantidadeCargaSoltaPorReserva(int reservaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ReservaId", value: reservaId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                        sum(qtde)
                    FROM
	                    REDEX..tb_booking_carga 
                    WHERE
	                    autonum_boo = @ReservaId AND FLAG_CS=1", parametros).FirstOrDefault();
            }
        }

        public int ObterQuantidadeEstufada(int reservaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ReservaId", value: reservaId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                        sum(qtde_saida)
                    FROM
	                    REDEX..tb_saida_carga sc
                    INNER JOIN
                        REDEX..TB_PATIO CC ON SC.AUTONUM_PATIO=CC.AUTONUM_PATIO
                    INNER JOIN
                        REDEX..TB_BOOKING_CARGA BCG ON CC.AUTONUM_BCG=BCG.AUTONUM_BCG
                    WHERE
	                    bcg.autonum_boo = @ReservaId", parametros).FirstOrDefault();
            }
        }

        public int ObterQuantidadePackingPorConteinerId(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "conteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                        sum(quantidade) QTDE
                    FROM
	                    REDEX..TB_PACKING 
                    WHERE
	                    autonum_patio = @conteinerId", parametros).FirstOrDefault();
            }
        }

        public int ObterQuantidadeSaidaPorConteinerId(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "conteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                        SUM(QTDE_SAIDA) QTDE
                    FROM
	                    REDEX..TB_SAIDA_CARGA 
                    WHERE
	                    autonum_patio = @conteinerId", parametros).FirstOrDefault();
            }
        }

        public void ReabrirEstufagem(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    var parametros = new DynamicParameters();


                    parametros.Add(name: "ConteinerId", value: conteinerId, direction: ParameterDirection.Input);

                    con.Execute(@"
                        UPDATE
                            REDEX..tb_talie
                            SET
                                termino = NULL,
                                flag_fechado = 0 
                            WHERE 
                                autonum_patio = @ConteinerId", parametros, transaction);

                    con.Execute(@"
                        UPDATE
                            REDEX..tb_patio
                            SET
                                ef = 'E'
                            WHERE 
                                autonum_patio = @ConteinerId", parametros, transaction);
                    transaction.Commit();
                }
            }
        }

        public void TransferirCargasDaReservaOrigemParaRservaEmbarque(int reservaId, int bookingargaId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ReservaId", value: reservaId, direction: ParameterDirection.Input);
                parametros.Add(name: "BookingCargaId", value: bookingargaId, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE A
                        SET
                            A.AUTONUM_BCG = @ReservaId 
                    FROM 
                        REDEX..TB_PATIO_CS A
                    INNER JOIN (
                                    SELECT 
                                        SC.AUTONUM_PCS 
                                    FROM 
                                        REDEX..TB_SAIDA_CARGA SC
                                    INNER JOIN 
                                        REDEX..TB_PATIO_CS PCS ON SC.AUTONUM_PCS=PCS.AUTONUM_PCS
                                    INNER JOIN 
                                        REDEX..TB_PATIO CC ON SC.AUTONUM_PATIO=CC.AUTONUM_PATIO
                                    INNER JOIN 
                                        REDEX..TB_BOOKING_CARGA BCG ON CC.AUTONUM_BCG=BCG.AUTONUM_BCG
                                     WHERE 
                                        BCG.AUTONUM_BOO=@ReservaId AND and pcs.autonum_bcg<>@BookingCargaId
                                ) B    
                    WHERE autonum_pcs = B.AUTONUM_PCS", parametros);

            }
        }
        public int GetFlagTalieFechado(int id)
        {
            int flag_fechado = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select FLAG_FECHADO from redex.dbo.tb_talie where autonum_talie= " + id);

                    flag_fechado = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return flag_fechado;
                }
            }
            catch (Exception ex)
            {
                return flag_fechado;
            }
        }
        public int getIDNF(int romaneioId, string os)
        {
            int nfID = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT ");
                    sb.AppendLine(" NF.AUTONUM_NF  ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO = RCS.AUTONUM_RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" RO.AUTONUM_RO =" + romaneioId + " ");
                    sb.AppendLine(" AND NF.NUM_NF = '" + os + "' ");

                    nfID = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return nfID;
                }
            }
            catch (Exception ex)
            {
                return nfID;
            }
        }
        public int GetValidaSaldoByOsRo(int romaneioId, int nf)
        {
            int qtde = 0;

            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select sum(isnull(qtde, 0)) -sum(isnull(qtde_saida,0)) as saldo from ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" select a.autonum_pcs, a.qtde, sum(sc.qtde_saida) as qtde_saida ");
                    sb.AppendLine(" from ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" select pcs.autonum_pcs, sum(rcs.qtde) qtde ");
                    sb.AppendLine(" from REDEX..tb_romaneio_cs rcs ");
                    sb.AppendLine(" inner ");
                    sb.AppendLine(" join REDEX..tb_patio_cs pcs on rcs.autonum_pcs = pcs.autonum_pcs ");
                    sb.AppendLine(" where rcs.autonum_ro = "+ romaneioId +" and pcs.autonum_nf = "+ nf +" ");
                    sb.AppendLine(" group by pcs.autonum_pcs ");
                    sb.AppendLine(" ) a ");
                    sb.AppendLine(" left join REDEX..tb_saida_carga sc on a.autonum_pcs = sc.AUTONUM_PCS ");
                    sb.AppendLine(" group by a.autonum_pcs, a.qtde ");
                    sb.AppendLine(" ) base ");
                    

                    qtde = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return qtde;
                }
            }
            catch (Exception ex)
            {
                return qtde;
            }
        }
        public IEnumerable <CargaEstufagem> GetDadosInsertEstufagem(int romaneio, int nf)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();


                    sb.AppendLine(" select* from ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" select ");
                    sb.AppendLine(" rcs.AUTONUM_PCS as PatioCSId,   ");
                    sb.AppendLine(" rcs.AUTONUM_RCS,   ");
                    sb.AppendLine(" rcs.autonum_ro,    ");
                    sb.AppendLine(" isnull(rcs.qtde,0) - isnull(sc.qtde_saida,0) as quantidade, ");
                    sb.AppendLine(" pcs.AUTONUM_EMB as EmbalagemId,   ");
                    sb.AppendLine(" pcs.bruto as Bruto,  ");
                    sb.AppendLine(" pcs.ALTURA as Altura,   ");
                    sb.AppendLine(" pcs.COMPRIMENTO as Comprimento,   ");
                    sb.AppendLine(" pcs.LARGURA as Largura,   ");
                    sb.AppendLine(" pcs.volume_declarado as VolumeTotal,   ");
                    sb.AppendLine(" ro.autonum_patio as ConteinerId,   ");
                    sb.AppendLine(" cc.ID_CONTEINER as Conteiner,    ");
                    sb.AppendLine(" AUTONUM_PRO as ProdutoId,   ");
                    sb.AppendLine(" DATA_ESTUFAGEM as Inicio,   ");
                    sb.AppendLine(" pcs.autonum_nf as NFItemId,   ");
                    sb.AppendLine(" ro.autonum_talie as TlieId,  ");
                    sb.AppendLine(" pcs.CODPRODUTO as Produto,  ");
                    sb.AppendLine(" cc.AUTONUM_PATIO as PatioId ");
                    sb.AppendLine(" from redex..tb_romaneio_cs rcs ");
                    sb.AppendLine(" inner ");
                    sb.AppendLine(" join redex..tb_patio_cs pcs on rcs.autonum_pcs = pcs.autonum_pcs ");
                    sb.AppendLine(" inner ");
                    sb.AppendLine(" join redex..tb_romaneio ro on rcs.autonum_ro = ro.autonum_ro ");
                    sb.AppendLine(" inner ");
                    sb.AppendLine(" join redex..tb_patio cc on ro.autonum_patio = cc.autonum_patio ");
                    sb.AppendLine(" left ");
                    sb.AppendLine(" join ( ");
                    sb.AppendLine(" select autonum_pcs, sum(qtde_saida) qtde_saida, data_estufagem ");
                    sb.AppendLine(" from redex..tb_saida_carga ");
                    sb.AppendLine(" group by autonum_pcs, data_estufagem ");
                    sb.AppendLine(" ) sc on rcs.autonum_pcs = sc.autonum_pcs ");
                    sb.AppendLine(" where rcs.autonum_ro = " + romaneio + " and pcs.autonum_nf = " + nf + " ");
                    sb.AppendLine(" ) base ");
                    sb.AppendLine(" where quantidade> 0 ");


                    //sb.AppendLine(" select ");
                    //sb.AppendLine(" rcs.AUTONUM_PCS as PatioCSId,  ");
                    //sb.AppendLine(" rcs.AUTONUM_RCS,  ");
                    //sb.AppendLine(" rcs.autonum_ro,   ");
                    ////sb.AppendLine(" "+ qtde + " as Quantidade,  ");
                    ////sb.AppendLine(" rcs.qtde as Quantidade,  ");
                    //sb.AppendLine(" sum(rcs.qtde - isnull(sc.qtde_saida,0)) as Quantidade,  ");
                    //sb.AppendLine(" pcs.AUTONUM_EMB as EmbalagemId,  ");
                    //sb.AppendLine(" pcs.bruto as Bruto, ");
                    //sb.AppendLine(" pcs.ALTURA as Altura,  ");
                    //sb.AppendLine(" pcs.COMPRIMENTO as Comprimento,  ");
                    //sb.AppendLine(" pcs.LARGURA as Largura,  ");
                    //sb.AppendLine(" pcs.volume_declarado as VolumeTotal,  ");
                    //sb.AppendLine(" ro.autonum_patio as ConteinerId,  ");
                    //sb.AppendLine(" cc.ID_CONTEINER as Conteiner,   ");
                    //sb.AppendLine(" AUTONUM_PRO as ProdutoId,  ");
                    //sb.AppendLine(" DATA_ESTUFAGEM as Inicio,  ");
                    //sb.AppendLine(" pcs.autonum_nf as NFItemId,  ");
                    //sb.AppendLine(" ro.autonum_talie as TlieId, ");
                    //sb.AppendLine(" pcs.CODPRODUTO as Produto, ");
                    //sb.AppendLine(" cc.AUTONUM_PATIO as PatioId ");
                    //sb.AppendLine(" from redex..tb_romaneio_cs rcs ");
                    //sb.AppendLine(" inner join redex..tb_patio_cs pcs on rcs.autonum_pcs = pcs.autonum_pcs ");
                    //sb.AppendLine(" inner join redex..tb_romaneio ro on rcs.autonum_ro = ro.autonum_ro ");
                    //sb.AppendLine(" inner join redex..tb_patio cc on ro.autonum_patio = cc.autonum_patio ");
                    //sb.AppendLine(" left join redex..tb_saida_carga sc on rcs.autonum_pcs = sc.autonum_pcs ");
                    //sb.AppendLine(" where rcs.autonum_ro = "+ romaneio + " and pcs.autonum_nf = " + nf  + " ");
                    //sb.AppendLine(" GROUP BY ");
                    //sb.AppendLine(" rcs.AUTONUM_PCS,  ");
                    //sb.AppendLine(" rcs.AUTONUM_RCS,  ");
                    //sb.AppendLine(" rcs.autonum_ro,   ");                    
                    //sb.AppendLine(" pcs.AUTONUM_EMB,  ");
                    //sb.AppendLine(" pcs.bruto, ");
                    //sb.AppendLine(" pcs.ALTURA,  ");
                    //sb.AppendLine(" pcs.COMPRIMENTO,  ");
                    //sb.AppendLine(" pcs.LARGURA,  ");
                    //sb.AppendLine(" pcs.volume_declarado,  ");
                    //sb.AppendLine(" ro.autonum_patio,  ");
                    //sb.AppendLine(" cc.ID_CONTEINER,   ");
                    //sb.AppendLine(" AUTONUM_PRO,  ");
                    //sb.AppendLine(" DATA_ESTUFAGEM,  ");
                    //sb.AppendLine(" pcs.autonum_nf,  ");
                    //sb.AppendLine(" ro.autonum_talie, ");
                    //sb.AppendLine(" pcs.CODPRODUTO, ");
                    //sb.AppendLine(" cc.AUTONUM_PATIO ");
                    //sb.AppendLine(" HAVING sum(rcs.qtde - isnull(sc.qtde_saida,0)) > 0   ");

                    var query = _db.Query<CargaEstufagem>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Estufagem> GetDadosNFByLote(string lote,string cntr)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("SELECT top 1 ");
                    sb.AppendLine(" NF.AUTONUM_NF,rcs.qtde as QuantNota,  ");
                    sb.AppendLine(" NF.NUM_NF, ");
                    sb.AppendLine(" rcs.autonum_rcs, ");
                    sb.AppendLine(" NF.NUM_NF + '    Qtde: ' + cast(RCS.QTDE as varchar) + '   (' + cast(PCS.COMPRIMENTO as varchar) + ' x ' + cast(PCS.LARGURA as varchar) + ' x ' + cast(PCS.ALTURA as varchar) + ' )' AS DISPLAY ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO = RCS.AUTONUM_RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ON RO.AUTONUM_PATIO = CC.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine("  left join ");
                    sb.AppendLine("  (select AUTONUM_RCS ,sum(QTDE_SAIDA) as saida from redex..TB_SAIDA_CARGA group by AUTONUM_RCS, QTDE_SAIDA ) k on k.AUTONUM_RCS = RCS.AUTONUM_RCS   ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" nf.autonum_reg = "+ lote +"");
                    sb.AppendLine(" and cc.id_conteiner = '" + cntr + "'");
                    sb.AppendLine("  group by ");
                    sb.AppendLine("  NF.AUTONUM_NF,rcs.qtde  , ");
                    sb.AppendLine("   NF.NUM_NF, pcs.AUTONUM_PCS,");
                    sb.AppendLine("   rcs.autonum_rcs,  ");
                    sb.AppendLine("  NF.NUM_NF + '    Qtde: ' + cast(RCS.QTDE as varchar) + '   (' + cast(PCS.COMPRIMENTO as varchar) + ' x ' + cast(PCS.LARGURA as varchar) + ' x '  ");
                    sb.AppendLine(" + cast(PCS.ALTURA as varchar) + ' )' ");
                    sb.AppendLine("  having  isnull(sum(k.saida),0)<RCS.QTDE   order by AUTONUM_RCS asc");


                    var query = _db.Query<Estufagem>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int CargaBagagem(string lote)
        {

            using(var _db = new SqlConnection(Config.StringConexao()))
            {
                StringBuilder sb = new StringBuilder();


                sb.Clear();
                sb.AppendLine(" select quantidade from REDEX.dbo.TB_REGISTRO_CS cs ");
                sb.AppendLine(" inner join ");
                sb.AppendLine(" REDEX.dbo.TB_BOOKING b on cs.autonum_boo = b.autonum_boo ");
                sb.AppendLine(" where  cs.autonum_reg = " + lote +" and isnull(b.flag_bagagem,0) = 1");


                var query = _db.Query<int>(sb.ToString()).FirstOrDefault();

                return query;


            }


        }


        public IEnumerable<Estufagem> GetDadosNFByOS(string conteiner, int romaneioId, string os)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT ");
                    sb.AppendLine(" NF.AUTONUM_NF,  ");
                    sb.AppendLine(" NF.NUM_NF, ");
                    sb.AppendLine(" rcs.autonum_rcs, ");
                    sb.AppendLine(" NF.NUM_NF + '    Qtde: ' + cast(RCS.QTDE as varchar) + '   (' + cast(PCS.COMPRIMENTO as varchar) + ' x ' + cast(PCS.LARGURA as varchar) + ' x ' + cast(PCS.ALTURA as varchar) + ' )' AS DISPLAY ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO = RCS.AUTONUM_RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ON RO.AUTONUM_PATIO = CC.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" ISNULL(RO.AUTONUM_PATIO, 0) <> 0 AND CC.ID_CONTEINER = '" + conteiner + "' ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" RO.AUTONUM_RO =" + romaneioId + " ");
                    sb.AppendLine(" AND NF.NUM_NF = '" + os + "' ");
                    sb.AppendLine(" ORDER BY ");
                    sb.AppendLine(" RO.AUTONUM_RO DESC ");

                    var query = _db.Query<Estufagem>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Estufagem GetDadosNF(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" BOO.OS, ");
                    sb.AppendLine(" CC.ID_CONTEINER,  ");
                    sb.AppendLine(" BOO.AUTONUM_BOO, ");
                    sb.AppendLine(" BOO.REFERENCE AS RESERVA,  ");
                    sb.AppendLine(" nf.autonum_nf, NF.NUM_NF, ISNULL(SC.QTDE_SAIDA, 0) QTDE_SAIDA,  ");
                    sb.AppendLine(" RCS.QTDE as Quantidade,  ");
                    sb.AppendLine(" RCS.QTDE - ISNULL(SC.QTDE_SAIDA, 0) AS SALDO, ");
                    sb.AppendLine(" ISNULL(SC.QTDE_SAIDA, 0) EstufadaInd,   ");
                    sb.AppendLine("  rcs.QTDE as QuantNota, ");
                    sb.AppendLine(" PCS.AUTONUM_PCS, ");
                    sb.AppendLine(" PCS.AUTONUM_EMB, ");
                    sb.AppendLine(" PCS.BRUTO,  ");
                    sb.AppendLine(" PCS.COMPRIMENTO, ");
                    sb.AppendLine(" PCS.LARGURA,  ");
                    sb.AppendLine(" PCS.ALTURA,  ");
                    sb.AppendLine(" PCS.AUTONUM_PRO, ");
                    sb.AppendLine(" RCS.AUTONUM_RCS, ");
                    sb.AppendLine(" PCS.CODPRODUTO ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO = RCS.AUTONUM_RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ON RO.AUTONUM_PATIO = CC.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" (SELECT AUTONUM_PCS, SUM(QTDE_SAIDA) AS QTDE_SAIDA FROM REDEX..TB_SAIDA_CARGA GROUP BY AUTONUM_PCS) SC ON PCS.AUTONUM_PCS = SC.AUTONUM_PCS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" rcs.autonum_rcs = " + id);
                    sb.AppendLine(" ORDER BY ");
                    sb.AppendLine(" RO.AUTONUM_RO DESC ");

                    var query = _db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Estufagem GetCarregaDadosConteiner(string conteiner)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine(" BOO.AUTONUM_BOO, ");
                    sb.AppendLine(" BOO.REFERENCE AS RESERVA, ");
                    sb.AppendLine(" RO.AUTONUM_PATIO, ");
                    sb.AppendLine(" RO.AUTONUM_RO, ");
                    sb.AppendLine(" RO.OBS ");
                    sb.AppendLine(" FROM              ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ON RO.AUTONUM_PATIO = CC.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON RO.AUTONUM_BOO = BOO.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA BCGC ON CC.AUTONUM_BCG = BCGC.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOOCONTEINER ON BCGC.AUTONUM_BOO = BOOCONTEINER.AUTONUM_BOO ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" CC.ID_CONTEINER = '" + conteiner + "' ORDER BY AUTONUM_RO DESC");


                    var query = _db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query; 
                }
            }
            catch (Exception ex)
            {
                return null; 
            }
        }
        public Estufagem GetTalieId(int patio,int romaneio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_TALIE FROM REDEX..TB_TALIE WHERE AUTONUM_PATIO=" + patio+" and autonum_ro = " + romaneio);

                    var query = _db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        public Estufagem GetTalieIdSemRomaneio(int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_TALIE FROM REDEX..TB_TALIE WHERE AUTONUM_PATIO=" + patio );

                    var query = _db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Estufagem GetDadosClienteByReserva(string reserva)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" AUTONUM as AUTONUM_CLIENTE,  ");
                    sb.AppendLine(" FANTASIA AS CLIENTE ");
                    sb.AppendLine(" FROM           ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO  ");
                    sb.AppendLine(" INNER JOIN                  ");
                    sb.AppendLine(" REDEX..tb_cad_parceiros CP ON BOO.AUTONUM_PARCEIRO = CP.AUTONUM  ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" BOO.REFERENCE  = '" + reserva + "' ");

                    var query = _db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countTalieByCarregamento(int patio)
        {
            int count = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT COUNT(1) FROM REDEX.dbo.tb_TALIE WHERE AUTONUM_PATIO=" + patio + " AND FLAG_CARREGAMENTO = 1 ");

                    count = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public Estufagem GetCarregaDadosTalieById(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("  SELECT  ");
                    sb.AppendLine("  INICIO,  ");
                    sb.AppendLine("  TERMINO,  ");
                    sb.AppendLine("  CONFERENTE ,  ");
                    sb.AppendLine("   EQUIPE,  ");
                    sb.AppendLine("  FORMA_OPERACAO,  ");
                    sb.AppendLine("  ISNULL(FLAG_FECHADO, 0) as Fechado  ");
                    sb.AppendLine("  FROM  ");
                    sb.AppendLine("  REDEX..TB_TALIE  ");
                    sb.AppendLine("  WHERE  ");
                    sb.AppendLine("  AUTONUM_TALIE = " + id);

                    var query = _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query; 
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Estufagem> GetItensEstufadosPorTalieConteinerId(int talie, int container)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" cc.id_conteiner Conteiner, ");
                    sb.AppendLine(" D.REFERENCE Reserva, ");
                    sb.AppendLine(" E.DESCRICAO_EMB Embalagem, ");
                    sb.AppendLine(" F.DESC_PRODUTO Produto, ");
                    sb.AppendLine(" A.QTDE_SAIDA Quantidade, ");
                    sb.AppendLine(" A.Mercadoria, ");

                    sb.AppendLine(" CASE WHEN b.bruto > 0 THEN convert(varchar,b.BRUTO)  WHEN  NF.PESO_BRUTO > 0 THEN convert(varchar, NF.PESO_BRUTO) ELSE 'SEM PESO' END AS Peso, ");
                    sb.AppendLine(" CASE WHEN ISNULL(A.VOLUME,0) = 0 THEN B.VOLUME_DECLARADO ELSE A.VOLUME END AS  Volume, ");
                    sb.AppendLine(" A.autonum_sc AUTONUM_SC, ");
                    sb.AppendLine(" B.autonum_pcs as AUTONUM_PCS, ");                    
                    sb.AppendLine(" B.AUTONUM_PCS as PatioCargaId,  ");
                    sb.AppendLine(" G.RAZAO Cliente, ");
                    sb.AppendLine(" H.DESCRICAO_NAV + ' || -- || ' +  I.NUM_VIAGEM AS NavioViagem, ");
                    sb.AppendLine(" NF.NUM_NF as NF,  ");
                    sb.AppendLine(" NF.AUTONUM_NF AS AUTONUM_NFI,  ");
                    sb.AppendLine(" A.AUTONUM_PATIO ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_SAIDA_CARGA A ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS B ON A.AUTONUM_PCS = B.AUTONUM_PCS ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA C ON B.AUTONUM_BCG = C.AUTONUM_BCG ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING D ON C.AUTONUM_BOO = D.AUTONUM_BOO ");
                    sb.AppendLine(" LEFT OUTER JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_EMBALAGENS E ON A.AUTONUM_EMB = E.AUTONUM_EMB ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_PRODUTOS F ON  B.AUTONUM_PRO = f.AUTONUM_PRO ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_PARCEIROS G ON D.AUTONUM_PARCEIRO = G.AUTONUM ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_VIAGENS I ON D.autonum_via = i.autonum_via ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_NAVIOS H ON i.autonum_nav = h.autonum_nav ");
                    sb.AppendLine(" LEFT JOIN ");

 //                   sb.AppendLine(" REDEX..TB_NOTAS_ITENS NI ON A.AUTONUM_NFI = NI.AUTONUM_NFI ");
//                    sb.AppendLine(" LEFT JOIN ");

                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON b.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" LEFT OUTER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ON A.AUTONUM_PATIO = CC.AUTONUM_PATIO ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" a.autonum_patio = "+ container + " AND a.autonum_talie=  " + talie);


                    var query = _db.Query<Estufagem>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Estufagem GetSaldoAtualizado(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao())) 
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" RCS.QTDE - ISNULL(SC.QTDE_SAIDA, 0) AS SALDO ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO = RCS.AUTONUM_RO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ON RO.AUTONUM_PATIO = CC.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" (SELECT AUTONUM_PCS, SUM(QTDE_SAIDA) AS QTDE_SAIDA FROM REDEX..TB_SAIDA_CARGA GROUP BY AUTONUM_PCS) SC ON PCS.AUTONUM_PCS = SC.AUTONUM_PCS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" RCS.AUTONUM_RCS = " + id + " ");
                    sb.AppendLine(" ORDER BY ");
                    sb.AppendLine(" RO.AUTONUM_RO DESC ");

                    var query = _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }            
        }
        public Estufagem GetDescarga(int autonumNFI, int scId, int patioId, int qtdeSaida, string produtoId, bool cargaSuzano)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("  DELETE");
                    sb.AppendLine(" REDEX..TB_AMR_NF_SAIDA ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" autonum_nfi = " + autonumNFI + " AND autonum_patio = " + patioId + " ");

                    _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" UPDATE  ");
                    sb.AppendLine(" REDEX..TB_NOTAS_ITENS ");
                    sb.AppendLine(" SET ");
                    sb.AppendLine(" QTDE_ESTUFADA = qtde_estufada - " + qtdeSaida + " ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" AUTONUM_NFI = " + autonumNFI + " ");

                    _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" DELETE ");
                    sb.AppendLine(" REDEX..tb_saida_carga ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" autonum_sc = " + scId + " ");


                    _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    if (cargaSuzano)
                    {
                        sb.AppendLine(" UPDATE ");
                        sb.AppendLine(" REDEX..tb_integra_carga ");
                        sb.AppendLine(" SET ");
                        sb.AppendLine(" estufado = 0 ");
                        sb.AppendLine(" WHERE ");
                        sb.AppendLine(" codbarra = '" + produtoId + "' ");

                        _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();
                    }

                    sb.Clear();

                    sb.AppendLine(" UPDATE  ");
                    sb.AppendLine(" REDEX..tb_patio ");
                    sb.AppendLine(" SET ");
                    sb.AppendLine(" ef = 'E' ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" autonum_patio = " + patioId);

                    _Db.Query<EstufagemDTO>(sb.ToString()).FirstOrDefault();


                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public RomaneioDTO GetDadosRomaneio(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select ro.autonum_gate_saida, ro.crossdocking, ro.autonum_patio, ro.autonum_ro, ro.autonum_talie, t.autonum_talie as talie, T.AUTONUM_PATIO AS CNTR_TALIE ");
                    sb.AppendLine(" ,RO.AUTONUM_BOO AS BOO_RO, T.AUTONUM_BOO AS BOO_TALIE, ISNULL(T.FLAG_FECHADO,0) as Fechado ");
                    sb.AppendLine(" from REDEX.dbo.tb_romaneio ro ");
                    sb.AppendLine(" left join REDEX.dbo.tb_talie t on ro.autonum_talie=t.autonum_talie ");
                    sb.AppendLine(" where ro.autonum_ro = " + id);

                    var query = _Db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countTalieByRomaneio(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT COUNT(1) FROM REDEX.dbo.tb_TALIE WHERE AUTONUM_PATIO=" + id + " AND FLAG_CARREGAMENTO=1 ");

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int countTalieByPatio(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT COUNT(1) FROM REDEX.dbo.tb_TALIE WHERE AUTONUM_ro=  " + id);

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int FlagOpFULL(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select flag_op_full_cntr from REDEX.dbo.tb_booking where autonum_boo = " + id);

                    int count = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int GravarTalie(int patio, string inicio, int boo, string formaOperacao, int conferente, int equipe, int idRomaneio, int gate)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select flag_op_full_cntr from REDEX.dbo.tb_booking where autonum_boo = " + boo);

                    int opNull = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine("  select ef from REDEX.dbo.tb_registro where autonum_ro =  " + idRomaneio);

                    string ef = _db.Query<string>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" INSERT INTO REDEX..TB_TALIE ( ");
                    sb.AppendLine(" autonum_patio, ");
                    sb.AppendLine(" inicio, ");
                    sb.AppendLine(" flag_estufagem,   ");
                    sb.AppendLine(" crossdocking, ");
                    sb.AppendLine(" autonum_boo,forma_operacao,conferente,equipe,flag_descarga,flag_carregamento,obs,autonum_ro,autonum_gate ");
                    sb.AppendLine(" ) values ( ");
                    sb.AppendLine(" "+ patio +", ");

                    if (String.IsNullOrEmpty(inicio))
                    {
                        sb.AppendLine(" getDate(), ");
                    }
                    else
                    {
                        sb.AppendLine(" convert(datetime, " + inicio + ", 103), ");                        
                    }

                    if (opNull == 1)
                    {
                        sb.AppendLine(" 0, ");
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(ef))
                        {
                            ef = "F";
                        }
                        else
                        {
                            ef = "";
                        }

                        if (ef == "F")
                        {
                            sb.AppendLine(" 1, ");

                        }
                        else
                        {
                            sb.AppendLine(" 0, ");
                        }
                    }

                    sb.AppendLine(" 0, ");

                    sb.AppendLine(" " + boo + ", ");

                    sb.AppendLine(" '" + formaOperacao + "'," + conferente + "," + equipe + ", 0, 1, '', " + idRomaneio + "," + gate );
                    sb.AppendLine(" ) ");

                    _db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();
                    
                    sb.AppendLine(" SELECT IDENT_CURRENT('REDEX.DBO.TB_TALIE') As ID ");
                    
                    int talieID = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return talieID;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public string GetDataInicioTalie(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT INICIO FROM REDEX..TB_TALIE WHERE AUTONUM_TALIE = " + id);

                    string data = _Db.Query<string>(sb.ToString()).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public RomaneioDTO UpdateRomaneio(int talie, int romaneio)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" update REDEX.dbo.tb_romaneio set autonum_talie=" + talie + " where autonum_ro = " + romaneio);

                     _Db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateSaidaCarga(int talie, int romaneio, int patio)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" update REDEX.dbo.tb_saida_carga set autonum_talie=" + talie + ", autonum_ro=" + romaneio + " where autonum_patio = " + patio + " and isnull(autonum_talie,0)=0 ");

                    _Db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" UPDATE REDEX.dbo.tb_TALIE SET FLAG_ESTUFAGEM=1 WHERE AUTONUM_TALIE= " + talie);

                    _Db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Talie UpdateTalie(string Inicio, string Termino, int TalieID, int boo, int Romaneio, int conferente, int equipe, int gate, int patio, string modo)        
        {
            try
            {
                

                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select flag_cntr from REDEX.dbo.tb_booking where autonum_boo = " + Romaneio);
                    int flagOp = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" select flag_cntr from REDEX.dbo.tb_booking where autonum_boo =  " + Romaneio);
                    
                    int flagCtnr = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" select count(1) from REDEX.dbo.tb_registro where autonum_ro = " + Romaneio);
                    
                    int countRegistro = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" select ef from REDEX.dbo.tb_registro where autonum_ro = " + Romaneio);

                    string ef = _Db.Query<string>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" SELECT COUNT(1) FROM REDEX.dbo.tb_saida_carga where autonum_patio= " + patio);

                    int countPatios = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine("  UPDATE Redex..TB_TALIE SET  ");
                    if (String.IsNullOrEmpty(Inicio))
                    {                        
                        sb.AppendLine(" INICIO = getDate(),  ");
                    }
                    else
                    {
                        sb.AppendLine(" INICIO = convert(datetime, '" + Inicio + "', 103),  ");
                    }

                    if (String.IsNullOrEmpty(Termino))
                    {
                        sb.AppendLine(" TERMINO = null,  ");
                    }
                    else
                    {
                        sb.AppendLine(" TERMINO = convert(datetime, '" + Termino + "', 103), ");                        
                    }
                    
                    sb.AppendLine(" CONFERENTE =  " + conferente + ", ");
                    sb.AppendLine(" EQUIPE =  " + equipe + ", ");
                    sb.AppendLine(" OBS = '', ");
                    sb.AppendLine(" autonum_gate =  " + gate + ", ");

                    if (flagOp == 1)
                    {
                        sb.AppendLine(" flag_estufagem = 0, ");
                    }
                    else
                    {
                        if (flagCtnr == 1)
                        {
                            if (countRegistro == 1)
                            {
                                if (ef == "")
                                {
                                    if (countPatios != 0)
                                    {
                                        sb.AppendLine(" flag_estufagem  = 1, ");

                                    }
                                    else
                                    {
                                        sb.AppendLine("  flag_estufagem = 0, ");
                                    }
                                }
                                else
                                {
                                    if (patio != 0)
                                    {
                                        sb.AppendLine(" flag_estufagem = 1, ");
                                    }
                                    else
                                    {
                                        sb.AppendLine(" flag_estufagem = 0, ");
                                    }
                                }
                            }
                        }
                        else
                        {
                            sb.AppendLine(" flag_estufagem = 0, ");
                        }
                    }

                    sb.AppendLine(" forma_operacao = '" + modo + "' ");


                    sb.AppendLine(" WHERE autonum_talie  = " + TalieID);


                    _Db.Query<Talie>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Estufagem FecharEstufagem(int talie, int romaneio,int conferente,int equipe)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" update REDEX.dbo.tb_talie set flag_fechado=1, conferente = " + conferente + " , equipe = " + equipe +",flag_pacotes=1, termino = getdate() where autonum_talie=  " + talie);

                    _Db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" update REDEX.dbo.tb_romaneio set flag_historico=1   where autonum_ro= " + romaneio);

                    _Db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #region ValidarFechamento
        //
        public int getFechamentoIDTalieByRomaneio(int romaneio)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select autonum_talie from REDEX.dbo.tb_romaneio where autonum_ro =  " + romaneio);

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int getFechamentoIDRomaneioByTalie(int talie)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select autonum_ro from REDEX.dbo.tb_talie where autonum_talie=  " + talie);

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int getFechamentoConsisteNF(int romaneio)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" select count(1) ");
                    sb.AppendLine(" from REDEX.dbo.tb_saida_carga sc ");
                    sb.AppendLine(" inner ");
                    sb.AppendLine(" join REDEX.dbo.tb_patio_cs pcs on sc.autonum_pcs = pcs.autonum_pcs ");
                    sb.AppendLine(" inner ");
                    sb.AppendLine(" join REDEX.dbo.tb_notas_fiscais nf on nf.autonum_nf = pcs.autonum_nf ");
                    sb.AppendLine(" left ");
                    sb.AppendLine(" join REDEX.dbo.tb_notas_itens nfi on nfi.autonum_nf = nf.autonum_nf ");

                    sb.AppendLine(" left ");
                    sb.AppendLine(" join REDEX.dbo.TB_REGISTRO_CS cs on pcs.AUTONUM_BCG = cs.AUTONUM_BCG ");
                    sb.AppendLine(" left ");
                    sb.AppendLine(" join REDEX.dbo.TB_BOOKING b on cs.AUTONUM_BOO = b.AUTONUM_BOO ");
                    sb.AppendLine(" where sc.autonum_ro = " + romaneio);
                    sb.AppendLine(" and ISNULL(nfi.autonum_nf,0)= 0 and isnull(b.flag_bagagem,0) = 0 ");

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public Estufagem GetDadosValidaFechamento(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select autonum_patio as AUTONUM_PATIO, flag_CARREGAMENTO, AUTONUM_BOO, flag_fechado, FLAG_ESTUFAGEM from REDEX.dbo.tb_talie where autonum_talie = " + id);

                    var query = _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region ValidarFechamentoParte2
        public Estufagem GetDadosValidaFechamentoParte2(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select autonum_patio as AUTONUM_PATIO, flag_CARREGAMENTO, AUTONUM_BOO, flag_fechado, FLAG_ESTUFAGEM from REDEX.dbo.tb_talie where autonum_talie = " + id);

                    var query = _Db.Query<Estufagem>(sb.ToString()).FirstOrDefault();

                    return query;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetDadosLacre(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select lacre from REDEX.dbo.tb_patio_lacres where autonum_patio = " + id + " and flag_ativo = 1  ");

                    string query = _Db.Query<string>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countPatiosByTpc(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select count(1) from REDEX.dbo.tb_patio where autonum_patio = " + id + " and autonum_tpc in ('FR','OT') ");

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int sumQuantidadeSaida(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select isnull(sum(qtde_saida),0) as qtde_saida  from REDEX.dbo.tb_saida_carga where autonum_patio=  " + id);

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int sumQuantidadeRomaneio(int romaneio)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select sum(qtde) from REDEX.dbo.tb_romaneio_cs where autonum_ro=  " + romaneio);

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int sumQuantidadeSaidasCargas(int patio, int talie)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" count(1)  ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" redex..tb_saida_carga A  ");
                    sb.AppendLine(" INNER JOIN                  ");
                    sb.AppendLine(" redex..tb_patio_cs B ON A.autonum_pcs = B.autonum_pcs ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" redex..tb_booking_carga C ON B.autonum_bcg = C.autonum_bcg ");
                    sb.AppendLine(" INNER JOIN  ");
                    sb.AppendLine(" redex..tb_booking D ON C.autonum_boo = D.autonum_boo  ");
                    sb.AppendLine(" LEFT JOIN                     ");
                    sb.AppendLine(" redex.. tb_cad_embalagens E ON B.autonum_emb = E.autonum_emb  ");
                    sb.AppendLine(" LEFT JOIN                         ");
                    sb.AppendLine(" redex..tb_cad_produtos F ON B.autonum_pro = f.autonum_pro ");
                    sb.AppendLine(" INNER JOIN                         ");
                    sb.AppendLine(" redex..tb_cad_parceiros G ON D.autonum_parceiro = G.autonum ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" redex..tb_viagens i ON D.autonum_via = i.autonum_via ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" redex..tb_cad_navios h ON i.autonum_nav = h.autonum_nav ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" redex..tb_notas_fiscais NF ON B.autonum_nf = NF.autonum_nf ");
                    sb.AppendLine(" RIGHT JOIN ");
                    sb.AppendLine(" redex..tb_patio CC ON A.autonum_patio = CC.autonum_patio ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" a.autonum_patio = " + patio + " ");
                    sb.AppendLine(" AND a.autonum_talie = " + talie + " ");

                    int query = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion
        public Estufagem GerarCancelamento(int talie, int romaneio)
        {

            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" update REDEX.dbo.tb_talie set flag_fechado=0 where autonum_talie= " + talie);

                    _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" update REDEX.dbo.tb_romaneio set flag_historico=0 where autonum_ro= " + romaneio);

                    _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" DELETE FROM REDEX.dbo.tb_SAIDA_CARGA WHERE AUTONUM_TALIE= " + talie);

                    _Db.Query<int>(sb.ToString()).FirstOrDefault();


                    return null; 
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

