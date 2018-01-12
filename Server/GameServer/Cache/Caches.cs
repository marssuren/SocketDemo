using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
	public class Caches
	{
		public static PlayerCache Player;        
		public static RoomCache Room;
		static Caches()
		{
			Player = new PlayerCache();
			Room = new RoomCache();
		}
	}
}
