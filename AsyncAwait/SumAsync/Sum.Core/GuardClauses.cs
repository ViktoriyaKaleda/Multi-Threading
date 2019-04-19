using System;

namespace Sum.Core
{
    public static class GuardClauses
    {
        public static void IsNonNegative(long number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Non negative number expected.", nameof(number));
            }
        }
    }
}
