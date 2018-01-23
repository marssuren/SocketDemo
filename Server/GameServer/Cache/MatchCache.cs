using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Cache.Match;
using Server;
using Server.Util.Concurrent;

namespace GameServer.Cache
{
	public class MatchCache     //匹配的缓存层
	{
		private Dictionary<int, int> uIdRoomIdDic = new Dictionary<int, int>();   //正在等待的用户id和房间id的映射
		private Dictionary<int, MatchRoom> idModelDic = new Dictionary<int, MatchRoom>();    //代表正在等待的房间id和房间的数据模型的映射
		private Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();        //重用的房间队列
		public MatchCache()
		{

		}
		private ConcurrentInt roomId = new ConcurrentInt(-1);       //房间的Id
		public MatchRoom StartMatch(int _userId, ClientPeer _clientPeer)     //进入匹配队列,进入匹配房间
		{
			foreach(MatchRoom matchRoom in idModelDic.Values)     //遍历等待的房间,看有没有正在等待的,如果有，就把这个玩家加进去
			{
				if(matchRoom.IsFull())          //房间满则继续
				{
					continue;
				}
				matchRoom.Enter(_userId, _clientPeer);       //没满
				uIdRoomIdDic.Add(_userId, matchRoom.Id);
				return matchRoom;
			}

			MatchRoom tMatchRoom = null;        //无正在等待匹配的房间，则新开房间
			if(roomQueue.Count > 0)             //判断是否有可重用的房间
			{
				tMatchRoom = roomQueue.Dequeue();
			}
			else
			{
				tMatchRoom = new MatchRoom(roomId.AddGet(), 4);
			}
			tMatchRoom.Enter(_userId, _clientPeer);
			idModelDic.Add(tMatchRoom.Id, tMatchRoom);
			uIdRoomIdDic.Add(_userId, tMatchRoom.Id);
			return tMatchRoom;
		}
		public MatchRoom LeaveMatch(int _userId, ClientPeer _clientPeer)    //离开匹配队列
		{
			int tRoomId = uIdRoomIdDic[_userId];    //获取要离开匹配队列的用户Id
			MatchRoom tRoom = idModelDic[tRoomId];  //获取用户所属的匹配的房间
			tRoom.Leave(_userId, _clientPeer);                   //用户离开所属的匹配的房间
			uIdRoomIdDic.Remove(_userId);           //将玩家从匹配队列中移除
			
			return tRoom;
		}
		public bool IsMatching(int _userId)         //判断用户是否正在匹配房间内
		{
			return uIdRoomIdDic.ContainsKey(_userId);
		}
		public MatchRoom GetRoom(int _userId)                   //获取玩家所在的等待房间
		{
			int tRoomId = uIdRoomIdDic[_userId];
			MatchRoom tMatchRoom = idModelDic[tRoomId];
			return tMatchRoom;
		}
		public void Destroy(MatchRoom _matchRoom)               //清除房间信息并回收
		{
			idModelDic.Remove(_matchRoom.Id);

			foreach(var item in _matchRoom.UidPeerDic)
			{
				uIdRoomIdDic.Remove(item.Key);
			}
			_matchRoom.UidPeerDic.Clear();
			_matchRoom.ReadyUIdLst.Clear();
			roomQueue.Enqueue(_matchRoom);                      //回收房间
		}

	}
}
