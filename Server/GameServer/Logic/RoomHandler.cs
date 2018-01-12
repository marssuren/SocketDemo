using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Cache;
using GameServer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class RoomHandler : IHandler
	{
		public void OnReceive(ClientPeer _clientPeer, int _subCode, JObject _jObject)
		{
			switch(_subCode)
			{
				case RoomCode.CreateRoom_ClienReq:

				string tRoomType = _jObject["RoomType"].ToString();
				string tRule = _jObject["Rule"].ToString();
				bool tIsFloatFlower = (bool)_jObject["IsFloatFlower"];
				int tRoundCount = (int)_jObject["RoundCount"];
				string tPayType = _jObject["PayType"].ToString();

				Console.WriteLine("客户端请求创建房间" + "房间类型：" + tRoomType + "规则:" + tRule + "是否飘花:" + tIsFloatFlower + "轮数：" + tRoundCount + "支付类型：" + tPayType);

				createRoom(_clientPeer, tRoomType, tRule, tIsFloatFlower, tRoundCount, tPayType);

				break;
				case RoomCode.EnterRoom_ClientReq:
				Console.WriteLine("客户端请求进入房间" + _jObject["RoomId"]);
				enterRoom(_clientPeer, _jObject["RoomId"].ToString());
				break;
				case RoomCode.ExitRoom_ClientReq:
				Console.WriteLine("客户端请求退出房间" + _jObject["RoomId"]);
				exitRoom(_clientPeer, _jObject["RoomId"].ToString());
				break;
				case RoomCode.RequestRoomPlayers_ClientReq:
				string tPlayerId = _jObject["PlayerId"].ToString();
				string tRoomId = _jObject["RoomId"].ToString();
				Console.WriteLine("客户端请求同房间内玩家信息,请求者id：" + tPlayerId + "房间号:" + tRoomId);

				break;


			}
		}

		public void OnDisconnect(ClientPeer _clientPeer)
		{

		}
		private void createRoom(ClientPeer _clientPeer, string _roomType, string _rule, bool _isFloatFlower, int _roundCount, string _payType)     //创建房间
		{
			RoomModel tRoomModel = Caches.Room.CreateNewRoom(_roomType, _rule, _isFloatFlower, _roundCount, _payType);
			Console.WriteLine("创建的房间号为：" + tRoomModel.RoomId);
			var tServerResMsg = new
			{
				RoomId = tRoomModel.RoomId,
			};
			string tJsonStr = JsonConvert.SerializeObject(tServerResMsg);
			_clientPeer.Send(OpCode.ROOM, RoomCode.CreateRoom_ServerRes, tJsonStr);
		}
		private void enterRoom(ClientPeer _clientPeer, string _roomId)			//进入房间
		{
			if(Caches.Room.IsRoomExist(_roomId))
			{
				if(Caches.Room.GetRoomPlayerCount(_roomId) < 4)					//房间内玩家数小于4
				{
					int tSeatNumber = Caches.Room.GetFreeSeat(_roomId);			//获取房间内空闲座位号
					string tPlayerId = Caches.Player.GetId(_clientPeer);		//获取玩家Id
					PlayerModel tPlayerModel = Caches.Player.GetPlayerModel(tPlayerId); //获取玩家
					Caches.Room.EnterRoom(_roomId, tSeatNumber,tPlayerModel);	//玩家入座



					_clientPeer.Send(OpCode.ROOM, RoomCode.EnterRoom_SuccessServerRes, );
				}
				else
				{
					var tServerResMsg = new
					{
						ErrorMsg = "请求加入的房间已满员",
					};
					string tJsonStr = JsonConvert.SerializeObject(tServerResMsg);
					_clientPeer.Send(OpCode.ROOM, RoomCode.EnterRoom_FailServerRes, tJsonStr);
				}
			}
			else
			{
				var tServerResMsg = new
				{
					ErrorMsg = "请求加入的房间不存在",
				};
				string tJsonStr = JsonConvert.SerializeObject(tServerResMsg);
				_clientPeer.Send(OpCode.ROOM, RoomCode.EnterRoom_FailServerRes, tJsonStr);
			}
		}
		private void exitRoom(ClientPeer _clientPeer, string _roomId)       //退出房间
		{
			if(Caches.Room.IsRoomExist(_roomId))
			{
				RoomModel tRoomModel = Caches.Room.GetRoom(_roomId);
				_clientPeer.Send(OpCode.ROOM, RoomCode.ExitRoom_ServerRes, tRoomModel.RoomId);
			}
		}
		private void getPlayersInfoInRoom(ClientPeer _clientPeer, string _playerId, string _roomId) //获取指定房间内玩家的信息
		{

		}
	}
}
