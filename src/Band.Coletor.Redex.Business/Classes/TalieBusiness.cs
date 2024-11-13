using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;

namespace Band.Coletor.Redex.Business.Classes
{
    public class TalieBusiness : ITalieBusiness
    {
        private readonly IMapper _mapper;
        private readonly ITalieRepositorio _repositorio;
        public TalieBusiness(IMapper mapper, ITalieRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }

        public TalieViewModel ObterDadosTaliePorRegistro(int registro)
        {
            var data =  _repositorio.ObterDadosTaliePorRegistro(registro);
            return _mapper.Map<TalieViewModel>(data);
        }
    }
}
