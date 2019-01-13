using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionChain
{
    public static class HttpTriggerFunction
    {
        [FunctionName("HttpTriggerFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [Queue("myqueue-items")]IAsyncCollector<User> queue)
        {
            log.LogInformation("Request Received");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<User>(requestBody);
            await queue.AddAsync(user);
            log.LogInformation("Request Processed");

            return user == null ? (IActionResult)new BadRequestObjectResult("User Name hasnot been passed") : new OkObjectResult($"Hello {user.UserName}");
        }
    }
}
