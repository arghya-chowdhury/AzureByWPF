using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace FunctionChain
{
    public static class QueueTriggerFunction
    {
        [FunctionName("QueueTriggerFunction")]
        public static async Task Run(
            [QueueTrigger("myqueue-items")]User myQueueItem,
            IBinder binder,
            ILogger log)
        {
            log.LogInformation("Queue Trigger Invoked");
            var myBlob = binder.Bind<CloudBlockBlob>(new BlobAttribute($"myblob-items/{myQueueItem.UserName}"));
            var user = JsonConvert.SerializeObject(myQueueItem);
            await myBlob.UploadTextAsync(user);
        }
    }
}
