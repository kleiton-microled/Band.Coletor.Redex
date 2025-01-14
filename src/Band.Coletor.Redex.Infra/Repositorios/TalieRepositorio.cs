using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Extensions;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Band.Coletor.Redex.Business.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Band.Coletor.Redex.Infra.Repositorios.Sql;
using Band.Coletor.Redex.Business.Models.Entities;
using System.Collections;
using Band.Coletor.Redex.Business.Classes.ServiceResult;


namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class TalieRepositorio : BaseRepositorio<Talie>, ITalieRepositorio
    {
        public TalieRepositorio(string connectionString) : base(connectionString)
        {
        }

        public int ObterConferentes(int idConferente)
        {
            MemoryCache cache = MemoryCache.Default;

            // var conferentes = cache["Conferente.ObterConferentes"] as int;


            using (var _db = new SqlConnection(Config.StringConexao()))
            {
                var conferentes = _db.Query<int>(@"SELECT AUTONUM_EQP As Id, NOME_EQP As Descricao FROM REDEX..TB_EQUIPE WHERE FLAG_ATIVO = 1 AND FLAG_CONFERENTE = 1 and id_login = " + idConferente + "").FirstOrDefault();

                return conferentes;
            }

            //      cache["Conferente.ObterConferentes"] = conferentes;


        }

        public IEnumerable<Business.Models.Equipe> ObterEquipes()
        {
            MemoryCache cache = MemoryCache.Default;

            var equipes = cache["Equipe.ObterEquipes"] as IEnumerable<Business.Models.Equipe>;

            if (equipes == null)
            {
                using (SqlConnection con = new SqlConnection(Config.StringConexao()))
                {
                    equipes = con.Query<Business.Models.Equipe>(@"SELECT AUTONUM_EQP As Id, NOME_EQP As Descricao FROM REDEX..TB_EQUIPE WHERE FLAG_ATIVO = 1 AND FLAG_OPERADOR = 1 ORDER BY NOME_EQP");
                }

                cache["Equipe.ObterEquipes"] = equipes;
            }

            return equipes;
        }

        public IEnumerable<Operacao> ObterOperacoes()
        {
            MemoryCache cache = MemoryCache.Default;

            var operacoes = cache["Operacao.ObterOperacoes"] as IEnumerable<Operacao>;

            if (operacoes == null)
            {
                using (SqlConnection con = new SqlConnection(Config.StringConexao()))
                {
                    operacoes = con.Query<Operacao>(@"SELECT 'A' As Sigla, 'Automatizada' As Descricao UNION ALL SELECT 'M' As Sigla, 'Manual' As Descricao UNION ALL SELECT 'P' As Sigla, 'Parcial' As Descricao ");
                }

                cache["Operacao.ObterOperacoes"] = operacoes;
            }

            return operacoes;
        }

        public Talie ObterDadosTaliePorRegistro(int registro)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Registro", value: registro, direction: ParameterDirection.Input);

                return con.Query<Talie>(@"
                    SELECT
                        T.AUTONUM_TALIE As Id,
                        T.AUTONUM_REG,
	                    T.AUTONUM_PATIO As PatioId,
	                    T.PLACA,
                        FORMAT(T.INICIO, 'dd/MM/yyyy HH:mm:ss') INICIO,
                        FORMAT(T.TERMINO, 'dd/MM/yyyy HH:mm:ss') TERMINO,
	                    T.FLAG_DESCARGA As Descarga,
	                    T.FLAG_ESTUFAGEM As Estufagem,
	                    T.CROSSDOCKING,
	                    ISNULL(T.CONFERENTE,0) AS ConferenteId,
	                    ISNULL(T.EQUIPE,0) AS EquipeId,
	                    T.AUTONUM_BOO As BookingId,
	                    T.FLAG_CARREGAMENTO As Carregamento,
	                    T.FORMA_OPERACAO As OperacaoId,
	                    T.AUTONUM_GATE As GateId,
	                    T.FLAG_FECHADO As Fechado,
	                    T.OBS As Observacoes,
	                    T.AUTONUM_RO As RomaneioId,
	                    T.AUDIT_225 As Audit225,
	                    T.ANO_TERMO As AnoTermo,
	                    T.TERMO,
	                    T.DATA_TERMO As DataTermo,
	                    T.FLAG_PACOTES As Pacotes,
	                    T.ALERTA_ETIQUETA As AlertaEtiqueta,
	                    T.AUTONUM_REG As RegistroId,
	                    T.FLAG_COMPLETO As Completo,
	                    T.EMAIL_ENVIADO As EmailEnviado,
	                    BOO.REFERENCE,
	                    CP.FANTASIA As Cliente
                    FROM
	                    REDEX..TB_TALIE T
                    INNER JOIN
	                    REDEX..TB_BOOKING BOO ON T.AUTONUM_BOO = BOO.AUTONUM_BOO
                    INNER JOIN
	                    REDEX..TB_CAD_PARCEIROS CP ON BOO.AUTONUM_PARCEIRO = CP.AUTONUM
                    WHERE
	                    T.AUTONUM_REG = @Registro", parametros).FirstOrDefault();
            }
        }

        public Talie ObterDadosTaliePorId(int idTalie)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "idTalie", value: idTalie, direction: ParameterDirection.Input);

                return con.Query<Talie>(@"
                    SELECT
                        T.AUTONUM_TALIE As Id,
                        T.AUTONUM_REG,
	                    T.AUTONUM_PATIO As PatioId,
	                    T.PLACA,
                        FORMAT(T.INICIO, 'dd/MM/yyyy HH:mm:ss') INICIO,
                        FORMAT(T.TERMINO, 'dd/MM/yyyy HH:mm:ss') TERMINO,
	                    T.FLAG_DESCARGA As Descarga,
	                    T.FLAG_ESTUFAGEM As Estufagem,
	                    T.CROSSDOCKING,
	                    ISNULL(T.CONFERENTE,0) AS ConferenteId,
	                    ISNULL(T.EQUIPE,0) AS EquipeId,
	                    T.AUTONUM_BOO As BookingId,
	                    T.FLAG_CARREGAMENTO As Carregamento,
	                    T.FORMA_OPERACAO As OperacaoId,
	                    T.AUTONUM_GATE As GateId,
	                    T.FLAG_FECHADO As Fechado,
	                    T.OBS As Observacoes,
	                    T.AUTONUM_RO As RomaneioId,
	                    T.AUDIT_225 As Audit225,
	                    T.ANO_TERMO As AnoTermo,
	                    T.TERMO,
	                    T.DATA_TERMO As DataTermo,
	                    T.FLAG_PACOTES As Pacotes,
	                    T.ALERTA_ETIQUETA As AlertaEtiqueta,
	                    T.AUTONUM_REG As RegistroId,
	                    T.FLAG_COMPLETO As Completo,
	                    T.EMAIL_ENVIADO As EmailEnviado,
	                    BOO.REFERENCE,
	                    CP.FANTASIA As Cliente
                    FROM
	                    REDEX..TB_TALIE T
                    INNER JOIN
	                    REDEX..TB_BOOKING BOO ON T.AUTONUM_BOO = BOO.AUTONUM_BOO
                    INNER JOIN
	                    REDEX..TB_CAD_PARCEIROS CP ON BOO.AUTONUM_PARCEIRO = CP.AUTONUM
                    WHERE
	                    T.AUTONUM_TALIE = @idTalie", parametros).FirstOrDefault();
            }
        }

        public Talie ObterDadosRegistro(int registroId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "RegistroId", value: registroId, direction: ParameterDirection.Input);

                return con.Query<Talie>(@"
                    SELECT
                        A.AUTONUM As GateId,
                        E.AUTONUM_PARCEIRO As ClienteId,
                        E.REFERENCE,
                        E.AUTONUM_BOO As BookingId,
                        CP.FANTASIA As Cliente,
                        A.DT_GATE_IN As DataGateIn,
                        B.Placa
                    FROM
                        REDEX..TB_REGISTRO B
                    LEFT JOIN
                        REDEX..TB_GATE_NEW A ON B.AUTONUM_GATE = A.AUTONUM
                    LEFT JOIN
                        REDEX..TB_BOOKING E ON B.AUTONUM_BOO = E.AUTONUM_BOO
                    LEFT JOIN
                        REDEX..TB_CAD_PARCEIROS CP ON E.AUTONUM_PARCEIRO = CP.AUTONUM
                    WHERE
                        B.AUTONUM_REG = @RegistroId
                    AND
                        A.DT_GATE_IN IS NOT NULL", parametros).FirstOrDefault();
            }
        }

        public int ObterCodigoConferentePorLogin(int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT AUTONUM_EQP FROM REDEX..TB_EQUIPE WHERE ID_LOGIN = @UsuarioId", parametros).FirstOrDefault();
            }
        }

        public bool ExisteCargaCadastrada(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

                return con.Query<bool>(@"SELECT AUTONUM_TALIE FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TALIE = @TalieId", parametros).Any();
            }
        }

        public int ObterQuantidadeDescarga(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT ISNULL(SUM(QTDE_DESCARGA),0) FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TALIE = @TalieId", parametros).FirstOrDefault();
            }
        }

        public int ObterQuantidadeAssociada(int talieId, int bookingId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: talieId, direction: ParameterDirection.Input);
                parametros.Add(name: "BookingId", value: bookingId, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT ISNULL(SUM(VOLUMES),0) AS QTO FROM REDEX..TB_MARCANTES_RDX WHERE AUTONUM_TALIE = @TalieId AND AUTONUM_BOO = @BookingId", parametros).FirstOrDefault();
            }
        }

        public decimal ObterQuantidadeNF(int registroId, string notaFiscal)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "RegistroId", value: registroId, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaFiscal", value: notaFiscal, direction: ParameterDirection.Input);

                return con.Query<decimal>(@"SELECT ROUND(ISNULL(PESO_BRUTO/VOLUMES,0),3) FROM REDEX..TB_NOTAS_FISCAIS WHERE AUTONUM_REG = @RegistroId AND NUM_NF = @NotaFiscal", parametros).FirstOrDefault();
            }
        }

        public string ObterEmbalagemPorSigla(string sigla)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Sigla", value: sigla, direction: ParameterDirection.Input);

                return con.Query<string>(@"SELECT DESCRICAO_EMB + '-' + AUTONUM_EMB AS SIGLA FROM REDEX..TB_CAD_EMBALAGENS WHERE SIGLA IS NOT NULL AND SIGLA = @Sigla ORDER BY SIGLA, DESCRICAO_EMB", parametros).FirstOrDefault();
            }
        }

        public int ObterQuantidadeRegistro(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "RegistroId", value: id, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT ISNULL(SUM(QUANTIDADE),0) FROM REDEX..TB_REGISTRO_CS WHERE AUTONUM_REG = @RegistroId", parametros).FirstOrDefault();
            }
        }

        public int ObterPesoBruto(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "GateId", value: id, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT ISNULL(BRUTO,0) BRUTO FROM REDEX..TB_GATE_NEW WHERE AUTONUM = @GateId", parametros).FirstOrDefault();
            }
        }

        public int ObterQuantidadeBooking(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "BookingId", value: bookingId, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT ISNULL(SUM(BCG.QTDE),0) FROM REDEX..TB_BOOKING BOO INNER JOIN REDEX..TB_BOOKING_CARGA BCG ON BOO.AUTONUM_BOO = BCG.AUTONUM_BOO WHERE BOO.AUTONUM_BOO = @BookingId AND BCG.FLAG_CS = 1", parametros).Single();
            }
        }

        public int ObterQuantidadeEntradaBooking(int bookingId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "BookingId", value: bookingId, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT ISNULL(SUM(PCS.QTDE_ENTRADA),0) FROM REDEX..TB_BOOKING BOO INNER JOIN REDEX..TB_BOOKING_CARGA BCG ON BOO.AUTONUM_BOO = BCG.AUTONUM_BOO INNER JOIN REDEX..TB_PATIO_CS PCS ON BCG.AUTONUM_BCG = PCS.AUTONUM_BCG AND BOO.AUTONUM_BOO = @BookingId", parametros).Single();
            }
        }

        public bool ObrigatorioDescargaYard()
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var resultado = con.Query<int>(@"SELECT ISNULL(FLAG_COL_DESCARGA_YARD,0) FROM REDEX..TB_PARAMETROS WHERE COD_EMPRESA = 1").Single();

                return resultado > 0;
            }
        }

        public bool ExistemItensSemPosicao(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

                var resultado = con.Query<int>(@"SELECT COUNT(*) FROM REDEX..TB_MARCANTES_RDX WHERE AUTONUM_TALIE = @TalieId AND YARD IS NULL", parametros).Single();

                return resultado > 0;
            }
        }

        public bool ExistemEtiquetas(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

                var resultado = con.Query<int>(@"SELECT COUNT(*) FROM REDEX..TB_MARCANTES_RDX T WHERE T.AUTONUM_TALIE = @TalieId", parametros).Single();

                return resultado > 0;
            }
        }

        public bool ExistemEtiquetasComPendencia(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

                var resultado = con.Query<int>(@"SELECT COUNT(*) FROM REDEX..TB_TALIE T INNER JOIN REDEX..TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE INNER JOIN REDEX..ETIQUETAS E ON TI.AUTONUM_REGCS = E.AUTONUM_RCS WHERE T.AUTONUM_TALIE = @TalieId AND EMISSAO IS NULL", parametros).Single();

                return resultado > 0;
            }
        }

        public bool ExistemMarcantesComPendencia(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

                var resultado = con.Query<int>(@"
                    SELECT COUNT(*) FROM
                        (SELECT SUM(VOLUMES) QTDE_MC FROM REDEX..TB_MARCANTES_RDX WHERE AUTONUM_TALIE = @TalieId) A,
                        (SELECT SUM(QTDE_DESCARGA) QTDE_DS FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TALIE = @TalieId) B
                    WHERE A.QTDE_MC <> B.QTDE_DS", parametros).Single();

                return resultado > 0;
            }
        }

        public void GerarAlertaEtiqueta(int id, int alerta)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);
                parametros.Add(name: "Alerta", value: alerta, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE REDEX..TB_TALIE SET ALERTA_ETIQUETA = @Alerta WHERE AUTONUM_TALIE = @TalieId", parametros);
            }
        }

        public Talie ObterTaliePorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                //B.AUTONUM_PATIOS As PatioId,

                return con.Query<Talie>(@"
                    SELECT
                        A.AUTONUM_TALIE As Id,
                        A.AUTONUM_REG As RegistroId,
                        A.INICIO,
                        A.TERMINO,
                        A.CROSSDOCKING,
                        A.CONFERENTE As ConferenteId,
                        A.EQUIPE As EquipeId,
                        A.AUTONUM_BOO As BookingId,
                        A.FORMA_OPERACAO As OperacaoId,
                        A.PLACA,
                        B.REFERENCE,
                        A.FLAG_FECHADO As Fechado,
                        A.AUTONUM_GATE As GateId,
                        A.OBS As Observacoes,
                        C.FANTASIA As Cliente,
                        R.PATIO as Patio,
                        A.AUTONUM_PATIO as PatioId,
                        P.ID_CONTEINER as ConteinerId
                    FROM
                        REDEX..TB_TALIE A
                    LEFT JOIN
                        REDEX..TB_BOOKING B ON A.AUTONUM_BOO = B.AUTONUM_BOO
                    LEFT JOIN 
                        REDEX..TB_REGISTRO R on b.autonum_boo = r.autonum_boo
                    LEFT JOIN 
                        REDEX..TB_PATIO P on A.autonum_patio = P.autonum_patio
                    LEFT JOIN
	                    REDEX..TB_CAD_PARCEIROS C ON B.AUTONUM_PARCEIRO = C.AUTONUM
                    WHERE
                        AUTONUM_TALIE = @Id", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<TalieConteiner> ObterTaliesConteinersPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<TalieConteiner>(@"
                    SELECT
                        A.AUTONUM_TALIE As TalieId,
                        A.AUTONUM_PATIO as PatioId,
                        ISNULL(P.ID_CONTEINER,'') as ConteinerId,
                        A.AUTONUM_REG as Lote
                    FROM
                        REDEX..TB_TALIE A
                    LEFT JOIN
                        REDEX..TB_BOOKING B ON A.AUTONUM_BOO = B.AUTONUM_BOO
                    LEFT JOIN TB_PATIO P on A.autonum_patio = P.autonum_patio
                    WHERE
                        AUTONUM_REG = @Id", parametros);
            }
        }

        public int CadastrarTalie(Talie talie)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Inicio", value: talie.Inicio, direction: ParameterDirection.Input);
                parametros.Add(name: "CrossDocking", value: talie.CrossDocking.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "ConferenteId", value: talie.ConferenteId, direction: ParameterDirection.Input);
                parametros.Add(name: "EquipeId", value: talie.EquipeId, direction: ParameterDirection.Input);
                parametros.Add(name: "BookingId", value: talie.BookingId, direction: ParameterDirection.Input);
                parametros.Add(name: "OperacaoId", value: talie.OperacaoId, direction: ParameterDirection.Input);
                parametros.Add(name: "Placa", value: talie.Placa, direction: ParameterDirection.Input);
                parametros.Add(name: "GateId", value: talie.GateId, direction: ParameterDirection.Input);
                parametros.Add(name: "RegistroId", value: talie.RegistroId, direction: ParameterDirection.Input);
                parametros.Add(name: "Observacoes", value: talie.Observacoes, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                return con.Query<int>(SqlQueries.CadastrarTalie, parametros).FirstOrDefault();
            }
        }

        public void AtualizarTalie(Talie talie)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Inicio", value: talie.Inicio, direction: ParameterDirection.Input);
                parametros.Add(name: "CrossDocking", value: talie.CrossDocking.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "ConferenteId", value: talie.ConferenteId, direction: ParameterDirection.Input);
                parametros.Add(name: "EquipeId", value: talie.EquipeId, direction: ParameterDirection.Input);
                parametros.Add(name: "OperacaoId", value: talie.OperacaoId, direction: ParameterDirection.Input);
                parametros.Add(name: "Observacoes", value: talie.Observacoes, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: talie.Id, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE REDEX..TB_TALIE
                        SET
		                    INICIO = @Inicio,
		                    CROSSDOCKING = @CrossDocking,
		                    CONFERENTE = @ConferenteId,
		                    EQUIPE = @EquipeId,
		                    FORMA_OPERACAO = @OperacaoId,
		                    OBS = @Observacoes
                    WHERE AUTONUM_TALIE = @Id", parametros);
            }
        }

        public void ExcluirTalie(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                con.Open();

                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                using (var transaction = con.BeginTransaction())
                {
                    con.Execute(@"DELETE FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TALIE = @Id", parametros);
                    con.Execute(@"DELETE FROM REDEX..TB_TALIE WHERE AUTONUM_TALIE = @Id", parametros);

                    transaction.Commit();
                }
            }
        }

        public int ObterProximoIdPatioCS()
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                return con.Query<int>(@"SELECT ident_current('redex.dbo.seq_tb_patio_Cs')").Single();

            }
        }

        public int ObterProximoIdCargaSoltaYard()
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                return con.Query<int>(@"SELECT ident_current('redex.dbo.SEQ_CARGA_SOLTA_YARD')").Single();
            }
        }

        public int ObterProximoIdArmGate()
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                return con.Query<int>(@"SELECT ident_current('REDEX..SEQ_TB_AMR_GATE')").Single();
            }
        }



        public void FinalizarTalie(int id, DateTime inicio, decimal pesoBruto, int bookingId)
        {
            //    using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            //    {
            //        con.Open();

            //        var parametros = new DynamicParameters();
            //        parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);
            //        parametros.Add(name: "Inicio", value: inicio, direction: ParameterDirection.Input);

            //        using (var transaction = con.BeginTransaction())
            //        {
            //            var linhas = con.Query<Talie>(@"
            //                    SELECT
            //                        D.AUTONUM_BCG As BookingCargaId,
            //                        B.QTDE_DESCARGA As QuantidadeDescarga,
            //                        DECODE(B.AUTONUM_EMB, NULL, D.AUTONUM_EMB, B.AUTONUM_EMB) As EmbalagemId,
            //                        D.AUTONUM_PRO As ProdutoId,
            //                        B.MARCA,
            //                        B.COMPRIMENTO,
            //                        B.LARGURA,
            //                        B.ALTURA,
            //                        B.PESO,
            //                        E.AUTONUM_REGCS As RegistroCsId,
            //                        B.AUTONUM_NF As NotaFiscalId,
            //                        B.AUTONUM_TI As TalieItemId,
            //                        B.QTDE_ESTUFAGEM As QuantidadeEstufagem,
            //                        B.YARD,
            //                        B.ARMAZEM,
            //                        E.AUTONUM_PATIOS As PatioId,
            //                        B.IMO,
            //                        B.UNO,
            //                        B.IMO2,
            //                        B.UNO2,
            //                        B.IMO3,
            //                        B.UNO3,
            //                        B.IMO4,
            //                        B.UNO4,
            //                        ETQ.CODPRODUTO As CodigoProduto
            //                    FROM
            //                        REDEX..TB_TALIE A
            //                    INNER JOIN
            //                        REDEX..TB_TALIE_ITEM B ON A.AUTONUM_TALIE = B.AUTONUM_TALIE
            //                    INNER JOIN
            //                        REDEX..TB_REGISTRO_CS E ON E.AUTONUM_REGCS = B.AUTONUM_REGCS
            //                    INNER JOIN
            //                        REDEX..TB_BOOKING_CARGA D ON D.AUTONUM_BCG = E.AUTONUM_BCG
            //                    INNER JOIN
            //                        REDEX..TB_BOOKING E ON D.AUTONUM_BOO = E.AUTONUM_BOO
            //                    LEFT JOIN
            //                        (
            //                            SELECT
            //                                AUTONUM_RCS,
            //                                SUBSTR(CODPRODUTO,1,8) CODPRODUTO
            //                            FROM
            //                                REDEX..ETIQUETAS
            //                            GROUP BY
            //                                AUTONUM_RCS,
            //                                SUBSTR(CODPRODUTO,1,8)
            //                        ) ETQ ON E.AUTONUM_REGCS = ETQ.AUTONUM_RCS
            //                    WHERE
            //                        A.AUTONUM_TALIE = @TalieId", parametros);

            //            foreach (var linha in linhas)
            //            {
            //                parametros = new DynamicParameters();

            //                var patioCsId = ObterProximoIdPatioCS();

            //                parametros.Add(name: "PatioCsId", value: patioCsId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "PrimeiraEntrada", value: inicio, direction: ParameterDirection.Input);
            //                parametros.Add(name: "BookingCargaId", value: linha.BookingCargaId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "QuantidadeDescarga", value: linha.QuantidadeDescarga, direction: ParameterDirection.Input);
            //                parametros.Add(name: "EmbalagemReserva", value: linha.EmbalagemReserva, direction: ParameterDirection.Input);
            //                parametros.Add(name: "ProdutoId", value: linha.ProdutoId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Marca", value: linha.Marca, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Comprimento", value: linha.Comprimento, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Largura", value: linha.Largura, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Altura", value: linha.Altura, direction: ParameterDirection.Input);
            //                parametros.Add(name: "VolumeDeclarado", value: linha.VolumeDeclarado, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Peso", value: linha.Peso, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Historico", value: linha.Historico.ToInt(), direction: ParameterDirection.Input);
            //                parametros.Add(name: "RegistroCsId", value: linha.RegistroCsId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "NotaFiscalId", value: linha.NotaFiscalId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "TalieItemId", value: linha.TalieItemId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "QuantidadeEstufagem", value: linha.QuantidadeEstufagem, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Yard", value: linha.Yard, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Armazem", value: linha.Armazem, direction: ParameterDirection.Input);
            //                parametros.Add(name: "PatioId", value: linha.PatioId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "IMO", value: linha.IMO, direction: ParameterDirection.Input);
            //                parametros.Add(name: "IMO2", value: linha.IMO2, direction: ParameterDirection.Input);
            //                parametros.Add(name: "IMO3", value: linha.IMO3, direction: ParameterDirection.Input);
            //                parametros.Add(name: "IMO4", value: linha.IMO4, direction: ParameterDirection.Input);
            //                parametros.Add(name: "UNO", value: linha.UNO, direction: ParameterDirection.Input);
            //                parametros.Add(name: "UNO2", value: linha.UNO2, direction: ParameterDirection.Input);
            //                parametros.Add(name: "UNO3", value: linha.UNO3, direction: ParameterDirection.Input);
            //                parametros.Add(name: "UNO4", value: linha.UNO4, direction: ParameterDirection.Input);
            //                parametros.Add(name: "CodigoProduto", value: linha.CodigoProduto, direction: ParameterDirection.Input);

            //                con.Execute(@"
            //                    INSERT INTO
            //                     REDEX..TB_PATIO_CS
            //                      (
            //                       AUTONUM_PCS,
            //                       AUTONUM_BCG,
            //                       QTDE_ENTRADA,
            //                       AUTONUM_EMB,
            //                       AUTONUM_PRO,
            //                       MARCA,
            //                       VOLUME_DECLARADO,
            //                       COMPRIMENTO,
            //                       LARGURA,
            //                       ALTURA,
            //                       BRUTO,
            //                       DT_PRIM_ENTRADA,
            //                       FLAG_HISTORICO,
            //                       AUTONUM_REGCS,
            //                       AUTONUM_NF,
            //                       TALIE_DESCARGA,
            //                       QTDE_ESTUFAGEM,
            //                       YARD,
            //                       ARMAZEM,
            //                       AUTONUM_PATIOS,
            //                       PATIO,
            //                       IMO,
            //                       IMO2,
            //                       IMO3,
            //                                IMO4,
            //                                UNO,
            //                                UNO2,
            //                       UNO3,
            //                       UNO4,
            //                       CODPRODUTO
            //                      ) VALUES (
            //                                @PatioCsId,
            //                                @BookingCargaId,
            //                                @QuantidadeDescarga,
            //                                @EmbalagemReserva,
            //                                @ProdutoId,
            //                                @Marca,
            //                                @VolumeDeclarado,
            //                                @Comprimento,
            //                                @Largura,
            //                                @Altura,
            //                                @Peso,
            //                                @PrimeiraEntrada,
            //                                @Historico,
            //                                @RegistroCsId,
            //                                @NotaFiscalId,
            //                                @TalieItemId,
            //                                @QuantidadeEstufagem,
            //                                @Yard,
            //                                @Armazem,
            //                                @PatioId,
            //                                @PatioId,
            //                                @IMO,
            //                                @IMO2,
            //                                @IMO3,
            //                                @IMO4,
            //                                @UNO,
            //                                @UNO2,
            //                                @UNO3,
            //                                @UNO4,
            //                                @CodigoProduto
            //                      )", parametros);

            //                parametros = new DynamicParameters();

            //                parametros.Add(name: "PatioCsId", value: patioCsId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

            //                con.Execute(@"UPDATE REDEX..TB_MARCANTES_RDX SET AUTONUM_PCS = @PatioCsId WHERE AUTONUM_TALIE = @TalieId AND VOLUMES > 0 AND ISNULL(AUTONUM_PCS,0) = 0", parametros);

            //                var marcantes = con.Query<int>(@"SELECT AUTONUM FROM REDEX..TB_MARCANTES_RDX WHERE AUTONUM_TALIE = @TalieId ORDER BY AUTONUM", parametros);

            //                foreach (var marcante in marcantes)
            //                {
            //                    parametros = new DynamicParameters();

            //                    parametros.Add(name: "CsYard", value: ObterProximoIdCargaSoltaYard(), direction: ParameterDirection.Input);
            //                    parametros.Add(name: "Autonum", value: marcante, direction: ParameterDirection.Input);

            //                    con.Execute(@"
            //                        INSERT INTO
            //                            REDEX..TB_CARGA_SOLTA_YARD
            //                                (
            //                                     AUTONUM,
            //                                     AUTONUM_PATIOCS,
            //                                     ARMAZEM,
            //                                     YARD,
            //                                     QUANTIDADE,
            //                                     MOTIVO_COL
            //                                ) SELECT
            //                                   @CsYard,
            //                                    AUTONUM_PCS,
            //                                    Armazem,
            //                                    Yard,
            //                                    VOLUMES,
            //                                    0
            //                                  FROM REDEX..TB_MARCANTES_RDX Where AUTONUM = @Autonum AND ISNULL(ARMAZEM,0) > 0 ", parametros);

            //                    con.Execute(@"UPDATE REDEX..TB_MARCANTES_RDX SET AUTONUM_CS_YARD = @CsYard WHERE AUTONUM = @Autonum", parametros);
            //                }

            //                if (linha.IMO != string.Empty)
            //                {
            //                    parametros = new DynamicParameters();

            //                    parametros.Add(name: "IMO", value: linha.IMO, direction: ParameterDirection.Input);
            //                    parametros.Add(name: "BookingCargaId", value: linha.BookingCargaId, direction: ParameterDirection.Input);

            //                    con.Execute(@"UPDATE REDEX..TB_BOOKING_CARGA SET IMO = @IMO WHERE AUTONUM_BCG = @BookingCargaId", parametros);
            //                }

            //                parametros = new DynamicParameters();

            //                parametros.Add(name: "IdAmrGate", value: ObterProximoIdArmGate(), direction: ParameterDirection.Input);
            //                parametros.Add(name: "GateId", value: linha.GateId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "PatioCsId", value: patioCsId, direction: ParameterDirection.Input);
            //                parametros.Add(name: "PesoBruto", value: pesoBruto, direction: ParameterDirection.Input);
            //                parametros.Add(name: "Inicio", value: inicio, direction: ParameterDirection.Input);
            //                parametros.Add(name: "BookingId", value: linha.BookingId, direction: ParameterDirection.Input);

            //                con.Execute(@"
            //                        INSERT INTO
            //                         REDEX..TB_AMR_GATE
            //                          (
            //                           AUTONUM,
            //                           GATE,
            //                           CNTR_RDX,
            //                           CS_RDX,
            //                           PESO_ENTRADA,
            //                           PESO_SAIDA,
            //                           DATA,
            //                           ID_BOOKING,
            //                           ID_OC,
            //                           FUNCAO_GATE,
            //                           FLAG_HISTORICO
            //                          ) VALUES (
            //                           @IdAmrGate,
            //                                    @GateId,
            //                                    0,
            //                                    @PatioCsId,
            //                                    @PesoBruto,
            //                                    0,
            //                                    @Inicio,
            //                                    @BookingId,
            //                                    0,
            //                                    203,
            //                                    0
            //                          )", parametros);

            //                parametros = new DynamicParameters();
            //                parametros.Add(name: "PatioCsId", value: patioCsId, direction: ParameterDirection.Input);

            //                con.Execute(@"UPDATE REDEX..TB_PATIO_CS SET PCS_PAI = @PatioCsId WHERE AUTONUM_PCS = @PatioCsId", parametros);
            //            }

            //            parametros = new DynamicParameters();
            //            parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);

            //            con.Execute(@"UPDATE REDEX..TB_TALIE SET FLAG_FECHADO = 1,TERMINO = SYSDATE WHERE AUTONUM_TALIE = @TalieId", parametros);

            //            var qtdeBooking = ObterQuantidadeBooking(bookingId);
            //            var qtdeEntradaBooking = ObterQuantidadeEntradaBooking(bookingId);

            //            if (qtdeBooking != qtdeEntradaBooking)
            //            {
            //                parametros = new DynamicParameters();
            //                parametros.Add(name: "BookingId", value: bookingId, direction: ParameterDirection.Input);

            //                con.Execute(@"UPDATE REDEX..TB_BOOKING SET FLAG_FINALIZADO = 1 WHERE AUTONUM_BOO = @BookingId", parametros);
            //            }

            //            parametros = new DynamicParameters();
            //            parametros.Add(name: "BookingId", value: bookingId, direction: ParameterDirection.Input);

            //            con.Execute(@"UPDATE REDEX..TB_BOOKING SET STATUS_RESERVA = 2 WHERE AUTONUM_BOO = @BookingId", parametros);

            //            transaction.Commit();
            //        }
            //    }
        }

        public TalieItem ObterItemNF(int registroId, string nf)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "RegistroId", value: registroId, direction: ParameterDirection.Input);
                parametros.Add(name: "NF", value: $"%{nf}", direction: ParameterDirection.Input);

                try
                {
                    return con.Query<TalieItem>(@"
                    SELECT
                        A.AUTONUM_REGCS AS RegistroCsId,
                        B.PESO_BRUTO As Peso,
                        A.NF As NotaFiscal,
                        B.AUTONUM_EMB As EmbalagemId,
                        C.SIGLA As EmbalagemSigla,
                        C.DESCRICAO_EMB + '-' + C.AUTONUM_EMB AS Embalagem,
                        B.IMO As IMO1,
                        B.IMO2,
                        B.IMO3,
                        B.IMO4,
                        B.UNO As UNO1,
                        B.UNO2,
                        B.UNO3,
                        B.UNO4,
                        (SELECT ISNULL(SUM(QTDE_DESCARGA),0) FROM REDEX.TB_TALIE_ITEM WHERE AUTONUM_REGCS = A.AUTONUM_REGCS) As QuantidadeDescarga,
                        (SELECT ISNULL(SUM(QUANTIDADE),0) FROM REDEX.TB_REGISTRO_CS WHERE AUTONUM_REGCS = A.AUTONUM_REGCS) As Quantidade,
                        (SELECT ISNULL(AUTONUM_NF,0) FROM REDEX.TB_NOTAS_FISCAIS WHERE AUTONUM_BOO = B.AUTONUM_BOO AND AUTONUM_REG = A.AUTONUM_REG AND NUM_NF LIKE @NF) As NotaFiscalId
                    FROM
                        REDEX..TB_REGISTRO_CS A
                    INNER JOIN
                        REDEX..TB_BOOKING_CARGA B ON A.AUTONUM_BCG = B.AUTONUM_BCG
                    INNER JOIN
                        REDEX..TB_CAD_EMBALAGENS C ON B.AUTONUM_EMB = C.AUTONUM_EMB
                    INNER JOIN
                        REDEX..TB_CAD_PRODUTOS D ON B.AUTONUM_PRO = D.AUTONUM_PRO
                    WHERE
                        A.AUTONUM_REG = @RegistroId
                     AND
                        A.NF LIKE @NF", parametros).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public int CadastrarItemTalie(TalieItem item, BrowserInfo browserInfo)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    var parametros = new DynamicParameters();

                    var armazem = ObterPosicaoPatio(item.PatioId, item.Yard);

                    parametros.Add(name: "TalieId", value: item.TalieId, direction: ParameterDirection.Input);
                    parametros.Add(name: "RegistroCsId", value: item.RegistroCsId, direction: ParameterDirection.Input);
                    parametros.Add(name: "Quantidade", value: item.Quantidade, direction: ParameterDirection.Input);
                    parametros.Add(name: "Comprimento", value: item.Comprimento, direction: ParameterDirection.Input);
                    parametros.Add(name: "Largura", value: item.Largura, direction: ParameterDirection.Input);
                    parametros.Add(name: "Altura", value: item.Altura, direction: ParameterDirection.Input);
                    parametros.Add(name: "Peso", value: item.Peso, direction: ParameterDirection.Input);
                    parametros.Add(name: "Remonte", value: item.Remonte, direction: ParameterDirection.Input);
                    parametros.Add(name: "Fumigacao", value: item.Fumigacao, direction: ParameterDirection.Input);
                    parametros.Add(name: "Fragil", value: item.Fragil.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Madeira", value: item.Madeira.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Avariado", value: item.Avariado.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Yard", value: item.Yard, direction: ParameterDirection.Input);
                    parametros.Add(name: "Armazem", value: armazem, direction: ParameterDirection.Input);
                    parametros.Add(name: "NotaFiscalId", value: item.NotaFiscalId, direction: ParameterDirection.Input);
                    parametros.Add(name: "NotaFiscal", value: item.NotaFiscal, direction: ParameterDirection.Input);
                    parametros.Add(name: "IMO1", value: item.IMO1, direction: ParameterDirection.Input);
                    parametros.Add(name: "IMO2", value: item.IMO2, direction: ParameterDirection.Input);
                    parametros.Add(name: "IMO3", value: item.IMO3, direction: ParameterDirection.Input);
                    parametros.Add(name: "IMO4", value: item.IMO4, direction: ParameterDirection.Input);
                    parametros.Add(name: "UNO1", value: item.UNO1, direction: ParameterDirection.Input);
                    parametros.Add(name: "UNO2", value: item.UNO2, direction: ParameterDirection.Input);
                    parametros.Add(name: "UNO3", value: item.UNO3, direction: ParameterDirection.Input);
                    parametros.Add(name: "UNO4", value: item.UNO4, direction: ParameterDirection.Input);
                    parametros.Add(name: "CodigoEmbalagem", value: item.EmbalagemId, direction: ParameterDirection.Input);

                    parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var id = con.Execute(@"
                        INSERT INTO
                            REDEX..TB_TALIE_ITEM
                            (

                                AUTONUM_TALIE,
                                AUTONUM_REGCS,
                                QTDE_DESCARGA,
                                COMPRIMENTO,
                                LARGURA,
                                ALTURA,
                                PESO,
                                QTDE_ESTUFAGEM,
                                REMONTE,
                                FUMIGACAO,
                                FLAG_FRAGIL,
                                FLAG_MADEIRA,
                                YARD,
                                ARMAZEM,
                                AUTONUM_NF,
                                NF,
                                IMO,
                                IMO2,
                                IMO3,
                                IMO4,
                                UNO,
                                UNO2,
                                UNO3,
                                UNO4,
                                AUTONUM_EMB
                            ) VALUES (

                                @TalieId,
                                @RegistroCsId,
                                @Quantidade,
                                @Comprimento,
                                @Largura,
                                @Altura,
                                @Peso,
                                @Quantidade,
                                @Remonte,
                                @Fumigacao,
                                @Fragil,
                                @Madeira,
                                @Yard,
                                @Armazem,
                                @NotaFiscalId,
                                @NotaFiscal,
                                @IMO1,
                                @IMO2,
                                @IMO3,
                                @IMO4,
                                @UNO1,
                                @UNO2,
                                @UNO3,
                                @UNO4,
                                @CodigoEmbalagem) SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction);

                    parametros = new DynamicParameters();

                    parametros.Add(name: "TalieId", value: id, direction: ParameterDirection.Input);
                    parametros.Add(name: "UsuarioId", value: 999, direction: ParameterDirection.Input);
                    parametros.Add(name: "BrowserName", value: browserInfo.Nome, direction: ParameterDirection.Input);
                    parametros.Add(name: "BrowserVersion", value: browserInfo.Versao, direction: ParameterDirection.Input);
                    parametros.Add(name: "MobileDeviceModel", value: browserInfo.Modelo, direction: ParameterDirection.Input);
                    parametros.Add(name: "MobileDevideManufacturer", value: browserInfo.Fabricante, direction: ParameterDirection.Input);
                    parametros.Add(name: "FlagMobile", value: browserInfo.Mobile.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "IpConnection", value: browserInfo.IP, direction: ParameterDirection.Input);

                    //con.Execute(@"
                    //    INSERT INTO
                    //        TB_TALIE_ITEM_COL
                    //            (
                    //                AUTONUM_TI,
                    //                USUARIO,
                    //                DT,
                    //                BROWSER_NAME,
                    //                BROWSER_VERSION,
                    //                MOBILEDEVICEMODEL,
                    //                MOBILEDEVICEMANUFACTURER,
                    //                FLAG_MOBILE

                    //            ) VALUES (
                    //                @TalieId,
                    //                @UsuarioId,
                    //                getdate(),
                    //                @BrowserName,
                    //                @BrowserVersion,
                    //                @MobileDeviceModel,
                    //                @MobileDevideManufacturer,
                    //                @FlagMobile
                    //                )", parametros, transaction);

                    transaction.Commit();

                    return id;
                }
            }
        }

        public void AtualizarItemTalie(TalieItem item)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                var armazem = ObterPosicaoPatio(item.PatioId, item.Yard);

                parametros.Add(name: "TalieId", value: item.TalieId, direction: ParameterDirection.Input);
                parametros.Add(name: "RegistroCsId", value: item.RegistroCsId, direction: ParameterDirection.Input);
                parametros.Add(name: "Quantidade", value: item.Quantidade, direction: ParameterDirection.Input);
                parametros.Add(name: "Comprimento", value: item.Comprimento, direction: ParameterDirection.Input);
                parametros.Add(name: "Largura", value: item.Largura, direction: ParameterDirection.Input);
                parametros.Add(name: "Altura", value: item.Altura, direction: ParameterDirection.Input);
                parametros.Add(name: "Peso", value: item.Peso, direction: ParameterDirection.Input);
                parametros.Add(name: "Remonte", value: item.Remonte, direction: ParameterDirection.Input);
                parametros.Add(name: "Fumigacao", value: item.Fumigacao.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "Fragil", value: item.Fragil.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "Madeira", value: item.Madeira.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "Avariado", value: item.Avariado.ToInt(), direction: ParameterDirection.Input);
                parametros.Add(name: "Yard", value: item.Yard, direction: ParameterDirection.Input);
                parametros.Add(name: "Armazem", value: armazem, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaFiscalId", value: item.NotaFiscalId, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaFiscal", value: item.NotaFiscal, direction: ParameterDirection.Input);
                parametros.Add(name: "IMO1", value: item.IMO1, direction: ParameterDirection.Input);
                parametros.Add(name: "IMO2", value: item.IMO2, direction: ParameterDirection.Input);
                parametros.Add(name: "IMO3", value: item.IMO3, direction: ParameterDirection.Input);
                parametros.Add(name: "IMO4", value: item.IMO4, direction: ParameterDirection.Input);
                parametros.Add(name: "UNO1", value: item.UNO1, direction: ParameterDirection.Input);
                parametros.Add(name: "UNO2", value: item.UNO2, direction: ParameterDirection.Input);
                parametros.Add(name: "UNO3", value: item.UNO3, direction: ParameterDirection.Input);
                parametros.Add(name: "UNO4", value: item.UNO4, direction: ParameterDirection.Input);
                parametros.Add(name: "CodigoEmbalagem", value: item.EmbalagemId, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: item.Id, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE
                        REDEX..TB_TALIE_ITEM
                            SET
                                QTDE_DESCARGA = @Quantidade,
                                COMPRIMENTO =@Comprimento,
                                LARGURA =@Largura,
                                ALTURA =@Altura,
                                PESO =@Peso,
                                QTDE_ESTUFAGEM =@Quantidade,
                                REMONTE =@Remonte,
                                FUMIGACAO =@Fumigacao,
                                FLAG_FRAGIL =@Fragil,
                                FLAG_MADEIRA =@Madeira,
                                YARD =@Yard,
                                ARMAZEM =@Armazem,
                                AUTONUM_NF =@NotaFiscalId,
                                NF =@NotaFiscal,
                                IMO =@IMO1,
                                IMO2 =@IMO2,
                                IMO3 =@IMO3,
                                IMO4 =@IMO4,
                                UNO =@UNO1,
                                UNO2 =@UNO2,
                                UNO3 =@UNO3,
                                UNO4 =@UNO4,
                                AUTONUM_EMB =@CodigoEmbalagem
                        WHERE AUTONUM_TI = @Id", parametros);
            }
        }

        public void ExcluirTalieItem(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieItemId", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TI = @TalieItemId", parametros);
            }
        }

        public TalieItem ObterItemTaliePorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<TalieItem>(@"
                    SELECT
                        TI.AUTONUM_TI As Id,
                        TI.AUTONUM_TALIE As TalieId,
                        TI.AUTONUM_NF As NotaFiscalId,
                        (SELECT ISNULL(SUM(QTDE_DESCARGA),0) FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_REGCS = TI.AUTONUM_REGCS AND AUTONUM_TI = @id) As QuantidadeDescarga,
                        (SELECT ISNULL(SUM(QUANTIDADE),0) FROM REDEX..TB_REGISTRO_CS WHERE AUTONUM_REGCS = TI.AUTONUM_REGCS) As Quantidade,
                        TI.REMONTE,
                        TI.FUMIGACAO,
                        TI.IMO As IMO1,
                        TI.UNO As UNO1,
                        TI.IMO2,
                        TI.UNO2,
                        TI.IMO3,
                        TI.UNO3,
                        TI.IMO4,
                        TI.UNO4,
                        TI.YARD,
                        TI.FLAG_MADEIRA As Madeira,
                        TI.FLAG_FRAGIL As Fragil,
                        TI.AUTONUM_REGCS As RegistroCsId,
                        TI.NF As NotaFiscal,
                        TI.COMPRIMENTO,
                        TI.LARGURA,
                        TI.ALTURA,
                        TI.PESO,
                        E.SIGLA,
                        E.SIGLA AS EmbalagemSigla,
                        E.AUTONUM_EMB As EmbalagemId,
                        E.DESCRICAO_EMB + '-' + E.AUTONUM_EMB AS EMBALAGEM,
                        TI.YARD
                    FROM
                        REDEX..TB_TALIE_ITEM TI
                    LEFT JOIN
                        REDEX..TB_NOTAS_FISCAIS NF ON TI.AUTONUM_NF = NF.AUTONUM_NF
                    LEFT JOIN
                        REDEX..TB_CAD_EMBALAGENS E ON TI.AUTONUM_EMB = E.AUTONUM_EMB
                    WHERE
                        TI.AUTONUM_TI = @Id", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<TalieItem> ObterItens(int talieId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: talieId, direction: ParameterDirection.Input);

                return con.Query<TalieItem>(@"
                    SELECT
                        TI.AUTONUM_TI AS Id,
                        'NF ' + ISNULL(TI.NF,'') + ' Contêiner: ' + ISNULL(P.ID_CONTEINER,'') + '  '  AS Descricao
                    FROM
                        REDEX..TB_TALIE T
                    INNER JOIN
                        REDEX..TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE
                    LEFT JOIN TB_PATIO P on T.autonum_patio = P.autonum_patio
                    LEFT JOIN
                        REDEX..TB_CAD_EMBALAGENS E ON TI.AUTONUM_EMB = E.AUTONUM_EMB
                    WHERE
                        T.AUTONUM_TALIE = @TalieId
                    ORDER BY
                        TI.AUTONUM_TI", parametros);
            }
        }

        public ResumoQuantidadeDescarga ObterResumoQuantidadeDescarga(int talieId, string nf)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "TalieId", value: talieId, direction: ParameterDirection.Input);
                parametros.Add(name: "NF", value: nf, direction: ParameterDirection.Input);

                return con.Query<ResumoQuantidadeDescarga>(@"
                    SELECT
                        (SELECT ISNULL(SUM(QTDE_DESCARGA),0) FROM TB_TALIE_ITEM WHERE AUTONUM_REGCS = A.AUTONUM_REGCS) As QuantidadeDescarga,
                        (SELECT ISNULL(SUM(QUANTIDADE),0) FROM TB_REGISTRO_CS WHERE AUTONUM_REGCS = A.AUTONUM_REGCS) As QuantidadeManifestada
                    FROM
                        REDEX..TB_REGISTRO_CS A
                    INNER JOIN
                        REDEX..TB_BOOKING_CARGA B ON A.AUTONUM_BCG = B.AUTONUM_BCG
                    INNER JOIN
                        REDEX..TB_TALIE C ON A.AUTONUM_REG = C.AUTONUM_REG
                    WHERE
                        C.AUTONUM_TALIE = @TalieId AND A.NF = @NF", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<NotaFiscalDTO> ObterBalancoTalie(int registroId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "RegistroId", value: registroId, direction: ParameterDirection.Input);

                return con.Query<NotaFiscalDTO>(@"
                    SELECT
                        Volumes.NumeroNF,
                        Volumes.Mercadoria,
                        Volumes.Embalagem,
                        Volumes.Descarregado,
                        Volumes.Total
                    FROM
                    (
                        SELECT
                            A.NF As NumeroNF,
                            D.DESC_PRODUTO AS Mercadoria,
                            C.DESCRICAO_EMB As Embalagem,
                            DECODE(E.AUTONUM_TI, NULL, 0, 1) Descarregado,
                            COUNT(*) Over() Total
                        FROM
                            REDEX..TB_REGISTRO_CS A
                        INNER JOIN
                            REDEX..TB_BOOKING_CARGA B ON A.AUTONUM_BCG = B.AUTONUM_BCG
                        INNER JOIN
                            REDEX..TB_CAD_EMBALAGENS C ON B.AUTONUM_EMB = C.AUTONUM_EMB
                        INNER JOIN
                            REDEX..TB_CAD_PRODUTOS D ON B.AUTONUM_PRO = D.AUTONUM_PRO
                        LEFT JOIN
                            REDEX..TB_TALIE_ITEM E ON A.AUTONUM_REGCS = E.AUTONUM_REGCS
                        WHERE
                            A.AUTONUM_REG = @RegistroId
                    ) Volumes", parametros);
            }
        }

        public bool IMOValido(string imo)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "IMO", value: imo, direction: ParameterDirection.Input);

                var result = con.Query<int>(@"SELECT COUNT(*) FROM REDEX..TB_CAD_CARGA_PERIGOSA WHERE CODE = @IMO", parametros).Single();

                return result > 0;
            }
        }

        public bool UNOValido(string uno)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "UNO", value: uno, direction: ParameterDirection.Input);

                var result = con.Query<int>(@"SELECT COUNT(*) FROM REDEX..TB_CAD_ONU WHERE CODE = @UNO", parametros).Single();

                return result > 0;
            }
        }

        public IEnumerable<Armazem> ObterArmazens(int patioId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "PatioId", value: patioId, direction: ParameterDirection.Input);

                return con.Query<Armazem>(@"SELECT AUTONUM As Id, DESCR As Descricao FROM SGIPA..TB_ARMAZENS_IPA WHERE DT_SAIDA IS NULL AND ISNULL(FLAG_HISTORICO,0) = 0 AND PATIO = @PatioId ORDER BY DESCR", parametros);
            }
        }

        public IEnumerable<Armazem> ObterDetalhesArmazem(int armazemId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ArmazemId", value: armazemId, direction: ParameterDirection.Input);

                return con.Query<Armazem>(@"
                    SELECT
                        Valida,
                        DECODE(VALIDA, 1, SUBSTR(YARD, 1, 1)) As Quadra,
                        DECODE(VALIDA, 1, SUBSTR(YARD, 2, 2)) As Rua,
                        DECODE(VALIDA, 1, SUBSTR(YARD, 4, 2)) As Fiada,
                        DECODE(VALIDA, 1, SUBSTR(YARD, -1)) As Altura
                    FROM
                        SGIPA..TB_YARD_CS WHERE ARMAZEM = @ArmazemId", parametros);
            }
        }

        public Marcante ObterMarcantePorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<Marcante>(@"SELECT AUTONUM As CodigoMarcante, AUTONUM_REGCS As RegistroCsId, AUTONUM_BOO As BookingId, VOLUMES, FLAG_REGISTRO As Registro, AUTONUM_BOO As BookingId, AUTONUM_TALIE As TalieId FROM REDEX..TB_MARCANTES_RDX WHERE AUTONUM = @Id", parametros).FirstOrDefault();
            }
        }

        public void GravarMarcante(Marcante marcante)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Volumes", value: marcante.Quantidade, direction: ParameterDirection.Input);
                parametros.Add(name: "Quadra", value: marcante.Quadra, direction: ParameterDirection.Input);
                parametros.Add(name: "TalieId", value: marcante.TalieId, direction: ParameterDirection.Input);
                parametros.Add(name: "Armazem", value: marcante.ArmazemId, direction: ParameterDirection.Input);
                parametros.Add(name: "Yard", value: marcante.Quadra, direction: ParameterDirection.Input);
                parametros.Add(name: "CodigoMarcante", value: marcante.CodigoMarcante, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE REDEX..TB_MARCANTES_RDX SET VOLUMES = @Volumes, AUTONUM_TALIE = @TalieId, ARMAZEM = @Armazem, YARD = @Yard WHERE AUTONUM = @CodigoMarcante", parametros);
            }
        }

        public bool ArmazemColetor(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ArmazemId", value: id, direction: ParameterDirection.Input);

                return con.Query<bool>(@"SELECT AUTONUM FROM SGIPA..TB_ARMAZENS_IPA WHERE AUTONUM = @ArmazemId AND FLAG_CT = 1", parametros).Any();
            }
        }

        public Armazem ObterPosicaoPatio(int armazemId, string yard)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ArmazemId", value: armazemId, direction: ParameterDirection.Input);
                parametros.Add(name: "Yard", value: yard, direction: ParameterDirection.Input);

                return con.Query<Armazem>(@"SELECT A.AUTONUM As Id, Y.YARD As Quadra, ISNULL(A.PATIO,0) AS PatioId FROM SGIPA..TB_YARD_CS Y INNER JOIN SGIPA..TB_ARMAZENS_IPA A ON Y.ARMAZEM = A.AUTONUM WHERE Y.ARMAZEM = @ArmazemId AND Y.YARD = @Yard", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<Marcante> ObterMarcantes(int bookingId, int talieId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "BookingId", value: bookingId, direction: ParameterDirection.Input);
                parametros.Add(name: "TalieId", value: talieId, direction: ParameterDirection.Input);

                return con.Query<Marcante>(@"SELECT LPAD(LTRIM(RTRIM(TO_CHAR(M.AUTONUM))), 12, 0) As CodigoMarcante, VOLUMES AS Quantidade, Yard As Quadra FROM REDEX..TB_MARCANTES_RDX M WHERE M.AUTONUM_BOO = @BookingId AND M.AUTONUM_TALIE = @TalieId AND VOLUMES > 0 ORDER BY M.AUTONUM", parametros);
            }
        }

        public void ExcluirMarcante(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "CodigoMarcante", value: id, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE REDEX..TB_MARCANTES_RDX SET VOLUMES = 0 WHERE AUTONUM = @CodigoMarcante", parametros);
            }
        }

        public IEnumerable<NF> ObterNFs(int talieId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "TalieId", value: talieId, direction: ParameterDirection.Input);

                return con.Query<NF>(@"
                    SELECT
	                    NFI.AUTONUM_NF AS Id,
	                    NF.NUM_NF AS NumNF,
						NFI.QTDE as QuantidadeItens
                    FROM
	                    TB_TALIE T
                    INNER JOIN
	                    TB_NOTAS_FISCAIS NF ON T.AUTONUM_REG =NF.AUTONUM_REG
                    INNER JOIN
	                    TB_NOTAS_ITENS NFI ON NF.AUTONUM_NF = NFI.AUTONUM_NF
                    LEFT JOIN
	                    TB_CAD_EMBALAGENS E ON NFI.AUTONUM_EMB = E.AUTONUM_EMB
                    WHERE T.AUTONUM_TALIE = @TalieId
                    ORDER BY
                        T.AUTONUM_TALIE ", parametros);
            }
        }

        public IEnumerable<ItemNF> ObterItensPorNF(int nfId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "nf", value: nfId, direction: ParameterDirection.Input);

                return con.Query<ItemNF>(@"
                    select NF.AUTONUM_NF as NFId,  bcg.qtde as qtde_manifestada, (isnull(BCG.PESO_bruto,0) / bcg.qtde) as peso_manifestado
                    ,bcg.imo,bcg.imo2,bcg.imo3,bcg.imo4,bcg.uno,bcg.uno2,bcg.uno3,bcg.uno4,BCG.AUTONUM_EMB as EmbalagemId,BCG.AUTONUM_PRO as ProdutoID
                     from redex.dbo.tb_registro reg

                     inner join redex.dbo.tb_registro_cs rcs on reg.autonum_reg=rcs.autonum_reg
                     inner join redex.dbo.tb_booking_carga bcg on rcs.autonum_bcg=bcg.autonum_bcg
                     inner join TB_NOTAS_FISCAIS NF on rcs.NF = NF.NUM_NF
                     where NF.AUTONUM_NF= @nf", parametros);
            }
        }

        public IEnumerable<Produto> ObterProdutosPorNF(int nfId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "nf", value: nfId, direction: ParameterDirection.Input);

                return con.Query<Produto>(@"
                    select
                        bcg.AUTONUM_PRO as Id,
                        p.DESC_Produto as Descricao
                     from redex.dbo.tb_registro reg

                     inner join redex.dbo.tb_registro_cs rcs on reg.autonum_reg=rcs.autonum_reg
                     inner join redex.dbo.tb_booking_carga bcg on rcs.autonum_bcg=bcg.autonum_bcg
                     inner join TB_NOTAS_FISCAIS NF on rcs.NF = NF.NUM_NF
                    inner join tb_cad_produtos p on BCG.AUTONUM_PRO = p.AUTONUM_PRO
                     where NF.AUTONUM_NF= @nf", parametros);
            }
        }

        public IEnumerable<Embalagem> ObterEmbalagensPorNF(int nfId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "nf", value: nfId, direction: ParameterDirection.Input);

                return con.Query<Embalagem>(@"
                    select
                        bcg.AUTONUM_PRO as Id,
                        E.DESCRICAO_EMB as Descricao
                     from redex.dbo.tb_registro reg

                     inner join redex.dbo.tb_registro_cs rcs on reg.autonum_reg=rcs.autonum_reg
                     inner join redex.dbo.tb_booking_carga bcg on rcs.autonum_bcg=bcg.autonum_bcg
                     inner join TB_NOTAS_FISCAIS NF on rcs.NF = NF.NUM_NF
                    inner join tb_cad_embalagens E on BCG.AUTONUM_EMB = E.AUTONUM_EMB
                     where NF.AUTONUM_NF= @nf", parametros);
            }
        }

        public NF ObterNFPorId(int nfId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "nf", value: nfId, direction: ParameterDirection.Input);

                return con.Query<NF>(@"
                    SELECT
	                    NFI.AUTONUM_NF AS Id,
	                    NF.NUM_NF AS NumNF,
						NFI.QTDE as QuantidadeItens
                    FROM
	                    TB_NOTAS_FISCAIS NF
                    INNER JOIN
	                    TB_NOTAS_ITENS NFI ON NF.AUTONUM_NF = NFI.AUTONUM_NF
                    LEFT JOIN
	                    TB_CAD_EMBALAGENS E ON NFI.AUTONUM_EMB = E.AUTONUM_EMB
                    WHERE NF.AUTONUM_NF = @nf
                    ORDER BY
                        NF.AUTONUM_NF ", parametros).SingleOrDefault();
            }
        }

        public int ObterRegCSIdPorEmbalagem(int idEmbalagem, int idNF)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "idNf", value: idNF, direction: ParameterDirection.Input);
                parametros.Add(name: "idEmbalagem", value: idEmbalagem, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                     select rcs.AUTONUM_REGCS
                     from redex.dbo.tb_registro reg

                     inner join redex.dbo.tb_registro_cs rcs on reg.autonum_reg=rcs.autonum_reg
                     inner join redex.dbo.tb_booking_carga bcg on rcs.autonum_bcg=bcg.autonum_bcg
                     inner join TB_NOTAS_FISCAIS NF on rcs.NF = NF.NUM_NF
                     where NF.AUTONUM_NF= @idNF and BCG.AUTONUM_PRO = @idEmbalagem ", parametros).SingleOrDefault();
            }
        }

        #region METODOS ASYNC
        public async Task<TalieEntity> ObterDadosTaliePorRegistroAsync(int registro)
        {
            try
            {
                string command = SqlQueries.BuscarDadosTaliePorRegistro;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("registro", registro);
                using (var connection = Connection)
                {
                    return await connection.QueryFirstOrDefaultAsync<TalieEntity>(command, parameters);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<Gate> ObterRegistrosGate(int registro)
        {
            try
            {
                string command = SqlQueries.BuscarDadosGate;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("registro", registro);
                using (var connection = Connection)
                {
                    return await connection.QueryFirstOrDefaultAsync<Gate>(command, parameters);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<ServiceResult<bool>> Update(TalieEntity talie)
        {
            var _serviceResult = new ServiceResult<bool>();
            var query = SqlQueries.AtualizarTalie;
            try
            {
                using (var connection = new SqlConnection(Config.StringConexao()))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Adiciona os parâmetros
                        command.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = talie.INICIO;
                        command.Parameters.Add("@ConferenteId", SqlDbType.Int).Value = talie.CONFERENTE;
                        command.Parameters.Add("@EquipeId", SqlDbType.Int).Value = talie.EQUIPE;
                        command.Parameters.Add("@OperacaoId", SqlDbType.Int).Value = talie.FORMA_OPERACAO;
                        command.Parameters.Add("@Observacoes", SqlDbType.VarChar, 100).Value = talie.OBS ?? (object)DBNull.Value;
                        command.Parameters.Add("@AutonumTalie", SqlDbType.Int).Value = talie.AUTONUM_TALIE;

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            _serviceResult.Result = true;
                            _serviceResult.Mensagens.Add($"Talie \"{talie.AUTONUM_TALIE}\" atualizado com sucesso!");
                        }
                        else
                        {
                            _serviceResult.Result = false;
                            _serviceResult.Mensagens.Add($"Falha ao tentar atualizar Talie \"{talie.AUTONUM_TALIE}\" !");
                        }
                        return _serviceResult;
                    }
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Result = false;
                _serviceResult.Error = ex.Message;
                return _serviceResult;
            }
        }

        public async Task<ServiceResult<int>> GravarTalieAsync(TalieEntity talie)
        {
            var query = SqlQueries.CadastrarTalie;
            var _serviceResult = new ServiceResult<int>();
            try
            {
                using (var connection = new SqlConnection(Config.StringConexao()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Define os parâmetros
                        command.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = DateTime.Now;
                        command.Parameters.Add("@CrossDocking", SqlDbType.Int).Value = 0;
                        command.Parameters.Add("@ConferenteId", SqlDbType.Int).Value = talie.CONFERENTE;
                        command.Parameters.Add("@EquipeId", SqlDbType.Int).Value = talie.EQUIPE;
                        command.Parameters.Add("@BookingId", SqlDbType.Int).Value = talie.AUTONUM_BOO;
                        command.Parameters.Add("@OperacaoId", SqlDbType.Int).Value = talie.FORMA_OPERACAO;
                        command.Parameters.Add("@Placa", SqlDbType.VarChar, 100).Value = talie.PLACA ?? string.Empty;
                        command.Parameters.Add("@GateId", SqlDbType.Int).Value = talie.AUTONUM_GATE;
                        command.Parameters.Add("@RegistroId", SqlDbType.Int).Value = talie.AUTONUM_REG;
                        command.Parameters.Add("@Observacoes", SqlDbType.VarChar, 100).Value = talie.OBS ?? (object)DBNull.Value;

                        // Define o timeout
                        command.CommandTimeout = 30;

                        // Executa a query
                        var result = command.ExecuteScalar();

                        // Retorna o ID gerado
                        _serviceResult.Result = result != null ? Convert.ToInt32(result) : 0;
                        return _serviceResult;
                    }
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Error = ex.Message;
                return _serviceResult;
            }
        }

        public ServiceResult<int> ObterIdNotaFiscal(string numeroNotaFiscal, string codigoBooking, string codigoRegistro)
        {
            var _serviceResult = new ServiceResult<int>();
            string query = SqlQueries.ObterIdNotaFiscal;
            try
            {
                using (var connection = Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AutonumBooking", codigoBooking, DbType.Int32);
                    parameters.Add("@CodigoRegistro", codigoRegistro, DbType.String);
                    parameters.Add("@NumeroNotaFiscal", numeroNotaFiscal, DbType.String);


                    var result = connection.QueryFirstOrDefault(query, parameters);

                    if (result == null)
                    {
                        _serviceResult.Result = 0;
                        _serviceResult.Mensagens.Add("Nota fiscal nao encotrada para o Item");
                    }
                    else
                    {
                        _serviceResult.Result = result;
                        _serviceResult.Mensagens.Add("Nota fiscal encotrada para o Item");
                    }

                    return _serviceResult;
                }

            }
            catch (Exception ex)
            {
                _serviceResult.Error = ex.Message;
                return _serviceResult;
            }
        }

        public async Task<ServiceResult<TalieItemDTO>> ObterItensNotaFiscal(string numeroNotaFiscal, string codigoRegistro)
        {
            var _serviceResult = new ServiceResult<TalieItemDTO>();
            string query = SqlQueries.ObterItensNotaFiscal;
            try
            {
                using (var connection = Connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CodigoRegistro", codigoRegistro, DbType.String);
                    parameters.Add("@NumeroNotaFiscal", numeroNotaFiscal, DbType.String);


                    var result = await connection.QueryFirstOrDefaultAsync<TalieItemDTO>(query, parameters);

                    if (result == null)
                    {
                        _serviceResult.Mensagens.Add("Nota Fiscal não identificada no registro.");
                    }
                    else
                    {
                        _serviceResult.Result = result;
                    }

                    return _serviceResult;
                }

            }
            catch (Exception ex)
            {
                _serviceResult.Error = ex.Message;
                return _serviceResult;
            }
        }

        #endregion
    }
}