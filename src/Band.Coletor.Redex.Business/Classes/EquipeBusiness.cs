using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Classes
{
    public class EquipeBusiness : IEquipeBusiness
    {
        private readonly IMapper _mapper;
        private readonly IEquipeRepositorio _repositorio;
        public EquipeBusiness(IMapper mapper, IEquipeRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<EquipeViewModel>> ListAll()
        {
            var equipes = await _repositorio.GetAllEquipes();
            return _mapper.Map<IEnumerable<EquipeViewModel>>(equipes);
        }
    }
}
