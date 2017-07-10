/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2017/1/23
 * 时间: 16:22
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace MF.Service
{
	/// <summary>
	/// Description of ICacheProvider.
	/// </summary>
	public interface ICacheProvider
	{
		 string Get(string key);
		
		 T Get<T>(string key);
		
		 void Set<T>(string key, T data, int cacheTime);
		
		 void Set(string key, object data, int cacheTime);
		
		 void Set(string key, object data);

         IEnumerable<KeyValuePair<string, object>> HGet(string key);

         bool IsSet(string key);
		
		 void Remove(string key);
	}
}
