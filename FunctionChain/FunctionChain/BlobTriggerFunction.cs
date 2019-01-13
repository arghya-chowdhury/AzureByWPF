using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionChain
{
    public static class BlobTriggerFunction
    {
        private static Stream myBlob;

        [FunctionName("BlobTriggerFunction")]
        public static async Task Run(
            [BlobTrigger("myblob-items/{name}")]string myBlob, string name,
            [Table("mytableitems")] IAsyncCollector<UserTableEntity> collector,            
            ILogger log)
        {
            var user = JsonConvert.DeserializeObject<User>(myBlob);
            log.LogInformation($"Blob Trigger Invoked");

            await collector.AddAsync(new UserTableEntity
            {
                UserName = user.UserName,
                PartitionKey = "workitems",
                RowKey = System.Guid.NewGuid().ToString(),
            });
        }
    }
}
