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
		public string RoomType;     //房间类型(花麻将/百搭麻将)
		public string Rule;     //规则(2摸3铳/3摸4铳)
		public bool IsFloatFlower;  //是否飘花
		public int RoundCount;  //局数(8局/16局)
		public string PayType;  //支付方式(AA平摊/房主支付)
		public List<PlayerModel> PlayersList;  //房间内玩家
		public Dictionary<int, PlayerModel> SeatsPlayersDic;       //房间内座位和玩家
		public RoomModel(string _roomId, string _roomType, string _rule, bool _isFloatFlower, int _roundCount, string _payType)
		{
			RoomId = _roomId;
			RoomType = _roomType;
			Rule = _rule;
			IsFloatFlower = _isFloatFlower;
			RoundCount = _roundCount;
			PayType = _payType;
			PlayersList = new List<PlayerModel>();
			SeatsPlayersDic = new Dictionary<int, PlayerModel>();
			for(int i = 0; i < 4; i++)
			{
				SeatsPlayersDic.Add(i, null);			//初始化0到3号座位
			}
		}
	}
}
