using System;
using System.Globalization;

namespace SeeBee.FxUtils
{
    public static class StringUtils
    {
        public static int StringToInt(string stringToConvert, bool throwOnFailure = false)
        {
            int convertedValue;
            if (int.TryParse(stringToConvert, out convertedValue))
            {
                return convertedValue;
            }
            if (throwOnFailure)
            {
                throw new FormatException(string.Format("Unable to convert {0} to int.", stringToConvert));
            }
            return 0;
        }

        public static long StringToLong(string stringToConvert, bool throwOnFailure = false)
        {
            long convertedValue;
            if (long.TryParse(stringToConvert, out convertedValue))
            {
                return convertedValue;
            }
            if (throwOnFailure)
            {
                throw new FormatException(string.Format("Unable to convert {0} to long.", stringToConvert));
            }
            return 0;
        }

        public static bool StringToBoolean(string stringToConvert)
        {
            if (stringToConvert.Equals("0"))
            {
                return false;
            }
            return true;
        }

        public static long HexStringToLong(string hexString, bool throwOnFailure = false)
        {
            if (hexString.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
            {
                hexString = hexString.Substring(2);
            }
            long value;
            if (!long.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
            {
                if (throwOnFailure)
                {
                    throw new FormatException(string.Format("Unable to convert {0} to long.", hexString));
                }
                value = 0;
            }
            return value;
        }

        public static string HTMLUnEscape(string escapedString)
        {
            return System.Net.WebUtility.HtmlDecode(escapedString);
        }
    }
}
