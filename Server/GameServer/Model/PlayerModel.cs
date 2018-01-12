using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model      //模型层提供给缓存层用
{
	public class PlayerModel           //玩家的数据模型
	{
		public string Id;               //玩家Uid
		public string Nickname;         //用户名
		public string IconUrl;			//玩家头像图片的Url

		public int SeatNumber;          //玩家在房间内的座位号

		public PlayerModel(string _id, string _nickname)
		{
			Id = _id;
			Nickname = _nickname;
		}
	}
}
