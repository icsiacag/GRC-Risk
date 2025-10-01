using System;

namespace GRC.SharedKernel
{
    public static class Guard
    {
        public static void AgainstNull(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void AgainstEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{parameterName} cannot be empty", parameterName);
        }
    }
}