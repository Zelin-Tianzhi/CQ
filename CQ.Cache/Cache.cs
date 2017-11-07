using Autofac;
using Autofac.Builder;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CQ.Plugin.Cache
{
    public static class Cache
    {
        private static ICache _cache = null;
        private static readonly object _cacheLocker = new object();
        static Cache()
        {
            Load();
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return string.IsNullOrWhiteSpace(key) ? null : _cache.Get(key);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            return string.IsNullOrWhiteSpace(key) ? null : _cache.Get<T>(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyColl"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string keyColl) where T:class
        {
            return string.IsNullOrWhiteSpace(keyColl) ? null : _cache.GetList <T>(keyColl);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void Insert(string key, object data)
        {
            if (!string.IsNullOrWhiteSpace(key) && (data != null))
            {
                lock (_cacheLocker)
                {
                    _cache.Insert(key, data);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void Insert<T>(string key, T data) where T : class
        {
            if (!string.IsNullOrWhiteSpace(key) && (data != null))
            {
                lock (_cacheLocker)
                {
                    _cache.Insert<T>(key, data);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expirtime"></param>
        public static void Insert(string key, object data, int expirtime)
        {
            if (!string.IsNullOrWhiteSpace(key) && (data != null))
            {
                lock (_cacheLocker)
                {
                    _cache.Insert(key, data, expirtime);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expirtime"></param>
        public static void Insert<T>(string key, T data, int expirtime) where T : class
        {
            if (!string.IsNullOrWhiteSpace(key) && (data != null))
            {
                lock (_cacheLocker)
                {
                    _cache.Insert<T>(key, data, expirtime);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expirtime"></param>
        /// <returns></returns>
        public static bool Replace(string key, object data, int expirtime)
        {
            if (!string.IsNullOrWhiteSpace(key) && (data != null))
            {
                lock (_cacheLocker)
                {
                    return _cache.Replace(key, data, expirtime);
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void UserOffLine(string key)
        {
            _cache.UserOffline(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="islike"></param>
        public static void Remove(string key, int islike=0)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            _cache.Remove(key, islike);
        }

        private static void Load()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("Configs/autofac.json");
            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            IContainer context = null;
            context = builder.Build(ContainerBuildOptions.None);
            _cache = context.Resolve<ICache>();

        }
    }

    public class Hosts
    {
        public Hosts()
        {
            this.Prot = 6379;
        }

        public string Host { get; set; }

        public int Prot { get; set; }

        public string Password { get; set; }
    }
}