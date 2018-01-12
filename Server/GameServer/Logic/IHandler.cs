using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Server;

namespace GameServer.Logic
{
    public interface IHandler
    {
        void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _value);
        void OnDisconnect(ClientPeer _clientPeer);
    }
}
