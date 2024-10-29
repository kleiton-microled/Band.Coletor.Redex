using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
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
    public class VeiculosRepositorio : IVeiculosRepositorio
    {
        public IEnumerable<VeiculosDTO> GetDescarregarVeiculos()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("SELECT A.AUTONUM AS AUTONUM, Convert(varchar, DT_GATE_IN, 103) +  Convert(varchar, DT_GATE_IN, 108) + ' - ' + PLACA + ' - ' + CARRETA + ' - ' + ID_CONTEINER AS IDENTIFICACAO FROM OPERADOR..TB_GATE_NEW A INNER JOIN OPERADOR..TB_AMR_GATE B ON A.AUTONUM = B.GATE INNER JOIN SGIPA..TB_CNTR_BL C ON B.CNTR_IPA = C.AUTONUM WHERE B.FUNCAO_GATE = 11 AND A.DT_GATE_OUT IS NULL AND C.YARD IS NULL ORDER BY DT_GATE_IN DESC ");

                    string sql = sb.ToString();

                    var veiculos = _db.Query<VeiculosDTO>(sql).AsEnumerable();


                    return veiculos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VeiculosDTO> GetCarregarVeiculos()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();


                    sb.AppendLine(" SELECT A.AUTONUM AS AUTONUM, PLACA + ' - ' + CARRETA + ' - ' + ID_CONTEINER + ' - ' + C.YARD AS IDENTIFICACAO FROM OPERADOR..TB_GATE_NEW A INNER JOIN OPERADOR..TB_AMR_GATE B ON A.AUTONUM = B.GATE INNER JOIN SGIPA..TB_CNTR_BL C ON B.CNTR_IPA = C.AUTONUM WHERE B.FUNCAO_GATE = 8 AND A.DT_GATE_OUT IS NULL AND DT_GATE_IN >= GETDATE() -365 ORDER BY C.YARD ");

                    string sql = sb.ToString();

                    var veiculos = _db.Query<VeiculosDTO>(sql).AsEnumerable();

                    return veiculos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

