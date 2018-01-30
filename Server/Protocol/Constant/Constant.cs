using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
	public class Constant
	{
		private static Dictionary<int, string> typeContentDic = new Dictionary<int, string>();//聊天的类型和对应的文字的Dictionary
		public Constant()
		{
			typeContentDic = new Dictionary<int, string>();
			typeContentDic.Add(1, "大家好。。。");
			typeContentDic.Add(2, "和你合作真是太愉快了");
			typeContentDic.Add(3, "快点吧");
			typeContentDic.Add(4, "你的牌打的太好了");
			typeContentDic.Add(5, "不要吵了");
			typeContentDic.Add(6, "不要走");
			typeContentDic.Add(7, "再见了");
		}
		public static string GetChatText(int _chatType)
		{
			return typeContentDic[_chatType];
		}
	}
}
