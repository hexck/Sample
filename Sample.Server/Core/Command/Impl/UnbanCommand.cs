using Sample.Server.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class UnbanCommand : Command
    {
        public override string Name => "unban";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Write(0, "unban <ip>");
                return;
            }

            new MongoCrud().RevokeBan(args[1]);

            Write(0, $"Unbanned: {args[1]}");
        }
    }
}
