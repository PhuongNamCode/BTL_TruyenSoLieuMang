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
            Console.WriteLine("Server ip: 127.0.0.1");
            //var address = IPAddress.Parse(Console.ReadLine());
            var address = IPAddress.Parse("127.0.0.1");

            var serverEndpoint = new IPEndPoint(address, 1308);
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("# COMMAND LCM/GCD A B>>> ");
                Console.ResetColor();
                var request = Console.ReadLine();
                var client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                client.Connect(serverEndpoint);

                var stream = new NetworkStream(client);
                var rd = new StreamReader(stream);
                var wr = new StreamWriter(stream);
                wr.WriteLine(request);
                wr.Flush();

                var response = rd.ReadLine();
                Console.WriteLine($"> {response}");
                client.Close();
            }

        }
    }
}