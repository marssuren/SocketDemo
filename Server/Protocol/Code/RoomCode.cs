using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
	public class RoomCode
	{
		public const int CreateRoom_ClienReq = 1200;        //创建房间客户端请求
		public const int CreateRoom_ServerRes = 1201;       //创建房间服务器回应
		public const int EnterRoom_ClientReq = 1202;        //进入房间客户端请求
		public const int EnterRoom_SuccessServerRes = 1203; //进入房间成功服务器回应
		public const int EnterRoom_FailServerRes = 1206;    //进入房间失败服务器回应
		public const int ExitRoom_ClientReq = 1204;         //退出房间客户端请求
		public const int ExitRoom_ServerRes = 1205;         //退出房间服务器回应
	}
}
