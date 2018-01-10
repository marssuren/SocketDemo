using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Cache;
using GameServer.Model;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class AccountHandler : IHandler //帐号处理的逻辑层
	{
		private AccountCache accountCache = Caches.Account;

		private RoomCache roomCache = Caches.Room;


		public void OnDisconnect(ClientPeer _clientPeer)
		{
			accountCache.Offline(_clientPeer);
		}

		public void OnReceive(ClientPeer _clientPeer, int _subCode, object _value)
		{
			switch(_subCode)
			{
				case AccountCode.Regist_ClientReq:
				{
					AccountDto tAccountDto = _value as AccountDto;
					regist(_clientPeer, tAccountDto.Account, tAccountDto.Password);
				}
				break;
				case AccountCode.Login_ClientReq:
				{
					AccountDto tAccountDto = _value as AccountDto;
					login(_clientPeer, tAccountDto.Account, tAccountDto.Password);
				}
				break;
				case AccountCode.CreateRoom_ClientReq:
				{
					Console.WriteLine("客户端请求创建房间" + _value);
					RoomModel tRoomModel = Caches.Room.CreateNewRoom();
					Console.WriteLine("创建的房间号为：" + tRoomModel.RoomId);
					_clientPeer.Send(0, 1001, tRoomModel.RoomId);
				}
				break;
			}


		}
		private void regist(ClientPeer _clientPeer, string _account, string _password)
		{
			SingleExecute.Instance.Excute(() =>             //保证下面的代码同时只能被一个线程访问
			{
				if(accountCache.IsExist(_account))
				{
					//表示帐号已经存在
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "帐号已存在");
					return;
				}
				if(string.IsNullOrEmpty(_account) || string.IsNullOrEmpty(_password))
				{
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "输入不合法");

					return;//表示帐号输入不合法
				}

				if(_password.Length < 4 || _password.Length > 16)
				{
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "密码不合法");

					return; //表示密码长度不正确
				}
				accountCache.CreateAccount(_account, _password);    //可以注册
				_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Regist_ServerRes, "注册成功");
			});


		}

		private void login(ClientPeer _clientPeer, string _account, string _password)
		{
			SingleExecute.Instance.Excute(() => //保证下面的代码同时只能被一个线程访问
			{
				if(!accountCache.IsExist(_account))
				{
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Login_ServerRes, "帐号不存在"); //帐号不存在
					return;
				}

				if(accountCache.IsOnline(_account))
				{
					//帐号在线
					return;
				}

				if(!accountCache.IsMatch(_account, _password))
				{
					//表示密码不匹配
					_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Login_ServerRes, "密码不匹配"); //密码不匹配
					return;
				}

				accountCache.Online(_clientPeer, _account);
				_clientPeer.Send(OpCode.ACCOUNT, AccountCode.Login_ServerRes, "登陆成功"); //登录成功
			});
		}
	}
}




