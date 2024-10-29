using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Site.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string valor)
        {
            return (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor));
        }

        public static int ToInt(this string valor)
        {
            int resultado = 0;

            if (Int32.TryParse(valor, out resultado))
                return resultado;

            return 0;
        }

        public static int? ToIntOrNull(this string valor)
        {
            int resultado = 0;

            if (Int32.TryParse(valor, out resultado))
                return resultado;

            return null;
        }

        public static decimal? ToDecimalOrNull(this string valor)
        {
            decimal resultado = 0;

            if (Decimal.TryParse(valor, out resultado))
                return resultado;

            return null;
        }

        public static decimal ToDecimal(this string valor)
        {
            decimal resultado = 0;

            if (Decimal.TryParse(valor, out resultado))
                return resultado;

            return 0;
        }

        public static double ToDouble(this string valor)
        {
            double resultado = 0;

            if (Double.TryParse(valor, out resultado))
                return resultado;

            return 0;
        }

        public static DateTime ToDateTime(this string valor)
        {
            DateTime resultado;

            if (DateTime.TryParse(valor, out resultado))
                return resultado;

            return SqlDateTime.MinValue.Value;
        }

        public static DateTime? ToNullDateTime(this string valor)
        {
            DateTime resultado;

            if (DateTime.TryParse(valor, out resultado))
                return resultado;

            return null;
        }

        public static string FormataCasasDecimais(this decimal valor, int casas)
        {
            return string.Format("{0:N" + casas + "}", valor);
        }

        public static string RemoverCaracteresEspeciais(this string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            valor = valor.Replace("&", "&amp;");
            valor = valor.Replace("<", "&lt;");
            valor = valor.Replace(">", "&gt;");

            return valor;
        }


        
    }
}
