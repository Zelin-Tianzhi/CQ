using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQ.Plugin.Cache.Redis
{
    public class RedisCache : RedisAction, ICache
    {
        public object Get(string key)
        {
            return Connect(client =>
            {
                return client.Exists(key) > 0 ? client.Get<object>(key) : default(object);
            });
        }

        public T Get<T>(string key) where T : class
        {
            return Connect((client) =>
            {
                return client.Exists(key) > 0 ? JsonConvert.DeserializeObject<T>(client.Get<string>(key)) : default(T);
            });
        }

        public List<T> GetList<T>(string keyColl) where T : class
        {
            return Connect((client) =>
            {
                var list = new List<T>();
                var obj = client.SearchKeys(keyColl + "*");
                if (!obj.Any()) return list;
                foreach (var key in obj)
                {
                    var cacheObj = client.Get<string>(key);
                    if (cacheObj == null) continue;
                    list.Add(JsonConvert.DeserializeObject<T>(cacheObj,
                        new JsonSerializerSettings()
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            NullValueHandling = NullValueHandling.Ignore
                        }));
                }
                return list;
            });
        }

        public void Insert(string key, object data)
        {
            Connect((client) =>
            {
                if (client.Get(key) == null)
                {
                    client.Set(key, data);
                }
                else
                {
                    client.Replace(key, data);
                }
            });
        }

        public void Insert(string key, object data, int expirtime)
        {
            Connect((client) =>
            {
                if (client.Get(key) == null)
                {
                    client.Set(key, data, new TimeSpan(0, expirtime, 0));
                }
                else
                {
                    client.Replace(key, data, new TimeSpan(0, expirtime, 0));
                }
            });
        }

        public void Insert<T>(string key, T data, int expirtime) where T : class
        {
            Connect((client) =>
            {
                if (client.Get(key) == null)
                {
                    client.Set(key, JsonConvert.SerializeObject(data), new TimeSpan(0, expirtime, 0));
                }
                else
                {
                    client.Replace(key, JsonConvert.SerializeObject(data), new TimeSpan(0, expirtime, 0));
                }
            });
        }

        public void Insert<T>(string key, T data) where T : class
        {
            Connect((client) =>
            {
                if (client.Get(key) == null)
                {
                    client.Set(key, JsonConvert.SerializeObject(data));
                }
                else
                {
                    client.Replace(key, JsonConvert.SerializeObject(data));
                }
            });
        }

        public void Remove(string key, int islike = 0)
        {
            Connect((client) =>
            {
                if (islike == 0)
                {
                    client.Del(key);
                }
                else
                {
                    client.RemoveByPattern(key + "");
                }
            });
        }

        public bool Replace(string key, object data, int expirtime)
        {
            return Connect((client) =>
            {
                return client.Replace(key, JsonConvert.SerializeObject(data), new TimeSpan(0, expirtime, 0));
            });
        }

        public void UserOffline(string key)
        {
            Connect((client) =>
            {
                if (client.Exists(key) == 0)
                {
                    return;
                }
                var keys = JsonConvert.DeserializeObject<List<string>>(client.Get<string>(key));
                var tmpKeys = new List<string>();
                if (!keys.Any())
                {
                    return;
                }
                foreach (var item in keys.Where(i=> client.Exists(i) > 0))
                {
                    tmpKeys.Add(item);
                    var data = client.Get<string>(item);
                    if (string.IsNullOrEmpty(data))
                    {
                        continue;
                    }
                    var value = (dynamic)data;
                    if (value == null)
                    {
                        continue;
                    }
                    value.BeUsed = 1;
                    string json = JsonConvert.SerializeObject(value);
                    Replace(item, json, 3);
                }
                if (tmpKeys.Any())
                {
                    var json = JsonConvert.SerializeObject(tmpKeys);
                    client.Del(key);
                    client.Set(key, json);
                }
            });
        }
    }
}