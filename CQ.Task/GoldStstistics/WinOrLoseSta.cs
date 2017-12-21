using System;
using CQ.Application.AutoService;
using CQ.Core;
using Quartz;

namespace CQ.Task
{
    public class WinOrLoseSta : BaseJob, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            PlatStatisticsAutoTask();
        }

        public void PlatStatisticsAutoTask()
        {

            Log.Info("-启动自动更新线程----");
            GoldStatisticsApp goldApp = new GoldStatisticsApp();
            try
            {
                var yeesterdayZero = DateTime.Today.AddDays(-1);

                var curTime = goldApp.GetTop1LogDay()?.ToDate();
                if (curTime == null || curTime < yeesterdayZero)
                {
                    Log.Info("统计开始。。。时间：" + DateTime.Now);
                    goldApp.GoldStaStart(yeesterdayZero);
                    Log.Info("统计结束。。。时间：" + DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}