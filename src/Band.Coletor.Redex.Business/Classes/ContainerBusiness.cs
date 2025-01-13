using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Classes
{
    public class ContainerBusiness : IConteinerBusiness
    {
        private readonly IMapper _mapper;
        private readonly IConteinerRepositorio _repositorio;
        public ContainerBusiness(IMapper mapper, IConteinerRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }

       
        public ServiceResult<IEnumerable<ConteinerViewModel>> ObterContainersMarcantes(string lote, int patio)
        {
            var _serviceResult = new ServiceResult<IEnumerable<ConteinerViewModel>>();

            try
            {
                var data = _repositorio.ObterContainersMarcantes(lote, patio);

                _serviceResult.Result = _mapper.Map<IEnumerable<ConteinerViewModel>>(data.Result);
            }
            catch (System.Exception ex)
            {

                throw;
            }
            

            return _serviceResult;
        }

        public ServiceResult<CargaConteinerViewModel> CarregarDadosContainer(string lote)
        {
            var _serviceResult = new ServiceResult<CargaConteinerViewModel>();

            try
            {
                var data = _repositorio.CarregarDadosContainer(lote);

                _serviceResult.Result = _mapper.Map<CargaConteinerViewModel>(data.Result);
            }
            catch (System.Exception ex)
            {
                _serviceResult.Error = ex.Message;
                return _serviceResult;
            }

            return _serviceResult;
        }
    }
}
