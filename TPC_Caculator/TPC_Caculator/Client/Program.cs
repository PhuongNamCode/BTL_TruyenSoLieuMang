using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tcp Client";
            Console.WriteLine("Client ");
            //var address = IPAddress.Parse(Console.ReadLine());
            var address = IPAddress.Parse("192.168.1.3");

            var serverEndpoint = new IPEndPoint(address, 1308);
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("# COMMAND >>> ");
                Console.ResetColor();

                var request = Console.ReadLine();
                var client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                client.Connect(serverEndpoint);

                var stream = new NetworkStream(client);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.WriteLine(request);
                writer.Flush();
               
                var response = reader.ReadLine();
                Console.WriteLine($"> {response}");
                client.Close();
            }
        }
    }
}