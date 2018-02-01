using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class BattleHandler : IHandler
	{
		public void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _value)
		{
			switch(_subCode)
			{
				case BattleCode.Ready_ClientReq:
				Console.WriteLine(_value);
				break;
			}
		}

		public void OnDisconnect(ClientPeer _clientPeer)
		{

		}
		private void ready(ClientPeer _clientPeer)
		{
			SingleExecute.Instance.Excute((() =>
			{

			}));
		}
		public void StartBattle(List<int> _uidList)
		{

		}
	}
}
