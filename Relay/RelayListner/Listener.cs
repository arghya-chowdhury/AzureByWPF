using Microsoft.Azure.Relay;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace RelayListener
{
public class Listener
{
    public async Task Listen()
    {
        var sas = ConfigurationManager.AppSettings["HybridConnectionString"];
        var sasKeyName = ConfigurationManager.AppSettings["SharedAccessKeyName"];
        var sasKey = ConfigurationManager.AppSettings["SharedAccessKey"];

        HybridConnectionListener listener = null;

        try
        {
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sasKeyName, sasKey);

            listener = new HybridConnectionListener(new Uri(sas), tokenProvider);
            listener.Connecting += (o, e) => { Console.WriteLine("Connecting"); };
            listener.Offline += (o, e) => { Console.WriteLine("Offline"); };
            listener.Online += (o, e) => { Console.WriteLine("Online"); };

            //Opens the Microsoft.Azure.Relay.HybridConnectionListener and registers it as a listener in ServiceBus.
            await listener.OpenAsync();
            Console.WriteLine("Server listening");

            //Accepts a new HybridConnection which was initiated by a remote client and returns the Stream.
            while (true)
            {
                try
                {
                    Console.WriteLine("Waiting for new Connection");
                    using (var stream = await listener.AcceptConnectionAsync())
                    using (var reader = new StreamReader(stream))
                    {
                        while (true)
                        {
                            var line = await reader.ReadLineAsync();
                            Console.WriteLine(line);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await listener.CloseAsync();
            Console.WriteLine("Server stop listening");
        }
    }
}
}
