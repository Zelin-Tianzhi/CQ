using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Plugin.Cache.Redis
{
    public class RedisConfig
    {
        public RedisConfig()
        {
            Prot = 6379;
        }
        public string Host { get; set; }

        public int Prot { get; set; }

        public string Password { get; set; }
    }
}
