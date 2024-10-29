using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Band.Coletor.Redex.Site.Models
{
    public class TalieMarcantesViewModel
    {
        public int TalieId { get; set; }

        public int PatioId { get; set; }

        [Display(Name = "Marcante")]
        public string CodigoMarcante { get; set; }

        public int Quantidade { get; set; }

        public int QuantidadeDescarregada { get; set; }

        public int QuantidadeAssociada { get; set; }

        [Display(Name = "Armazém")]
        public int ArmazemId { get; set; }

        public string Armazem { get; set; }

        public string Quadra { get; set; }

        //public string Rua { get; set; }

        //public string Fiada { get; set; }

        //public string Altura { get; set; }

        public List<Armazem> Armazens { get; set; }

        public List<Marcante> Marcantes { get; set; }
    }
}