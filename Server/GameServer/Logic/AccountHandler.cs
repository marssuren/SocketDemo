using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class AccountHandler : IHandler //帐号处理的逻辑层
	{
		public void OnDisconnect(ClientPeer _clientPeer)
		{
			throw new NotImplementedException();
		}

		public void OnReceive(ClientPeer _clientPeer, int _subCode, object _value)
		{
			switch(_subCode)
			{
				case AccountCode.Regist_ClientReq:
				{
					AccountDto tAccountDto = _value as AccountDto;
					Console.WriteLine(tAccountDto.Account);
					Console.WriteLine(tAccountDto.Password);
				}
				break;
				case AccountCode.Login:
				{
					AccountDto tAccountDto = _value as AccountDto;
					Console.WriteLine(tAccountDto.Account);
					Console.WriteLine(tAccountDto.Password);
				}
				break;
				default:
				break;
			}


		}

	}
}




