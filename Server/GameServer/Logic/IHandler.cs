using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace GameServer.Logic
{
    public interface IHandler
    {
        void OnReceive(ClientPeer _clientPeer, int _subCode, object _value);
        void OnDisconnect(ClientPeer _clientPeer);
    }
}
