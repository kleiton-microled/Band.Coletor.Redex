using System;

namespace Band.Coletor.Redex.Business.Extensions
{
    public static class BooleanExtensions
    {
        public static int ToInt(this bool valor)
            => Convert.ToInt32(valor);
    }
}