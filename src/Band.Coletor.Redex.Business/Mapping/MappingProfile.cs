using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Business.Models.Entities;
using Band.Coletor.Redex.Business.Models.ModelsLogic;
using System.Collections.Generic;

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

            CreateMap<ConteinerBL, ConteinerViewModel>();
            CreateMap<CargaConteiner, CargaConteinerViewModel>();

            CreateMap<TalieDescargaDTO, TalieDescargaViewModel>();
            
        }
    }
}
