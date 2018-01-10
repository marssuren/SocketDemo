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
		private RoomCache roomCache = Caches.Room;


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

				case AccountCode.CreateRoom_ClientReq:
				Console.WriteLine("客户端请求创建房间" + _value);
				RoomModel tRoomModel = Caches.Room.CreateNewRoom();
				Console.WriteLine("创建的房间号为：" + tRoomModel.RoomId);
				_clientPeer.Send(0, 1001, tRoomModel.RoomId);
				break;

				//case 0:
				//	Console.WriteLine(_value.ToString());
				//break;
				default:
				break;
			}


		}

	}
}




