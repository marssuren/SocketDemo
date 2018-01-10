using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientPeer
    {
		public Socket ClientSocket;
		private Queue<byte[]> sendQueue = new Queue<byte[]>();          //发送的消息队列 
		private bool isSendProcessing = false;
		/// <summary>
		/// 一旦接收到数据，就存到缓存区
		/// </summary>
		private List<byte> dataCache = new List<byte>();

		public SocketAsyncEventArgs ReceiveArgs;                        //接收的异步套接字请求
		private bool isReceiveProcessing;                               //是否正在处理接收的数据

		public delegate void ReceiveCompleted(ClientPeer _clientPeer, SocketMessage _socketMessage);

		public ReceiveCompleted ReceiveCompletedDel;                    //消息解析完成的回调
		private SocketAsyncEventArgs sendArgs;                          //发送的异步套接字请求

		public delegate void SendDisconnectDel(ClientPeer _clientPeer, string _reason);    //发送时发现断开连接的回调

		public SendDisconnectDel SendDisconnect;
		public ClientPeer()
		{
			ReceiveArgs = new SocketAsyncEventArgs();           //分配一个新 SocketAsyncEventArgs 上下文对象，或获取一个空闲从应用程序池。
			ReceiveArgs.UserToken = this;                       //获取或设置与此异步套接字操作关联的用户或应用程序对象。
			ReceiveArgs.SetBuffer(new byte[1024], 0, 1024);     //设置要与异步套接字方法一起使用的数据缓冲区(客户端发送消息至服务端，服务端要把收到的字节存储进一个变量，然后反序列化转化为消息)
			sendArgs = new SocketAsyncEventArgs();				
			sendArgs.Completed += sendArgsCompleted;
		}
		public void StartReceive(byte[] _packet)            //自身处理数据包
		{
			dataCache.AddRange(_packet);
			if(!isReceiveProcessing)
			{
				processReceive();
			}
		}

		private void processReceive()           //处理接收的数据
		{
			isReceiveProcessing = true;
			byte[] tData = EncodeTool.DecodePacket(ref dataCache);      //解析数据包
			if(null == tData)
			{
				isReceiveProcessing = false;
				return;
			}
			SocketMessage tSocketMessage = EncodeTool.DecodeMessage(tData);		//将接收到的数据解析为能用的SocketMessage

			//回调给上层(sever端)
			if(null != ReceiveCompletedDel)
			{
				ReceiveCompletedDel(this, tSocketMessage);
			}
			processReceive();       //尾递归
		}
		//粘包拆包问题：解决策略：消息头和消息尾
		//比如：发送的数据：12345
		//private void test()
		//{
		//    byte[] tbytes = Encoding.Default.GetBytes("12345");
		//    //消息头：消息的长度 int     bt.Length
		//    //尾：具体的消息             bt
		//    int tBytesLength = tbytes.Length;
		//    byte[] tByteHead = BitConverter.GetBytes(tBytesLength);

		//}
		public void Disconnect() //断开连接
		{
			try
			{
				dataCache.Clear();//清空数据
				isReceiveProcessing = false;
				sendQueue.Clear();
				isSendProcessing = false;
				ClientSocket.Shutdown(SocketShutdown.Both);
				ClientSocket.Close();
				ClientSocket = null;
			}
			catch(Exception _exception)
			{
				Console.WriteLine(_exception.Message);
			}
		}


		public void Send(int _opCode, int _subCode, object _value)              //发送网络消息；操作码，子操作，参数
		{
			SocketMessage tSocketMessage = new SocketMessage(_opCode, _subCode, _value);
			byte[] tData = EncodeTool.EncodeMessage(tSocketMessage);
			byte[] tPacket = EncodeTool.EncodePacket(tData);
			sendQueue.Enqueue(tPacket);     //存入消息队列中
			if(!isSendProcessing)      //如果没有处理
			{
				send();
			}
		}

		private void send()      //处理发送的消息
		{
			isSendProcessing = true;
			if(sendQueue.Count == 0)       //数据条数为0，停止发送
			{
				isSendProcessing = false;
				return;
			}
			byte[] tPacket = sendQueue.Dequeue();       //取出一条数据
														
			sendArgs.SetBuffer(tPacket, 0, tPacket.Length);     //设置消息发送异步套接字操作的发送数据缓冲区
			bool tResult = ClientSocket.SendAsync(sendArgs);
			if(tResult == false)
			{
				processSend();
			}
		}

		private void sendArgsCompleted(object _sender, SocketAsyncEventArgs _eventArgs)
		{
			processSend();
		}
		private void processSend()     //异步发送请求完成时调用
		{
			if(sendArgs.SocketError != SocketError.Success)  //发送的有没有错误
			{
				//发送出错   客户端断开连接
				SendDisconnect(this, sendArgs.SocketError.ToString());
			}
			else
			{
				send();
			}
		}
		//public Socket ClientSocket
		//{
		//	get; set;
		//}

		//public ClientPeer()
		//{
		//	this.ReceiveArgs = new SocketAsyncEventArgs();
		//	this.ReceiveArgs.UserToken = this;
		//	this.ReceiveArgs.SetBuffer(new byte[1024], 0, 1024);
		//	this.SendArgs = new SocketAsyncEventArgs();
		//	this.SendArgs.Completed += SendArgs_Completed;
		//}


		//#region 接收数据

		//public delegate void ReceiveCompleted(ClientPeer client, SocketMessage msg);

		///// <summary>
		///// 一个消息解析完成的回调
		///// </summary>
		//public ReceiveCompleted receiveCompleted;

		///// <summary>
		///// 一旦接收到数据 就存到缓存区里面
		///// </summary>
		//private List<byte> dataCache = new List<byte>();

		///// <summary>
		///// 接受的异步套接字请求
		///// </summary>
		//public SocketAsyncEventArgs ReceiveArgs
		//{
		//	get; set;
		//}

		///// <summary>
		///// 是否正在处理接受的数据
		///// </summary>
		//private bool isReceiveProcess = false;

		///// <summary>
		///// 自身处理数据包
		///// </summary>
		///// <param name="packet"></param>
		//public void StartReceive(byte[] packet)
		//{
		//	dataCache.AddRange(packet);
		//	if(!isReceiveProcess)
		//		processReceive();
		//}

		///// <summary>
		///// 处理接受的数据
		///// </summary>
		//private void processReceive()
		//{
		//	isReceiveProcess = true;
		//	//解析数据包
		//	byte[] data = EncodeTool.DecodePacket(ref dataCache);

		//	if(data == null)
		//	{
		//		isReceiveProcess = false;
		//		return;
		//	}

		//	SocketMessage msg = EncodeTool.DecodeMessage(data);
		//	//回调给上层
		//	if(receiveCompleted != null)
		//		receiveCompleted(this, msg);

		//	//尾递归
		//	processReceive();
		//}

		//////粘包拆包问题 ： 解决决策 ：消息头和消息尾。
		////// 比如 发送的数据：  12345
		////void test()
		////{
		////    byte[] bt = Encoding.Default.GetBytes("12345");

		////    //怎么构造
		////    //头：消息的长度 bt.Length
		////    //尾：具体的消息 bt
		////    int length = bt.Length;
		////    byte[] bt1 = BitConverter.GetBytes(length);
		////    //得到消息就是：bt1 + bt

		////    ///怎么读取
		////    //int length = 前四个字节转成int类型
		////    //然后读取 这个长度的数据
		////}

		//#endregion

		//#region 断开连接

		///// <summary>
		///// 断开连接
		///// </summary>
		//public void Disconnect()
		//{
		//	//清空数据
		//	dataCache.Clear();
		//	isReceiveProcess = false;
		//	sendQueue.Clear();
		//	isSendProcess = false;

		//	ClientSocket.Shutdown(SocketShutdown.Both);
		//	ClientSocket.Close();
		//	ClientSocket = null;
		//}

		//#endregion

		//#region 发送数据

		///// <summary>
		///// 发送的消息的一个队列
		///// </summary>
		//private Queue<byte[]> sendQueue = new Queue<byte[]>();

		//private bool isSendProcess = false;

		///// <summary>
		///// 发送的异步套接字操作
		///// </summary>
		//private SocketAsyncEventArgs SendArgs;

		///// <summary>
		///// 发送的时候 发现 断开连接的回调
		///// </summary>
		///// <param name="client"></param>
		///// <param name="reason"></param>
		//public delegate void SendDisconnect(ClientPeer client, string reason);

		//public SendDisconnect sendDisconnect;


		///// <summary>
		///// 发送网络消息
		///// </summary>
		///// <param name="opCode">操作码</param>
		///// <param name="subCode">子操作</param>
		///// <param name="value">参数</param>
		//public void Send(int opCode, int subCode, object value)
		//{
		//	SocketMessage msg = new SocketMessage(opCode, subCode, value);
		//	byte[] data = EncodeTool.EncodeMessage(msg);
		//	byte[] packet = EncodeTool.EncodePacket(data);

		//	Send(packet);
		//}


		//public void Send(byte[] packet)
		//{
		//	//存入消息队列里面
		//	sendQueue.Enqueue(packet);
		//	if(!isSendProcess)
		//		send();
		//}

		///// <summary>
		///// 处理发送的消息
		///// </summary>
		//private void send()
		//{
		//	isSendProcess = true;

		//	//如果数据的条数等于0的话 就停止发送
		//	if(sendQueue.Count == 0)
		//	{
		//		isSendProcess = false;
		//		return;
		//	}
		//	//取出一条数据
		//	byte[] packet = sendQueue.Dequeue();
		//	//设置消息 发送的异步套接字操作 的发送数据缓冲区
		//	SendArgs.SetBuffer(packet, 0, packet.Length);
		//	bool result = ClientSocket.SendAsync(SendArgs);
		//	if(result == false)
		//	{
		//		processSend();
		//	}
		//}

		//private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
		//{
		//	processSend();
		//}

		///// <summary>
		///// 当异步发送请求完成的时候调用
		///// </summary>
		//private void processSend()
		//{
		//	//发送的有没有错误
		//	if(SendArgs.SocketError != SocketError.Success)
		//	{
		//		//发送出错了 客户端断开连接了
		//		sendDisconnect(this, SendArgs.SocketError.ToString());
		//	}
		//	else
		//	{
		//		send();
		//	}
		//}

		//#endregion
	}
}
