using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	class Program
	{


	}





	public enum Protocol            //后端协议号，前端需要保持一致
	{
		LogIn = 0,                  //登录
		CreateRoom = 1,             //创建房间
		EnterRoom = 2,              //进入房间
		Ready = 3,                  //准备
		StartGame = 4,              //开始游戏
		DisbandRoom = 5,            //解散房间
		CreateTeahouse = 6,         //创建茶馆
		EnterTeahouse = 7,          //加入茶馆
		CheckRecord = 8,            //查看战绩
		LogOff = 9,                 //退出登录

	}
}



