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
    }
}

