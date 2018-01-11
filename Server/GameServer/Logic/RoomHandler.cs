using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Cache;
using GameServer.Model;
using Protocol.Code;
using Server;

namespace GameServer.Logic
{
	public class RoomHandler : IHandler
	{
		public void OnReceive(ClientPeer _clientPeer, int _subCode, object _value)
		{
			switch(_subCode)
			{
				case RoomCode.CreateRoom_ClienReq:
				Console.WriteLine("客户端请求创建房间" + _value);
				createRoom(_clientPeer);
				break;
				case RoomCode.EnterRoom_ClientReq:
				Console.WriteLine("客户端请求进入房间" + _value);
				enterRoom(_clientPeer, _value.ToString());
				break;
				case RoomCode.ExitRoom_ClientReq:
				Console.WriteLine("客户端请求退出房间" + _value);
				exitRoom(_clientPeer, _value.ToString());
				break;
			}
		}

		public void OnDisconnect(ClientPeer _clientPeer)
		{

		}
		private void createRoom(ClientPeer _clientPeer)
		{
			RoomModel tRoomModel = Caches.Room.CreateNewRoom();
			Console.WriteLine("创建的房间号为：" + tRoomModel.RoomId);
			_clientPeer.Send(OpCode.ROOM, RoomCode.CreateRoom_ServerRes, tRoomModel.RoomId);
		}
		private void enterRoom(ClientPeer _clientPeer, string _roomId)
		{
			if(Caches.Room.IsRoomExist(_roomId))
			{
				RoomModel tRoomModel = Caches.Room.GetRoom(_roomId);
				_clientPeer.Send(OpCode.ROOM, RoomCode.EnterRoom_SuccessServerRes, tRoomModel.RoomId);
			}
			else
			{
				_clientPeer.Send(OpCode.ROOM, RoomCode.EnterRoom_FailServerRes, "请求加入的房间不存在");
			}
		}
		private void exitRoom(ClientPeer _clientPeer, string _roomId)
		{
			if(Caches.Room.IsRoomExist(_roomId))
			{
				RoomModel tRoomModel = Caches.Room.GetRoom(_roomId);
				_clientPeer.Send(OpCode.ROOM, RoomCode.ExitRoom_ServerRes, tRoomModel.RoomId);
			}
		}
	}
}
