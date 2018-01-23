using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code.Dto
{
	[Serializable]
	public class PlayerDto          //传输类
	{
		public int Id;				//玩家ID
		public string Name;         //玩家名
		public int Lv;              //等级
		public int Exp;             //经验
		public PlayerDto(int _id,string _name, int _lv, int _exp)
		{
			Id = _id;
			Name = _name;
			Lv = _lv;
			Exp = _exp;
		}

	}
}
