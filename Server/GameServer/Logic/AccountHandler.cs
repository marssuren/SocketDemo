using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace GameServer.Logic
{
   public class AccountHandler:IHandler //帐号处理的逻辑层
    {
       public void OnReceive(ClientPeer _clientPeer, int _subCode, object _value)
       {
           throw new NotImplementedException();
       }

       public void OnDisconnect(ClientPeer _clientPeer)
       {
           throw new NotImplementedException();
       }
    }
}
