using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Cache;
using GameServer.Cache.Match;
using Newtonsoft.Json.Linq;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class ChatHandler : IHandler
	{
		private PlayerCache playerCache = Caches.Player;
		private MatchCache matchCache = Caches.Match;
		public void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _value)
		{
			int tChatType = (int)_value["ChatType"];
			switch(_subCode)
			{
				case ChatCode.DEFAULT:
				chatRequest(_clientPeer, tChatType);
				break;
			}
		}

		public void OnDisconnect(ClientPeer _clientPeer)
		{
			throw new NotImplementedException();
		}
		private void chatRequest(ClientPeer _clientPeer, int _chatType) //接收聊天类型，给房间内的每个客户端都广播发送者和发送的消息
		{
			if(!playerCache.IsOnline(_clientPeer))
			{
				return;
			}
			var tChatDto = new
			{
				UserId = playerCache.GetId(_clientPeer),
				ChatType = _chatType,
			};
			if(matchCache.IsMatching(Convert.ToInt16(tChatDto.UserId)))
			{
				MatchRoom tMatchRoom = matchCache.GetRoom(Convert.ToInt16(tChatDto.UserId));
				tMatchRoom.Broadcast(OpCode.CHAT, ChatCode.ServerRes, tChatDto);
			}
			//else if()			//战斗房间的聊天
			//{
			//	//todo:
			//}

		}
	}
}
