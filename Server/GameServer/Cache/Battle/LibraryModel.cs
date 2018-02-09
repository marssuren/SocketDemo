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
			create();//创建牌
			suffle();//洗牌
		}
		public void Init()
		{
			create();//创建牌
			suffle();//洗牌
		}
		private void create()
		{
			CardQueue = new Queue<CardDto>();
			//创建普通的牌
			for(int color = CardColor.WAN; color <= CardColor.Wind; color++)
			{
				if(color >= CardColor.WAN && color <= CardColor.BING)   //字牌
				{
					for(int weight = 1; weight < 10; weight++)
					{
						CardQueue.Enqueue(new CardDto(color, weight));
					}
				}
				else if(color == CardColor.Wind)
				{
					for(int weight = 1; weight <= 4; weight++)          //风将
					{
						CardQueue.Enqueue(new CardDto(color, weight));
					}
				}
				else                                                    //花牌
				{
					for(int i = 1; i < 9; i++)
					{
						CardQueue.Enqueue(new CardDto(color, i));
					}
				}


			}
		}
		private void suffle()       //洗牌
		{
			List<CardDto> cardLst = new List<CardDto>();
			Random tRandom = new Random();
			for(int i = 0; i < CardQueue.Count; i++)        //遍历
			{
				int tIndex = tRandom.Next(0, cardLst.Count + 1);        //往已有的牌堆里随机插入
				cardLst.Insert(tIndex, cardLst[i]);
			}
		}
	}

}
