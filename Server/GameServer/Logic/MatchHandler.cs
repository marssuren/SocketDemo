using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GameServer.Cache;
using GameServer.Cache.Match;
using GameServer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Code;
using Protocol.Code.Dto;
using Server;

namespace GameServer.Logic
{
	public class MatchHandler : IHandler
	{
		private MatchCache matchCache = Caches.Match;
		private PlayerCache userCache = Caches.Player;
		public void OnDisconnect(ClientPeer _clientPeer)
		{
		}
		public void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _value)
		{
			switch(_subCode)
			{
				case MatchCode.EnterMatch_ClientReq:

				break;
				case MatchCode.LeaveMatch_ClientReq:

				break;
				case MatchCode.ReadyMatch_ClientReq:
				break;
			}
		}
		private void enter(ClientPeer _clientPeer)
		{
			SingleExecute.Instance.Excute(delegate ()
				{
					int tUserId = Convert.ToInt16(userCache.GetId(_clientPeer));
					if(matchCache.IsMatching(tUserId))      //检查用户是否已经在匹配
					{
						var tResponse = new
						{
							ErrorMsg = "用户已经在匹配",
						};
						string tRes = JsonConvert.SerializeObject(tResponse);
						_clientPeer.Send(OpCode.MATCH, MatchCode.EnterMatch_ServerRes, tRes);
						Console.WriteLine("用户已经在匹配");
						return;
					}
					MatchRoom tMatchRoom = matchCache.StartMatch(tUserId, _clientPeer);      //开始匹配
					tMatchRoom.Broadcast(OpCode.MATCH, MatchCode.EnterMatch_ServerBro, tUserId);//广播给房间内所有玩家,有新玩家加入
					makeRoomDto(tMatchRoom);
				}
				);
		}
		private MatchRoomDto makeRoomDto(MatchRoom _matchRoom)
		{
			//返回给当前客户端当前房间的数据模型
			MatchRoomDto tMatchRoomDto = new MatchRoomDto();
			foreach(var item in tMatchRoomDto.UidUserDic.Keys)
			{
				PlayerModel tPlayerModel = userCache.GetPlayerModel(item.ToString());
				PlayerDto tPlayerDto = new PlayerDto(tPlayerModel.Nickname, tPlayerModel.Level, tPlayerModel.Exp);
				tMatchRoomDto.UidUserDic.Add(item, tPlayerDto);  //将当前玩家加入到匹配房间的Dic中
			}
			tMatchRoomDto.ReadyUidLst = _matchRoom.ReadyUIdLst;
			return tMatchRoomDto;
		}
	}
}
