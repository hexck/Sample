using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Command.Impl
{
    public class AccessCommand : Command
    {
        public override string Name => "access";

        public override void Execute(string[] args)
        {
            if(args.Length < 2 || (args[1].ToLower() != "allow" && args[1].ToLower() != "deny"))
            {
                Write(0, "access <allow|deny>");
                return;
            }

            Write(0, $"Set access: {args[1].ToLower()}");
            Engine.Instance.Server.Pause = args[1].ToLower() == "deny";
        }
    }
}
