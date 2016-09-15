using System;
using System.Globalization;

namespace SeeBee.FxUtils
{
    public static class StringUtils
    {
        public static long HexStringToLong(string hexString)
        {
            if (hexString.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
            {
                hexString = hexString.Substring(2);
            }
            long value;
            if (!long.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
            {
                value = 0;
            }
            return value;
        }
    }
}
