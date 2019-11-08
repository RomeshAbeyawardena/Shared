using System.Text;

namespace Shared.Library.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string value, Encoding encoding)
        {
            return encoding.GetBytes(value);
        }
    }
}
