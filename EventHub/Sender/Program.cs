using Microsoft.Azure.EventHubs;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start send events..");
            Console.ReadLine();

            var connectionString = ConfigurationManager.AppSettings["Microsoft.EventHub.ConnectionString"];
            var eventHubName = ConfigurationManager.AppSettings["Microsoft.EventHub.Name"];

            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
            {
                EntityPath = eventHubName
            };
            var client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            SendMessages(client).GetAwaiter().GetResult();

            Console.WriteLine("Press any key to continue..");
            Console.ReadLine();
        }

        private static async Task SendMessages(EventHubClient client)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < 200; i++)
            {
                var ev = $"Sending {i}th Event";
                Console.WriteLine(ev);
                await client.SendAsync(new EventData(Encoding.UTF8.GetBytes(ev)));
            }

            stopWatch.Stop();
            Console.WriteLine($"Elapsed Time: {stopWatch.Elapsed.Seconds} secs");
            await client.CloseAsync();
        }
    }
}
