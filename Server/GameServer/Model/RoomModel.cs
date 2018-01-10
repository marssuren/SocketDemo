using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
	[Serializable]
	public class RoomModel
	{

		public string RoomId;   //房间号
		public string RoomContent;  //房间名

		public RoomModel(string _roomId)
		{
			RoomId = _roomId;
		}
	}
}
