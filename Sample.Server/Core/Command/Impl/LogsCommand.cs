using Sample.Server.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class LogsCommand : Command
    {
        public override string Name => "logs";

        public override void Execute(string[] args)
        {
            if (args.Length < 2 || (args[1].ToLower() != "on" && args[1].ToLower() != "off"))
            {
                Write(0, "logs <on|off>");
                return;
            }

            Write(0, $"Set logs: {args[1].ToLower()}");
            Logger.LogsEnabled = args[1].ToLower() == "on";
        }
    }
}
