using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Models.Entities;
using Band.Coletor.Redex.Business.Models;

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

        public int Gravar(TalieViewModel view)
        {
            var entity = Talie.InsertCommand(view.CodigoTalie, 
                                                   Convert.ToDateTime(view.Inicio), 
                                                   view.Conferente, 
                                                   view.Equipe,
                                                   view.CodigoBooking,
                                                   view.Operacao,
                                                   view.Placa, 
                                                   view.CodigoGate,
                                                   view.Registro,
                                                   view.Observacao);

            return _repositorio.CadastrarTalie(entity);
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

        public async Task<bool> UpdateTalie(TalieViewModel view)
        {
            var talie = TalieEntity.CreateNew(Convert.ToDateTime(view.Inicio), view.Conferente, view.Equipe, view.Operacao, view.Observacao, 1, view.CodigoTalie);
            
            return await _repositorio.Update(talie);
        }
    }
}
