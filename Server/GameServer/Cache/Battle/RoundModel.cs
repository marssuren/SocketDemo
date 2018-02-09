using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Battle
{
	public class RoundModel     //回合管理类
	{
		public int BiggestPlayerUid;    //当前回合最大的出牌者(管上)
		public int CurrentPlayerUid;    //当前的出牌者
		public int LastLength;          //上一次出牌的长度
		public int LastWeight;          //上一次出牌的权值
		public int LastCardType;        //上一次出牌的类型(顺子，连对，飞机。。。)

		public RoundModel()
		{
			BiggestPlayerUid = -1;
			CurrentPlayerUid = -1;
			LastLength = -1;
			LastWeight = -1;
			LastCardType = -1;
		}
		public void Init()
		{
			BiggestPlayerUid = -1;
			CurrentPlayerUid = -1;
			LastLength = -1;
			LastWeight = -1;
			LastCardType = -1;
		}
		public void Start(int _playerId)      //开始出牌
		{
			CurrentPlayerUid = _playerId;
			BiggestPlayerUid = _playerId;
		}
		public void Change(int _length, int _cardType, int _weight, int _playerId)  //改变出牌者
		{
			BiggestPlayerUid = _playerId;
			LastLength = _length;
			LastCardType = _cardType;
			LastWeight = _weight;
		}
		public void Turn(int _playerId)     //转换出牌
		{
			CurrentPlayerUid = _playerId;
		}
	}
}
