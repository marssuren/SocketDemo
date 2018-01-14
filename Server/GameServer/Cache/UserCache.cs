using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using Server;
using Server.Util.Concurrent;

namespace GameServer.Cache
{
	public class UserCache          //角色数据缓存层
	{
		private Dictionary<int, PlayerModel> idModelDic = new Dictionary<int, PlayerModel>();   //uid 对应的角色数据模型
		private Dictionary<int, int> accountIdDic = new Dictionary<int, int>();        //帐号id，对应的uid
		private Dictionary<int, ClientPeer> idClientDic = new Dictionary<int, ClientPeer>();
		private Dictionary<ClientPeer, int> clientIdDic = new Dictionary<ClientPeer, int>();
		ConcurrentInt id = new ConcurrentInt(-1);       //作为角色的id
		public void Create(ClientPeer _clientPeer, string _name, int _accountId)        //创建角色
		{
			//PlayerModel tPlayerModel = new PlayerModel();
			//idModelDic.Add(tPlayerModel);
			//accountIdDic.Add(tPlayerModel.AccountId, tPlayerModel.Id);
		}
		public bool IsExist(int _accountUid)
		{
			return accountIdDic.ContainsKey(_accountUid);
		}
		public PlayerModel GetModelByAccountId(int _accountId)      //根据帐号Id获取角色信息
		{
			int tUserId = accountIdDic[_accountId];
			return idModelDic[tUserId];
		}
		public int GetId(int _accountId)
		{
			return accountIdDic[_accountId];
		}
		public bool IsOnline(ClientPeer _clientPeer)
		{
			return clientIdDic.ContainsKey(_clientPeer);
		}
		public bool IsOnline(int _id)
		{
			return idClientDic.ContainsKey(_id);
		}
		public void Online(ClientPeer _clientPeer, int _id)     //角色上线
		{
			idClientDic.Add(_id, _clientPeer);
			clientIdDic.Add(_clientPeer, _id);
		}
		public void Offline(ClientPeer _clientPeer)     //角色下线
		{
			int tId = clientIdDic[_clientPeer];
			clientIdDic.Remove(_clientPeer);
			idClientDic.Remove(tId);
		}
		public PlayerModel GetModelByClientPeer(ClientPeer _clientPeer)     //根据连接对象获取角色模型
		{
			int tUserId = clientIdDic[_clientPeer];
			PlayerModel tPlayerModel = idModelDic[tUserId];
			return tPlayerModel;
		}
		public ClientPeer GetClient(int _id)            //根据角色id获取连接对象
		{
			return idClientDic[_id];
		}
		public int GetId(ClientPeer _clientPeer)        //根据在线玩家的连接对象获取角色id
		{
			return clientIdDic[_clientPeer];
		}

	}
}
