using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Client.Core.Util
{
    public class Generator
    {
        public static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[new Random(Guid.NewGuid().GetHashCode()).Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
