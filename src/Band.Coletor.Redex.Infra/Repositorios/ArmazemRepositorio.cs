using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Business.Models.ModelsLogic;
using Band.Coletor.Redex.Infra.Repositorios.Sql;
using Dapper;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class ArmazemRepositorio : BaseRepositorio<ArmazemModel>, IArmazenRepositorio
    {
        public ArmazemRepositorio(string connectionString) : base(connectionString)
        {
        }

        
    }
}
