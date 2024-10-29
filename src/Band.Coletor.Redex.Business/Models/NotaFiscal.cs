using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class NotaFiscal : Entidade<NotaFiscal>
    {
        public string Descricao { get; set; }

        public override void Validar()
        {
            throw new NotImplementedException();
        }
    }
}