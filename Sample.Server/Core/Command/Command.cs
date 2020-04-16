using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract void Execute(string[] args);

        protected void Write(int c, string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ".Repeat(c));
            Console.WriteLine(text);
        }
    }
}
