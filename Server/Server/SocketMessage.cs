using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    public class SocketMessage     //网络消息，发送的时候都要发送这个类
    {
        public int OPCode;      //操作码
        public int SubCode;     //子操作
        public object Value;    //参数

        public SocketMessage()
        {

        }

        public SocketMessage(int _opCode, int _subCode, int _value)
        {
            OPCode = _opCode;
            SubCode = _subCode;
            Value = _value;
        }
    }
}
