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
        public const string BuscarDadosTaliePorRegistro = @"SELECT 
                                                                T.AUTONUM_TALIE, 
                                                                T.AUTONUM_PATIO, 
                                                                T.PLACA, 
                                                                T.INICIO, 
                                                                T.TERMINO, 
                                                                T.FLAG_DESCARGA, 
                                                                T.FLAG_ESTUFAGEM, 
                                                                T.CROSSDOCKING, 
                                                                ISNULL(T.CONFERENTE, 0) AS CONFERENTE, 
                                                                ISNULL(T.EQUIPE, 0) AS EQUIPE, 
                                                                T.AUTONUM_BOO, 
                                                                T.FLAG_CARREGAMENTO, 
                                                                T.FORMA_OPERACAO, 
                                                                T.AUTONUM_GATE, 
                                                                T.FLAG_FECHADO, 
                                                                T.OBS, 
                                                                T.AUTONUM_RO, 
                                                                T.AUDIT_225, 
                                                                T.ANO_TERMO, 
                                                                T.TERMO, 
                                                                T.DATA_TERMO, 
                                                                T.FLAG_PACOTES, 
                                                                T.ALERTA_ETIQUETA, 
                                                                T.AUTONUM_REG, 
                                                                T.FLAG_COMPLETO, 
                                                                T.EMAIL_ENVIADO, 
                                                                BOO.REFERENCE, 
                                                                CP.FANTASIA
                                                                --T.ID_GEO_CAMERA 
                                                            FROM 
                                                                REDEX..TB_TALIE T 
                                                            INNER JOIN 
                                                                REDEX..TB_BOOKING BOO ON T.AUTONUM_BOO = BOO.AUTONUM_BOO 
                                                            INNER JOIN 
                                                                REDEX..TB_CAD_PARCEIROS CP ON BOO.AUTONUM_PARCEIRO = CP.AUTONUM 
                                                            WHERE 
                                                                T.AUTONUM_REG = @registro";
        public const string BuscarDadosGate = @"SELECT 
                                                    E.AUTONUM_PARCEIRO, 
                                                    E.REFERENCE as Reserva, 
                                                    A.AUTONUM as CodigoGate, 
                                                    E.AUTONUM_BOO as CodigoBooking, 
                                                    CP.FANTASIA as CLiente
                                                FROM 
                                                    REDEX..TB_REGISTRO B 
                                                INNER JOIN 
                                                    REDEX..TB_GATE_NEW A ON B.AUTONUM_GATE = A.AUTONUM 
                                                INNER JOIN 
                                                    REDEX..TB_BOOKING E ON B.AUTONUM_BOO = E.AUTONUM_BOO 
                                                INNER JOIN 
                                                    REDEX..TB_CAD_PARCEIROS CP ON E.AUTONUM_PARCEIRO = CP.AUTONUM 
                                                WHERE 
                                                    B.AUTONUM_REG = @registro 
                                                    AND A.DT_GATE_IN IS NOT NULL";
        public const string BuscarItensDescarregados = @"SELECT 
                                                            TI.AUTONUM_TI AS CodigoItem, 
                                                            'NF ' + ISNULL(TI.NF, '') + ' ' + 
                                                            CAST(ISNULL(TI.QTDE_DESCARGA, 0) AS VARCHAR) + '   ' + 
                                                            ISNULL(E.SIGLA, E.DESCRICAO_EMB) + ' ' + 
                                                            RTRIM(FORMAT(ISNULL(TI.COMPRIMENTO, 0), '00.00')) + 'x' + 
                                                            RTRIM(FORMAT(ISNULL(TI.LARGURA, 0), '00.00')) + 'x' + 
                                                            RTRIM(FORMAT(ISNULL(TI.ALTURA, 0), '00.00')) AS Descricao
                                                        FROM 
                                                            REDEX..TB_TALIE T
                                                        INNER JOIN 
                                                            REDEX..TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE
                                                        LEFT JOIN 
                                                            REDEX..TB_CAD_EMBALAGENS E ON TI.AUTONUM_EMB = E.AUTONUM_EMB
                                                        WHERE 
                                                            T.AUTONUM_TALIE = @talie
                                                        ORDER BY 
                                                            TI.AUTONUM_TI;";
        public const string CadastrarTalie = @"INSERT INTO REDEX..TB_TALIE
                                                   (
		                                               INICIO,
		                                               CROSSDOCKING,
		                                               CONFERENTE,
		                                               EQUIPE,
		                                               AUTONUM_BOO,
		                                               FORMA_OPERACAO,
		                                               PLACA,
		                                               AUTONUM_GATE,
		                                               AUTONUM_REG,
		                                               OBS
                                                   ) VALUES (
                                                       @Inicio,
                                                       @CrossDocking,
                                                       @ConferenteId,
                                                       @EquipeId,
                                                       @BookingId,
                                                       @OperacaoId,
                                                       @Placa,
                                                       @GateId,
                                                       @RegistroId,
                                                       @Observacoes); SELECT CAST(SCOPE_IDENTITY() AS INT)";
        public const string AtualizarTalie = @"
                                              UPDATE REDEX..TB_TALIE SET 
                                                  INICIO = CONVERT(DATETIME, @Inicio, 120), 
                                                  CONFERENTE = @ConferenteId, 
                                                  EQUIPE = @EquipeId, 
                                                  FORMA_OPERACAO = @OperacaoId, 
                                                  OBS = @Observacoes
                                              WHERE 
                                                  AUTONUM_TALIE = @AutonumTalie";
        public const string ObterIdNotaFiscal = @"SELECT 
                                                      A.AUTONUM_NF
                                                  FROM 
                                                      REDEX..TB_NOTAS_FISCAIS A
                                                  INNER JOIN 
                                                      REDEX..TB_BOOKING BOO ON A.AUTONUM_BOO = BOO.AUTONUM_BOO
                                                  WHERE 
                                                      BOO.AUTONUM_BOO = @AutonumBooking
                                                      AND A.NUM_NF = @NumeroNotaFiscal";
        public const string ObterItensNotaFiscal = @"SELECT 
                                                        A.AUTONUM_REGCS AS ID,
                                                        A.QUANTIDADE,
                                                        B.PESO_BRUTO,
                                                        B.QTDE,
                                                        B.AUTONUM_EMB,
                                                        D.DESC_PRODUTO,
                                                        C.SIGLA,
                                                        CONCAT(C.DESCRICAO_EMB, '-', C.AUTONUM_EMB) AS EMBALAGEM,
                                                        B.IMO,
                                                        B.IMO2,
                                                        B.IMO3,
                                                        B.IMO4,
                                                        B.UNO,
                                                        B.UNO2,
                                                        B.UNO3,
                                                        B.UNO4
                                                    FROM 
                                                        REDEX..TB_REGISTRO_CS A
                                                    INNER JOIN 
                                                        REDEX..TB_BOOKING_CARGA B ON A.AUTONUM_BCG = B.AUTONUM_BCG
                                                    INNER JOIN 
                                                        REDEX..TB_CAD_EMBALAGENS C ON B.AUTONUM_EMB = C.AUTONUM_EMB
                                                    INNER JOIN 
                                                        REDEX..TB_CAD_PRODUTOS D ON B.AUTONUM_PRO = D.AUTONUM_PRO
                                                    WHERE 
                                                        A.AUTONUM_REG = @CodigoRegistro
                                                        AND A.NF = @numeroNotaFiscal;
                                                    ";
        public const string BuscarContainersMarcantes = @"SELECT 
	                                                          c.autonum as Id, 
	                                                          c.ID_CONTEINER AS Descricao
	                                                      FROM 
	                                                          SGIPA..tb_cntr_bl c 
	                                                      INNER JOIN 
	                                                          SGIPA..tb_amr_cntr_bl a 
	                                                      ON 
	                                                          c.autonum = a.cntr
	                                                      WHERE 
	                                                          a.bl = @lote
	                                                          AND c.PATIO = @patio
	                                                      ORDER BY 
	                                                          ID_CONTEINER";
        public const string CarregarDadosContainer = @"SELECT 
                                                            c.QUANTIDADE, 
                                                            E.DESCR AS EMBALAGEM 
                                                        FROM 
                                                            SGIPA..TB_CARGA_CNTR C 
                                                        LEFT JOIN 
                                                            SGIPA..DTE_TB_EMBALAGENS E ON C.EMBALAGEM = E.CODE 
                                                        WHERE  
                                                            C.BL = @lote";

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

