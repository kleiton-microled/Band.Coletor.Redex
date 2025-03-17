using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Classes
{
    public class Marcantebusiness : IMarcanteBusiness
    {
        private readonly IMapper _mapper;
        private readonly IMarcanteRepositorio _repositorio;
        public Marcantebusiness(IMapper mapper, IMarcanteRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }

        public async Task<MarcanteViewModel> BuscarMarcante(string marcante)
        {
            var data = await _repositorio.BuscarMarcante(marcante);

            return _mapper.Map<MarcanteViewModel>(marcante);
        }
    }
}
