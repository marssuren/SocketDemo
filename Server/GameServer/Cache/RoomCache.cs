using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;

namespace GameServer.Cache
{
	public class RoomCache             //房间的缓存,后期转数据库处理
	{
		private Dictionary<string, RoomModel> inUseRoomsCache = new Dictionary<string, RoomModel>();
		public bool IsRoomExist(string _roomId) //房间是否存在
		{
			return inUseRoomsCache.ContainsKey(_roomId);
		}
		public RoomModel CreateNewRoom(string _roomType, string _rule, bool _isFloatFlower, int _roundCount, string _payType)                //创建新房间
		{
			Random tRamdom = new Random();
			RoomModel tRoomModel;
			while(true)
			{
				string tRoomId = tRamdom.Next(0, 999999).ToString();
				if(tRoomId.Length < 6)
				{
					for(int i = 0; i < 6 - tRoomId.Length; i++)
					{
						tRoomId = "0" + tRoomId;
					}
				}
				if(!inUseRoomsCache.ContainsKey(tRoomId))
				{
					tRoomModel = new RoomModel(tRoomId, _roomType, _rule, _isFloatFlower, _roundCount, _payType);
					Console.WriteLine("创建房间：" + tRoomId);
					inUseRoomsCache.Add(tRoomId, tRoomModel);
					break;
				}
			}
			return tRoomModel;
		}
		public RoomModel GetRoom(string _roomId)    //获取指定的房间
		{
			return inUseRoomsCache[_roomId];
		}
		public int GetRoomPlayerCount(string _roomId)    //获取房间内的玩家数量
		{
			return inUseRoomsCache[_roomId].PlayersList.Count;
		}
		public int GetFreeSeat(string _roomId)      //获取指定房间内一个空闲座位
		{
			for(int i = 0; i < inUseRoomsCache[_roomId].SeatsPlayersDic.Count; i++)
			{
				if(null == inUseRoomsCache[_roomId].SeatsPlayersDic[i])
				{
					return i;
				}
			}
			return 5;
		}
		public void RemoveRoom(string _roomId)      //移除房间
		{
			Console.WriteLine("移除房间:" + _roomId);
			inUseRoomsCache.Remove(_roomId);
		}
		public void EnterRoom(string _roomId, int _seatNumber, PlayerModel _player)     //加入房间
		{
			inUseRoomsCache[_roomId].PlayersList.Add(_player);
			inUseRoomsCache[_roomId].SeatsPlayersDic[_seatNumber] = _player;	//玩家入座
		}
		public void ExitRoom(string _roomId, PlayerModel _player)          //退出房间
		{
			inUseRoomsCache[_roomId].PlayersList.Remove(_player);
			inUseRoomsCache[_roomId].SeatsPlayersDic[_player.SeatNumber] = null;

		}
	}
}
