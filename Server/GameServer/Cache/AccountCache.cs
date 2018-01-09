using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using Server;
using Server.Util.Concurrent;

namespace GameServer.Cache
{
	public class AccountCache           //帐号缓存,后期转数据库处理
	{
		private Dictionary<string, AccountModel> accountModelsDic = new Dictionary<string, AccountModel>();      //帐号id和帐号对应的数据模型的Dic
		private ConcurrentInt id = new ConcurrentInt(-1);   //线程安全的自增
		private Dictionary<string, ClientPeer> accountClientPeersDic = new Dictionary<string, ClientPeer>();            //帐号和帐号连接对象的Dic

		public bool IsOnline(string _account)       //是否在线
		{
			return accountClientPeersDic.ContainsKey(_account);
		}
		public bool IsOnline(ClientPeer _clientPeer)
		{
			return accountClientPeersDic.ContainsValue(_clientPeer);
		}

		public bool IsExist(string _account)        //帐号是否存在
		{
			return accountModelsDic.ContainsKey(_account);
		}
		public void CreateAccount(string _account, string _password)    //创建帐号数据模型信息		
		{
			AccountModel tModel = new AccountModel(id.AddGet(), _account, _password);
			accountModelsDic.Add(tModel.Account, tModel);
		}
		public AccountModel GetAccountModel(string _account)        //获取帐号对应的数据模型
		{
			return accountModelsDic[_account];
		}
		public bool IsMatch(string _account, string _password)      //帐号密码是否匹配
		{
			return accountModelsDic[_account].Password == _password;
		}
		public void Online(ClientPeer _clientPeer, string _account) //用户上线
		{
			accountClientPeersDic.Add(_account, _clientPeer);
		}
		public void Offline(ClientPeer _clientPeer, string _account)
		{
			accountClientPeersDic.Remove(_account);
		}
		public string GetId(ClientPeer _clientPeer)     //获取在线玩家的Id
		{
			foreach(var item in accountClientPeersDic)
			{
				if(item.Value == _clientPeer)
				{
					return item.Key;
				}
			}
			return "";
		}
	}
}
