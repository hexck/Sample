using Sample.Server.Core.Database;
using Sample.Server.Core.Database.Models;
using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class BanCommand : Command
    {
        public override string Name => "ban";

        public override void Execute(string[] args)
        {
            if (args.Length < 3 || !args[2].IsInt())
            {
                Write(0, "ban <ip> <days>");
                return;
            }
            new MongoCrud().Ban(args[1], int.Parse(args[2]));

            Write(0, $"Banned: {args[1]} for {args[2]} days");
        }
    }
}
