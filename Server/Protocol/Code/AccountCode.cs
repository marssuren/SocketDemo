using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Code
{
	public class AccountCode        //注册的操作码
	{
		public const int Regist_ClientReq = 0;      //client request	//参数：AccountDto
		public const int Regist_ServerRes = 1;      //server response
		public const int Login = 2;                //登录的操作码		//参数：AccountDto

		public const int Ready_ClientReq = 101;		//客户端请求

		public const int CreateRoom_ClientReq = 1000;		//客户端创建房间请求
		public const int CreateRoom_ClientRes = 1001;		//服务器创建房间回应
	}
}
