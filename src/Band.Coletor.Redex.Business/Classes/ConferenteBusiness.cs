using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Classes
{
    public class ConferenteBusiness : IConferenteBusiness
    {
        private readonly IMapper _mapper;
        private readonly IConferenteRepositorio _repositorio;
        public ConferenteBusiness(IMapper mapper, IConferenteRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<ConferenteViewModel>> ListAll()
        {
            string command = @"SELECT te.AUTONUM_EQP AS ID, te.NOME_EQP as DESCRICAO  FROM REDEX..TB_EQUIPE te WHERE te.FLAG_ATIVO = 1 AND te.FLAG_CONFERENTE = 1";
            var data = await _repositorio.ListAll(command);

            return _mapper.Map<IEnumerable<ConferenteViewModel>>(data);
        }
    }
}
