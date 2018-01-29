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
		public List<int> UidList = new List<int>();         //房间内的玩家Uid
		public int LeftPlayerId;            //左边玩家Id
		public int OppsitePlayerId;         //对面玩家Id
		public int RightPlayerId;           //右边玩家Id
		public MatchRoomDto()
		{
			UidUserDic = new Dictionary<int, PlayerDto>();
			ReadyUidLst = new List<int>();
		}
		public void Add(PlayerDto _newPlayer)       //玩家进入
		{
			UidUserDic.Add(_newPlayer.Id, _newPlayer);
			UidList.Add(_newPlayer.Id);
		}
		public void Leave(PlayerDto _player)    //玩家离开
		{
			UidUserDic.Remove(_player.Id);
			UidList.Remove(_player.Id);
		}
		public void Ready(int _uid)                 //玩家准备
		{
			ReadyUidLst.Add(_uid);
		}
		public void ResetPosition(int _myUserId)                    //重置位置，在每次玩家进入或者离开房间的时候都需要调整玩家位置	  取决于自己是第几个进入房间的
		{
			LeftPlayerId = -1;
			RightPlayerId = -1;
			OppsitePlayerId = -1;
			if(UidList.Count == 1)
			{
				return;
			}

			if(UidList.Count == 2)               //如果有两个玩家
			{
				if(UidList[0] == _myUserId)      //如果房间内第一个玩家是玩家自己
				{
					RightPlayerId = UidList[1]; //将右边的玩家设为第二个玩家
				}

				if(UidList[1] == _myUserId)      //如果第二个是玩家自己
				{
					LeftPlayerId = UidList[0];  //将左边的玩家设置为房间内的第一个玩家
				}
			}
			else if(UidList.Count == 3)         //如果房间内有三个玩家
			{
				if(UidList[0] == _myUserId)     //如果房间内第一个玩家是自己
				{
					LeftPlayerId = UidList[2];  //将最后一个进入房间的玩家放到左边
					RightPlayerId = UidList[1]; //将第二个进房间的玩家放到右边
				}

				if(UidList[1] == _myUserId)     //如果房间内第二个玩家是自己
				{
					LeftPlayerId = UidList[0];  //将左边的玩家设置为第一个进入房间的玩家
					RightPlayerId = UidList[2]; //将右边的玩家设置为最后进入房间的玩家
				}

				if(UidList[2] == _myUserId)     //如果房间内最后进入的玩家是自己
				{
					LeftPlayerId = UidList[1];  //将左边的玩家设置为第二个进入房间的玩家
					RightPlayerId = UidList[0]; //将右边的玩家设置为第一个进入房间的玩家
				}

			}


		}
	}
}
