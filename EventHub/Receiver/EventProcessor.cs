using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace Receiver
{
    class EventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Closed due to {reason} for Partition Id:{context.PartitionId}");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Processor Opened for Partition Id:{context.PartitionId}");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error Encountered for Partition Id:{context.PartitionId}, Error:{error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var m in messages)
            {
                Console.WriteLine($"Message Processed for Partition Id:{context.PartitionId}, MessageBody:{Encoding.UTF8.GetString(m.Body.Array, 0, m.Body.Count)}");
            }

            return context.CheckpointAsync();

            Console.WriteLine("Press any key to continue..");
            Console.ReadLine();
        }
    }
}
