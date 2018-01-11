using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server;
using Protocol.Code;

namespace GameServer
{
	public class NetMsgCenter : IApplication           //网络的消息中心,对服务器接收到的消息进行提前分类并转发(但是不处理)，交由帐号模块相应和处理
	{
		private IHandler account = new AccountHandler();
		private IHandler battle = new BattleHandler();
		private IHandler room = new RoomHandler();
		public void OnDisConnect(ClientPeer _clientPeer)
		{
			account.OnDisconnect(_clientPeer);
			battle.OnDisconnect(_clientPeer);
		}

		public void OnReceive(ClientPeer _clientPeer, SocketMessage _socketMessage)
		{
			string tJsonStr = "[{'number'='123456'}]";
			JArray tJArray = (JArray)JsonConvert.DeserializeObject(tJsonStr);
			JObject tJObject = (JObject) tJArray[1];
			Console.WriteLine(tJObject["number"]);
			switch(_socketMessage.OPCode)
			{
					
				case OpCode.ACCOUNT:
				account.OnReceive(_clientPeer, _socketMessage.SubCode, _socketMessage.Value);
				break;
				case OpCode.ROOM:
				room.OnReceive(_clientPeer, _socketMessage.SubCode, _socketMessage.Value);
				break;
				case OpCode.BATTLE:
				battle.OnReceive(_clientPeer, _socketMessage.SubCode, _socketMessage.Value);
				break;
				default:
				break;
			}
		}


	}
}
