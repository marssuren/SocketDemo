using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.UserToken = this;
            sendArgs = new SocketAsyncEventArgs();
            sendArgs.Completed += sendArgsCompleted;
        }


        public void StartReceive(byte[] _packet)            //自身处理数据包
        {
            dataCache.AddRange(_packet);
            if (!isReceiveProcessing)
            {
                processReceive();
            }
        }

        private void processReceive()           //处理接收的数据
        {
            isReceiveProcessing = true;
            byte[] tData = EncodeTool.DecodePacket(ref dataCache);      //解析数据包
            if (null == tData)
            {
                isReceiveProcessing = false;
                return;
            }
            SocketMessage tSocketMessage = EncodeTool.DecodeMessage(tData);      //todo:需要再次转成一个具体的类型供我们使用

            //回调给上层(sever端)
            if (null != ReceiveCompletedDel)
            {
                ReceiveCompletedDel(this, tSocketMessage);
            }
            processReceive();       //尾递归
        }
        //粘包拆包问题：解决策略：消息头和消息尾
        //比如：发送的数据：12345
        private void test()
        {
            byte[] tbytes = Encoding.Default.GetBytes("12345");
            //消息头：消息的长度 int     bt.Length
            //尾：具体的消息             bt
            int tBytesLength = tbytes.Length;
            byte[] tByteHead = BitConverter.GetBytes(tBytesLength);

        }

        public void Disconnect() //断开连接
        {
            try
            {
                dataCache.Clear();//清空数据
                isReceiveProcessing = false;
                //todo:清空发送的数据
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
                ClientSocket = null;
            }
            catch (Exception _exception)
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
            if (!isSendProcessing)      //如果没有处理
            {
                send();
            }
        }

        private void send()      //处理发送的消息
        {
            isSendProcessing = true;
            if (sendQueue.Count == 0)       //数据条数为0，停止发送
            {
                isSendProcessing = false;
                return;
            }
            byte[] tPacket = sendQueue.Dequeue();       //取出一条数据
            //设置消息发送异步套接字操作的发送数据缓冲区
            sendArgs.SetBuffer(tPacket, 0, tPacket.Length);
            bool tResult = ClientSocket.SendAsync(sendArgs);
            if (tResult == false)
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
            if (sendArgs.SocketError != SocketError.Success)  //发送的有没有错误
            {
                //发送出错   客户端断开连接
                SendDisconnect(this, sendArgs.SocketError.ToString());
            }
            else
            {
                send();
            }
        }
    }
}
