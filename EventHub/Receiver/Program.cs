using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start receive events..");
            Console.ReadLine();

            RegisterForReceiveMessage().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to continue..");
            Console.ReadLine();
        }

        private static async Task RegisterForReceiveMessage()
        {
            var connectionString = ConfigurationManager.AppSettings["Microsoft.EventHub.ConnectionString"];
            var eventHubName = ConfigurationManager.AppSettings["Microsoft.EventHub.Name"];

            var storageConnectionString = ConfigurationManager.AppSettings["Microsoft.Storage.ConnectionString"];
            var storageContainerName = ConfigurationManager.AppSettings["Microsoft.Storage.ContainerName"];

            var host = new EventProcessorHost(eventHubName, PartitionReceiver.DefaultConsumerGroupName, connectionString, storageConnectionString, storageContainerName);
            await host.RegisterEventProcessorAsync<EventProcessor>();
        }
    }
}
