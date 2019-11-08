using System;

namespace Shared.Library.Extensions
{
    public static class ObjectExtensions
    {
        public static bool TryParse(this string value, out int result)
        {
            return int.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out bool result)
        {
            return bool.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out double result)
        {
            return double.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out float result)
        {
            return float.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out DateTime result)
        {
            return DateTime.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out Guid result)
        {
            return Guid.TryParse(value, out result);
        }

        public static bool TryParse(this string value, out DateTimeOffset result)
        {
            return DateTimeOffset.TryParse(value, out result);
        }
    }
}
