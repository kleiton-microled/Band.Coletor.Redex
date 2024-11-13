using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Classes
{
    public class OperacaoBusiness : IOperacaoBusiness
    {
        private readonly IMapper _mapper;
        private readonly IOperacaoRepositorio _repositorio;
        public OperacaoBusiness(IMapper mapper, IOperacaoRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<OperacaoViewModel>> ListAll()
        {
            string command = @"SELECT te.AUTONUM_EQP AS ID, te.NOME_EQP as DESCRICAO  FROM REDEX..TB_EQUIPE te WHERE te.FLAG_ATIVO = 1 AND te.FLAG_OPERADOR = 1";
            var data = await _repositorio.ListAll(command);
            return _mapper.Map<IEnumerable<OperacaoViewModel>>(data);
        }
    }
}
