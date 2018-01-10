using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;

namespace GameServer.Cache
{
	public class RoomCache             //房间的缓存
	{
		private Dictionary<string, RoomModel> inUseRoomsCache = new Dictionary<string, RoomModel>();


		public bool IsRoomExist(string _roomId) //房间是否存在
		{
			return inUseRoomsCache.ContainsKey(_roomId);
		}
		public RoomModel CreateNewRoom()                //创建新房间
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
					tRoomModel = new RoomModel(tRoomId);
					Console.WriteLine("创建房间：" + tRoomId);
					inUseRoomsCache.Add(tRoomId, tRoomModel);
					break;
				}
			}
			return tRoomModel;
		}
		public void RemoveRoom(string _roomId)      //移除房间
		{
			Console.WriteLine("移除房间:" + _roomId);

			inUseRoomsCache.Remove(_roomId);
		}
	}
}
