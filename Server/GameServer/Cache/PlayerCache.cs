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
	public class PlayerCache           //玩家信息缓存,后期转数据库处理
	{
		private Dictionary<string, PlayerModel> playerModelsDic = new Dictionary<string, PlayerModel>();      //玩家uid和帐号对应的数据模型的Dic
		private ConcurrentInt id = new ConcurrentInt(-1);   //线程安全的自增
		private Dictionary<string, ClientPeer> playerClientPeersDic = new Dictionary<string, ClientPeer>();            //帐号和帐号连接对象的Dic

		public bool IsOnline(string _uid)       //是否在线
		{
			return playerClientPeersDic.ContainsKey(_uid);
		}
		public bool IsOnline(ClientPeer _clientPeer)
		{
			return playerClientPeersDic.ContainsValue(_clientPeer);
		}

		public bool IsExist(string _uid)        //玩家是否存在
		{
			return playerModelsDic.ContainsKey(_uid);
		}
		public void CreatePlayer(string _uid,string _nickname)    //创建玩家数据模型信息		
		{
			PlayerModel tPlayer = new PlayerModel(id.AddGet().ToString(),_nickname);
			playerModelsDic.Add(tPlayer.Id, tPlayer);
		}
		public PlayerModel GetPlayerModel(string _uid)        //获取玩家对应的数据模型
		{
			return playerModelsDic[_uid];
		}
		public void Online(ClientPeer _clientPeer, string _uid) //用户上线
		{
			playerClientPeersDic.Add(_uid, _clientPeer);
		}
		public void Offline(ClientPeer _clientPeer)
		{
			foreach (var item in playerClientPeersDic)
			{
				if (item.Value==_clientPeer)
				{
					playerClientPeersDic.Remove(item.Key);
				}
			}
		}
		public string GetId(ClientPeer _clientPeer)     //获取在线玩家的Id
		{
			foreach(var item in playerClientPeersDic)
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
