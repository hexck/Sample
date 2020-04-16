using Sample.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Test
{
    class Program
    {
        static void Main(string[] args)
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
            
            Console.WriteLine("Gg! you are verified!");

            Console.ReadKey();
        }
    }
}
