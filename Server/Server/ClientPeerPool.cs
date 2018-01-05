using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    /// <summary>
    /// 客户端的连接池
    /// 作用：重用客户端连接对象
    /// </summary>
    class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeerQueue;

        public ClientPeerPool(int _capacity)
        {
            clientPeerQueue = new Queue<ClientPeer>(_capacity);
        }

        public void Enqueue(ClientPeer _client)     //回收
        {
            clientPeerQueue.Enqueue(_client);
        }

        public ClientPeer Dequeue()                 //取出
        {
            return clientPeerQueue.Dequeue();
        }
    }
}
