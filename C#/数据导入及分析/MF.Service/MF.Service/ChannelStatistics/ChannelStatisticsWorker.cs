using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Threading;
using MF.Service.SystemReportWorker;
using System.Data.SqlClient;
using MF.Service.Util;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace MF.Service.ChannelStatistics
{
    [Serializable]
    public class ChannelStatisticsHash
    {
        private static readonly Regex NUM_REG = new Regex(@"\d+");

        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 游戏
        /// </summary>
        public string Game { get; set; }

        /// <summary>
        /// 终端
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 加载次数
        /// </summary>
        public long LOAD { get; set; }

        /// <summary>
        /// 加载时间
        /// </summary>
        public long LOADTIME { get; set; }

        /// <summary>
        /// 停留次数
        /// </summary>
        public long STAY { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public long DOWN { get; set; }

        /// <summary>
        /// 首次激活次数
        /// </summary>
        public long FIRSTACTIVE { get; set; }

        /// <summary>
        /// 第二次下载次数
        /// </summary>
        public long SECONDDOWN { get; set; }

        /// <summary>
        /// 第二次下载时间
        /// </summary>
        public long SECONDDOWNTIME { get; set; }


        /// <summary>
        /// 注册次数
        /// </summary>
        public long REGISTER { get; set; }

        public void SetValue(IEnumerable<KeyValuePair<string,object>> ie)
        {
            foreach (var kv in ie)
            {
                this.SetValue(kv.Key, kv.Value);
            }
        }
        public void SetValue(string key, object value)
        {
            if (value == null) return;
            if (!NUM_REG.IsMatch(value.ToString()))
            {
                return;
            }
            switch (key)
            {
                case "LOAD":
                    this.LOAD = Convert.ToInt64(value);
                    break;
                case "LOADTIME":
                    this.LOADTIME = Convert.ToInt64(value);
                    break;
                case "STAY":
                    this.STAY = Convert.ToInt64(value);
                    break;
                case "DOWN":
                    this.DOWN = Convert.ToInt64(value);
                    break;
                case "FIRSTACTIVE":
                    this.FIRSTACTIVE = Convert.ToInt64(value);
                    break;
                case "SECONDDOWN":
                    this.SECONDDOWN = Convert.ToInt64(value);
                    break;
                case "SECONDDOWNTIME":
                    this.SECONDDOWNTIME = Convert.ToInt64(value);
                    break;
                case "REGISTER":
                    this.REGISTER = Convert.ToInt64(value);
                    break;
                default:
                    break;
            }
        }
    }

    public class _ChannelStatisticsHelper
    {
        private const string IOS = "IOS";
        private const string ANDROID = "ANDROID";
        private const string PC = "PC";
        private static readonly DateTime D2012 = new DateTime(2012, 10, 1);

        /// <summary>
        /// 分割appsetting中制定key的值
        /// </summary>
        /// <param name="key">要分割字符串的AppSettings的Key</param>
        /// <returns>一个分割好了的字符串迭代器</returns>
        private static IEnumerable<string> GetValueEnumerable(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                yield return string.Empty;
            }
            var arry = value.Split(',');

            foreach (var item in arry)
            {
                yield return item;
            }
        }


        private static IEnumerable<Task<ChannelStatisticsHash>> GetKeyEnumerableAsync(string date, string channel, string game, IEnumerable<string> ie)
        {
            var hash_ie = GetKeyEnumerable(date, channel, game, ie);
            var factory = Task.Factory;
            foreach (var item in hash_ie)
            {
                yield return factory.StartNew(() => item);
            }
        }

        private static IEnumerable<ChannelStatisticsHash> GetKeyEnumerable(string date, string channel, string game, IEnumerable<string> ie)
        {

            var redis = ConfigHelper.GetAppSettingValue("ChannelStatisticsRedis");

            foreach (var terminal in ie)
            {
                var key = string.Format("{0}_{1}_{2}_{3}", date, channel, game, terminal);

                var arry = CacheManager.GetProvider(redis).HGet(key).ToArray();

                if (arry!=null&&arry.Length>0)
                {
                    ChannelStatisticsHash hash = new ChannelStatisticsHash();
                    hash.SetValue(arry);
                    hash.Channel = channel;
                    hash.Game = game;
                    hash.Terminal = terminal;
                    yield return hash;
                }
            }
        }

        private static Task<NewExtendChannelReport> MakeChannelReportAsync(DateTime time,string date, string channel, string game, IEnumerable<Task<ChannelStatisticsHash>> ie)
        {
            return Task.WhenAll(ie).ContinueWith(task => MakeChannelReport(time,date, channel, game, task.Result));
        }

        private static NewExtendChannelReport MakeChannelReport(DateTime time,string date,string channel,string game, IEnumerable<ChannelStatisticsHash> ie)
        {

            //var down_sum = ie.Sum(p => p.DOWN);

            var load_sum = ie.Sum(p => p.LOAD);
            var second_down_sum = ie.Sum(p => p.SECONDDOWN);

            return new NewExtendChannelReport
            {
                Day = Convert.ToInt32((time - D2012).TotalDays),
                Channel = channel,
                ChannelNum = game,
                PCLoad = ie.Where(p => p.Terminal == PC).Sum(p => p.LOAD),
                AndroidLoad = ie.Where(p => p.Terminal == ANDROID).Sum(p => p.LOAD),
                iOSLoad = ie.Where(p => p.Terminal == IOS).Sum(p => p.LOAD),

                LoadTimeAvg = load_sum == 0 ? 0 : ie.Sum(p => p.LOADTIME) / load_sum,
                Stay = ie.Sum(p => p.STAY),

                PCDown = ie.Where(p => p.Terminal == PC).Sum(p => p.DOWN),
                AndroidDown = ie.Where(p => p.Terminal == ANDROID).Sum(p => p.DOWN),
                iOSDown = ie.Where(p => p.Terminal == IOS).Sum(p => p.DOWN),

                PCFirstActive = ie.Where(p => p.Terminal == PC).Sum(p => p.FIRSTACTIVE),
                AndroidFirstActive = ie.Where(p => p.Terminal == ANDROID).Sum(p => p.FIRSTACTIVE),
                iOSFirstActive = ie.Where(p => p.Terminal == IOS).Sum(p => p.FIRSTACTIVE),

                SecondDown = ie.Sum(p => p.SECONDDOWN),

                SecondDownTimeAvg = second_down_sum == 0 ? 0 : ie.Sum(p => p.SECONDDOWNTIME) / second_down_sum,
                CreateTime = DateTime.Now,

                Register = ie.Sum(p=>p.REGISTER)
            };

        }


        public static IEnumerable<Task<NewExtendChannelReport>> GetChannelReportEnumerableAsync(DateTime dtime)
        {

            //var date = DateTime.Now.Date.AddDays(-1).ToString("yyyyMMdd");
            var date = dtime.ToString("yyyyMMdd");

            var channel_collection = GetValueEnumerable("Channel");
            var game_collection = GetValueEnumerable("Game");
            var terminal_collection = GetValueEnumerable("Terminal");

            foreach (var channel in channel_collection)
            {
                if (channel == "APPLE")
                {
                    var ie = GetKeyEnumerableAsync(date, channel, "1", new string[] { "IOS" });
                    yield return MakeChannelReportAsync(dtime,date, channel, "1", ie);
                }
                else
                {
                    foreach (var game in game_collection)
                    {
                        var ie = GetKeyEnumerableAsync(date, channel, game, terminal_collection);
                        yield return MakeChannelReportAsync(dtime, date, channel, game, ie);
                    }
                }
            }

        }


        public static DataTable CastToDataTable(IEnumerable<NewExtendChannelReport> ie)
        {
            var type = typeof(NewExtendChannelReport);
            var properities = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var table = new DataTable();

            foreach (var property in properities)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            foreach (var item in ie)
            {
                var row = table.NewRow();

                foreach (var property in properities)
                {
                    var value = property.GetValue(item);
                    row[property.Name] = value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
        
    }

    public class NewExtendChannelReportExportor : IDataExportor
    {
        private DateTime? _date;
        public NewExtendChannelReportExportor()
        {

        }

        public NewExtendChannelReportExportor(DateTime date)
        {
            this._date = date;
        }

        private DataTable GetSouraceDataTable(string db)
        {
            //var ie = _ChannelStatisticsHelper.GetChannelReportEnumerable();
            //return _ChannelStatisticsHelper.CastToDataTable(ie);

            var date = this._date == null ? DateTime.Now.AddDays(-1) : this._date.Value;

            var ietask = _ChannelStatisticsHelper.GetChannelReportEnumerableAsync(date).ToArray();

            Task.WaitAll(ietask);

            return _ChannelStatisticsHelper.CastToDataTable(ietask.Select(p => p.Result));
        }

        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {
            mapping.Add("Day", "Day");
            mapping.Add("Channel", "Channel");
            mapping.Add("ChannelNum", "ChannelNum");
            mapping.Add("PCLoad", "PCLoad");
            mapping.Add("AndroidLoad", "AndroidLoad");
            mapping.Add("iOSLoad", "iOSLoad");
            mapping.Add("LoadTimeAvg", "LoadTimeAvg");
            mapping.Add("Stay", "Stay");
            mapping.Add("PCDown", "PCDown");
            mapping.Add("AndroidDown", "AndroidDown");
            mapping.Add("iOSDown", "iOSDown");
            mapping.Add("PCFirstActive", "PCFirstActive");
            mapping.Add("AndroidFirstActive", "AndroidFirstActive");
            mapping.Add("iOSFirstActive", "iOSFirstActive");
            mapping.Add("SecondDown", "SecondDown");
            mapping.Add("SecondDownTimeAvg", "SecondDownTimeAvg");
            mapping.Add("CreateTime", "CreateTime");
            mapping.Add("Register", "Register");

        }

        public void Export()
        {
            var table = this.GetSouraceDataTable(string.Empty);
            var bcp = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB).GetBCP();
            this.ColumnMapping(bcp.ColumnMappings);
            bcp.DestinationTableName = "NewExtendChannelReport";
            bcp.WriteToServer(table);

            Console.WriteLine("over");
        }
    }


    public abstract class _Worker: IWorker
    {
        private Thread _thread;

        private int _state = -1;
        protected virtual int State
        {
            get
            {
                lock (this)
                {
                    return this._state;
                }
            }
            set
            {
                lock (this)
                {
                    this._state = value;
                }
            }
        }

        public abstract TimeSpan WaitTime { get ; }

        public virtual void DoWork()
        {
            
        }

        public virtual void Start()
        {
            var state = this.State;
            if (state == 1)
            {
                return;
            }
            Console.WriteLine("开始执行");
            this.State = 1;
            this._thread = new System.Threading.Thread(StartWork) { IsBackground = true };
            this._thread.Start();
        }

        private void StartWork()
        {
            if (this.State == 2)
            {
                return;
            }

            Thread.Sleep(this.WaitTime);
            Log.WriteLog("间隔：", this.WaitTime.TotalMilliseconds.ToString(), "s，开始执行任务");
            DoWork();
            StartWork();
        }

        public virtual void Stop()
        {
            this.State = 2;
            try
            {
                this._thread.Abort();
                this._thread = null;
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                Console.WriteLine("停止执行");
            }
        }
    }

    public class ChannelStatisticsWorker : _Worker
    {

        public override TimeSpan WaitTime
        {
            get
            {
                var now = DateTime.Now;
                var end = now.Date.AddHours(6);
                if (end < now)
                {
                    end = now.Date.AddDays(1).AddHours(6);
                }
                return end - now;
            }
        }

        public override void DoWork()
        {
            var exportor = new NewExtendChannelReportExportor();
            exportor.Export();
        }
    }
}
