using System;

namespace Band.Coletor.Redex.Business.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToNumero(this decimal valor)
        {
            if (Decimal.TryParse(valor.ToString(), out decimal resultado))
                return string.Format("{0:N2}", resultado);

            return string.Empty;
        }
    }
}