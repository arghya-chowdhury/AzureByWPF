using Microsoft.Rest;
using System;
using System.Net;

namespace ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Any Key To Continue..");
            Console.ReadLine();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var studentinfodevapiapp = new Studentinfodevapiapp(new Uri("http://studentinfodevapiapp.azurewebsites.net/"), new AnonymousCredential()))
            {
                var students = studentinfodevapiapp.GetAll();
                foreach (var s in students)
                {
                    Console.WriteLine($"Name:{s.FIRSTNAME} {s.LASTNAME}");
                }
            }

            Console.WriteLine("Press Any Key To Continue..");
            Console.ReadLine();
        }
    }

    public class AnonymousCredential : ServiceClientCredentials
    {
    }
}
