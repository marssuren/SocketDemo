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
		private IHandler account = new PlayerHandler();
		private IHandler battle = new BattleHandler();
		private IHandler room = new RoomHandler();
		private IHandler match = new MatchHandler();
		private IHandler chat = new ChatHandler();
		public void OnDisConnect(ClientPeer _clientPeer)
		{
			account.OnDisconnect(_clientPeer);
			battle.OnDisconnect(_clientPeer);
			room.OnDisconnect(_clientPeer);
			match.OnDisconnect(_clientPeer);
			chat.OnDisconnect(_clientPeer);
		}

		public void OnReceive(ClientPeer _clientPeer, SocketMessage _socketMessage)
		{
			string tJsonStr = _socketMessage.Value.ToString();
			Console.WriteLine(tJsonStr);
			JObject tJObject = (JObject)JsonConvert.DeserializeObject(tJsonStr);        //转化为JsonObject

			switch(_socketMessage.OPCode)
			{
				case OpCode.PLAYER:
				account.OnReceive(_clientPeer, _socketMessage.SubCode, tJObject);
				break;
				case OpCode.ROOM:
				room.OnReceive(_clientPeer, _socketMessage.SubCode, tJObject);
				break;
				case OpCode.BATTLE:
				battle.OnReceive(_clientPeer, _socketMessage.SubCode, tJObject);
				break;
				case OpCode.MATCH:
				match.OnReceive(_clientPeer, _socketMessage.SubCode, tJObject);
				break;
				case OpCode.CHAT:
				chat.OnReceive(_clientPeer, _socketMessage.SubCode, tJObject);
				break;
				default:
				break;
			}
		}


	}
}
