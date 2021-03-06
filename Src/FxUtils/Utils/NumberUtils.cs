﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SeeBee.FxUtils.Utils
{
    public static class NumberUtils
    {
        private static readonly Random prng = new Random();

        public static string LongToHexString(long longValue)
        {
            return longValue.ToString("X");
        }

        // [BIB]:  http://stackoverflow.com/questions/128618/c-file-size-format-provider
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        public static string FormatNumberAsFileSize(long fileSize)
        {
            StringBuilder buffer = new StringBuilder(20);
            StrFormatByteSize(fileSize, buffer, buffer.Capacity);
            return buffer.ToString();
        }

        public static int GetRandomInt()
        {
            return prng.Next();
        }
    }
}
