using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Util
{
    public static class StringExtensions
    {
        public static string GetString(this byte[] buf)
        {
            return Encoding.UTF8.GetString(buf);
        }
        public static byte[] GetBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static bool IsInt(this string s)
        {
            try
            {
                var x = int.Parse(s);
                return true;
            }
            catch { return false;  }
        }

        public static string Repeat(this string s, int n)
       => new StringBuilder(s.Length * n).Insert(0, s, n).ToString();

        public static bool IsInt(this char c)
        {
            try
            {
                var x = int.Parse(c.ToString());
                return true;
            }
            catch { return false; }
        }

        public static string Reverse(this string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
