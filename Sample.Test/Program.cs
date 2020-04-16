using System;
using Sample.Client.Core;

namespace Sample.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sample = new SampleClient("127.0.0.1", 3202).Connect();
            if (!sample.Authenticate())
                return;

            while (!sample.Whitelisted())
            {
                Console.Write("You are not whitelisted, please enter serial: ");
                var res = sample.Register(Console.ReadLine());
                Console.WriteLine(res == RequestState.Success ? "Key was valid" : "Key was not valid");
            }

            sample.Close();
            Console.WriteLine("Gg! you are verified!");

            Console.ReadKey();
        }
    }
}