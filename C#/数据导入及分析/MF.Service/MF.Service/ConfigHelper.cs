using System.Configuration;
using System.Reflection;

namespace MF.Service
{
    public class ConfigHelper
    {
        public static string GetAppSettingValue(string key)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ConfigHelper)).Location);
            return config.AppSettings.Settings[key].Value;
        }

        public static Configuration GetConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ConfigHelper)).Location);
        }

    }
}
