using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace StorageManagement
{
    public class QueueManager
    {
        public const int VisibilityTimeOutInSec = 10;

        CloudQueueClient client;

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["QuickStartStorage"].ConnectionString;
                var account = CloudStorageAccount.Parse(connectionString);
                client = account.CreateCloudQueueClient();
            });
        }

        public async Task EnqueueAsync(string message)
        {
            var queue = client.GetQueueReference("quickstart");

            //If Exists Throws Exception, Hence Check Is Required
            if (!queue.Exists())
            {
                queue.CreateIfNotExists();
            }

            var msg = new CloudQueueMessage(message);
            await queue.AddMessageAsync(msg);

            // Make it invisible for another 10 seconds.
            await queue.UpdateMessageAsync(msg,
                TimeSpan.FromSeconds(VisibilityTimeOutInSec),
                MessageUpdateFields.Visibility);
        }

        public async Task DequeueAsync()
        {
            var queue = GetStudentQueueReference();

            var msg = await queue.GetMessageAsync();
            await queue.DeleteMessageAsync(msg.Id, msg.PopReceipt);
        }

        public IList<string> PeekQueueMessages()
        {
            var queue = GetStudentQueueReference();
            return queue.PeekMessages(32).Select(m => m.AsString).ToList();
        }

        public async Task ClearMessageQueueAsync()
        {
            var queue = GetStudentQueueReference();
            queue.FetchAttributes();

            while (queue.ApproximateMessageCount > 0)
            {
                await queue.ClearAsync();
                queue.FetchAttributes();
            }
        }

        private CloudQueue GetStudentQueueReference()
        {
            var queue = client.GetQueueReference("quickstart");
            if (!queue.Exists())
            {
                queue.CreateIfNotExists();
            }
            return queue;
        }
    }
}
