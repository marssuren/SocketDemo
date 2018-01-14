using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Cache;
using GameServer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class PlayerHandler : IHandler //玩家信息处理的逻辑层
	{
		private PlayerCache playerCache = Caches.Player;
		private RoomCache roomCache = Caches.Room;
		public void OnDisconnect(ClientPeer _clientPeer)
		{
			if(playerCache.IsOnline(_clientPeer))
			{
				playerCache.Offline(_clientPeer);
			}
		}
		public void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _value)
		{

			switch(_subCode)
			{
				case PlayerCode.GetInfo_ClientReq:
				string tUid = _value["uid"].ToString();
				getInfo(_clientPeer, tUid);
				break;
				case PlayerCode.Online_ClientReq:
				online(_clientPeer);
				break;
			}
		}
		private void regist(ClientPeer _clientPeer, string _uid, string _password)
		{
			SingleExecute.Instance.Excute(() =>             //保证下面的代码同时只能被一个线程访问
			{
				#region 注册相关(暂时不需要)
				//if(playerCache.IsExist(_uid))
				//{
				//	//表示帐号已经存在
				//	_clientPeer.Send(OpCode.ACCOUNT, PlayerCode.Regist_ServerRes, "帐号已存在");
				//	return;
				//}
				//if(string.IsNullOrEmpty(_uid) || string.IsNullOrEmpty(_password))
				//{
				//	_clientPeer.Send(OpCode.ACCOUNT, PlayerCode.Regist_ServerRes, "输入不合法");

				//	return;//表示帐号输入不合法
				//}

				//if(_password.Length < 4 || _password.Length > 16)
				//{
				//	_clientPeer.Send(OpCode.ACCOUNT, PlayerCode.Regist_ServerRes, "密码不合法");

				//	return; //表示密码长度不正确
				//}
				////playerCache.CreatePlayer(_uid, _password);    //可以注册
				//_clientPeer.Send(OpCode.ACCOUNT, PlayerCode.Regist_ServerRes, "注册成功"); 
				#endregion

			});
		}

		private void login(ClientPeer _clientPeer, string _account, string _password)
		{
			SingleExecute.Instance.Excute(() => //保证下面的代码同时只能被一个线程访问
			{
				//if(!playerCache.IsExist(_account))
				//{
				//	_clientPeer.Send(OpCode.ACCOUNT, PlayerCode.Login_ServerRes, "帐号不存在"); //帐号不存在
				//	return;
				//}

				//if(playerCache.IsOnline(_account))
				//{
				//	//帐号在线
				//	return;
				//}

				//playerCache.Online(_clientPeer, _account);
				//_clientPeer.Send(OpCode.ACCOUNT, PlayerCode.Login_ServerRes, "登陆成功"); //登录成功
			});
		}
		private void getInfo(ClientPeer _clientPeer, string _uid)        //获取角色的信息
		{
			SingleExecute.Instance.Excute(() =>
			{
				string tStr;
				if(playerCache.IsOnline(_clientPeer))
				{
					var tResData = new
					{
						ErrorMsg = "非法登录",
					};
					tStr = JsonConvert.SerializeObject(tResData);
					_clientPeer.Send(OpCode.PLAYER, PlayerCode.GetInfo_ServerRes, tStr);
					return;
				}
				string tUserId = playerCache.GetId(_clientPeer);
				if(playerCache.IsExist(tUserId) == false)
				{
					var tResData = new
					{
						ErrorMsg = "角色不存在",
					};
					tStr = JsonConvert.SerializeObject(tResData);
					_clientPeer.Send(OpCode.PLAYER, PlayerCode.GetInfo_ServerRes, tStr);
					return;
				}
				PlayerModel tPlayerModel = playerCache.GetPlayerModel(_uid);
				tStr = JsonConvert.SerializeObject(tPlayerModel);
				_clientPeer.Send(OpCode.PLAYER, PlayerCode.GetInfo_ServerRes, tStr);        //获取成功	
			});
		}
		private void online(ClientPeer _clientPeer)
		{
			SingleExecute.Instance.Excute(() =>
			{
				string tStr;
				if(!playerCache.IsOnline(_clientPeer))
				{
					var tResData = new
					{
						ErrorMsg = "非法登录",
					};
					tStr = JsonConvert.SerializeObject(tResData);
					_clientPeer.Send(OpCode.PLAYER, PlayerCode.Online_ServerRes, tStr);
					return;
				}
				string tUid = playerCache.GetId(_clientPeer);
				if(playerCache.IsExist(tUid) == false)
				{
					var tResData = new
					{
						ErrorMsg = "没有角色",
					};
					tStr = JsonConvert.SerializeObject(tResData);
					_clientPeer.Send(OpCode.PLAYER, PlayerCode.Online_ServerRes, tStr); //角色不存在，不能上线
					return;
				}
				playerCache.Online(_clientPeer, tUid);      //成功上线 
				var tSuccessData = new
				{
					SuccessRes = "上线成功",
				};
				tStr = JsonConvert.SerializeObject(tSuccessData);
				_clientPeer.Send(OpCode.PLAYER, PlayerCode.Online_ServerRes, tStr);
			});

		}
	}
}




