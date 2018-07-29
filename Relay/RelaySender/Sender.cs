using Microsoft.Azure.Relay;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace RelaySender
{
public class Sender
{
    public async Task Send()
    {
        var sas = ConfigurationManager.AppSettings["HybridConnectionString"];
        var sasKeyName = ConfigurationManager.AppSettings["SharedAccessKeyName"];
        var sasKey = ConfigurationManager.AppSettings["SharedAccessKey"];

        HybridConnectionClient client = null;

        try
        {
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sasKeyName, sasKey);
            client = new HybridConnectionClient(new Uri(sas), tokenProvider);

            Console.WriteLine("Enter Message To Send, Type :q to Exit");

            using (var stream = await client.CreateConnectionAsync())
            using (var writer = new StreamWriter(stream) { AutoFlush = true })
            {
                while (true)
                {
                    var line = Console.ReadLine();
                    if (line == ":q")
                    {
                        break;
                    }
                    writer.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Console.WriteLine("Client stop sending");
        }
    }
}
}
