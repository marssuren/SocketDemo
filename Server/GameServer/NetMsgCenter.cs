using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace GameServer
{
    public class NetMsgCenter : IApplication           //网络的消息中心,对服务器接收到的消息进行提前分类并转发(但是不处理)，交由帐号模块相应和处理
    {
        public void OnDisConnect(ClientPeer _clientPeer)
        {
            throw new NotImplementedException();
        }

        public void OnReceive(ClientPeer _clientPeer, SocketMessage _socketMessage)
        {
            throw new NotImplementedException();
        }
    }
}
