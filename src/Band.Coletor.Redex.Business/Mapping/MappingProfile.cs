using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Entity;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Business.Models.Entities;
using Band.Coletor.Redex.Business.Models.ModelsLogic;
using System.Collections.Generic;
using System.Linq;
using Talie = Band.Coletor.Redex.Business.Entity.Talie;
using TalieItem = Band.Coletor.Redex.Business.Entity.TalieItem;

namespace Band.Coletor.Redex.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Equipe, EquipeViewModel>();
            CreateMap<Conferente, ConferenteViewModel>();
            CreateMap<ArmazemModel, ArmazemViewModel>();
            CreateMap<Operacao, OperacaoViewModel>();
            CreateMap<TalieEntity, TalieViewModel>();
            CreateMap<RegistroDTO, RegistroViewModel>();

            // Mapeamento de Talie -> TalieViewModel
            CreateMap<Talie, TalieViewModel>()
                .ForMember(dest => dest.TalieItem, opt => opt.MapFrom(src => src.TalieItem.FirstOrDefault()));

            // Mapeamento de TalieItem -> TalieItemViewModel
            CreateMap<TalieItem, TalieItemViewModel>();

            // Outros mapeamentos
            CreateMap<RegistroDTO, RegistroViewModel>()
                .ForMember(dest => dest.Talie, opt => opt.MapFrom(src => src.Talie));

            CreateMap<ConteinerBL, ConteinerViewModel>();
            CreateMap<CargaConteiner, CargaConteinerViewModel>();

            CreateMap<TalieDescargaDTO, TalieDescargaViewModel>();
            
        }
    }
}
