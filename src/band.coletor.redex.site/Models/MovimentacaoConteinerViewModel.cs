using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Site.Models
{
    public class MovimentacaoConteinerViewModel
    {
        [Display(Name = "Contêiner")]
        public string Conteiner { get; set; }

        public decimal Tamanho { get; set; }

        public string Tipo { get; set; }

        public string EF { get; set; }

        public string GWT { get; set; }

        public string Temperatura { get; set; }

        public string IMO { get; set; }

        public DateTime Entrada { get; set; }

        public string Categoria { get; set; }

        public string Navio { get; set; }

        public string Posicionar { get; set; }

        public string LocalAtracacao { get; set; }

        public string Regime { get; set; }

        public string Camera { get; set; }

        public string Local { get; set; }


        public int MotivoPosicaoId { get; set; }
        
        public int EmpilhadeiraId { get; set; }
        
        public int VeiculoId { get; set; }
        //public List<MotivoPosicao> MotivosPosicao { get; set; }
        //public List<Empilhadeira> Empilhadeiras { get; set; }
        //public List<Veiculo> Veiculos { get; set; }
    }
}
