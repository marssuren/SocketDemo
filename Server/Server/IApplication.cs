using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IApplication
    {
        void OnDisConnect(ClientPeer _clientPeer);      //断开连接
        void OnReceive(ClientPeer _clientPeer, SocketMessage _socketMessage);       //接收数据
    }
}
