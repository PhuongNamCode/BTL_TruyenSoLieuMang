using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TCP Server: ";
            var server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, 1308));
            server.Listen(10);
            Console.WriteLine($"Server started at {server.LocalEndPoint}");

            while (true)
            {

                var worker = server.Accept();

                var stream = new NetworkStream(worker);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);

                var streamWriter = new System.IO.StreamWriter(stream);

                var request = reader.ReadLine();
                string response = XuLy(request);


                Console.WriteLine($"Client Message : {request} from {worker.RemoteEndPoint} ");


                streamWriter.WriteLine(response);
                streamWriter.Flush();
                worker.Close();
            }
        }
        private static string XuLy(string request)
        {
            string response = string.Empty;
            // vị trí xuất hiện phép toán
            int indexOfCaculator = 0;
            for (int i = 0; i < request.Length; i++)
            {
                if (!Char.IsNumber(request[i]))
                {
                    indexOfCaculator = i;
                    break;
                }
            }

            // phép toán là 1 ký tự tại indexOfCaculator
            char phepToan = request[indexOfCaculator];

            // ép kiểu vế trước (A) và vế sau (B), nếu thành công => thực hiện, nếu sai trả về UNKNOW COMMAND
            if (int.TryParse(request.Substring(0, indexOfCaculator), out int a) && int.TryParse(request.Substring(indexOfCaculator + 1), out int b))
            {
                switch (phepToan)
                {
                    case '+':
                        response = (a + b).ToString();
                        break;

                    case '-':
                        response = (a - b).ToString();
                        break;

                    case '*':
                        response = (a * b).ToString();
                        break;

                    case '/':
                        response = ((decimal)a / b).ToString();
                        break;
                    default:
                        response = request;
                        break;
                }
            }
            else
            {
                response = request;
            }
            return response;
        }
    }
}
