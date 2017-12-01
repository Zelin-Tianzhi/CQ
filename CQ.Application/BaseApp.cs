using CQ.Core.Log;

namespace CQ.Application
{
    public class BaseApp
    {
        public Log Log => LogFactory.GetLogger(this.GetType().ToString());


    }
}