using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Code
{
	[Serializable]
	public class AccountDto
	{
		public string Account;
		public string Password;
		public AccountDto()
		{

		}
		public AccountDto(string _account, string _password)
		{
			Account = _account;
			Password = _password;
		}

	}
}
