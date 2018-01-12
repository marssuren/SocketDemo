using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Cache;
using GameServer.Model;
using Newtonsoft.Json.Linq;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class PlayerHandler : IHandler //帐号处理的逻辑层
	{
		private PlayerCache playerCache = Caches.Player;

		private RoomCache roomCache = Caches.Room;


		public void OnDisconnect(ClientPeer _clientPeer)
		{
			playerCache.Offline(_clientPeer);
		}

		public void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _value)
		{
			switch(_subCode)
			{
				//case AccountCode.Regist_ClientReq:
				//{
				//	AccountDto tAccountDto = _value as AccountDto;
				//	regist(_clientPeer, tAccountDto.Account, tAccountDto.Password);
				//}
				//break;
				//case AccountCode.Login_ClientReq:
				//{
				//	AccountDto tAccountDto = _value as AccountDto;
				//	login(_clientPeer, tAccountDto.Account, tAccountDto.Password);
				//}
				//break;

			}


		}
		private void regist(ClientPeer _clientPeer, string _uid, string _password)
		{
			SingleExecute.Instance.Excute(() =>             //保证下面的代码同时只能被一个线程访问
			{
				if(playerCache.IsExist(_uid))
				{
					//表示帐号已经存在
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "帐号已存在");
					return;
				}
				if(string.IsNullOrEmpty(_uid) || string.IsNullOrEmpty(_password))
				{
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "输入不合法");

					return;//表示帐号输入不合法
				}

				if(_password.Length < 4 || _password.Length > 16)
				{
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "密码不合法");

					return; //表示密码长度不正确
				}
				//playerCache.CreatePlayer(_uid, _password);    //可以注册
				_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "注册成功");
			});


		}

		private void login(ClientPeer _clientPeer, string _account, string _password)
		{
			SingleExecute.Instance.Excute(() => //保证下面的代码同时只能被一个线程访问
			{
				if(!playerCache.IsExist(_account))
				{
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Login_ServerRes, "帐号不存在"); //帐号不存在
					return;
				}

				if(playerCache.IsOnline(_account))
				{
					//帐号在线
					return;
				}

				//if(!playerCache.IsMatch(_account, _password))
				//{
				//	//表示密码不匹配
				//	_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Login_ServerRes, "密码不匹配"); //密码不匹配
				//	return;
				//}

				playerCache.Online(_clientPeer, _account);
				_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Login_ServerRes, "登陆成功"); //登录成功
			});
		}
		
	}
}




