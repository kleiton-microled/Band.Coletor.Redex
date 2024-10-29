using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Operacao : Entidade<Operacao>
    {
        public string Sigla { get; set; }

        public string Descricao { get; set; }

        public override void Validar()
        {
            throw new NotImplementedException();
        }
    }
}