using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Match
{
	public class MatchRoom      //匹配房间
	{
		public int Id;          //唯一标识
		private int maxCount;    //最大玩家数量
		private List<int> uidLst;        //在房间内的用户id列表
		private List<int> readyUIdLst;      //已经准备的玩家列表	

		public MatchRoom(int _id, int _maxCount)
		{
			Id = _id;
			maxCount = _maxCount;
			uidLst = new List<int>();
			readyUIdLst = new List<int>();
		}
		public bool IsFull()				//房间是否满了
		{
			return uidLst.Count >= maxCount;
		}
		public bool IsEmpty()				//房间是否空
		{
			return uidLst.Count == 0;
		}
		public bool IsAllReady()			//是否全部准备完毕
		{
			return readyUIdLst.Count == maxCount;
		}
		public void Enter(int _userId)      //玩家进入房间
		{
			uidLst.Add(_userId);
		}
		public void Leave(int _userId)		//玩家离开房间
		{
			uidLst.Remove(_userId);
			if (readyUIdLst.Contains(_userId))
			{
				readyUIdLst.Remove(_userId);
			}
		}
		public void Ready(int _userId)		//玩家准备
		{
			readyUIdLst.Add(_userId);
		}
	}
}
