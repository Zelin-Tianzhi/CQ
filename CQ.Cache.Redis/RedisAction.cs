using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using CQ.Core;

namespace CQ.Plugin.Cache.Redis
{
    public class RedisAction
    {
        private static RedisConfig Config
        {
            get
            {
                try
                {
                    return Configs.GetConfig<List<RedisConfig>>(FileHelper.MapPath("/Configs/Redis.config")).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return new RedisConfig
                    {
                        Host = "127.0.0.1",
                        Prot = 6379
                    };

                }
            }
        }

        public dynamic Connect(Func<RedisClient, dynamic> func)
        {
            try
            {
                using (var client = new RedisClient(Config.Host,Config.Prot, Config.Password))
                {
                    return func(client);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Connect(Action<RedisClient> action)
        {
            try
            {
                using (var client = new RedisClient(Config.Host, Config.Prot, Config.Password))
                {
                    action(client);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
