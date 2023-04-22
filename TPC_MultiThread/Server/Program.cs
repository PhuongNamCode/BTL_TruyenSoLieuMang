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

            char requestName = ' ';
            string requestParameter = String.Empty;
            // lấy yêu cầu
            if (!string.IsNullOrEmpty(request))
            {
                // ký tự đầu tiên là phép toán
                requestName = request[0];
            }

            switch (requestName)
            {
                case '!':
                    // cắt chuỗi loại bỏ phép toán
                    if(int.TryParse(request.Substring(1), out int c))
                    {
                        int result = 1;
                        for (int i = 1; i <= c; i++)
                        {
                            result *= i;
                        }
                        response = result.ToString();
                    }
                    else
                    {
                        response = "Parameter incorrect";

                    }
                    break;

                case '^':
                    var AB = request.Substring(1).Split();
                    if(int.TryParse(AB[0], out int a) && int.TryParse(AB[1], out int b) && AB.Length == 2)
                    {
                        response = Math.Pow((double)a, (double)b).ToString();
                    }
                    else
                    {
                        response = "Parameter incorrect";
                    }
                    break;

                default:
                    response = "Parameter incorrect";
                    break;
            }

            writer.WriteLine(response);
            writer.Flush();
            socket.Close();
        }
    }
}