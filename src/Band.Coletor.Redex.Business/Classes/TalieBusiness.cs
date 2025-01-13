using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Models.Entities;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Business.Classes.ServiceResult;

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

        public async Task<ServiceResult<int>> Save(TalieViewModel view)
        {

            var entity = TalieEntity.InsertCommand(view.CodigoTalie, 
                                                   Convert.ToDateTime(view.Inicio), 
                                                   view.Conferente, 
                                                   view.Equipe,
                                                   view.CodigoBooking,
                                                   view.Operacao,
                                                   view.Placa, 
                                                   view.CodigoGate,
                                                   view.CodigoRegistro,
                                                   view.Observacao);

            var _serviceResult =  await _repositorio.GravarTalieAsync(entity);
           
            return _serviceResult;
            
        }

        public async Task<TalieViewModel> ObterDadosTaliePorRegistro(int registro)
        {
            var data = await _repositorio.ObterDadosTaliePorRegistroAsync(registro);

            try
            {
                if (data != null)
                {
                    var talie = TalieViewModel.CreateNew(true,
                                                         registro,
                                                         data.AUTONUM_TALIE,
                                                         data.AUTONUM_BOO,
                                                         data.FORMA_OPERACAO,
                                                         data.CONFERENTE,
                                                         0,
                                                         data.EQUIPE,
                                                         data.PLACA,
                                                         data.REFERENCE,
                                                         data.AUTONUM_GATE,
                                                         registro,
                                                         data.FANTASIA,
                                                         data.INICIO.ToString(),
                                                         data.TERMINO.ToString(),
                                                         data.FLAG_FECHADO ? "TALIE FECHADO" : string.Empty,
                                                         data.OBS);


                    var gate = await _repositorio.ObterRegistrosGate(registro);
                    if (gate != null)
                    {
                        talie.Reserva = gate.Reserva;
                        talie.CodigoBooking = gate.CodigoBooking;
                        talie.CodigoGate = gate.CodigoGate;
                        talie.Cliente = gate.Cliente;
                        talie.CodigoRegistro = registro;
                    }

                    return talie;
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
            return null;
        }

        public async Task<ServiceResult<bool>> Update(TalieViewModel view)
        {
            var talie = TalieEntity.CreateNew(Convert.ToDateTime(view.Inicio), 
                                             view.Conferente, 
                                             view.Equipe, 
                                             view.Operacao, 
                                             view.Observacao, 
                                             1, 
                                             view.CodigoTalie);

            var _serviceResult =  await _repositorio.Update(talie);
            
            return _serviceResult;

        }

        public ServiceResult<int> ObterNotaFiscal(string numeroNotaFiscal, string codigoBooking, string codigoRegistro)
        {
            return _repositorio.ObterIdNotaFiscal(numeroNotaFiscal, codigoBooking, codigoRegistro);
        }

        public async Task<ServiceResult<TalieItemViewModel>> ObterItensNotaFiscal(string numeroNotaFiscal, string codigoRegistro)
        {
            var data = await _repositorio.ObterItensNotaFiscal(numeroNotaFiscal, codigoRegistro);
            if (data.Status)
            {
                return new ServiceResult<TalieItemViewModel>();
            }

            return new ServiceResult<TalieItemViewModel>();
        }
    }
}
