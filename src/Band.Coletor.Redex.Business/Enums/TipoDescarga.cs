using System.ComponentModel.DataAnnotations;

namespace Band.Coletor.Redex.Business.Enums
{
    public enum TipoDescarga
    {
        [Display(Name = "Descarga com Unitização")]
        UNITIZACAO,

        [Display(Name = "Descarga para Armazém")]
        ARMAZEM
    }
}