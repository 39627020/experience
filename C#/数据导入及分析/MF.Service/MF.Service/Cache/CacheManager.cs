/*
 * 由SharpDevelop创建。
 * 用户： admin
 * 日期: 2017/1/23
 * 时间: 16:35
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */

using System.Configuration;


namespace MF.Service
{
    /// <summary>
    /// Description of CacheManager.
    /// </summary>
    public class CacheManager
	{

        private static ICacheProvider _provider;
        private static object obj = new object();

        public static ICacheProvider Provider
		{
			get
			{
                lock (obj)
                {
                    if (_provider == null)
                    {
                        var redis = ConfigHelper.GetAppSettingValue("ReportRedis");
                        if (string.IsNullOrWhiteSpace(redis))
                        {
                            _provider = new RedisCacheProvider();
                        }
                        else
                        {
                            _provider = new RedisCacheProvider(redis, 0);
                        }
                    }
                    return _provider;
                }
			}
		}



        public static ICacheProvider GetProvider(string address)
        {
            return new RedisCacheProvider(address, 0);
        }
		
		
		
		
	}
}
