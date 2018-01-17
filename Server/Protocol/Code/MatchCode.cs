using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
	public class MatchCode      //有关匹配的操作码
	{
		//Bro:需要广播给房间内所有玩家的操作码
		//进入匹配队列
		public const int EnterMatch_ClientReq = 1600;
		public const int EnterMatch_ServerRes = 1601;

		public const int EnterMatch_ServerBro = 1608;
		//离开匹配队列
		public const int LeaveMatch_ClientReq = 1602;
		public const int LeaveMatch_ServerBro = 1603;
		//准备
		public const int ReadyMatch_ClientReq = 1604;
		public const int ReadyMatch_ServerBro = 1605;       //收到准备请求时给所有房间内的玩家发送广播
															//开始游戏
		public const int StartBattle_ServerBro = 1607;
	}
}
