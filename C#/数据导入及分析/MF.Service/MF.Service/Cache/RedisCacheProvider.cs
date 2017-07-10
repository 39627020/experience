/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2017/1/23
 * 时间: 16:21
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections;
using System.Collections.Generic;

namespace MF.Service
{
	/// <summary>
	/// Description of RedisCacheManager.
	/// </summary>
	public class RedisCacheProvider:ICacheProvider
	{

        private IDatabase _db;

        public RedisCacheProvider():this("172.16.0.14:7020",0)
        {

        }

        public RedisCacheProvider(string address,int index)
        {
            var connection = ConnectionMultiplexer.Connect(address);
            this._db = connection.GetDatabase(index);
        }
   
        public string Get(string key)
        {
            return this._db.StringGet(key);
        }
        public T Get<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(this._db.StringGet(key));
        }
        public void Set<T>(string key, T data, int cacheTime)
        {
            string str = JsonConvert.SerializeObject(data);
            this._db.StringSet(key, str, TimeSpan.FromMinutes(cacheTime));
        }
        public void Set(string key, object data, int cacheTime)
        {
            string str = JsonConvert.SerializeObject(data);
            this._db.StringSet(key, str, TimeSpan.FromMinutes(cacheTime));
        }
        public void Set(string key, object data)
        {
            var str=string.Empty;
            if(data is string)
            {
            	if(data != null)
            	{
            		str=data.ToString();
            	}
            }
            else
            {
            	str = JsonConvert.SerializeObject(data);
            }
            this._db.StringSet(key, str);
        }

        public IEnumerable<KeyValuePair<string, object>> HGet(string key)
        {
            foreach (var item in this._db.HashGetAll(key))
            {
                yield return new KeyValuePair<string, object>(item.Name, item.Value);
            }
        }
        
        public bool IsSet(string key)
        {
            return this._db.KeyExists(key);
        }
        public void Remove(string key)
        {
            this._db.KeyDelete(key);
        }
	}
}
