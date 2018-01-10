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
		private static SingleExecute instance = null;
		public static SingleExecute Instance
		{
			get
			{
				lock(instance)          //锁任意一个静态对象都行
				{
					if(null == instance)
					{
						instance = new SingleExecute();
					}
					return instance;
				}
			}
		}
		public Mutex mutex;     //互斥锁
		private SingleExecute()
		{
			mutex = new Mutex();
		}
		public void Excute(ExecuteDelegate _executeDelegate)        //单线程处理逻辑
		{
			lock(this)
			{
				mutex.WaitOne();
				_executeDelegate();
				mutex.ReleaseMutex();
			}

		}
	}
}
