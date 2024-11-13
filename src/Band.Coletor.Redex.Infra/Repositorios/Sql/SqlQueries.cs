namespace Band.Coletor.Redex.Infra.Repositorios.Sql
{
    public static class SqlQueries
    {
        public const string GetTalieData = @"
                                        SELECT 
                                            a.autonum_reg,
                                            ISNULL(c.id_conteiner, '') AS id_conteiner,
                                            b.reference,
                                            b.instrucao,
                                            d.fantasia,
                                            b.autonum_parceiro,
                                            a.AUTONUM_TALIE AS Id,
                                            a.AUTONUM_PATIO,
                                            ISNULL(a.Placa, '') AS Placa,
                                            a.Inicio,
                                            a.TERMINO,
                                            a.FLAG_DESCARGA,
                                            a.FLAG_ESTUFAGEM,
                                            a.CROSSDOCKING,
                                            a.CONFERENTE,
                                            a.EQUIPE,
                                            a.AUTONUM_BOO,
                                            a.FLAG_CARREGAMENTO,
                                            a.AUTONUM_GATE,
                                            a.flag_fechado,
                                            a.FLAG_COMPLETO,
                                            a.forma_operacao
                                        FROM 
                                            REDEX..tb_talie a
                                        INNER JOIN  
                                            REDEX..tb_booking b ON a.autonum_boo = b.autonum_boo
                                        LEFT JOIN  
                                            REDEX..tb_patio c ON a.autonum_patio = c.autonum_patio
                                        INNER JOIN  
                                            REDEX..tb_cad_parceiros d ON b.autonum_parceiro = d.autonum
                                        WHERE 
                                            a.flag_fechado = 0
                                            AND (@talie IS NULL OR a.AUTONUM_TALIE = @talie)
                                            AND (@registro IS NULL OR a.AUTONUM_TALIE = @registro)
                                            AND (flag_descarga = CASE WHEN @tipoDescarga = 'DA' THEN 1 ELSE 0 END)
                                            AND (crossdocking = CASE WHEN @tipoDescarga = 'CD' THEN 1 ELSE 0 END);
                                         ";
        public const string GetTalieDataPaginado = @"SELECT 
                                                         a.autonum_reg,
                                                         ISNULL(c.id_conteiner, '') AS id_conteiner,
                                                         b.reference,
                                                         b.instrucao,
                                                         d.fantasia,
                                                         b.autonum_parceiro,
                                                         a.AUTONUM_TALIE AS Id,
                                                         a.AUTONUM_PATIO,
                                                         ISNULL(a.Placa, '') AS Placa,
                                                         a.Inicio,
                                                         a.TERMINO,
                                                         a.FLAG_DESCARGA,
                                                         a.FLAG_ESTUFAGEM,
                                                         a.CROSSDOCKING,
                                                         a.CONFERENTE,
                                                         a.EQUIPE,
                                                         a.AUTONUM_BOO,
                                                         a.FLAG_CARREGAMENTO,
                                                         a.AUTONUM_GATE,
                                                         a.flag_fechado,
                                                         a.FLAG_COMPLETO,
                                                         a.forma_operacao
                                                     FROM 
                                                         REDEX..tb_talie a
                                                     INNER JOIN  
                                                         REDEX..tb_booking b ON a.autonum_boo = b.autonum_boo
                                                     LEFT JOIN  
                                                         REDEX..tb_patio c ON a.autonum_patio = c.autonum_patio
                                                     INNER JOIN  
                                                         REDEX..tb_cad_parceiros d ON b.autonum_parceiro = d.autonum
                                                     WHERE 
                                                         a.flag_fechado = 0
                                                         AND (@talie IS NULL OR a.AUTONUM_TALIE = @talie)
                                                         AND (@registro IS NULL OR a.AUTONUM_TALIE = @registro)
                                                         AND (flag_descarga = CASE WHEN @tipoDescarga = 'DA' THEN 1 ELSE 0 END)
                                                         AND (crossdocking = CASE WHEN @tipoDescarga = 'CD' THEN 1 ELSE 0 END)
                                                     ORDER BY a.Inicio
                                                     OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";
        public const string CountQuery = @"
                                           SELECT COUNT(*) AS TotalRecords
                                           FROM 
                                               REDEX..tb_talie a
                                           INNER JOIN  
                                               REDEX..tb_booking b ON a.autonum_boo = b.autonum_boo
                                           LEFT JOIN  
                                               REDEX..tb_patio c ON a.autonum_patio = c.autonum_patio
                                           INNER JOIN  
                                               REDEX..tb_cad_parceiros d ON b.autonum_parceiro = d.autonum
                                           WHERE 
                                               a.flag_fechado = 0
                                               AND (@talie IS NULL OR a.AUTONUM_TALIE = @talie)
                                               AND (@registro IS NULL OR a.AUTONUM_TALIE = @registro)
                                               AND (flag_descarga = CASE WHEN @tipoDescarga = 'DA' THEN 1 ELSE 0 END)
                                               AND (crossdocking = CASE WHEN @tipoDescarga = 'CD' THEN 1 ELSE 0 END);
                                            ";

        #region DESCARGA CD
        public const string QueryGetTalieByIdConteiner = @"
                                                           SELECT 
                                                               A.AUTONUM_TALIE AS Id, 
                                                               A.AUTONUM_REG AS RegistroId, 
                                                               C.ID_CONTEINER AS ConteinerId, 
                                                               B.REFERENCE AS Reference, 
                                                               B.INSTRUCAO AS instrucao, 
                                                               D.FANTASIA as fantasia, 
                                                               B.AUTONUM_EXPORTADOR AS ExportadorId, 
                                                               A.AUTONUM_PATIO AS PatioId, 
                                                               A.PLACA as Placa, 
                                                               CONVERT(VarChar(10), a.inicio, 103) + ' ' + CONVERT(VarChar(8), a.inicio, 108) as Inicio, 
                                                               CONVERT(VarChar(10), a.termino, 103) + ' ' + CONVERT(VarChar(8), a.termino, 108) as Termino, 
                                                               ISNULL(a.FLAG_DESCARGA, 0) as Descarga, 
                                                               ISNULL(a.FLAG_ESTUFAGEM, 0) as Estufagem, 
                                                               a.CROSSDOCKING as CrossDocking, 
                                                               a.CONFERENTE as ConferenteId, 
                                                               a.EQUIPE as EquipeId, 
                                                               a.AUTONUM_BOO as BookingId, 
                                                               ISNULL(a.FLAG_CARREGAMENTO, 0) as Carregamento, 
                                                               A.AUTONUM_GATE as GateId, 
                                                               ISNULL(A.FLAG_FECHADO, 0) as Fechado, 
                                                               B.AUTONUM_PATIOS as Patio, 
                                                               A.FORMA_OPERACAO as OperacaoId, 
                                                               Conf.FANTASIA AS Conferente, 
                                                               Eq.FANTASIA as Equipe, 
                                                               CASE WHEN A.FORMA_OPERACAO = 'A' THEN 'Automático' ELSE 'Manual' END AS Operacao 
                                                           FROM 
                                                               REDEX..TB_TALIE A 
                                                           INNER JOIN 
                                                               REDEX..TB_BOOKING B ON A.AUTONUM_BOO = B.AUTONUM_BOO 
                                                           LEFT JOIN 
                                                               REDEX..TB_PATIO C ON A.AUTONUM_PATIO = C.AUTONUM_PATIO 
                                                           INNER JOIN 
                                                               REDEX..TB_CAD_PARCEIROS D ON B.AUTONUM_PARCEIRO = D.AUTONUM 
                                                           LEFT JOIN 
                                                               REDEX..tb_cad_parceiros Conf ON a.CONFERENTE = CONF.AUTONUM 
                                                           LEFT JOIN 
                                                               REDEX..tb_cad_parceiros Eq ON a.EQUIPE = Eq.AUTONUM 
                                                           INNER JOIN 
                                                               REDEX..tb_patio cc ON a.autonum_patio = cc.AUTONUM_patio 
                                                           WHERE  
                                                               (A.AUTONUM_REG = @Id OR A.AUTONUM_TALIE = @Id) 
                                                               AND cc.id_conteiner = @Conteiner 
                                                           ORDER BY  
                                                               A.INICIO DESC, D.FANTASIA, C.ID_CONTEINER, B.REFERENCE, B.INSTRUCAO";

        public const string QueryValidaConteiner = @"SELECT ISNULL(MAX(p.autonum_patio), 0)
                                                        FROM redex..tb_patio p
                                                      WHERE ISNULL(p.flag_historico, 0) = 0 AND p.id_conteiner = @IdConteiner";

        #region Dominios
        public const string BuscarEquipes = @"SELECT
                                                  AUTONUM_EQP as Id,
                                                  NOME_EQP as Descricao
                                              FROM
                                                  REDEX..TB_EQUIPE
                                              WHERE
                                                  FLAG_ATIVO = 1 AND FLAG_OPERADOR = 1
                                              ORDER BY
                                                  NOME_EQP";
        #endregion
        #endregion
    }
}

