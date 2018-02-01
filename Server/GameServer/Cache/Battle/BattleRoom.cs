using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Code.Dto;

namespace GameServer.Cache.Battle
{
	public class BattleRoom     //战斗房间
	{
		public int RoomId
		{
			get;
		}
		public List<PlayerDto> PlayerList;  //存储所有玩家
		public List<int> LeaveUIDList       //中途退出的玩家列表
		{
			get;
			set;
		}
	}
}
