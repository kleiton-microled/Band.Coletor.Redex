using AutoMapper;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Interfaces.Business;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Classes
{
    public class ArmazenBusiness :  IArmazemBusiness
    {
        private readonly IMapper _mapper;
        private readonly IArmazenRepositorio _repositorio;
        public ArmazenBusiness(IMapper mapper, IArmazenRepositorio repositorio)
        {
            _mapper = mapper;
            _repositorio = repositorio;
        }
        public async Task<IEnumerable<ArmazemViewModel>> ListAll()
        {
            string command = @"SELECT tai.AUTONUM, 
	                               tai.DESCR as DISPLAY 
	                            FROM SGIPA.dbo.TB_ARMAZENS_IPA tai 
	                            WHERE tai.DT_SAIDA is null and tai.FLAG_HISTORICO = 0 
	                            AND tai.PATIO = 3";

            var data = await _repositorio.ListAll(command);

            return _mapper.Map<IEnumerable<ArmazemViewModel>>(data);
        }
    }
}
