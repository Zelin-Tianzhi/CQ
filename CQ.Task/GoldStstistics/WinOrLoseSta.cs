using System;
using CQ.Application.AutoService;
using Quartz;

namespace CQ.Task.GoldStstistics
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
                DateTime yeesterdayZero = DateTime.Today.AddDays(-1);

                var curTime = goldApp.GetTop1LogDay();
                if (curTime == null)
                {
                    
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}