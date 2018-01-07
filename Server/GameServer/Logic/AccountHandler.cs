using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class AccountHandler : IHandler //帐号处理的逻辑层
	{
		public void OnReceive(ClientPeer _clientPeer, int _subCode, object _value)
		{
			switch(_subCode)
			{
				case AccountCode.Regist_ClientReq:
				Console.WriteLine("");
				break;
				case AccountCode.Login:
				break;
				default:
				break;
			}
		}

		public void OnDisconnect(ClientPeer _clientPeer)
		{
			throw new NotImplementedException();
		}
	}
}
