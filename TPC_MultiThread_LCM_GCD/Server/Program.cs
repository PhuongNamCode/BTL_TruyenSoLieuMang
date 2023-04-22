using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TCP Server: ";
            var listenner = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listenner.Bind(new IPEndPoint(IPAddress.Any, 1308));
            listenner.Listen(10);
            Console.WriteLine($"Server started at {listenner.LocalEndPoint} and");

            while (true)
            {
                Socket socket = listenner.Accept();
                Console.WriteLine($"Request from: {socket.LocalEndPoint}");
                ThreadPool.QueueUserWorkItem(Callback, socket);
            }
        }

        private static void Callback(object state)
        {
            var socket = state as Socket;
            Calculate(socket);
        }

        private static void Calculate(Socket socket)
        {
            var stream = new NetworkStream(socket);
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            var request = reader.ReadLine();
            var response = string.Empty;

            string requestName = String.Empty;
            string requestParameter = String.Empty;
            // lấy yêu cầu
            if (request.Length >= 3)
            {
                requestName = request.Substring(0, 3);
                requestParameter = request.Replace(requestName, "");
                requestParameter = requestParameter.Trim();
            }
           
            var AB = requestParameter.Split();

            // đảm bảo chỉ có 2 số A và B
            if(AB.Length == 2)
            {
                // ép kiểu vế trước (A) và vế sau (B), nếu thành công => thực hiện, nếu sai trả về UNKNOW COMMAND
                if (int.TryParse(AB[0], out int a) && int.TryParse(AB[1], out int b) && a > 0 && b > 0)
                {
                    switch (requestName)
                    {
                        case "GCD":
                            response = Gcd(a,b).ToString();
                            break;

                        case "LCM":
                            response = ((a*b)/Gcd(a,b)).ToString();
                            break;

                        default:
                            response = "Parameter incorrect";
                            break;
                    }
                }
                else
                {
                    response = "UNKNOW COMMAND";
                }
            }
            else
            {
                response = "UNKNOW COMMAND";
            }
            writer.WriteLine(response);
            writer.Flush();
            socket.Close();
        }

        static long Gcd(int n1, int n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return Gcd(n2, n1 % n2);
            }
        }
    }
}