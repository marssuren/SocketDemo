using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Code
{
	public class AccountCode        //注册的操作码
	{
		public const int Regist_ClientReq = 0;      //客户端注册请求	//参数：AccountDto
		public const int Regist_ServerRes = 1;      //服务器注册回应
		public const int Login_ClientReq = 2;       //客户端登录请求	//参数：AccountDto
		public const int Login_ServerRes = 3;       //服务器登录回应
		
	}
}
