using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Util.MyTimer
{
    public delegate void TimeDelegate();     //当定时器达到时间后的触发
    public class TimerModel        //定时器的数据模型
    {
        public int ID;          //每个对象的唯一标识
        public long Time;       //代表任务执行的时间
        private TimeDelegate timeDel;
        public TimerModel(int _id, long _time, TimeDelegate _timeDelegate)
        {
            ID = _id;
            Time = _time;
            timeDel = _timeDelegate;
        }

        public void Run()       //触发任务
        {
            timeDel();
        }
    }
}
