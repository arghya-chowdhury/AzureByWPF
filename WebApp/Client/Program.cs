using System;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Any Key To Continue..");
            Console.ReadLine();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://studentinfodevapp.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("Client/json"));

                var result = client.GetAsync("Student/").GetAwaiter().GetResult();
                if (result != null)
                {
                    var students = result.Content.ReadAsAsync<STUDENT[]>().GetAwaiter().GetResult();
                    foreach (var s in students)
                    {
                        Console.WriteLine($"Name:{s.FIRST_NAME} {s.LAST_NAME}");
                    }
                }
                Console.WriteLine("Press Any Key To Continue..");
                Console.ReadLine();
            }
        }
    }

    public class STUDENT
    {
        public int ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string PHONE_NO { get; set; }
    }
}
