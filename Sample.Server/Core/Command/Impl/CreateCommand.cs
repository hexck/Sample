using Sample.Server.Core.Database;
using Sample.Server.Core.Database.Models;
using Sample.Server.Core.Util;
using System;

namespace Sample.Server.Core.Command.Impl
{
    public class CreateCommand : Command
    {
        public override string Name => "create";

        public override void Execute(string[] args)
        {
            if (args.Length >= 2 && !args[1].IsInt())
            {
                Write(0, "create <days>");
                return;
            }
            
            var key = Generator.GenerateLicense();
            new MongoCrud().InsertRecord("Licenses", new License { Id = Guid.NewGuid(), Key = key, Hwid = "", Issued = DateTime.Now, ExpireAfterDays = args.Length >= 2 ? int.Parse(args[1]):10000});

            Console.WriteLine($"Created license: {key}");
        }
    }
}
