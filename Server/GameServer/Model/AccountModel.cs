using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model      //模型层提供给缓存层用
{
	public class AccountModel           //帐号的数据模型
	{
		public int Id;                  //帐号Id
		public string Account;          //用户名
		public string Password;         //密码

		public AccountModel(int _id, string _account, string _password)
		{
			Id = _id;
			Account = _account;
			Password = _password;
		}
	}
}
