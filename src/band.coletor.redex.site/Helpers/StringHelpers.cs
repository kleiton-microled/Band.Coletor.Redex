using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Site.Helpers
{
    public class StringHelpers
    {
        public static bool IsDate(string valor)
        {
            DateTime data;

            if (DateTime.TryParse(valor, out data))
                return true;

            return false;
        }

        public static bool IsDate(DateTime? valor)
        {
            DateTime data;

            if (valor == null)
                return false;

            if (DateTime.TryParse(valor.ToString(), out data))
            {
                return data.Year > 2000;
            }

            return false;
        }

        public static bool IsNumero(string texto)
        {
            decimal valor;

            if (Decimal.TryParse(texto, out valor))
                return true;

            return false;
        }
    }
}
