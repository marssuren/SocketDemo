using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server;

namespace GameServer.Cache.Match
{
	public class MatchRoom      //匹配房间
	{
		public int Id;          //唯一标识
		private int maxCount;    //最大玩家数量
		public Dictionary<int, ClientPeer> UidPeerDic;        //在房间内的用户id和连接对象的映射
		public List<int> ReadyUIdLst;      //已经准备的玩家列表	

		public MatchRoom(int _id, int _maxCount)
		{
			Id = _id;
			maxCount = _maxCount;
			UidPeerDic = new Dictionary<int, ClientPeer>();
			ReadyUIdLst = new List<int>();
		}
		public bool IsFull()                //房间是否满了
		{
			return UidPeerDic.Count >= maxCount;
		}
		public bool IsEmpty()               //房间是否空
		{
			return UidPeerDic.Count == 0;
		}
		public bool IsAllReady()            //是否全部准备完毕
		{
			return ReadyUIdLst.Count == maxCount;
		}
		public void Enter(int _userId, ClientPeer _clientPeer)      //玩家进入房间
		{
			UidPeerDic.Add(_userId, _clientPeer);
		}
		public void Leave(int _userId, ClientPeer _clientPeer)      //玩家离开房间
		{
			UidPeerDic.Remove(_userId);
			if(ReadyUIdLst.Contains(_userId))       //如果当前玩家已经准备
			{
				ReadyUIdLst.Remove(_userId);        //从准备列表中移除
			}
		}
		public void Ready(int _userId)      //玩家准备
		{
			ReadyUIdLst.Add(_userId);
		}
		public void Broadcast(int _opCode, int _subCode, int _userId)   //广播给房间内所有人
		{
			foreach(var clientPeer in UidPeerDic.Values)
			{
				var tRes = new
				{
					UserId = _userId,
				};
				string tResStr = JsonConvert.SerializeObject(tRes);
				clientPeer.Send(_opCode, _subCode, tResStr);
			}
		}
	}
}
