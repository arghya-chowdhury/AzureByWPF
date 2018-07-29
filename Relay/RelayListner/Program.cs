using System;

namespace RelayListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var Listener = new Listener();
            Listener.Listen().Wait();

            Console.ReadLine();
        }
    }
}
