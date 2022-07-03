using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAvonale
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:8080/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Api de Teste Avonale... Endereço " + baseAddress);
                Console.ReadLine();
            }
        }
    }
}
