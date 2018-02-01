using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Code.Dto;
using Protocol.Constant;

namespace GameServer.Cache.Battle
{
	public class LibraryModel       //牌库
	{
		public Queue<CardDto> CardQueue;        //所有牌的队列

		public LibraryModel()
		{
			//创建牌
			//洗牌
		}
		private void create()
		{
			CardQueue = new Queue<CardDto>();
			//创建普通的牌
			for(int color = CardColor.WAN; color <= CardColor.FLOWER; color++)
			{
				for(int weight = 1; weight < 9; weight++)
				{

				}
			}
		}
	}
}
