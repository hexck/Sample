using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Util
{
    public class Generator
    {
        public static string Fucked(int length)
        {
            IList<Range> ranges = new[]
            {
                new Range(0x4e00, 0x4f80),
                new Range(0x5000, 0x9fa0),
                new Range(0x3400, 0x4db0),
                new Range(0x30a0, 0x30f0)
            };
            var builder = new StringBuilder(length);
            var random = new Random(Guid.NewGuid().GetHashCode());

            for (var i = 0; i < length; i++)
            {
                var rangeIndex = random.Next(ranges.Count);
                var range = ranges[rangeIndex];
                builder.Append((char)random.Next(range.Begin, range.End));
            }

            return builder.ToString();
        }

        public static string GenerateLicense()
        {
            return $"{RandomString(4)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}".ToUpper();
        }

        private static string RandomString(int length)
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
    public class Range
    {
        public Range(int begin, int end)
        {
            Begin = begin;
            End = end;
        }

        public int Begin { get; set; }

        public int End { get; set; }
    }
}
