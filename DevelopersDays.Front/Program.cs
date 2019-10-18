using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DevelopersDays.Front
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Console World!");

            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            using (var http = new HttpClient())
            {
                while (true)
                {
                    var response = await http.GetAsync("http://10.0.145.163");
                    var result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(result);

                    Thread.Sleep(5000);
                }
            }
        }
    }
}
