using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class QuitCommand : Command
    {
        public override string Name => "quit";

        public override void Execute(string[] args)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
