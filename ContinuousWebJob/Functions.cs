using System.IO;
using Microsoft.Azure.WebJobs;

namespace QueueTriggerWebJob
{
    public class Functions
{
    // This function will get triggered/executed when a new message is written 
    // on an Azure Queue called queue.
    public static void ProcessQueueMessage([QueueTrigger("student")] string message, TextWriter log)
    {
        log.WriteLine(message);
    }
}
}
