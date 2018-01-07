using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
	public class AccountCode        //注册的操作码
	{
		public const int Regist_ClientReq = 0;      //client request	//参数：AccountDto
		public const int Regist_ServerRes = 1;      //server response
		public const int Login = 2;                //登录的操作码		//参数：AccountDto


	}
}
