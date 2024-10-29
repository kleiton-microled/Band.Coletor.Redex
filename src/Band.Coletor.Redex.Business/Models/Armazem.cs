using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Armazem : Entidade<Armazem>
    {
        public int PatioId { get; set; }

        public string Descricao { get; set; }

        public string Quadra { get; set; }

        public string Rua { get; set; }

        public string Fiada { get; set; }

        public string Altura { get; set; }

        public bool Valida { get; set; }

        public override void Validar()
        {
            throw new NotImplementedException();
        }
    }
}