using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerPeer

    public class ServerPeer
    {
        private Socket serverSocket;

        public ServerPeer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



        }
        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="_port"></param>
        /// <param name="_maxCount"></param>
        public void StartServer(int _port, int _maxCount)
        {
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
                serverSocket.Listen(_maxCount);
            }
            catch (Exception _exception)
            {
                Console.WriteLine(_exception.Message);
            }
        }
    }

}
