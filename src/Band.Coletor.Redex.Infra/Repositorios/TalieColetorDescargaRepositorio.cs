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
using Band.Coletor.Redex.Infra.Repositorios.Sql;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class TalieColetorDescargaRepositorio : ITalieColetorDescargaRepositorio
    {
        //public IEnumerable<TalieDTO> GetAllDadosTalie(string talie, string registro, string tipoDescarga)
        //{
        //    if (talie == "")
        //        talie = null;

        //    if (registro == "")
        //        registro = null;

        //    if (tipoDescarga == "")
        //        tipoDescarga = null;

        //    try
        //    {
        //        using (var _db = new SqlConnection(Config.StringConexao()))
        //        {
        //            StringBuilder sb = new StringBuilder();

        //            sb.AppendLine(" SELECT ");
        //            sb.AppendLine(" a.autonum_reg, ");
        //            sb.AppendLine(" isnull(c.id_conteiner, '') as id_conteiner, ");
        //            sb.AppendLine(" b.reference, ");
        //            sb.AppendLine(" b.instrucao, ");
        //            sb.AppendLine(" d.fantasia, ");
        //            sb.AppendLine(" b.autonum_parceiro, ");
        //            sb.AppendLine(" a.AUTONUM_TALIE as Id, ");
        //            sb.AppendLine(" a.AUTONUM_PATIO, ");
        //            sb.AppendLine(" isnull(a.Placa, '') as Placa, ");
        //            sb.AppendLine(" a.Inicio, ");
        //            sb.AppendLine(" a.TERMINO, ");
        //            sb.AppendLine(" a.FLAG_DESCARGA, ");
        //            sb.AppendLine(" a.FLAG_ESTUFAGEM, ");
        //            sb.AppendLine(" a.CROSSDOCKING, ");
        //            sb.AppendLine(" a.CONFERENTE, ");
        //            sb.AppendLine(" a.EQUIPE, ");
        //            sb.AppendLine(" a.AUTONUM_BOO, ");
        //            sb.AppendLine(" a.FLAG_CARREGAMENTO, ");
        //            sb.AppendLine(" A.AUTONUM_GATE, ");
        //            sb.AppendLine(" a.flag_fechado, ");
        //            sb.AppendLine(" A.FLAG_COMPLETO, ");
        //            sb.AppendLine(" a.forma_operacao ");
        //            sb.AppendLine(" FROM ");
        //            sb.AppendLine(" REDEX..tb_talie a ");
        //            sb.AppendLine(" INNER JOIN  ");
        //            sb.AppendLine(" REDEX..tb_booking b on a.autonum_boo = b.autonum_boo ");
        //            sb.AppendLine(" left JOIN  ");
        //            sb.AppendLine(" REDEX..tb_patio c on a.autonum_patio = c.autonum_patio ");
        //            sb.AppendLine(" INNER JOIN  ");
        //            sb.AppendLine(" REDEX..tb_cad_parceiros d on b.autonum_parceiro = d.autonum ");
        //            sb.AppendLine(" WHERE  ");
        //            sb.AppendLine("  1=1 AND a.flag_fechado = 0  ");

        //            if (talie != null)
        //            {
        //                sb.AppendLine(" AND a.AUTONUM_TALIE = '" + talie + "' ");
        //            }

        //            if (registro != null)
        //            {
        //                sb.AppendLine(" AND a.AUTONUM_TALIE = '" + talie + "'  ");

        //            }
        //            if (tipoDescarga == OpcoesDescarga.DA.ToString())
        //            {
        //                sb.AppendLine(" AND flag_descarga= 1 ");
        //            }
        //            else
        //            {
        //                sb.AppendLine(" AND flag_descarga= 0 ");
        //            }
        //            if (tipoDescarga == OpcoesDescarga.CD.ToString())
        //            {
        //                sb.AppendLine(" AND crossdocking = 1 ");
        //            }
        //            else
        //            {
        //                sb.AppendLine(" AND crossdocking = 0 ");
        //            }


        //            var query = _db.Query<TalieDTO>(sb.ToString()).AsEnumerable();

        //            return query;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public IEnumerable<TalieDTO> GetAllDadosTalie(string talie, string registro, string tipoDescarga)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var parameters = new DynamicParameters();

                    // Adiciona todos os parâmetros
                    parameters.Add("talie", string.IsNullOrWhiteSpace(talie) ? (object)null : talie);
                    parameters.Add("registro", string.IsNullOrWhiteSpace(registro) ? (object)null : registro);
                    parameters.Add("tipoDescarga", tipoDescarga);

                    // Executa a consulta sem montar outra cláusula WHERE
                    var result = _db.Query<TalieDTO>(SqlQueries.GetTalieData, parameters).AsEnumerable();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Erro ao buscar dados de talie.", ex);
            }
        }

        public (IEnumerable<TalieDTO>, int TotalRecords) GetAllDadosTalie(string talie, string registro, string tipoDescarga, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();

            parameters.Add("talie", string.IsNullOrWhiteSpace(talie) ? (object)null : talie);
            parameters.Add("registro", string.IsNullOrWhiteSpace(registro) ? (object)null : registro);
            parameters.Add("tipoDescarga", tipoDescarga);
            parameters.Add("offset", (pageNumber - 1) * pageSize);
            parameters.Add("limit", pageSize);

            string query = SqlQueries.GetTalieDataPaginado;

            using (var _db = new SqlConnection(Config.StringConexao()))
            {
                int totalRecords = _db.QuerySingle<int>(SqlQueries.CountQuery, parameters);

                var result = _db.Query<TalieDTO>(query, parameters).AsEnumerable();
                return (result, totalRecords);
            }
        }

        public TalieDTO GetTalieByIdConteiner(int id, string conteiner)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" A.AUTONUM_TALIE AS Id, ");
                    sb.AppendLine(" A.AUTONUM_REG AS RegistroId, ");
                    sb.AppendLine(" C.ID_CONTEINER AS ConteinerId, ");
                    sb.AppendLine(" B.REFERENCE AS Reference, ");
                    sb.AppendLine(" B.INSTRUCAO AS instrucao, ");
                    sb.AppendLine(" D.FANTASIA as fantasia, ");
                    sb.AppendLine(" B.AUTONUM_EXPORTADOR AS ExportadorId, ");
                    sb.AppendLine(" A.AUTONUM_PATIO AS PatioId, ");
                    sb.AppendLine(" A.PLACA as Placa, ");
                    sb.AppendLine(" CONVERT(VarChar(10), a.inicio, 103) + ' ' + CONVERT(VarChar(8), a.inicio, 108) as Inicio, ");
                    sb.AppendLine(" CONVERT(VarChar(10), a.termino, 103) + ' ' + CONVERT(VarChar(8), a.termino, 108) as Termino, ");
                    sb.AppendLine(" ISNULL(a.FLAG_DESCARGA, 0) as Descarga, ");
                    sb.AppendLine(" ISNULL(a.FLAG_ESTUFAGEM, 0) as Estufagem, ");
                    sb.AppendLine(" a.CROSSDOCKING as CrossDocking, ");
                    sb.AppendLine(" a.CONFERENTE as ConferenteId, ");
                    sb.AppendLine(" a.EQUIPE as EquipeId, ");
                    sb.AppendLine(" a.AUTONUM_BOO as BookingId, ");
                    sb.AppendLine(" ISNULL(a.FLAG_CARREGAMENTO, 0) as Carregamento, ");
                    sb.AppendLine(" A.AUTONUM_GATE as GateId, ");
                    sb.AppendLine(" ISNULL(A.FLAG_FECHADO, 0) as Fechado, ");
                    sb.AppendLine(" B.AUTONUM_PATIOS as Patio, ");
                    sb.AppendLine(" A.FORMA_OPERACAO as OperacaoId, ");
                    sb.AppendLine(" Conf.FANTASIA AS Conferente, ");
                    sb.AppendLine(" Eq.FANTASIA as Equipe, ");
                    sb.AppendLine(" case when A.FORMA_OPERACAO = 'A' then 'Automático' else 'Manual' end Operacao ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE A ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING B ON A.AUTONUM_BOO = B.AUTONUM_BOO ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO C ON A.AUTONUM_PATIO = C.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_PARCEIROS D ON B.AUTONUM_PARCEIRO = D.AUTONUM ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..tb_cad_parceiros Conf on a.CONFERENTE = CONF.AUTONUM ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..tb_cad_parceiros Eq on a.EQUIPE = Eq.AUTONUM ");
                    sb.AppendLine(" inner JOIN ");
                    sb.AppendLine(" REDEX..tb_patio cc on a.autonum_patio = cc.AUTONUM_patio ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine("( A.AUTONUM_REG = " + id + "or A.AUTONUM_TALIE = " + id + " )");
                    sb.AppendLine(" and cc.id_conteiner='" + conteiner + "' ");
                    sb.AppendLine(" ORDER BY  ");
                    sb.AppendLine(" A.INICIO DESC, D.FANTASIA, C.ID_CONTEINER, B.REFERENCE, B.INSTRUCAO ");

                    var query = _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO GetTalieById(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" A.AUTONUM_TALIE AS Id, ");
                    sb.AppendLine(" A.AUTONUM_REG AS RegistroId, ");
                    sb.AppendLine(" C.ID_CONTEINER AS ConteinerId, ");
                    sb.AppendLine(" B.REFERENCE AS Reference, ");
                    sb.AppendLine(" B.INSTRUCAO AS instrucao, ");
                    sb.AppendLine(" D.FANTASIA as fantasia, ");
                    sb.AppendLine(" B.AUTONUM_EXPORTADOR AS ExportadorId, ");
                    sb.AppendLine(" A.AUTONUM_PATIO AS PatioId, ");
                    sb.AppendLine(" A.PLACA as Placa, ");
                    sb.AppendLine(" CONVERT(VarChar(10), a.inicio, 103) + ' ' + CONVERT(VarChar(8), a.inicio, 108) as Inicio, ");
                    sb.AppendLine(" CONVERT(VarChar(10), a.termino, 103) + ' ' + CONVERT(VarChar(8), a.termino, 108) as Termino, ");
                    sb.AppendLine(" ISNULL(a.FLAG_DESCARGA, 0) as Descarga, ");
                    sb.AppendLine(" ISNULL(a.FLAG_ESTUFAGEM, 0) as Estufagem, ");
                    sb.AppendLine(" a.CROSSDOCKING as CrossDocking, ");
                    sb.AppendLine(" a.CONFERENTE as ConferenteId, ");
                    sb.AppendLine(" a.EQUIPE as EquipeId, ");
                    sb.AppendLine(" a.AUTONUM_BOO as BookingId, ");
                    sb.AppendLine(" ISNULL(a.FLAG_CARREGAMENTO, 0) as Carregamento, ");
                    sb.AppendLine(" A.AUTONUM_GATE as GateId, ");
                    sb.AppendLine(" ISNULL(A.FLAG_FECHADO, 0) as Fechado, ");
                    sb.AppendLine(" B.AUTONUM_PATIOS as Patio, ");
                    sb.AppendLine(" A.FORMA_OPERACAO as OperacaoId, ");
                    sb.AppendLine(" Conf.FANTASIA AS Conferente, ");
                    sb.AppendLine(" Eq.FANTASIA as Equipe, ");
                    sb.AppendLine(" case when A.FORMA_OPERACAO = 'A' then 'Automático' else 'Manual' end Operacao ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE A ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING B ON A.AUTONUM_BOO = B.AUTONUM_BOO ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_PATIO C ON A.AUTONUM_PATIO = C.AUTONUM_PATIO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_PARCEIROS D ON B.AUTONUM_PARCEIRO = D.AUTONUM ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..tb_cad_parceiros Conf on a.CONFERENTE = CONF.AUTONUM ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..tb_cad_parceiros Eq on a.EQUIPE = Eq.AUTONUM ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" A.AUTONUM_REG = " + id);
                    //  sb.AppendLine(" A.autonum_talie = " + id);
                    sb.AppendLine(" ORDER BY  ");
                    sb.AppendLine(" A.INICIO DESC, D.FANTASIA, C.ID_CONTEINER, B.REFERENCE, B.INSTRUCAO ");

                    var query = _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO GetTalieByRegistro(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" A.AUTONUM AS GateId, ");
                    sb.AppendLine(" B.PLACA, ");
                    sb.AppendLine(" E.AUTONUM_EXPORTADOR as ExportadorId, ");
                    sb.AppendLine(" CP.RAZAO AS Exportador, ");
                    sb.AppendLine(" CP.RAZAO as fantasia, ");
                    sb.AppendLine(" E.REFERENCE AS Reference, ");
                    sb.AppendLine(" E.AUTONUM_BOO as BookingId, ");
                    sb.AppendLine(" B.AUTONUM_REG as RegistroId, ");
                    sb.AppendLine(" B.PATIO, ");
                    sb.AppendLine(" getdate() as Inicio, ");
                    sb.AppendLine(" B.DT_LIB_ENT_CAM ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_GATE_NEW A ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_REGISTRO B ON A.AUTONUM = B.AUTONUM_GATE ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING E ON B.AUTONUM_BOO = E.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_PARCEIROS CP ON E.AUTONUM_EXPORTADOR = CP.AUTONUM ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" B.AUTONUM_REG = " + id);
                    sb.AppendLine(" ORDER BY ");
                    sb.AppendLine(" A.AUTONUM DESC ");

                    var query = _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<TalieDTO> GetAllDadosTalieItens(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" a.AUTONUM_PRO, ");
                    sb.AppendLine(" a.obs, ");
                    sb.AppendLine(" rcs.QUANTIDADE as qtde, ");
                    sb.AppendLine(" a.qtde_estufagem, ");
                    sb.AppendLine(" a.autonum_ti as AUTONUM_TI, ");
                    sb.AppendLine(" c.num_nf, ");
                    sb.AppendLine(" c.serie_nf, ");
                    sb.AppendLine(" d.Lote, ");
                    sb.AppendLine(" a.QTDE_DESCARGA, ");
                    sb.AppendLine(" ISNULL( rcs.PESO_BRUTO,C.PESO_BRUTO) as PesoBruto,  ");
                    sb.AppendLine(" a.tipo_descarga, ");
                    sb.AppendLine(" a.diferenca, ");
                    sb.AppendLine(" isnull(f.descricao_emb, '-') as descricao_emb, ");
                    sb.AppendLine(" e.desc_produto, ");
                    sb.AppendLine(" d.instrucao, ");
                    //sb.AppendLine(" b.item as ItemTalie, ");
                    sb.AppendLine(" a.comprimento as Comprimento, ");
                    sb.AppendLine(" a.largura as Largura, ");
                    sb.AppendLine(" a.altura as Altura, ");
                    sb.AppendLine(" a.peso as Peso, ");
                    sb.AppendLine(" a.armazem as ARMAZEM, ");
                    sb.AppendLine(" isnull(a.YARD, '-') as YARD, ");
                    sb.AppendLine(" g.autonum_talie as AUTONUM_TALIE, ");
                    //sb.AppendLine(" b.autonum_nfi as AUTONUM_NFI, ");
                    //sb.AppendLine(" b.autonum_nf as AUTONUM_NF, ");
                    sb.AppendLine(" e.autonum_pro AS AUTONUM_PRODUTO, ");
                    sb.AppendLine(" f.autonum_emb AS AUTONUM_EMBALAGEM, ");
                    sb.AppendLine(" a.MARCACAO + ' - ' + a.MARCA  as marcacao, ");
                    sb.AppendLine(" CONVERT(VARCHAR,a.OBSERVACAO) + ' - ' + a.obs as observacao, ");
                    sb.AppendLine(" isnull(a.flag_numerada,0) as flag_numerada ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..tb_talie g ");
                    sb.AppendLine(" INNER JOIN  REDEX..tb_talie_item a on a.autonum_talie = g.autonum_talie ");
                    sb.AppendLine(" INNER JOIN  REDEX..TB_REGISTRO_CS rcs on a.AUTONUM_REGCS = rcs.AUTONUM_REGCS  ");
                    sb.AppendLine(" LEFT JOIN  REDEX..tb_notas_fiscais c on a.AUTONUM_NF = c.AUTONUM_NF ");
                    //sb.AppendLine(" LEFT JOIN  REDEX..tb_notas_itens b on C.autonum_nf = b.autonum_nf  ");
                    sb.AppendLine(" LEFT JOIN  REDEX..tb_booking_carga d on rcs.autonum_bcg = d.autonum_bcg ");
                    sb.AppendLine(" LEFT JOIN  REDEX..tb_cad_produtos e on d.autonum_pro = e.autonum_pro ");
                    sb.AppendLine(" LEFT JOIN  REDEX..tb_cad_embalagens f on a.autonum_emb = f.autonum_emb ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" a.autonum_talie=  " + id);
                    sb.AppendLine(" ORDER BY ");
                    sb.AppendLine(" d.instrucao, ");
                    sb.AppendLine(" c.num_nf ");
                    //sb.AppendLine(" ,b.item ");

                    var query = _db.Query<TalieDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int CadastrarTalie(Talie obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    if (obj.AUTONUM_PATIO == 0)
                    {
                        sb.AppendLine(" INSERT INTO ");
                        sb.AppendLine(" REDEX..TB_TALIE ");
                        sb.AppendLine(" ( ");
                        sb.AppendLine(" INICIO, ");
                        sb.AppendLine(" CROSSDOCKING, ");
                        sb.AppendLine(" CONFERENTE, ");
                        sb.AppendLine(" EQUIPE, ");
                        sb.AppendLine(" AUTONUM_BOO, ");
                        sb.AppendLine(" FORMA_OPERACAO, ");
                        sb.AppendLine(" PLACA, ");
                        sb.AppendLine(" AUTONUM_GATE, ");
                        sb.AppendLine(" AUTONUM_REG, ");
                        sb.AppendLine(" FLAG_DESCARGA, ");
                        sb.AppendLine(" FLAG_CARREGAMENTO, ");
                        sb.AppendLine(" FLAG_ESTUFAGEM,  ");
                        sb.AppendLine(" OBS ");
                        sb.AppendLine(" ) VALUES( ");
                        //sb.AppendLine(" @Inicio, ");
                        sb.AppendLine(" GETDATE(), ");
                        sb.AppendLine(" @CrossDocking, ");
                        sb.AppendLine(" @ConferenteId, ");
                        sb.AppendLine(" @EquipeId, ");
                        sb.AppendLine(" @BookingId, ");
                        sb.AppendLine(" @OperacaoId, ");
                        sb.AppendLine(" @Placa, ");
                        sb.AppendLine(" @AUTONUM_GATE, ");
                        sb.AppendLine(" @RegistroId, ");
                        sb.AppendLine(" 1, ");
                        sb.AppendLine(" 0,  ");
                        sb.AppendLine(" @FLAG_ESTUFAGEM,  ");
                        sb.AppendLine(" @Observacoes ");
                        sb.AppendLine(" ); SELECT CAST(SCOPE_IDENTITY() AS INT)");


                        int ret = _db.Query<int>(sb.ToString(), new
                        {
                            //Inicio = Convert.ToDateTime(obj.DtInicio),
                            CrossDocking = obj.CrossDocking,
                            ConferenteId = obj.ConferenteId,
                            EquipeId = obj.EquipeId,
                            BookingId = obj.BookingId,
                            OperacaoId = obj.OperacaoId,
                            Placa = obj.Placa,
                            AUTONUM_GATE = obj.GateId,
                            RegistroId = obj.AUTONUM_REG,
                            Observacoes = obj.Observacoes,
                            FLAG_ESTUFAGEM = obj.FLAG_ESTUFAGEM,


                        }).FirstOrDefault();




                        return ret;
                    }
                    else
                    {
                        sb.AppendLine(" INSERT INTO ");
                        sb.AppendLine(" REDEX..TB_TALIE ");
                        sb.AppendLine(" ( ");
                        sb.AppendLine(" AUTONUM_PATIO, ");
                        sb.AppendLine(" INICIO, ");
                        sb.AppendLine(" CROSSDOCKING, ");
                        sb.AppendLine(" CONFERENTE, ");
                        sb.AppendLine(" EQUIPE, ");
                        sb.AppendLine(" AUTONUM_BOO, ");
                        sb.AppendLine(" FORMA_OPERACAO, ");
                        sb.AppendLine(" PLACA, ");
                        sb.AppendLine(" AUTONUM_GATE, ");
                        sb.AppendLine(" AUTONUM_REG, ");
                        sb.AppendLine(" FLAG_DESCARGA, ");
                        sb.AppendLine(" FLAG_CARREGAMENTO, ");
                        sb.AppendLine(" FLAG_ESTUFAGEM,  ");
                        sb.AppendLine(" OBS ");
                        sb.AppendLine(" ) VALUES( ");
                        sb.AppendLine(" @AUTONUM_PATIO, ");
                        ///sb.AppendLine(" @Inicio, ");
                        sb.AppendLine(" GETDATE(), ");
                        sb.AppendLine(" @CrossDocking, ");
                        sb.AppendLine(" @ConferenteId, ");
                        sb.AppendLine(" @EquipeId, ");
                        sb.AppendLine(" @BookingId, ");
                        sb.AppendLine(" @OperacaoId, ");
                        sb.AppendLine(" @Placa, ");
                        sb.AppendLine(" @AUTONUM_GATE, ");
                        sb.AppendLine(" @RegistroId, ");
                        sb.AppendLine(" 1, ");
                        sb.AppendLine(" 0,  ");
                        sb.AppendLine(" @FLAG_ESTUFAGEM,  ");
                        sb.AppendLine(" @Observacoes ");
                        sb.AppendLine(" );SELECT CAST(SCOPE_IDENTITY() AS INT)");


                        var ret = _db.Query<int>(sb.ToString(), new
                        {
                            AUTONUM_PATIO = obj.AUTONUM_PATIO,
                            //Inicio = Convert.ToDateTime(obj.DtInicio),
                            CrossDocking = obj.CrossDocking,
                            ConferenteId = obj.ConferenteId,
                            EquipeId = obj.EquipeId,
                            BookingId = obj.BookingId,
                            OperacaoId = obj.OperacaoId,
                            Placa = obj.Placa,
                            AUTONUM_GATE = obj.GateId,
                            RegistroId = obj.AUTONUM_REG,
                            Observacoes = obj.Observacoes,
                            FLAG_ESTUFAGEM = obj.FLAG_ESTUFAGEM,

                        }).FirstOrDefault();



                        return ret;
                    }

                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Talie AtualizarTalie(Talie obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    if (obj.AUTONUM_PATIO == 0)
                    {
                        sb.AppendLine(" UPDATE REDEX..TB_TALIE ");
                        sb.AppendLine("  SET ");
                        //sb.AppendLine(" INICIO = GETDATE(), ");
                        sb.AppendLine(" CROSSDOCKING = @CrossDocking, ");
                        sb.AppendLine(" CONFERENTE = @ConferenteId, ");
                        sb.AppendLine(" EQUIPE = @EquipeId, ");
                        sb.AppendLine(" FORMA_OPERACAO = @OperacaoId, ");
                        sb.AppendLine(" OBS = @Observacoes ");
                        sb.AppendLine(" WHERE AUTONUM_TALIE = @TalieId  ");

                        _db.Query<TalieDTO>(sb.ToString(), new
                        {
                            //Inicio = Convert.ToDateTime(obj.DtInicio),
                            CrossDocking = obj.CrossDocking,
                            ConferenteId = obj.ConferenteId,
                            EquipeId = obj.EquipeId,
                            BookingId = obj.BookingId,
                            OperacaoId = obj.OperacaoId,
                            Observacoes = obj.Observacoes,
                            TalieId = obj.AUTONUM_TALIE,

                        }).FirstOrDefault();
                    }
                    else
                    {
                        sb.AppendLine(" UPDATE REDEX..TB_TALIE ");
                        sb.AppendLine("  SET ");
                        sb.AppendLine(" AUTONUM_PATIO = @AUTONUM_PATIO, ");
                        //sb.AppendLine(" INICIO = @Inicio, ");
                        sb.AppendLine(" CROSSDOCKING = @CrossDocking, ");
                        sb.AppendLine(" CONFERENTE = @ConferenteId, ");
                        sb.AppendLine(" EQUIPE = @EquipeId, ");
                        sb.AppendLine(" FORMA_OPERACAO = @OperacaoId, ");
                        sb.AppendLine(" OBS = @Observacoes ");
                        sb.AppendLine(" WHERE AUTONUM_TALIE = @TalieId  ");

                        _db.Query<TalieDTO>(sb.ToString(), new
                        {
                            AUTONUM_PATIO = obj.AUTONUM_PATIO,
                            //Inicio = Convert.ToDateTime(obj.DtInicio),
                            CrossDocking = obj.CrossDocking,
                            ConferenteId = obj.ConferenteId,
                            EquipeId = obj.EquipeId,
                            BookingId = obj.BookingId,
                            OperacaoId = obj.OperacaoId,
                            Observacoes = obj.Observacoes,
                            TalieId = obj.AUTONUM_TALIE,

                        }).FirstOrDefault();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<ArmazensDTO> GetDadosNF(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" 'NF: ' + NF.NUM_NF + ' Qtde: ' + cast(NF.VOLUMES as varchar) DISPLAY, ");
                    sb.AppendLine(" NF.AUTONUM_NF as AUTONUM, ");
                    sb.AppendLine(" NF.autonum_reg ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..tb_NOTAS_FISCAIS NF ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" NF.autonum_reg = " + id);
                    sb.AppendLine(" ORDER BY ");
                    sb.AppendLine(" DISPLAY ");

                    var query = _Db.Query<ArmazensDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO getRegistroBusca(string id)
        {

            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select isnull(max(p.autonum_patio),0) as REGISTRO_BUSCA from redex..tb_patio ");
                    sb.AppendLine(" where isnull(p.flag_historico,0)=0 and p.id_conteiner='" + id + "' ");

                    var registroBusca = _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return registroBusca;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        #region Finalizar Talie 
        public int GetCountTalie(int id)
        {
            int count = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select count(1) from REDEX..tb_talie_item where autonum_talie = " + id);

                    count = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public Talie GetUpdateEtiqueta(int talie)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX.dbo.tb_TALIE SET ALERTA_ETIQUETA=1 WHERE AUTONUM_TALIE = " + talie);

                    _db.Query<Talie>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Talie GetUpdateEtiqueta2(int talie)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX.dbo.tb_TALIE SET ALERTA_ETIQUETA=2 WHERE AUTONUM_TALIE = " + talie);

                    _db.Query<Talie>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetCountTalieNF(int id)
        {
            int count = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select count(1) from REDEX..tb_talie_item where autonum_talie = " + id);
                    sb.AppendLine(" and isnull(autonum_nf,0)=0 ");

                    count = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public IEnumerable<TalieDTO> GetDadosFinalizarTalie(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine(" D.AUTONUM_BCG As BookingCargaId, ");
                    //sb.AppendLine("  DECODE(B.AUTONUM_EMB, NULL, D.AUTONUM_EMB, B.AUTONUM_EMB) As EmbalagemId, ");
                    sb.AppendLine("  B.AUTONUM_EMB As EmbalagemId, ");
                    sb.AppendLine("  D.AUTONUM_EMB As emb_reserva, ");
                    sb.AppendLine("  D.AUTONUM_PRO As ProdutoId, ");
                    sb.AppendLine("  B.MARCA, ");
                    sb.AppendLine("  B.COMPRIMENTO, ");
                    sb.AppendLine("  B.LARGURA, ");
                    sb.AppendLine("  B.ALTURA, ");
                    sb.AppendLine("  B.PESO as Peso, ");
                    sb.AppendLine("  E.AUTONUM_REGCS As RegistroCsId, ");
                    sb.AppendLine("  B.AUTONUM_NF As NotaFiscalId, ");
                    sb.AppendLine("  B.AUTONUM_TI As TalieItemId, ");
                    sb.AppendLine("  A.AUTONUM_GATE as autonum_gate, ");
                    sb.AppendLine("  B.QTDE_ESTUFAGEM As QuantidadeEstufagem, ");
                    sb.AppendLine("  B.qtde_descarga as QuantidadeDescarga,  ");
                    sb.AppendLine("  B.YARD, ");
                    sb.AppendLine("  B.ARMAZEM, ");
                    sb.AppendLine("  BOO.AUTONUM_PATIOS As PatioId, ");
                    sb.AppendLine("  BOO.AUTONUM_BOO, ");
                    sb.AppendLine("  a.forma_operacao as FORMA_OPERACAO, ");
                    sb.AppendLine("  B.IMO, ");
                    sb.AppendLine("  B.UNO, ");
                    sb.AppendLine("  B.IMO2, ");
                    sb.AppendLine("  B.UNO2, ");
                    sb.AppendLine("  B.IMO3, ");
                    sb.AppendLine("  B.UNO3, ");
                    sb.AppendLine("  B.IMO4, ");
                    sb.AppendLine("  B.UNO4, ");
                    sb.AppendLine("  convert(varchar,B.observacao) + ' - ' + b.obs as observacao, ");
                    sb.AppendLine("  B.marcacao + ' - ' + b.MARCA AS marcacao,  ");
                    sb.AppendLine("  A.INICIO as Inicio, ");
                    sb.AppendLine("  ETQ.CODPRODUTO As CodigoProduto, ");
                    sb.AppendLine("  boo.AUTONUM_PATIOS, ");
                    sb.AppendLine("  boo.fcl_lcl, ");
                    sb.AppendLine("  b.CODIGO_CARGA");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE A ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM B ON A.AUTONUM_TALIE = B.AUTONUM_TALIE ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_REGISTRO_CS E ON E.AUTONUM_REGCS = B.AUTONUM_REGCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA D ON D.AUTONUM_BCG = E.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON D.AUTONUM_BOO = boo.AUTONUM_BOO ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM_RCS, ");
                    sb.AppendLine(" SUBSTRING(CODPRODUTO,1,8) CODPRODUTO ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..ETIQUETAS ");
                    sb.AppendLine(" GROUP BY ");
                    sb.AppendLine(" AUTONUM_RCS, ");
                    sb.AppendLine(" SUBSTRING(CODPRODUTO, 1, 8) ");
                    sb.AppendLine(" ) ETQ ON E.AUTONUM_REGCS = ETQ.AUTONUM_RCS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" A.AUTONUM_TALIE = " + id);

                    var query = _db.Query<TalieDTO>(sb.ToString()).AsEnumerable();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateTalieAlertaEtiqueta(int alertaId, int talieId)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX.dbo.tb_TALIE SET ALERTA_ETIQUETA=" + alertaId + " WHERE AUTONUM_TALIE " + talieId);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int getAutonumGate(int talie)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();


                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine(" a.autonum_gate");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE A ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM B ON A.AUTONUM_TALIE = B.AUTONUM_TALIE ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_REGISTRO_CS E ON E.AUTONUM_REGCS = B.AUTONUM_REGCS ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING_CARGA D ON D.AUTONUM_BCG = E.AUTONUM_BCG ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON D.AUTONUM_BOO = E.AUTONUM_BOO ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM_RCS, ");
                    sb.AppendLine(" SUBSTRING(CODPRODUTO,1,8) CODPRODUTO ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..ETIQUETAS ");
                    sb.AppendLine(" GROUP BY ");
                    sb.AppendLine(" AUTONUM_RCS, ");
                    sb.AppendLine(" SUBSTRING(CODPRODUTO, 1, 8) ");
                    sb.AppendLine(" ) ETQ ON E.AUTONUM_REGCS = ETQ.AUTONUM_RCS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" A.AUTONUM_TALIE = " + talie);

                    int gate = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return gate;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public string getBrutoByTbNewGate(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT BRUTO FROM REDEX..TB_GATE_NEW WHERE AUTONUM  =  " + id);

                    string bruto = _db.Query<string>(sb.ToString()).FirstOrDefault();

                    return bruto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public double getBrutoByTalieId(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select sum(isnull(nf.peso_bruto,0)) from ");
                    sb.AppendLine(" (select distinct autonum_nf from REDEX..tb_talie_item where isnull(autonum_nf, 0) <> 0 and autonum_talie = " + id + ") ti                    ");
                    sb.AppendLine(" inner join REDEX..tb_notas_fiscais nf on ti.autonum_nf = nf.autonum_nf");

                    double bruto = _db.Query<double>(sb.ToString()).FirstOrDefault();

                    return bruto;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public TalieDTO GetDadosItemTalieId(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" TI.AUTONUM_TI As AUTONUM_TI, ");
                    sb.AppendLine(" TI.AUTONUM_TALIE As TalieId, ");
                    sb.AppendLine(" TI.AUTONUM_NF As NotaFiscalId, ");
                    sb.AppendLine(" (SELECT ISNULL(SUM(QTDE_DESCARGA), 0) FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_REGCS = TI.AUTONUM_REGCS AND AUTONUM_TI = " + id + ") As QuantidadeDescarga, ");
                    sb.AppendLine(" (SELECT ISNULL(SUM(QUANTIDADE), 0) FROM REDEX..TB_REGISTRO_CS WHERE AUTONUM_REGCS = TI.AUTONUM_REGCS) As Quantidade, ");
                    sb.AppendLine(" TI.REMONTE, ");
                    sb.AppendLine(" TI.FUMIGACAO, ");
                    sb.AppendLine(" TI.IMO As IMO1, ");
                    sb.AppendLine(" TI.UNO As UNO1, ");
                    sb.AppendLine(" TI.IMO2, ");
                    sb.AppendLine(" TI.UNO2, ");
                    sb.AppendLine(" TI.IMO3, ");
                    sb.AppendLine(" TI.UNO3, ");
                    sb.AppendLine(" TI.IMO4, ");
                    sb.AppendLine(" TI.UNO4, ");
                    sb.AppendLine(" TI.YARD, ");
                    sb.AppendLine(" TI.FLAG_MADEIRA As Madeira, ");
                    sb.AppendLine(" TI.FLAG_FRAGIL As Fragil, ");
                    sb.AppendLine(" TI.AUTONUM_REGCS As RegistroCsId, ");
                    sb.AppendLine(" TI.NF As NotaFiscal, ");
                    sb.AppendLine(" TI.COMPRIMENTO as COMPRIMENTO, ");
                    sb.AppendLine(" TI.LARGURA as LARGURA, ");
                    sb.AppendLine(" TI.ALTURA as ALTURA, ");
                    sb.AppendLine(" TI.PESO as Peso, ");
                    sb.AppendLine(" TI.CARIMBO as Carimbo, ");
                    sb.AppendLine(" TI.FLAG_AVARIA as Avaria, ");
                    sb.AppendLine(" TI.FLAG_REMONTE, ");
                    sb.AppendLine(" E.SIGLA, ");
                    sb.AppendLine(" E.SIGLA AS EmbalagemSigla, ");
                    sb.AppendLine(" E.AUTONUM_EMB As EmbalagemId, ");
                    sb.AppendLine(" E.DESCRICAO_EMB + '-' + CAST(E.AUTONUM_EMB AS VARCHAR) AS EMBALAGEM, ");
                    sb.AppendLine(" TI.YARD, ");
                    sb.AppendLine(" TI.MARCACAO + ' - ' + ti.MARCA AS marcacao, ");
                    sb.AppendLine(" convert(varchar,TI.OBSERVACAO) + ' - ' + ti.obs as observacao, ");
                    sb.AppendLine(" isnull(TI.flag_numerada,0) as flag_numerada ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM TI ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON TI.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" LEFT JOIN ");
                    sb.AppendLine(" REDEX..TB_CAD_EMBALAGENS E ON TI.AUTONUM_EMB = E.AUTONUM_EMB ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" TI.AUTONUM_TI =" + id);

                    var query = _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO InsertTaliePatioCS(TalieDTO obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO ");
                    sb.AppendLine(" REDEX..TB_PATIO_CS");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" AUTONUM_PCS,");
                    sb.AppendLine(" AUTONUM_BCG,");
                    sb.AppendLine(" QTDE_ENTRADA,");
                    sb.AppendLine(" AUTONUM_EMB,");
                    sb.AppendLine(" AUTONUM_PRO,");
                    sb.AppendLine(" MARCA,");
                    sb.AppendLine(" VOLUME_DECLARADO,");
                    sb.AppendLine(" COMPRIMENTO,");
                    sb.AppendLine(" LARGURA,");
                    sb.AppendLine(" ALTURA,");
                    sb.AppendLine(" BRUTO,");
                    sb.AppendLine(" DT_PRIM_ENTRADA,");
                    sb.AppendLine(" FLAG_HISTORICO,");
                    sb.AppendLine(" AUTONUM_REGCS,");
                    sb.AppendLine(" AUTONUM_NF,");
                    sb.AppendLine(" TALIE_DESCARGA,");
                    sb.AppendLine(" QTDE_ESTUFAGEM,");
                    sb.AppendLine(" YARD,");
                    sb.AppendLine(" ARMAZEM,");
                    sb.AppendLine(" AUTONUM_PATIOS,");
                    sb.AppendLine(" PATIO,");
                    sb.AppendLine(" IMO,");
                    sb.AppendLine(" IMO2,");
                    sb.AppendLine(" IMO3,");
                    sb.AppendLine(" IMO4,");
                    sb.AppendLine(" UNO,");
                    sb.AppendLine(" UNO2,");
                    sb.AppendLine(" UNO3,");
                    sb.AppendLine(" UNO4,");
                    sb.AppendLine(" CODPRODUTO,");
                    sb.AppendLine(" CODIGO_CARGA ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" @AUTONUM_PCS,");
                    sb.AppendLine(" @AUTONUM_BCG,");
                    sb.AppendLine(" @QTDE_ENTRADA,");
                    sb.AppendLine(" @AUTONUM_EMB,");
                    sb.AppendLine(" @AUTONUM_PRO,");
                    sb.AppendLine(" @MARCA,");
                    sb.AppendLine(" @VOLUME_DECLARADO,");
                    sb.AppendLine(" @COMPRIMENTO,");
                    sb.AppendLine(" @LARGURA,");
                    sb.AppendLine(" @ALTURA,");
                    sb.AppendLine(" replace('" + obj.BRUTONF + "', ',', '.'),");
                    sb.AppendLine(" @DT_PRIM_ENTRADA,");
                    sb.AppendLine(" @FLAG_HISTORICO,");
                    sb.AppendLine(" @AUTONUM_REGCS,");
                    sb.AppendLine(" @AUTONUM_NF,");
                    sb.AppendLine(" @TALIE_DESCARGA,");
                    sb.AppendLine(" @QTDE_ESTUFAGEM,");
                    sb.AppendLine(" @YARD,");
                    sb.AppendLine(" @ARMAZEM,");
                    sb.AppendLine(" @AUTONUM_PATIOS,");
                    sb.AppendLine(" @PATIO,");
                    sb.AppendLine(" @IMO,");
                    sb.AppendLine(" @IMO2,");
                    sb.AppendLine(" @IMO3,");
                    sb.AppendLine(" @IMO4,");
                    sb.AppendLine(" @UNO,");
                    sb.AppendLine(" @UNO2,");
                    sb.AppendLine(" @UNO3,");
                    sb.AppendLine(" @UNO4,");
                    sb.AppendLine(" @CODPRODUTO, ");
                    sb.AppendLine(" @CODIGO_CARGA ");
                    sb.AppendLine(" ) ");

                    _db.Query<TalieDTO>(sb.ToString(), new
                    {
                        AUTONUM_PCS = obj.AUTONUM_PCS,
                        AUTONUM_BCG = obj.AUTONUM_BCG,
                        QTDE_ENTRADA = obj.QTDE_ENTRADA,
                        AUTONUM_EMB = obj.AUTONUM_EMB,
                        AUTONUM_PRO = obj.AUTONUM_PRO,
                        MARCA = obj.MARCA,
                        VOLUME_DECLARADO = obj.VOLUME_DECLARADO,
                        COMPRIMENTO = obj.COMPRIMENTO,
                        LARGURA = obj.LARGURA,
                        ALTURA = obj.ALTURA,
                        //BRUTO = obj.BRUTO,
                        DT_PRIM_ENTRADA = obj.DT_PRIM_ENTRADA,
                        FLAG_HISTORICO = obj.FLAG_HISTORICO,
                        AUTONUM_REGCS = obj.AUTONUM_REGCS,
                        AUTONUM_NF = obj.AUTONUM_NF,
                        TALIE_DESCARGA = obj.AUTONUM_TI,
                        QTDE_ESTUFAGEM = obj.QTDE_ESTUFAGEM,
                        YARD = obj.YARD,
                        ARMAZEM = obj.ARMAZEM,
                        AUTONUM_PATIOS = obj.AUTONUM_PATIOS,
                        PATIO = obj.PATIO,
                        IMO = obj.IMO,
                        IMO2 = obj.IMO2,
                        IMO3 = obj.IMO3,
                        IMO4 = obj.IMO4,
                        UNO = obj.UNO,
                        UNO2 = obj.UNO2,
                        UNO3 = obj.UNO3,
                        UNO4 = obj.UNO4,
                        CODPRODUTO = obj.CODPRODUTO,
                        CODIGO_CARGA = obj.CODIGO_CARGA,

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public TalieDTO UpdateTalieFlagFechado(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("UPDATE REDEX..TB_TALIE SET FLAG_FECHADO = 1,TERMINO = GETDATE() WHERE AUTONUM_TALIE = " + id);

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateTalieFlagFinalizado(int idBoo)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_BOOKING SET FLAG_FINALIZADO = 1 WHERE AUTONUM_BOO =  " + idBoo);

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateTalieStatusReserva(int idBoo)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_BOOKING SET STATUS_RESERVA = 2 WHERE AUTONUM_BOO = " + idBoo);

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO InsertCargaSolta(int autonum, int yard, int armazem)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO ");
                    sb.AppendLine(" SGIPA..TB_CARGA_SOLTA_YARD ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" Autonum_CS, ");
                    sb.AppendLine(" Armazem, ");
                    sb.AppendLine(" yard, ");
                    sb.AppendLine(" Origem, ");
                    sb.AppendLine(" DATA, ");
                    sb.AppendLine(" AUDIT_94, ");
                    sb.AppendLine(" FL_FRENTE, ");
                    sb.AppendLine(" FL_FUNDO, ");
                    sb.AppendLine(" FL_LE, ");
                    sb.AppendLine(" FL_LD ");
                    sb.AppendLine(" ) VALUES ( ");
                    sb.AppendLine(" " + autonum + ", ");
                    sb.AppendLine(" " + armazem + ", ");
                    sb.AppendLine(" '" + yard + "', ");
                    sb.AppendLine(" 'R', ");
                    sb.AppendLine(" GETDATE(), ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0 ");
                    sb.AppendLine(" ) ");

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TalieDTO UpdateTbBookingCarga(int id, string IMO)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_BOOKING_CARGA SET IMO = '" + IMO + "' WHERE AUTONUM_BCG  =  " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO insertDataSEQPATIOCS()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX.DBO.SEQ_TB_PATIO_CS(data) values(GetDate()) ");

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO insertDataSEQAMRGate()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" insert into redex.dbo.seq_tb_amr_gate (data) values (GetDate()) ");

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }   //
        public TalieDTO UpdateArmGate(TalieDTO obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb0 = new StringBuilder();
                    StringBuilder sb = new StringBuilder();

                    sb0.AppendLine(" SELECT ident_current('redex.dbo.seq_tb_amr_gate') as AMRIDGate");

                    int amr = _db.Query<int>(sb0.ToString()).FirstOrDefault();


                    sb.AppendLine(" INSERT INTO ");
                    sb.AppendLine(" REDEX..TB_AMR_GATE ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" AUTONUM, ");
                    sb.AppendLine(" GATE, ");
                    sb.AppendLine(" CNTR_RDX, ");
                    sb.AppendLine(" CS_RDX, ");
                    sb.AppendLine(" PESO_ENTRADA, ");
                    sb.AppendLine(" PESO_SAIDA, ");
                    sb.AppendLine(" DATA, ");
                    sb.AppendLine(" ID_BOOKING, ");
                    sb.AppendLine(" ID_OC, ");
                    sb.AppendLine(" FUNCAO_GATE, ");
                    sb.AppendLine(" FLAG_HISTORICO ");
                    sb.AppendLine(" ) VALUES ( ");
                    sb.AppendLine(" " + amr + ", ");
                    sb.AppendLine(" @GateId, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" @PatioCsId, ");
                    sb.AppendLine(" @PesoBruto, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" @Inicio, ");
                    sb.AppendLine(" @BookingId, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 203, ");
                    sb.AppendLine(" 0 ");
                    sb.AppendLine(" ) ");

                    _db.Query<TalieDTO>(sb.ToString(), new
                    {

                        GateId = obj.autonum_gate,
                        PatioCsId = obj.AUTONUM_PCS,
                        PesoBruto = obj.Peso,
                        Inicio = obj.Inicio,
                        BookingId = obj.AUTONUM_BOO,

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdatePatioPaiCS(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("  UPDATE REDEX..TB_PATIO_CS SET PCS_PAI = " + id + " WHERE AUTONUM_PCS = " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countItensPatio(int id)
        {
            int count = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine("     COUNT(1) ");
                    sb.AppendLine(" FROM                ");
                    sb.AppendLine(" REDEX..tb_patio_cs pcs ");
                    sb.AppendLine(" INNER JOIN              ");
                    sb.AppendLine(" REDEX..tb_talie_item ti on pcs.talie_descarga = ti.autonum_ti ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" ti.autonum_talie = " + id);

                    count = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public TalieDTO UpdateFlagFechadoByTalieID(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_TALIE SET FLAG_FECHADO = 1, dt_fechamento=getdate(), TERMINO = GETDATE() WHERE AUTONUM_TALIE =  " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateFlagFechadoByBoo(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_BOOKING SET FLAG_FINALIZADO = 1 WHERE AUTONUM_BOO =  " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetCargaEntrada(int id)
        {
            int soma = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" sum(bcg.qtde)  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..tb_booking boo ");
                    sb.AppendLine(" INNER JOIN REDEX..tb_booking_carga bcg on boo.autonum_boo = bcg.autonum_boo ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" boo.autonum_boo = " + id);
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" bcg.flag_cs=1 ");

                    soma = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return soma;
                }
            }
            catch (Exception ex)
            {
                return soma;
            }
        }
        public int GetQuantidadeEntrada(int id)
        {
            int soma = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" sum(pcs.qtde_entrada)  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..tb_booking boo ");
                    sb.AppendLine(" INNER JOIN REDEX..tb_booking_carga bcg on boo.autonum_boo = bcg.autonum_boo ");
                    sb.AppendLine(" INNER JOIN REDEX..tb_patio_cs pcs on bcg.autonum_bcg = pcs.autonum_bcg and boo.autonum_boo = " + id);

                    soma = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return soma;
                }
            }
            catch (Exception ex)
            {
                return soma;
            }
        }
        public TalieDTO UpdateImoBookingCarga(string imo, int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_BOOKING_CARGA SET IMO = '" + imo + "' WHERE AUTONUM_BCG = " + id);

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateFlagFechadoByReserva(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_BOOKING SET STATUS_RESERVA = 2  WHERE AUTONUM_BOO =  " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countTotalItensPatio(int id)
        {
            int count = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" COUNT(1) "); ;
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..tb_patio_cs pcs  ");
                    sb.AppendLine(" INNER JOIN tb_talie_item ti on pcs.talie_descarga = ti.autonum_ti ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" ti.autonum_talie = " + id);

                    count = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public int GetContainerId(int id)
        {
            int count = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_PATIO FROM REDEX..TB_TALIE WHERE AUTONUM_TALIE=" + id);

                    count = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }

            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public IEnumerable<TalieDTO> GetDadosPatioCS(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" pcs.autonum_pcs as AUTONUM_PCS,  ");
                    sb.AppendLine(" pcs.qtde_entrada as QTDE_ENTRADA,  ");
                    sb.AppendLine(" pcs.Autonum_NF  as AUTONUM_NF,   ");
                    sb.AppendLine(" pcs.AUTONUM_EMB,  ");
                    sb.AppendLine(" pcs.bruto as BRUTO,   ");
                    sb.AppendLine(" pcs.ALTURA,   ");
                    sb.AppendLine(" pcs.COMPRIMENTO,  ");
                    sb.AppendLine(" pcs.VOLUME_DECLARADO,  ");
                    sb.AppendLine(" ti.AUTONUM_TALIE, PCS.AUTONUM_NF ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..tb_patio_cs pcs ");
                    sb.AppendLine(" inner join ");
                    sb.AppendLine(" REDEX..tb_talie_item ti on pcs.talie_descarga = ti.autonum_ti ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" ti.autonum_talie =  " + id);

                    var query = _Db.Query<TalieDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdatePatioEF(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..tb_patio SET ef='F'  WHERE autonum_patio =  " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdatePatioE(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..tb_patio SET ef='E'  WHERE autonum_patio =  " + id);

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public long GetReservaCC(int id)
        {
            long count = 0;

            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();


                    sb.AppendLine("SELECT ");
                    sb.AppendLine(" bcg.autonum_boo ");
                    sb.AppendLine("from ");
                    sb.AppendLine(" REDEX..tb_patio cc ");
                    sb.AppendLine(" inner join         ");
                    sb.AppendLine("REDEX..tb_booking_carga bcg on cc.autonum_bcg = bcg.autonum_bcg");
                    sb.AppendLine("where");
                    sb.AppendLine("cc.autonum_patio = " + id);

                    count = _Db.Query<long>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        public long GetRomaneioId(int id)
        {
            long count = 0;

            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();


                    sb.AppendLine(" select autonum_ro from REDEX..tb_romaneio where autonum_patio= " + id);

                    count = _Db.Query<long>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public TalieDTO insertDataSEQRomaneioID()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" insert into REDEX..seq_tb_romaneio (data) values (GetDate()) ");

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }   //
        public long GetCurrentIdRomaneio()
        {
            long idRomaneio = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ident_current('redex.dbo.seq_tb_romaneio') ");

                    idRomaneio = _db.Query<long>(sb.ToString()).FirstOrDefault();

                    return idRomaneio;
                }
            }
            catch (Exception ex)
            {
                return idRomaneio;
            }
        }
        public TalieDTO InsertRomaneio(int usuarioId, int conteinerId, long reservaCC, long idRomaneio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" insert into redex..seq_tb_romaneio (data) values (GetDate())  ");

                    _db.Query<int>(sb.ToString());

                    sb.Clear();

                    sb.AppendLine(" INSERT INTO redex..TB_Romaneio ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" autonum_ro, ");
                    sb.AppendLine(" data_inclusao, ");
                    sb.AppendLine(" usuario, ");
                    sb.AppendLine(" autonum_patio, ");
                    sb.AppendLine(" data_programacao, ");
                    sb.AppendLine(" obs, ");
                    sb.AppendLine(" autonum_boo, ");
                    sb.AppendLine(" VISIT_CODE, ");
                    sb.AppendLine(" DATA_AGENDAMENTO, ");
                    sb.AppendLine(" SEM_NF, ");
                    sb.AppendLine(" flag_historico, ");
                    sb.AppendLine(" crossdocking ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" " + idRomaneio + ", ");
                    sb.AppendLine(" getDate(), ");
                    sb.AppendLine(" " + usuarioId + ", ");
                    sb.AppendLine(" " + conteinerId + ", ");
                    sb.AppendLine(" getDate(), ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" " + reservaCC + ", ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" NULL, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 1, ");
                    sb.AppendLine(" 1 ");
                    sb.AppendLine(" ) ");


                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO InsertRomaneioCS(int autonum_pcs, long autonum_ro, int quantidade)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(" insert into REDEX..seq_tb_romaneio_cs (data) values (GetDate())  ");

                    _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.Append(" SELECT ident_current('REDEX..seq_tb_romaneio_cs') ");

                    int autonum_rcs = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" INSERT INTO REDEX..TB_ROMANEIO_CS ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" autonum_rcs, ");
                    sb.AppendLine(" autonum_ro, ");
                    sb.AppendLine(" autonum_pcs,  ");
                    sb.AppendLine(" qtde,  ");
                    sb.AppendLine(" volume ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" " + autonum_rcs + ", ");
                    sb.AppendLine(" " + autonum_ro + ", ");
                    sb.AppendLine(" " + autonum_pcs + ", ");
                    sb.AppendLine(" " + quantidade + ", ");
                    sb.AppendLine(" 0 ");
                    sb.AppendLine(" ) ");

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetDataInicioTalie(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT Min(Inicio) FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO=" + id + " AND FLAG_DESCARGA=1 ");

                    DateTime dtInicio = _Db.Query<DateTime>(sb.ToString()).FirstOrDefault();

                    string date = dtInicio.ToString("dd-MM-yyyy HH:mm:ss");

                    return date;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetDataTerminoTalie(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT Min(TERMINO) FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO=" + id + " AND FLAG_DESCARGA=1 ");

                    DateTime dtTermino = _Db.Query<DateTime>(sb.ToString()).FirstOrDefault();

                    string date = dtTermino.ToString("dd-MM-yyyy HH:mm:ss");

                    return date;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetEquipeTalie(int id)
        {
            int equipe = 0;

            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT EQUIPE FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO=" + id + " AND FLAG_DESCARGA=1 ");

                    equipe = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return equipe;
                }
            }
            catch (Exception ex)
            {
                return equipe;
            }
        }
        public int GetConferenteTalie(int id)
        {
            int conferente = 0;

            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT CONFERENTE FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO=" + id + " AND FLAG_DESCARGA=1 ");

                    conferente = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return conferente;
                }
            }
            catch (Exception ex)
            {
                return conferente;
            }
        }
        public string GetFormaOperacaoTalie(int id)
        {
            string forma_operacao = "";
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT FORMA_OPERACAO FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO=" + id + " AND FLAG_DESCARGA=1 ");

                    forma_operacao = _Db.Query<string>(sb.ToString()).FirstOrDefault();

                    return forma_operacao;
                }
            }
            catch (Exception ex)
            {
                return forma_operacao;
            }
        }
        public int countFlagCarregamentoTalie(int id)
        {
            int count = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT COUNT(1) FROM REDEX..tb_talie where autonum_patio= " + id + " AND flag_carregamento=1");

                    count = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;

                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        public TalieDTO InserirTalieFechamento(int conteinerId, string dtInicio, string dtFim, long reserva, string modo, int conferenteId, int equipeId, long idRomaneio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX..TB_Talie ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" autonum_patio, ");
                    sb.AppendLine(" inicio, ");
                    sb.AppendLine(" termino, ");
                    sb.AppendLine(" flag_estufagem, ");
                    sb.AppendLine(" crossdocking, ");
                    sb.AppendLine(" autonum_boo, ");
                    sb.AppendLine(" forma_operacao, ");
                    sb.AppendLine(" conferente, ");
                    sb.AppendLine(" equipe, ");
                    sb.AppendLine(" flag_descarga, ");
                    sb.AppendLine(" flag_carregamento, ");
                    sb.AppendLine(" obs, ");
                    sb.AppendLine(" autonum_ro, ");
                    sb.AppendLine(" autonum_gate, ");
                    sb.AppendLine(" flag_fechado ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES  ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" " + conteinerId + " ,  ");
                    sb.AppendLine(" Convert(datetime, '" + dtInicio + "', 103), ");
                    sb.AppendLine(" Convert(datetime, '" + dtFim + "', 103), ");
                    sb.AppendLine(" 1, ");
                    sb.AppendLine(" 1, ");
                    sb.AppendLine(" " + reserva + ", ");
                    sb.AppendLine(" '" + modo + "', ");
                    sb.AppendLine(" " + conferenteId + ", ");
                    sb.AppendLine(" " + equipeId + ", ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 1, ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" " + idRomaneio + ", ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 1 ");
                    sb.AppendLine(" ) ");

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public long GetTalieCarregamentoId()
        {
            long TalieCarregamento = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ident_current('redex.dbo.TB_TALIE') ");

                    TalieCarregamento = _Db.Query<long>(sb.ToString()).FirstOrDefault();

                    return TalieCarregamento;
                }
            }
            catch (Exception ex)
            {
                return TalieCarregamento;
            }
        }
        public TalieDTO UpdateRomaneioIdTalie(long talieId, long romaneioID)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("UPDATE REDEX..tb_romaneio set autonum_talie=" + talieId + " WHERE autonum_ro =" + romaneioID);

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateTalieByInicioTermino(int id, string inicio, string termino)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_Talie SET ");
                    sb.AppendLine(" inicio = convert(datetime, '" + inicio + "', 103), ");
                    sb.AppendLine(" termino = convert(datetime, '" + termino + "', 103), ");
                    sb.AppendLine(" where autonum_patio = " + id + " and flag_carregamento = 1 ");

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO InsertAMRNFSaida(int id, int nf, int quantidade)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX..TB_AMR_NF_SAIDA ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" AUTONUM_PATIO, AUTONUM_NFI, QTDE_ESTUFADA ");
                    sb.AppendLine(" ) VALUES ( ");

                    sb.AppendLine(" " + id + ", ");
                    sb.AppendLine(" " + nf + ", ");
                    sb.AppendLine(" " + quantidade + " ");
                    sb.AppendLine(" ) ");



                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieDTO UpdateQuantidadeEstufada(int quantidade, int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_NOTAS_ITENS SET QTDE_ESTUFADA = " + quantidade + "  WHERE AUTONUM_NFI = " + id + " ");

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public long GetSaidaCargaId()
        {
            long id = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" insert into REDEX..seq_saida_carga (data) values (GetDate()) ");

                    _Db.Query<long>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" SELECT ident_current('redex.dbo.seq_saida_carga') ");

                    id = _Db.Query<long>(sb.ToString()).FirstOrDefault();

                    return id;

                }
            }
            catch (Exception ex)
            {
                return id;
            }
        }
        public TalieDTO InsertSaidaCarga(long idCS, int autonumPCS, int quantidade, int autonumEmb, int pBruto, decimal altura, decimal comprimento, decimal largura, int volume, int conteinerId, string conteiner, string dtEstufagem, int autonumNF, int talieCarregamento, int autonum_ro)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX..TB_SAIDA_CARGA  ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" autonum_sc, ");
                    sb.AppendLine(" AUTONUM_PCS, ");
                    sb.AppendLine(" QTDE_SAIDA, ");
                    sb.AppendLine(" AUTONUM_EMB, ");
                    sb.AppendLine(" PESO_BRUTO, ");
                    sb.AppendLine(" ALTURA, ");
                    sb.AppendLine(" COMPRIMENTO, ");
                    sb.AppendLine(" LARGURA,  ");
                    sb.AppendLine(" VOLUME, ");
                    sb.AppendLine(" autonum_patio, ");
                    sb.AppendLine(" ID_CONTEINER, ");
                    sb.AppendLine(" MERCADORIA, ");
                    sb.AppendLine(" DATA_ESTUFAGEM,  ");
                    sb.AppendLine(" autonum_nfi,  ");
                    sb.AppendLine(" autonum_talie, ");
                    sb.AppendLine(" autonum_ro ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES  ");
                    sb.AppendLine(" (  ");
                    sb.AppendLine(" " + idCS + ", ");
                    sb.AppendLine(" " + autonumPCS + ", ");
                    sb.AppendLine(" " + quantidade + ", ");
                    sb.AppendLine(" " + autonumEmb + ", ");
                    sb.AppendLine(" replace('" + pBruto + "', ',', '.'), ");
                    sb.AppendLine(" replace('" + altura + "', ',', '.'), ");
                    sb.AppendLine(" replace('" + comprimento + "', ',', '.'), ");
                    sb.AppendLine(" " + largura + ",  ");
                    sb.AppendLine(" " + volume + ", ");
                    sb.AppendLine(" " + conteinerId + ", ");
                    sb.AppendLine(" '" + conteiner + "', ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" convert(datetime, '" + dtEstufagem + "', 103),  ");
                    sb.AppendLine(" " + autonumNF + ", ");
                    sb.AppendLine(" " + talieCarregamento + ", ");
                    sb.AppendLine(" " + autonum_ro + " ");
                    sb.AppendLine("  ) ");

                    _db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetSomaQuantidadeSaida(int id)
        {
            int soma = 0;
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select sum(qtde_saida) from REDEX..tb_saida_carga where autonum_pcs= " + id);

                    soma = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return soma;
                }
            }
            catch (Exception ex)
            {
                return soma;
            }
        }
        public TalieDTO UpdatePatioHistorico(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" update REDEX..tb_patio_cs set flag_historico=1 where autonum_pcs= " + id);

                    _Db.Query<TalieDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region talie item 
        public long countQuantidadeDescarga(int id)
        {
            long qtde_descarga = 0;
            try
            {

                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select QTDE_DESCARGA from REDEX..tb_talie_item where autonum_ti= " + id);


                    qtde_descarga = _db.Query<long>(sb.ToString()).FirstOrDefault();

                    return qtde_descarga;
                }
            }
            catch (Exception ex)
            {
                return qtde_descarga;
            }
        }


        public long countQuantidadeTotalDescarga(int id)
        {
            long qtde_descarga = 0;
            try
            {

                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    //sb.AppendLine(" select sum(QTDE_DESCARGA) from REDEX..tb_talie_item where autonum_ti= " + id);
                    sb.AppendLine(" select sum(QTDE_DESCARGA) from REDEX..tb_talie_item where autonum_nf = " + id);

                    qtde_descarga = _db.Query<long>(sb.ToString()).FirstOrDefault();

                    return qtde_descarga;
                }
            }
            catch (Exception ex)
            {
                return qtde_descarga;
            }
        }
        public TalieItem InsertTalieItem(long dif, int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(" insert into REDEX..TB_TALIE_ITEM (AUTONUM_TALIE, NF, QTDE_DESCARGA, AUTONUM_EMB, QTDE_ESTUFAGEM, LOTE, ");
                    sb.AppendLine(" AUTONUM_PRO, AUTONUM_NF, PESO, COMPRIMENTO, LARGURA, ALTURA, DIFERENCA, TIPO_DESCARGA, OBS, QTDE_DISPONIVEL, ");
                    sb.AppendLine(" MARCA, FLAG_MADEIRA, FLAG_FRAGIL, REMONTE, FUMIGACAO, YARD, AUTONUM_REGCS, ARMAZEM, IMO, IMO2, IMO3, IMO4, IMO5, ");
                    sb.AppendLine(" UNO, UNO2, UNO3, UNO4, UNO5, CODIGO_CARGA, carimbo, data_liberacao, CARGA_NUMERADA, FLAG_AVARIA, FLAG_REMONTE, FLAG_NUMERADA) ");
                    sb.AppendLine(" select AUTONUM_TALIE, NF," + dif + ",AUTONUM_EMB,QTDE_ESTUFAGEM,LOTE, ");
                    sb.AppendLine(" AUTONUM_PRO,AUTONUM_NF,PESO,COMPRIMENTO,LARGURA,ALTURA,DIFERENCA,TIPO_DESCARGA,OBS,QTDE_DISPONIVEL, ");
                    sb.AppendLine(" MARCA,FLAG_MADEIRA,FLAG_FRAGIL,REMONTE,FUMIGACAO,YARD,AUTONUM_REGCS,ARMAZEM,IMO,IMO2,IMO3,IMO4,IMO5, ");
                    sb.AppendLine(" UNO,UNO2,UNO3,UNO4,UNO5,CODIGO_CARGA,carimbo,data_liberacao,CARGA_NUMERADA,FLAG_AVARIA, FLAG_REMONTE, FLAG_NUMERADA ");
                    sb.AppendLine(" from redex.dbo.TB_TALIE_ITEM where autonum_ti = " + id + "  ");


                    _db.Query<TalieItem>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieItemDTO UpdateTalieItem(TalieItemDTO obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE       ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM    ");
                    sb.AppendLine(" SET ");
                    sb.AppendLine(" QTDE_DESCARGA =  @QTDE_DESCARGA, ");
                    sb.AppendLine(" LARGURA =   @LARGURA, ");
                    sb.AppendLine(" ALTURA =  @ALTURA, ");
                    sb.AppendLine(" COMPRIMENTO =  @COMPRIMENTO, ");
                    sb.AppendLine(" PESO =  @PESO,");
                    sb.AppendLine(" YARD = @YARD, ");
                    sb.AppendLine(" Carimbo = @Carimbo,");
                    sb.AppendLine(" FLAG_FRAGIL  = @FLAG_FRAGIL, ");
                    sb.AppendLine(" FLAG_AVARIA = @FLAG_AVARIA, ");
                    sb.AppendLine(" REMONTE  = @REMONTE,  ");
                    sb.AppendLine(" IMO = @IMO, ");
                    sb.AppendLine(" IMO2 = @IMO2, ");
                    sb.AppendLine(" IMO3 = @IMO3,");
                    sb.AppendLine(" IMO4 = @IMO4, ");
                    sb.AppendLine(" UNO = @UNO, ");
                    sb.AppendLine(" UNO2 = @UNO2, ");
                    sb.AppendLine(" UNO3 = @UNO3,");
                    sb.AppendLine(" UNO4 = @UNO4, ");
                    sb.AppendLine(" MARCA = @MARCACAO, ");
                    sb.AppendLine(" OBS = @OBSERVACAO, ");
                    sb.AppendLine(" FLAG_NUMERADA = @FLAG_NUMERADA, ");
                    sb.AppendLine(" CARGA_NUMERADA = @CARGA_NUMERADA, ");
                    sb.AppendLine(" AUTONUM_EMB = @AUTONUM_EMB ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" AUTONUM_TI = @AUTONUM_TI ");


                    _db.Query<TalieItemDTO>(sb.ToString(), new
                    {
                        AUTONUM_TI = obj.AUTONUM_TI,
                        QTDE_DESCARGA = obj.Quantidade,
                        LARGURA = obj.LARGURA,
                        ALTURA = obj.ALTURA,
                        COMPRIMENTO = obj.COMPRIMENTO,
                        PESO = obj.Peso,
                        YARD = obj.YARD,
                        Carimbo = obj.Carimbo,
                        FLAG_FRAGIL = obj.FLAG_FRAGIL,
                        FLAG_AVARIA = obj.FLAG_AVARIA,
                        REMONTE = obj.REMONTE,
                        IMO = obj.IMO,
                        IMO2 = obj.IMO2,
                        IMO3 = obj.IMO3,
                        IMO4 = obj.IMO4,
                        UNO = obj.UNO,
                        UNO2 = obj.UNO2,
                        UNO3 = obj.UNO3,
                        UNO4 = obj.UNO4,
                        MARCACAO = obj.MARCACAO,
                        OBSERVACAO = obj.OBSERVACAO,
                        FLAG_NUMERADA = obj.FLAG_NUMERADA,
                        CARGA_NUMERADA = obj.FLAG_NUMERADA,
                        AUTONUM_EMB = obj.AUTONUM_EMB,

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        #region descarga automatica 
        public IEnumerable<DescargaAutomaticaDTO> GetDadosDescargaAutomatica(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" rcs.AUTONUM_REGCS, ");
                    sb.AppendLine(" ISNULL(rcs.QUANTIDADE, 0) - (select isnull(sum(qtde_descarga), 0) descarregado from REDEX..tb_talie t ");
                    sb.AppendLine(" inner join REDEX..tb_talie_item b on t.autonum_talie = b.AUTONUM_TALIE ");
                    sb.AppendLine(" where t.autonum_reg = REG.AUTONUM_REG ");
                    sb.AppendLine(" ) AS QUANTIDADE, ");
                    //sb.AppendLine(" rcs.QUANTIDADE,");
                    sb.AppendLine(" rcs.NF, ");
                    sb.AppendLine(" bcg.qtde as qtde_manifestada,  ");
                    sb.AppendLine(" (isnull(BCG.PESO_bruto, 0) / bcg.qtde) as peso_manifestado, ");
                    sb.AppendLine(" bcg.imo, ");
                    sb.AppendLine(" bcg.imo2, ");
                    sb.AppendLine(" bcg.imo3, ");
                    sb.AppendLine(" bcg.imo4, ");
                    sb.AppendLine(" bcg.uno, ");
                    sb.AppendLine(" bcg.uno2, ");
                    sb.AppendLine(" bcg.uno3, ");
                    sb.AppendLine(" bcg.uno4, ");
                    sb.AppendLine(" BCG.AUTONUM_PRO, ");
                    sb.AppendLine(" BCG.AUTONUM_EMB ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..tb_registro reg ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..tb_registro_cs rcs on reg.autonum_reg = rcs.autonum_reg ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..tb_booking_carga bcg on rcs.autonum_bcg = bcg.autonum_bcg ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" reg.autonum_reg = " + id);

                    var query = _db.Query<DescargaAutomaticaDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetMaxNotaFiscal(string id)
        {
            int max = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT max(a.AUTONUM_NF) AUTONUM_NF FROM REDEX.dbo.tb_NOTAS_FISCAIS A where A.NUM_NF = '" + id + "' ");

                    max = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return max;
                }
            }
            catch (Exception ex)
            {
                return max;
            }
        }
        public double GetMaxPesoBruto(int id)
        {
            double peso = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select max(isnull(peso_bruto,0)) from redex.dbo.tb_notas_fiscais where autonum_nf= " + id);

                    peso = _db.Query<double>(sb.ToString()).FirstOrDefault();

                    return peso;

                }
            }
            catch (Exception ex)
            {
                return peso;
            }
        }
        public int GetCountPesoBruto(int id)
        {
            int count = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select count(1) from redex.dbo.TB_TALIE_ITEM where autonum_nf= " + id);

                    count = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
        public double GetPesoBruto(int id)
        {
            double peso = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select PESO_BRUTO from REDEX..TB_NOTAS_FISCAIS where AUTONUM_NF = " + id);

                    peso = _db.Query<double>(sb.ToString()).FirstOrDefault();

                    return peso;

                }
            }
            catch (Exception ex)
            {
                return peso;
            }
        }

        public DescargaAutomaticaDTO InserirTalieItemDescargaAutomatica(DescargaAutomaticaDTO obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    int nf = obj.AUTONUM_NF;

                    sb.Append(" select PESO_BRUTO from REDEX..TB_NOTAS_FISCAIS where AUTONUM_NF = " + nf + " ORDER BY AUTONUM_REG  ");

                    decimal peso = _db.Query<decimal>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" INSERT INTO            ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" AUTONUM_TALIE, ");
                    sb.AppendLine(" AUTONUM_REGCS, ");
                    sb.AppendLine(" QTDE_DESCARGA, ");
                    sb.AppendLine(" TIPO_DESCARGA, ");
                    sb.AppendLine(" DIFERENCA, ");
                    sb.AppendLine(" OBS, ");
                    sb.AppendLine(" QTDE_DISPONIVEL, ");
                    sb.AppendLine(" COMPRIMENTO, ");
                    sb.AppendLine(" LARGURA, ");
                    sb.AppendLine(" ALTURA, ");
                    sb.AppendLine(" PESO, ");
                    sb.AppendLine(" QTDE_ESTUFAGEM, ");
                    sb.AppendLine(" MARCA, ");
                    sb.AppendLine(" REMONTE, ");
                    sb.AppendLine(" FUMIGACAO, ");
                    sb.AppendLine(" FLAG_FRAGIL, ");
                    sb.AppendLine(" FLAG_MADEIRA, ");
                    sb.AppendLine(" YARD, ");
                    sb.AppendLine(" ARMAZEM, ");
                    sb.AppendLine(" AUTONUM_NF, ");
                    sb.AppendLine(" NF, ");
                    sb.AppendLine(" IMO, ");
                    sb.AppendLine(" UNO, ");
                    sb.AppendLine(" IMO2, ");
                    sb.AppendLine(" UNO2, ");
                    sb.AppendLine(" IMO3, ");
                    sb.AppendLine(" UNO3, ");
                    sb.AppendLine(" IMO4, ");
                    sb.AppendLine(" UNO4, ");
                    sb.AppendLine(" AUTONUM_EMB, ");
                    sb.AppendLine(" AUTONUM_PRO ");
                    sb.AppendLine(" ) values( ");

                    sb.AppendLine(" @AUTONUM_TALIE, ");
                    sb.AppendLine(" @AUTONUM_REGCS, ");
                    sb.AppendLine(" @QTDE_DESCARGA, ");
                    sb.AppendLine(" 'TOTAL', ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" replace('" + peso + "', ',', '.'), ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" '', ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" 0, ");
                    sb.AppendLine(" null, ");
                    sb.AppendLine(" null, ");
                    sb.AppendLine(" @AUTONUM_NF, ");
                    sb.AppendLine(" @NF, ");
                    sb.AppendLine(" @IMO, ");
                    sb.AppendLine(" @UNO, ");
                    sb.AppendLine(" @IMO2, ");
                    sb.AppendLine(" @UNO2, ");
                    sb.AppendLine(" @IMO3, ");
                    sb.AppendLine(" @UNO3, ");
                    sb.AppendLine(" @IMO4, ");
                    sb.AppendLine(" @UNO4, ");
                    sb.AppendLine(" @AUTONUM_EMB, ");
                    sb.AppendLine(" @AUTONUM_PRO ");
                    sb.AppendLine(" ) ");

                    _db.Query<DescargaAutomaticaDTO>(sb.ToString(), new
                    {
                        AUTONUM_TALIE = obj.AUTONUM_TALIE,
                        AUTONUM_REGCS = obj.AUTONUM_REGCS,
                        QTDE_DESCARGA = obj.QUANTIDADE,
                        AUTONUM_NF = obj.AUTONUM_NF,
                        NF = obj.NF,
                        IMO = obj.imo,
                        UNO = obj.uno,
                        IMO2 = obj.imo2,
                        UNO2 = obj.uno2,
                        IMO3 = obj.imo3,
                        UNO3 = obj.uno3,
                        IMO4 = obj.imo4,
                        UNO4 = obj.uno4,
                        AUTONUM_EMB = obj.AUTONUM_EMB,
                        AUTONUM_PRO = obj.AUTONUM_PRO,
                        //PESO = obj.Peso

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion 
        public Talie ExcluirTalie(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("DELETE FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TALIE =" + id);

                    _db.Query<Talie>(sb.ToString()).FirstOrDefault();
                    sb.Clear();

                    sb.AppendLine(" DELETE FROM REDEX..TB_TALIE WHERE AUTONUM_TALIE =  " + id);

                    _db.Query<Talie>(sb.ToString()).FirstOrDefault();
                    sb.Clear();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieItem ExcluirTalieITem(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("DELETE FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_TI =" + id);

                    _db.Query<TalieItem>(sb.ToString()).FirstOrDefault();

                    sb.Clear();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public TalieItem UpdateQuantidades(int talieId, int quantidade)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" update redex..tb_talie_item set qtde_descarga = qtde_descarga + " + quantidade + " where autonum_ti = ");
                    sb.AppendLine(" (select max(autonum_ti) from redex..tb_talie_item where autonum_talie = " + talieId + ") ");

                    _db.Query<TalieItem>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<TalieDTO> GetNFByTalieId(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" NF.num_nf ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..tb_talie_item ti ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_TALIE T ON TI.AUTONUM_TALIE = T.AUTONUM_TALIE ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_BOOKING BOO ON T.AUTONUM_BOO = BOO.AUTONUM_BOO ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_NOTAS_FISCAIS NF ON TI.AUTONUM_NF = NF.AUTONUM_NF ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" TI.AUTONUM_TALIE = " + id);
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" ISNULL(TI.AUTONUM_NF, 0) <> 0 ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" ISNULL(NF.AUTONUM_REG, 0) = 0 ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" ISNULL(BOO.FLAG_BAGAGEM, 0) = 0 ");

                    var query = _db.Query<TalieDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countEtiquetas(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" COUNT(1) FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE T ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..ETIQUETAS E ON TI.AUTONUM_REGCS = E.AUTONUM_RCS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" T.AUTONUM_TALIE = " + id);

                    int etiquetas = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return etiquetas;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int getValidaConteiner(string id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select isnull(max(p.autonum_patio),0) from redex..tb_patio p ");
                    sb.AppendLine(" where isnull(p.flag_historico,0)=0 and p.id_conteiner='" + id + "' ");

                    int tr = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return tr;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int countPendencias(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" COUNT(1) FROM ");
                    sb.AppendLine(" REDEX..TB_TALIE T ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE ");
                    sb.AppendLine(" INNER JOIN ");
                    sb.AppendLine(" REDEX..ETIQUETAS E ON TI.AUTONUM_REGCS = E.AUTONUM_RCS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" T.AUTONUM_TALIE = " + id);
                    sb.AppendLine(" AND EMISSAO IS NULL ");

                    int etiquetas = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return etiquetas;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public IEnumerable<Embalagem> GetListarEmbalagens()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" select autonum_emb as AUTONUM_EMB, DESCRICAO_EMB + '-' + Convert(varchar, AUTONUM_EMB) as DESCRICAO_EMB from redex..TB_CAD_embalagens order by DESCRICAO_EMB desc ");

                    var query = _db.Query<Embalagem>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region UTILS
        private (List<string> conditions, DynamicParameters parameters) BuildFilter(string talie, string registro, string tipoDescarga)
        {
            var conditions = new List<string> { "a.flag_fechado = 0" }; // Condição padrão
            var parameters = new DynamicParameters();

            // Adicione `talie` ao `DynamicParameters` sempre
            conditions.Add("(@talie IS NULL OR a.AUTONUM_TALIE = @talie)");
            parameters.Add("talie", string.IsNullOrWhiteSpace(talie) ? (object)null : talie);

            // Adicione `registro` ao `DynamicParameters` sempre
            conditions.Add("(@registro IS NULL OR a.AUTONUM_TALIE = @registro)");
            parameters.Add("registro", string.IsNullOrWhiteSpace(registro) ? (object)null : registro);

            // Adicione `tipoDescarga` ao `DynamicParameters` sempre
            parameters.Add("tipoDescarga", tipoDescarga);

            if (tipoDescarga == OpcoesDescarga.DA.ToString())
            {
                conditions.Add("flag_descarga = 1");
            }
            else if (tipoDescarga == OpcoesDescarga.CD.ToString())
            {
                conditions.Add("crossdocking = 1");
            }
            else
            {
                conditions.Add("flag_descarga = 0");
                conditions.Add("crossdocking = 0");
            }

            return (conditions, parameters);
        }




        #endregion UTILS

    }
}
