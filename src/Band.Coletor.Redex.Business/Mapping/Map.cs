using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Entity;


namespace Band.Coletor.Redex.Business.Mapping
{
    public class Map
    {
        public static RegistroViewModel MapToViewModel(RegistroDTO registroDTO)
        {
            if (registroDTO == null) return null;

            return new RegistroViewModel
            {
                CodigoRegistro = registroDTO.Id,
                Placa = registroDTO.Placa,
                Reserva = registroDTO.Reserva,
                Cliente = registroDTO.Cliente,

                Talie = MapTalieToViewModel(registroDTO.Talie)
            };
        }

        private static Application.ViewModel.View.TalieViewModel MapTalieToViewModel(Talie talie)
        {
            if (talie == null) return null;

            return new Application.ViewModel.View.TalieViewModel
            {
                Id = talie.Id,
                Inicio = talie.Inicio,
                Termino = talie.Termino,
                Conferente = talie.Conferente,
                Equipe = talie.Equipe,
                Operacao = talie.Operacao,
                Observacao = talie.Observacao,
                TalieItem = MapTalieItemToViewModel(talie.TalieItem)
            };
        }

        private static List<Application.ViewModel.View.TalieItemViewModel> MapTalieItemToViewModel(List<TalieItem> talieItem)
        {
            List<Application.ViewModel.View.TalieItemViewModel> map = new List<Application.ViewModel.View.TalieItemViewModel>();

            if (talieItem == null) return null;
            foreach (var item in talieItem)
            {
                map.Add(new Application.ViewModel.View.TalieItemViewModel()
                {
                    Id = item.Id,
                    NF = item.NF,
                    Embalagem = item.Embalagem,
                    QtdNf = item.QtdNf,
                    QtdDescarga = item.QtdDescarga
                });
            }

            return map;

        }


    }
}
