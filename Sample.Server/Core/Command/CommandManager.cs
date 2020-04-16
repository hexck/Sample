using Sample.Server.Core.Command.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Server.Core.Command
{
    public class CommandManager
    {
        public List<Command> Commands { get; set; }

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

        public void Start()
        {
            Console.WriteLine("Sample Server");
            Console.WriteLine();
            while (true)
            {
                try
                {
                    string[] args = Console.ReadLine().Split(' ');

                    Commands.FirstOrDefault(cmd => cmd.Name == args[0].ToLower())?.Execute(args);
                    Console.WriteLine();

                } catch {     }
            }
        }
    }
}
