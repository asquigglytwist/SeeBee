using System;
using System.Globalization;

namespace SeeBee.FxUtils.Utils
{
    public static class StringUtils
    {
        public static int StringToInt(this string stringToConvert, bool throwOnFailure = false)
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

        public static long StringToLong(this string stringToConvert, bool throwOnFailure = false)
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

        public static bool StringToBoolean(this string stringToConvert)
        {
            if (stringToConvert.Equals("0"))
            {
                return false;
            }
            return true;
        }

        public static long HexStringToLong(this string hexString, bool throwOnFailure = false)
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

        public static string HTMLUnEscape(this string escapedString)
        {
            return System.Net.WebUtility.HtmlDecode(escapedString);
        }

        public static string[] CSVSplit(this string source, bool ignoreEmptyStrings = true)
        {
            StringSplitOptions splitOptions = ignoreEmptyStrings ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return source.Split(new char[] { ',', ' ', '\r', '\n', '\t' }, splitOptions);
        }

        public static T StringToEnum<T>(this string text) where T : struct, IConvertible
        {
            // [BIB]:  https://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum?rq=1
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if (Enum.TryParse(text, out T result))
            {
                return result;
            }
            return default(T);
        }
    }
}
