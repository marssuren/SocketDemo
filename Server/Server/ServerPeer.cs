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
		Connect = 101,                  //登录
		Exception = 102,                //异常掉线
		Disconnect = 103,               //正常断线
		Message = 104,                  //接收消息
		CreateRoom = 105,               //创建房间
		EnterRoom = 106,                //进入房间
		Ready = 107,                    //准备
		StartGame = 108,                //开始游戏
		DisbandRoom = 109,              //解散房间
		CreateTeahouse = 110,           //创建茶馆
		EnterTeahouse = 111,            //加入茶馆
		CheckRecord = 112,              //查看战绩
		LogOff = 113,                   //退出登录

	}
}



