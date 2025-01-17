using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Band.Coletor.Redex.Infra.Repositorios.Sql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class RegistroRepositorio : BaseRepositorio<RegistroDTO>, IRegistroRepositorio
    {
        public RegistroRepositorio(string connectionString) : base(connectionString)
        {
        }

        public async Task<RegistroDTO> CarregarRegistro(int codigoRegistro)
        {
            try
            {
                string command = SqlQueries.CarregarRegistroNew;

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("CodigoRegistro", codigoRegistro);

                using (var connection = Connection)
                {
                    var registroDict = new Dictionary<int, RegistroDTO>();

                    var result = await connection.QueryAsync<RegistroDTO, Business.Entity.Talie, Business.Entity.TalieItem, RegistroDTO>(
                        command,
                        (registro, talie, talieItem) =>
                        {
                            if (!registroDict.TryGetValue(registro.Id, out var registroEntry))
                            {
                                registroEntry = registro;
                                registroEntry.Talie = talie;
                                if(registroEntry.Talie != null)
                                    registroEntry.Talie.TalieItem = new List<Business.Entity.TalieItem>();

                                registroDict.Add(registro.Id, registroEntry);
                            }

                            if (talieItem != null)
                            {
                                registroEntry.Talie.TalieItem.Add(talieItem);
                            }

                            return registroEntry;
                        },
                        parameters,
                        splitOn: "Id,Id"
                    );

                    return registroDict.Values.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar registro: {ex.Message}", ex);
            }
        }


        public Registro ObterRegistroPorLote(int lote)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "lote", value: lote, direction: ParameterDirection.Input);

                return con.Query<Registro>(@"
                    SELECT
                        A.AUTONUM AS GateId,
                        B.PLACA,
                        E.AUTONUM_EXPORTADOR as ExportadorId,
                        CP.RAZAO AS Exportador,
                        E.REFERENCE AS Referencia,
                        E.AUTONUM_BOO as Reserva,
                        B.AUTONUM_REG as Lote,
                        B.PATIO,
                        GETDATE() as Inicio
                    FROM
                        TB_GATE_NEW A
                    INNER JOIN
                        TB_REGISTRO B ON A.AUTONUM = B.AUTONUM_GATE
                    INNER JOIN
                        TB_BOOKING E ON B.AUTONUM_BOO = E.AUTONUM_BOO
                    INNER JOIN
                        TB_CAD_PARCEIROS CP ON E.AUTONUM_EXPORTADOR=CP.AUTONUM
                    WHERE
                        B.AUTONUM_REG= @lote
                    ORDER BY
                        A.AUTONUM DESC", parametros).FirstOrDefault();
            }
        }

        public int SaveOrUpdate(RegistroViewModel registro)
        {
            using (var connection = Connection)
            {
                // Verifica se já existe um registro com o autonum_reg
                string verificaTalieSql = @"
                SELECT COUNT(*) 
                FROM REDEX..tb_talie 
                WHERE autonum_reg = @CodigoRegistro";

                int count = connection.ExecuteScalar<int>(verificaTalieSql, new { CodigoRegistro = registro.CodigoRegistro });

                if (count > 0)
                {
                    // Atualizar
                    string updateSql = @"
                    UPDATE REDEX..tb_talie 
                    SET 
                        flag_descarga = 1,
                        flag_estufagem = 1,
                        flag_carregamento = 0,
                        crossdocking = 0,
                        conferente = @Conferente,
                        equipe = @Equipe,
                        --autonum_boo = @Reserva,
                        forma_operacao = @Operacao
                    WHERE autonum_reg = @CodigoRegistro";

                    connection.Execute(updateSql, new
                    {
                        Conferente = registro.Talie.Conferente,
                        Equipe = registro.Talie.Equipe,
                        Operacao = registro.Talie.Operacao,
                        CodigoRegistro = registro.CodigoRegistro
                    });

                    return registro.Talie.Id;
                }
                else
                {
                    // Inserir
                    string insertSql = @"
                                        INSERT INTO REDEX..tb_talie 
                                        (
                                            placa, inicio, flag_descarga, 
                                            flag_estufagem, flag_carregamento, crossdocking, 
                                            conferente, equipe, autonum_reg
                                        ) 
                                        VALUES 
                                        (
                                            @Placa,
                                            @Inicio, 
                                            1,--FLAG DESCARGA 
                                            0,--FLAG ESTUFAGEM
                                            0,--FLAG CARREGAMENTO 
                                            0,--CROSDOCKING 
                                            @Conferente, --ID 
                                            @Equipe, --ID
                                            --autonum_boo
                                            --forma_operacao
                                            --autonum_gate
                                            @CodigoRegistro --AutonumRegistro
                                        );
                                        
                                        SELECT CAST(SCOPE_IDENTITY() as int)";


                    int talieId = connection.ExecuteScalar<int>(insertSql, new
                    {
                        Placa = registro.Placa,
                        Inicio = DateTime.Now,
                        Conferente = registro.Talie.Conferente,
                        Equipe = registro.Talie.Equipe,
                        CodigoRegistro = registro.CodigoRegistro
                    });

                    return talieId;
                }
            }
        }

        public void GeraDescargaAutomatica(int codigoRegistro, int autonumTalie)
        {
            using (var connection = Connection)
            {
                // Obter os registros da descarga
                string queryDescarga = @"
                                        SELECT 
                                            rcs.*, 
                                            bcg.qtde AS qtde_manifestada, 
                                            ISNULL(bcg.peso_bruto, 0) / NULLIF(bcg.qtde, 0) AS peso_manifestado,
                                            bcg.imo, bcg.imo2, bcg.imo3, bcg.imo4, 
                                            bcg.uno, bcg.uno2, bcg.uno3, bcg.uno4, 
                                            bcg.autonum_pro, bcg.autonum_emb
                                        FROM REDEX..tb_registro reg
                                        INNER JOIN REDEX..tb_registro_cs rcs ON reg.autonum_reg = rcs.autonum_reg
                                        INNER JOIN REDEX..tb_booking_carga bcg ON rcs.autonum_bcg = bcg.autonum_bcg
                                        WHERE reg.autonum_reg = @CodigoRegistro";

                var registros = connection.Query(queryDescarga, new { CodigoRegistro = codigoRegistro }).ToList();

                foreach (var reg in registros)
                {
                    long idNF = 0;
                    double pesoNF = 0;

                    // Verifica se a NF já está cadastrada
                    if (!string.IsNullOrEmpty(reg.DANFE))
                    {
                        string queryNF = @"
                                         SELECT MAX(AUTONUM_NF) AS AUTONUM_NF 
                                         FROM REDEX..tb_NOTAS_FISCAIS 
                                         WHERE DANFE = @DANFE";

                        var idNFResult = connection.ExecuteScalar<long?>(queryNF, new { DANFE = reg.DANFE });
                        idNF = (idNFResult.HasValue ? idNFResult.Value : 0);
                    }

                    // Define o peso da NF
                    if (idNF != 0)
                    {
                        string queryPesoNF = @"
                                             SELECT ISNULL(peso_bruto, 0) 
                                             FROM REDEX..tb_notas_fiscais 
                                             WHERE autonum_nf = @IdNF";

                        var pesoNFResult = connection.ExecuteScalar<double?>(queryPesoNF, new { IdNF = idNF });
                        pesoNF = pesoNFResult.HasValue ? pesoNFResult.Value : 0;
                    }
                    else
                    {
                        pesoNF = reg.peso_bruto ?? 0;
                    }

                    // Inserir na tabela de itens da descarga
                    string insertItem = @"
                                        INSERT INTO REDEX..tb_talie_item (
                                            autonum_talie, autonum_regcs, qtde_descarga, tipo_descarga, 
                                            diferenca, obs, qtde_disponivel, comprimento, largura, altura, peso, 
                                            qtde_estufagem, marca, remonte, fumigacao, flag_fragil, flag_madeira, 
                                            YARD, armazem, autonum_nf, nf, imo, uno, imo2, uno2, imo3, uno3, imo4, uno4, 
                                            autonum_emb, autonum_pro
                                        ) VALUES (
                                            @AutonumTalie, @AutonumRegcs, @QtdeDescarga, 'TOTAL', 
                                            '0', '', 0, 0, 0, 0, @Peso, 0, '', 0, '', 0, 0, NULL, NULL, 
                                            @IdNF, @NF, @IMO, @UNO, @IMO2, @UNO2, @IMO3, @UNO3, @IMO4, @UNO4, 
                                            @AutonumEmb, @AutonumPro
                                        )";


                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("AutonumTalie", autonumTalie);
                    parameters.Add("AutonumRegcs", reg.autonum_regcs);
                    parameters.Add("QtdeDescarga", reg.QUANTIDADE);
                    parameters.Add("Peso", pesoNF);
                    parameters.Add("IdNF", idNF);
                    parameters.Add("NF", reg.NF);
                    parameters.Add("IMO", reg.IMO);
                    parameters.Add("UNO", reg.UNO);
                    parameters.Add("IMO2", reg.IMO2);
                    parameters.Add("UNO2", reg.UNO2);
                    parameters.Add("IMO3", reg.IMO3);
                    parameters.Add("UNO3", reg.UNO3);
                    parameters.Add("IMO4", reg.IMO4);
                    parameters.Add("UNO4", reg.UNO4);
                    parameters.Add("AutonumEmb", reg.autonum_emb);
                    parameters.Add("AutonumPro", reg.autonum_pro);

                    connection.Execute(insertItem, parameters);
                }
            }
        }

        public void GravarObservacao(string observacao, long talie)
        {
            using (var connection = Connection)
            {
                string query = @"UPDATE REDEX.dbo.TB_TALIE SET OBS = @obs WHERE AUTONUM_TALIE = @talie";

                connection.Execute(query, new { obs = observacao, talie = talie });
            }
        }

        #region VALIDACOES
        public int ValidarDanfe(int codigoRegistro)
        {
            try
            {
                string command = SqlQueries.ValidarDanfe;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("CodigoRegistro", codigoRegistro);
                using (var connection = Connection)
                {
                    return connection.QueryFirstOrDefault<int>(command, parameters);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public bool ValidarNotaCadastrada(int codigoRegistro)
        {
            using (var connection = Connection)
            {
                // Query para tb_registro_cs
                string queryRegistroCs = @"
                                          SELECT rcs.NF, rcs.DANFE
                                          FROM REDEX.dbo.tb_registro reg
                                          INNER JOIN REDEX.dbo.tb_registro_cs rcs ON reg.autonum_reg = rcs.autonum_reg
                                          WHERE reg.autonum_reg = @Lote";

                var registros = connection.Query(queryRegistroCs, new { Lote = codigoRegistro }).ToList();

                if (!registros.Any())
                {
                    // Query para tb_registro_cntr
                    string queryRegistroCntr = @"
                                                SELECT rc.NF, rc.DANFE
                                                FROM REDEX.dbo.tb_registro reg
                                                INNER JOIN REDEX.dbo.tb_registro_cntr rc ON reg.autonum_reg = rc.autonum_reg
                                                WHERE reg.autonum_reg = @Lote";

                    registros = connection.Query(queryRegistroCntr, new { Lote = codigoRegistro }).ToList();
                }

                // Validar cada registro
                foreach (var registro in registros)
                {
                    if (!string.IsNullOrEmpty(registro.NF) && !string.IsNullOrEmpty(registro.DANFE))
                    {
                        string queryNotaFiscal = @"
                                                 SELECT COUNT(*)
                                                 FROM REDEX.dbo.tb_NOTAS_FISCAIS
                                                 WHERE DANFE = @Danfe";

                        int count = connection.ExecuteScalar<int>(queryNotaFiscal, new { Danfe = registro.DANFE });

                        if (count == 0)
                        {
                            return false; // NF não cadastrada
                        }
                    }
                }
            }

            return true; // Tudo validado
        }


        #endregion VALIDACOES

    }
}