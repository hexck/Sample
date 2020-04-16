using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Server.Core.Command.Impl;

namespace Sample.Server.Core.Command
{
    public class CommandManager
    {
        public CommandManager()
        {
            Commands = new List<Command>
            {
                new HelpCommand(),
                new AccessCommand(),
                new LogsCommand(),
                new BanCommand(),
                new UnbanCommand(),
                new CreateCommand(),
                new DeleteCommand(),
                new QuitCommand(),
                new ClearCommand()
            };
        }

        private List<Command> Commands { get; }

        public void Start()
        {
            Console.WriteLine("Sample Server");
            Console.WriteLine();
            while (true)
                try
                {
                    var args = Console.ReadLine()?.Split(' ') ?? new string[] { };
                    if (args.Length == 0)
                        continue;

                    Commands.FirstOrDefault(cmd => cmd.Name == args[0].ToLower())?.Execute(args);
                    Console.WriteLine();
                }
                catch
                {
                }
        }
    }
}