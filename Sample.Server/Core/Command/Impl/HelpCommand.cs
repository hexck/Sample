using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class HelpCommand : Command
    {
        public override string Name => "help";

        public override void Execute(string[] args)
        {
            Write(0, "- access <allow|deny>");
            Write(0, "- logs <on|off>");
            Write(0, "- ban <ip> <days>");
            Write(0, "- unban <ip>");
            Write(0, "- delete <hwid|key>");
            Write(0, "- create");
            Write(0, "- quit");
        }
    }
}
