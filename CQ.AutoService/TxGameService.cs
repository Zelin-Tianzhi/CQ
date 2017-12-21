using System;
using System.ServiceProcess;
using CQ.Core;
using CQ.Core.Log;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Xml;

namespace CQ.AutoService
{
    partial class TxGameService : ServiceBase
    {
        private IScheduler sched;
        private Log Log => LogFactory.GetLogger(this.GetType().ToString());

        public TxGameService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Log.Debug("自动化服务开始运行......");
                XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
                ISchedulerFactory factory = new StdSchedulerFactory();
                sched = factory.GetScheduler();
                processor.ProcessFileAndScheduleJobs(FileHelper.MapPath("/quartz_jobs.xml"), sched);
                sched.Start();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        protected override void OnStop()
        {
            Log.Debug("自动化服务停止运行......");
        }
    }
}
