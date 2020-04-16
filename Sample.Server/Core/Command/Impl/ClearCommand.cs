using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class ClearCommand : Command
    {
        public override string Name => "clear";

        public override void Execute(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Sample Server");
        }
    }
}
