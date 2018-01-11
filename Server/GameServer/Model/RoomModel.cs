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
		public List<AccountModel> PlayersList;	//房间内玩家

		public RoomModel(string _roomId)
		{
			RoomId = _roomId;
			PlayersList=new List<AccountModel>();
		}
	}
}
