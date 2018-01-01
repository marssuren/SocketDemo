using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientPeer
    {
        public Socket ClientSocket;

        public ClientPeer()
        {
            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.UserToken = this;
        }
        /// <summary>
        /// 一旦接收到数据，就存到缓存区
        /// </summary>
        private List<byte> dataCache = new List<byte>();

        public SocketAsyncEventArgs ReceiveArgs;        //接收的异步套接字请求
        private bool isProcessing;          //是否正在处理接收的数据

        public delegate void ReceiveCompleted(ClientPeer _clientPeer, SocketMessage _socketMessage);

        public ReceiveCompleted ReceiveCompletedDel;            //消息解析完成的回调


        public void StartReceive(byte[] _packet)            //自身处理数据包
        {
            dataCache.AddRange(_packet);
            if (!isProcessing)
            {
                processReceive();
            }
        }

        private void processReceive()           //处理接收的数据
        {
            isProcessing = true;
            byte[] tData = EncodeTool.DecodePacket(ref dataCache);      //解析数据包
            if (null == tData)
            {
                isProcessing = false;
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
                isProcessing = false;

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


    }
}
