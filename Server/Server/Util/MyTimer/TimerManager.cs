using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Server.Util.Concurrent;

namespace Server.Util.MyTimer
{
    public class TimerManager       //计时器管理类
    {
        private static TimerManager instance;
        public static TimerManager Instance
        {
            get
            {
                lock (instance)
                {
                    if (null == instance)
                    {
                        instance = new TimerManager();
                    }
                    return instance;
                }
            }
        }
        private Timer timer;        //实现定时器的主要功能
        private ConcurrentDictionary<int, TimerModel> idModelDic = new ConcurrentDictionary<int, TimerModel>();       //这个线程安全的字典存储任务id和任务模型的映射
        private List<int> removeList = new List<int>();      //要移除的任务ID列表
        private ConcurrentInt id = new ConcurrentInt(-1);
        public TimerManager()
        {
            timer = new Timer(10);
            timer.Elapsed += timerElapsed;
        }
        private void timerElapsed(object _sender, ElapsedEventArgs _elapsedEventArgs)   //达到时间间隔时触发
        {
            lock (removeList)
            {
                TimerModel tModel;
                for (int i = 0; i < removeList.Count; i++)
                {
                    idModelDic.TryRemove(removeList[i], out tModel);
                }
                removeList.Clear();
            }

            for (int i = 0; i < idModelDic.Count; i++)
            {
                if (idModelDic[i].Time <= DateTime.Now.Ticks)
                {
                    idModelDic[i].Run();
                }
            }
        }

        public void AddTimeEvent(DateTime _dateTime, TimeDelegate _timeDelegate)   //添加定时任务,指定触发的时间
        {
            long tDelayTime = _dateTime.Ticks - DateTime.Now.Ticks;
            if (tDelayTime <= 0)
            {
                return;
            }
            AddTimeEvent(tDelayTime, _timeDelegate);
        }

        public void AddTimeEvent(long _delayTime, TimeDelegate _timeDelegate)        //添加定时任务,指定延迟的时间
        {
            TimerModel tModel = new TimerModel(id.AddGet(), DateTime.Now.Ticks + _delayTime, _timeDelegate);
            idModelDic.TryAdd(tModel.ID, tModel);
        }
    }
}
