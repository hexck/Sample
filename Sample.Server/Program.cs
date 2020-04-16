using Sample.Server.Core.Command;
using System;
using System.Threading;

namespace Sample.System
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Thread(() => new Server.Core.Engine()) { IsBackground = true }.Start();

            var commandManager = new CommandManager();
            commandManager.Start();
        }
    }
}
