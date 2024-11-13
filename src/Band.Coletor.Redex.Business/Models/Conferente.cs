using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Conferente : Entidade<Conferente>
    {
        public string Descricao { get; set; }

        public override void Validar()
        {
            throw new NotImplementedException();
        }
    }
}