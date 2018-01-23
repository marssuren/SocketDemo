using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code.Dto
{
	[Serializable]
	public class MatchRoomDto       //房间数据对应的传输模型
	{
		public Dictionary<int, PlayerDto> UidUserDic;   //用户uid对应的用户数据传输模型
		public List<int> ReadyUidLst;       //准备的玩家id列表
		public MatchRoomDto()
		{
			UidUserDic = new Dictionary<int, PlayerDto>();
			ReadyUidLst = new List<int>();
		}
		public void Add(PlayerDto _newPlayer)		//玩家进入
		{
			UidUserDic.Add(_newPlayer.Id, _newPlayer);
		}
		public void Remove(PlayerDto _newPlayer)	//移除玩家
		{
			UidUserDic.Remove(_newPlayer.Id);
		}
		public void Ready(int _uid)					//玩家准备
		{
			ReadyUidLst.Add(_uid);
		}

	}
}
