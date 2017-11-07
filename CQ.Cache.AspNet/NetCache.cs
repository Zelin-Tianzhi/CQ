using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace CQ.Plugin.Cache.AspNet
{
    public class NetCache : ICache
    {
        private int _timeout = 60;
        private readonly System.Web.Caching.Cache _cache = HttpRuntime.Cache;
        private static object _cacheLocker = new object();
        private const int DefualtTimeOut = 60;

        public void Clear()
        {
            var enumerator = this._cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this._cache.Remove(enumerator.Key.ToString());
            }
        }
        public object Get(string key)
        {
            return this._cache.Get(key);
        }

        public T Get<T>(string key) where T : class
        {
            return this._cache.Get(key) != null ? JsonConvert.DeserializeObject<T>(_cache.Get(key).ToString()) : default(T);
        }

        public List<T> GetList<T>(string keyColl) where T : class
        {
            var list = new List<T>();
            if (this._cache.Get(keyColl) != null)
            {
                var keys = (List<string>)_cache.Get(keyColl);
                var tmpKeys = new List<string>();
                if (!keys.Any()) return new List<T>();
                foreach (var item in keys)
                {
                    tmpKeys.Add(item);
                    var data = (T)_cache.Get(item);
                    if (data != null)
                    {
                        list.Add(data);
                    }
                }
                if (tmpKeys.Any())
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(tmpKeys) + "";
                    _cache.Remove(keyColl);
                    _cache.Insert(keyColl, json);
                }
            }
            return list;
        }

        public void Insert(string key, object data)
        {
            if (this._cache.Get(key) != null)
                this._cache.Remove(key);
            this._cache.Insert(key, data);
        }

        public void Insert<T>(string key, T data) where T : class
        {
            if (this._cache.Get(key) != null)
            {
                this._cache.Remove(key);
            }
            this._cache.Insert(key, JsonConvert.SerializeObject(data), null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public void Insert(string key, object data, int expireTime)
        {
            if (this._cache.Get(key) != null)
            {
                this._cache.Remove(key);
            }
            this._cache.Insert(key, data, null, DateTime.Now.AddMinutes((double)expireTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public void Insert<T>(string key, T data, int expireTime) where T : class
        {
            if(this._cache.Get(key) != null)
            {
                this._cache.Remove(key);
            }
            this._cache.Insert(key, JsonConvert.SerializeObject(data), null, DateTime.Now.AddMinutes((double)expireTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public void Remove(string key, int islike = 0)
        {
            if (islike == 0)
            {
                this._cache.Remove(key);
            }
            else
            {
                var enumerator = this._cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var ckey = enumerator.Key.ToString();
                    if (!ckey.StartsWith(key))
                    {
                        continue;
                    }
                    _cache.Remove(key);
                }
            }
        }

        public bool Replace(string key, object data, int expirtime)
        {
            var flag = true;
            _cache.Remove(key);
            _cache.Insert(key, data, null, DateTime.Now.AddMinutes(expirtime), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            if (this._cache.Get(key) == null)
            {
                flag = false;
            }
            return flag;
        }

        public void UserOffline(string key)
        {
            var tmpKeys = new List<string>();
            var coll = _cache.Get(key);
            if (coll == null)
            {
                return;
            }
            var keys = JsonConvert.DeserializeObject<List<string>>(coll + "");
            if (keys != null && !keys.Any())
            {
                return;
            }
            foreach (var item in keys)
            {
                tmpKeys.Add(item);
                var data = _cache.Get(item) + "";
                if (string.IsNullOrEmpty(data)) continue;
                var value = (dynamic)JsonConvert.DeserializeObject(data);
                if (value == null) continue;
                value.BeUsed = 1;
                string json = JsonConvert.SerializeObject(value) + "";
                Replace(item, json, 3);
            }
            if (tmpKeys.Any())
            {
                var json = JsonConvert.SerializeObject(tmpKeys) + "";
                _cache.Remove(key);
                _cache.Insert(key, json);
            }

        }
    }
}
