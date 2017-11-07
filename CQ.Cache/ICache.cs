using System.Collections.Generic;

namespace CQ.Plugin.Cache
{
    public interface ICache:IStrategy
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据</param>
        void Insert(string key, object data);
        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="cacheKey"></param>
        void Insert<T>(string key, T value) where T : class;

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据</param>
        /// <param name="expirtime">缓存时间（分钟）</param>
        void Insert(string key, object data, int expirtime);
        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="cacheKey"></param>
        /// <param name="expireTime"></param>
        void Insert<T>(string cacheKey, T data, int expireTime) where T : class;

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据</param>
        /// <param name="expirtime">缓存时间（分钟）</param>
        /// <returns></returns>
        bool Replace(string key, object data, int expirtime);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="islike"></param>
        void Remove(string key, int islike = 0);

        /// <summary>
        /// 设置超时
        /// </summary>
        /// <param name="key">键值</param>
        void UserOffline(string key);

        /// <summary>
        /// 根据键值前缀获取缓存集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyColl"></param>
        /// <returns></returns>
        List<T> GetList<T>(string keyColl) where T : class;


    }
}