using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Models;

namespace Band.Coletor.Redex.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Equipe, EquipeViewModel>();
            CreateMap<Conferente, ConferenteViewModel>();
            CreateMap<Operacao, OperacaoViewModel>();
            CreateMap<Talie, TalieViewModel>();
        }
    }
}
