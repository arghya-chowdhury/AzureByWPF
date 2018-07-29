using System;

namespace RelaySender
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = new Sender();
            sender.Send().Wait();

            Console.ReadLine();
        }
    }
}
