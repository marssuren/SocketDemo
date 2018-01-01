using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class EncodeTool      //关于编码的工具类
    {
        #region 解决粘包拆包问题，封装一个有规定的数据包

        /// <summary>
        /// 构造消息包：包头+包尾
        /// </summary>
        /// <returns></returns>
        public static byte[] EncodePacket(byte[] _data)
        {
            using (MemoryStream tMemoryStream = new MemoryStream())//内存流对象
            {
                using (BinaryWriter tBinaryWriter = new BinaryWriter(tMemoryStream))
                {
                    tBinaryWriter.Write(_data.Length);     //先写入长度
                    tBinaryWriter.Write(_data);            //再写入数据
                    byte[] tByteArray = new byte[(int)tMemoryStream.Length];
                    Buffer.BlockCopy(tMemoryStream.GetBuffer(), 0, tByteArray, 0, (int)tMemoryStream.Length);
                    return tByteArray;
                }
            }
        }
        /// <summary>
        /// 解析消息体   从缓存里取出一个一个完整的包
        /// </summary>
        /// <returns></returns>
        public static byte[] DecodePacket(ref List<byte> _dataCache)
        {
            if (_dataCache.Count < 4)
            {
                return null;
                //throw new Exception("数据缓存长度不足4，不能构成完整消息");//4个字节构成一个int长度，小于4不能构成一个完整的消息
            }
            using (MemoryStream tMemoryStream = new MemoryStream(_dataCache.ToArray()))//内存流对象
            {
                using (BinaryReader tBinaryReader = new BinaryReader(tMemoryStream))
                {
                    int tLength = tBinaryReader.ReadInt32();
                    int tDataRemainLength = (int)(tMemoryStream.Length - tMemoryStream.Position);
                    if (tLength > tDataRemainLength)
                    {
                        throw new Exception("数据长度短于包头约定的长度，不能构成一个完整的消息");
                    }
                    byte[] tData = tBinaryReader.ReadBytes(tLength);        //读取完后更新缓存区
                    _dataCache.Clear();
                    _dataCache.AddRange(tBinaryReader.ReadBytes(tDataRemainLength));
                    return tData;
                }
            }

        }
        #endregion

        public static byte[] EncodeMessage(SocketMessage _socketMessage) //把SocketMessage类转换成字节数组，发送出去
        {
            MemoryStream tMemoryStream = new MemoryStream();
            BinaryWriter tBinaryWriter = new BinaryWriter(tMemoryStream);
            tBinaryWriter.Write(_socketMessage.OPCode);
            tBinaryWriter.Write(_socketMessage.SubCode);
            if (null != _socketMessage)     //不为空，才需要把Object类型转为字节数组存起来
            {
                byte[] tByteValues=EncodeObject(_socketMessage.Value);
                tBinaryWriter.Write(tByteValues);
            }
            byte[] tData = new byte[tMemoryStream.Length];
            Buffer.BlockCopy(tMemoryStream.GetBuffer(), 0, tData, 0, (int)tMemoryStream.Length);
            tBinaryWriter.Close();
            tMemoryStream.Close();
            return tData;
        }

        public static SocketMessage DecodeMessage(byte[] _data)     //将收到的字节数组转换成socketMessage对象
        {
            MemoryStream tMemoryStream = new MemoryStream(_data);
            BinaryReader tBinaryReader = new BinaryReader(tMemoryStream);
            SocketMessage tSocketMessage = new SocketMessage();
            tSocketMessage.OPCode = tBinaryReader.ReadInt32();
            tSocketMessage.SubCode = tBinaryReader.ReadInt32();
            if (tMemoryStream.Length > tMemoryStream.Position)            //还有剩余的字节未读完，代表value有值
            {
                byte[] tBytesValue = tBinaryReader.ReadBytes((int)(tMemoryStream.Length - tMemoryStream.Position));
                object tValue=DecodeObj(tBytesValue);
                tSocketMessage.Value = tValue;
            }
            tBinaryReader.Close();
            tMemoryStream.Close();
            return tSocketMessage;
        }

        //把object类型转换为byte[]
        public static byte[] EncodeObject(object _value)        //序列化对象
        {
            using (MemoryStream tMemoryStream = new MemoryStream())
            {
                BinaryFormatter tBinaryFormatter = new BinaryFormatter();
                tBinaryFormatter.Serialize(tMemoryStream, _value);
                byte[] tBytesValue = new byte[tMemoryStream.Length];
                Buffer.BlockCopy(tMemoryStream.GetBuffer(), 0, tBytesValue, 0, (int)tMemoryStream.Length);
                return tBytesValue;
            }
        }

        public static object DecodeObj(byte[] _bytesValue)      //反序列化对象
        {
            using (MemoryStream tMemoryStream = new MemoryStream(_bytesValue))
            {
                BinaryFormatter tBinaryFormatter = new BinaryFormatter();
                object tValue = tBinaryFormatter.Deserialize(tMemoryStream);
                return tValue;
                
            }
        }
    }
}
