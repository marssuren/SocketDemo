using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static Socket clientSocket;
        static void Main(string[] args)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint tRemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.101"), 5056);
            clientSocket.Connect(tRemoteEndPoint);     //客户端只需连接，不需bind
            Console.WriteLine("客户端连接至服务器成功：");

            byte[] tResult = new byte[1024];
            int tLength = clientSocket.Receive(tResult);
            Console.WriteLine("收到消息：" + Encoding.Default.GetString(tResult, 0, tLength));

            clientSocket.Send(Encoding.Default.GetBytes("服务器你好，我是客户端"));
            while (true)
            { }
        }
    }
}
