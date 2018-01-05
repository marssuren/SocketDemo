using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server
{
    public delegate void ExecuteDelegate();     //一个需要执行的方法
    public class SingleExecute         //单线程池
    {
        public Mutex mutex;     //互斥锁

        public SingleExecute()
        {
            mutex=new Mutex();

        }
        public void Excute(ExecuteDelegate _executeDelegate)        //单线程处理逻辑
        {
            lock (this)
            {
                mutex.WaitOne();
                _executeDelegate();
                mutex.ReleaseMutex();
            }

        }
    }
}
