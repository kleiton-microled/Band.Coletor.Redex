﻿using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Equipe : Entidade<Equipe>
    {
        public string Descricao { get; set; }

        public override void Validar()
        {
            throw new NotImplementedException();
        }
    }
}