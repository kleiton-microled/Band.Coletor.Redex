using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business
{
    public class DescargaExportacaoBusiness : IDescargaExportacaoBusiness
    {
        private readonly IDescargaExportacaoRepositorio _repositorio;
        private readonly IMapper _mapper;
        public DescargaExportacaoBusiness(IDescargaExportacaoRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
       
    }
}
