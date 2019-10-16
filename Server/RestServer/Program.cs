using CommonLibrary;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;
using System.Net.Http;

namespace RestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var baseAddress = CommonConfig.ServerConfig.FullAddress;
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine($"MultiCalculator is running now!\nAddress is {baseAddress}");
                Console.ReadLine();
            }
        }
    }
}
