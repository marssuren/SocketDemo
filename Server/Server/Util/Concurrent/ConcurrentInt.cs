using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Util.Concurrent
{
    public class ConcurrentInt      //线程安全的int类型
    {
        private int value;

        public ConcurrentInt(int _value)
        {
            value = _value;
        }

        public int AddGet()     //添加并获取
        {
            lock (this)
            {
                value++;
                return value;
            }
        }
        public int ReduceGet()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }
        public int Get()
        {
            return value;
        }
    }
}
