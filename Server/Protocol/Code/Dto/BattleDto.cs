using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code.Dto
{
	[Serializable]
	public class BattleDto
	{
		public int PlayerId;            //用户Id
		public int Identity;            //身份
		public List<CardDto> CardList;  //自己拥有的手牌
		
		public BattleDto(int _playerId)
		{
			Identity = -1;
			PlayerId = _playerId;
			CardList = new List<CardDto>();
		}
		public bool IsHasCard()     //判断是否还有手牌
		{
			return CardList.Count > 0;
		}
		public int CardCount()      //查看剩余卡牌数量
		{
			return CardList.Count;
		}
		public void Add(CardDto _cardDto)           //添加卡牌
		{
			CardList.Add(_cardDto);
		}
		public void Remove(CardDto _cardDto)        //移除卡牌
		{
			CardList.Remove(_cardDto);
		}
	}
}
