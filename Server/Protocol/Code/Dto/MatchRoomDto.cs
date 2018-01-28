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
		public List<int> UidList=new List<int>();			//房间内的玩家Uid
		public int LeftPlayerId;			//左边玩家Id
		public int OppsitePlayerId;			//对面玩家Id
		public int RightPlayerId;			//右边玩家Id
		public MatchRoomDto()
		{
			UidUserDic = new Dictionary<int, PlayerDto>();
			ReadyUidLst = new List<int>();
		}
		public void Add(PlayerDto _newPlayer)		//玩家进入
		{
			UidUserDic.Add(_newPlayer.Id, _newPlayer);
			UidList.Add(_newPlayer.Id);
		}
		public void Leave(PlayerDto _player)	//玩家离开
		{
			UidUserDic.Remove(_player.Id);
			UidList.Remove(_player.Id);
		}
		public void Ready(int _uid)					//玩家准备
		{
			ReadyUidLst.Add(_uid);
		}
		public void ResetPosition(int _myUserId)					//重置位置，在每次玩家进入或者离开房间的时候都需要调整玩家位置
		{
			LeftPlayerId = -1;
			RightPlayerId = -1;
			OppsitePlayerId = -1;
			if (UidList.Count==1)
			{
				return;
			}

			if (UidList.Count==2)
			{
				if (UidList[0]==_myUserId)
				{
					RightPlayerId = UidList[1];
				}

				if (UidList[1]==_myUserId)
				{
					
				}
				return;
			}

			
		}
	}
}
