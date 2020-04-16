using Sample.Server.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class DeleteCommand : Command
    {
        public override string Name => "delete";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Write(0, "delete <hwid|key>");
                return;
            }

            bool b = new MongoCrud().DeleteLicenseByHwid(args[1]) || new MongoCrud().DeleteLicenseByKey(args[1]);

            Write(0, $"Deleted: {b.ToString()}");
        }
    }
}
