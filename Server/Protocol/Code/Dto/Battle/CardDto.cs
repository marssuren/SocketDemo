using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code.Dto
{
	public class CardDto
	{
		public int CardColor;
		public int CardWeight;
		public CardDto(int _cardColor, int _cardWeight)
		{
			CardColor = _cardColor;
			CardWeight = _cardWeight;
		}
	}
}
