using System;
using System.ServiceProcess;
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
                Log.Debug("棋牌自动化服务开始运行......");
                XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
                ISchedulerFactory factory = new StdSchedulerFactory();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
