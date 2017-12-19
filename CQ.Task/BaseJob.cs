using CQ.Core.Log;

namespace CQ.Task
{
    public class BaseJob
    {
        public Log Log => LogFactory.GetLogger(this.GetType().ToString());
    }
}