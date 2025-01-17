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
using Band.Coletor.Redex.Application.ViewModel.View;
using Band.Coletor.Redex.Business.Mapping;

namespace Band.Coletor.Redex.Business.Classes
{
    public class RegistroBusiness : IRegistroBusiness
    {
        private readonly IMapper _mapper;
        private readonly IRegistroRepositorio _repositorio;
        public RegistroBusiness(IMapper mapper, IRegistroRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }

        //public async Task<ServiceResult<int>> Save(TalieViewModel view)
        //{

        //    var entity = TalieEntity.InsertCommand(view.CodigoTalie, 
        //                                           Convert.ToDateTime(view.Inicio), 
        //                                           view.Conferente, 
        //                                           view.Equipe,
        //                                           view.CodigoBooking,
        //                                           view.Operacao,
        //                                           view.Placa, 
        //                                           view.CodigoGate,
        //                                           view.CodigoRegistro,
        //                                           view.Observacao);

        //    var _serviceResult =  await _repositorio.GravarTalieAsync(entity);
           
        //    return _serviceResult;
            
        //}

        public async Task<RegistroViewModel> CarregarRegistro(int codigoRegistro)
        {
            RegistroViewModel registroViewModel = null;
            var data = await _repositorio.CarregarRegistro(codigoRegistro);

            try
            {
                if (data != null)
                {
                    registroViewModel = Map.MapToViewModel(data);
                }

                return registroViewModel;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public ServiceResult<int> SaveOrUpdate(RegistroViewModel registro)
        {
            ServiceResult<int> result = new ServiceResult<int>();
            if (registro == null || registro.CodigoRegistro <= 0)
            {
                throw new ArgumentException("Registro inválido.");
            }

            // Chama o repositório para salvar ou atualizar
            result.Result = _repositorio.SaveOrUpdate(registro);
            return result;
        }

        public void GeraDescargaAutomatica(int codigoRegistro, int autonumTalie)
        {
            _repositorio.GeraDescargaAutomatica(codigoRegistro, autonumTalie);
        }

        public int ValidarDanfe(int codigoRegistro)
        {
            return _repositorio.ValidarDanfe(codigoRegistro);
        }

        public bool ValidarNotaCadastrada(int lote)
        {
            return _repositorio.ValidarNotaCadastrada(lote);
        }

        public void GravarObservacao(string observacao, long talie)
        {
            _repositorio.GravarObservacao(observacao, talie);
        }


        //public async Task<ServiceResult<bool>> Update(TalieViewModel view)
        //{
        //    var talie = TalieEntity.CreateNew(Convert.ToDateTime(view.Inicio), 
        //                                     view.Conferente, 
        //                                     view.Equipe, 
        //                                     view.Operacao, 
        //                                     view.Observacao, 
        //                                     1, 
        //                                     view.CodigoTalie);

        //    var _serviceResult =  await _repositorio.Update(talie);

        //    return _serviceResult;

        //}

        //public ServiceResult<int> ObterNotaFiscal(string numeroNotaFiscal, string codigoBooking, string codigoRegistro)
        //{
        //    return _repositorio.ObterIdNotaFiscal(numeroNotaFiscal, codigoBooking, codigoRegistro);
        //}

        //public async Task<ServiceResult<TalieItemViewModel>> ObterItensNotaFiscal(string numeroNotaFiscal, string codigoRegistro)
        //{
        //    var data = await _repositorio.ObterItensNotaFiscal(numeroNotaFiscal, codigoRegistro);
        //    if (data.Status)
        //    {
        //        return new ServiceResult<TalieItemViewModel>();
        //    }

        //    return new ServiceResult<TalieItemViewModel>();
        //}
    }
}
