using MF.Service.DataWorker;
using System;
using System.ServiceProcess;
using MF.Service.SystemReportWorker;
using MF.Service.ChannelStatistics;

namespace MF.Service
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {

            System.Threading.Thread _thread;

            if (args != null && args.Length > 0)
            {
                if ("-R".Equals(args[0], StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Report Model");
                    Report(out _thread);
                }
                else if ("-C".Equals(args[0], StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("数据迁移开始");
                    throw new NotSupportedException("此方法已不受支持，请不要调用");
                    //OldDataCopy.CopyData();
                    //var name = ConfigHelper.GetAppSettingValue("ServiceName");
                    //var description = ConfigHelper.GetAppSettingValue("ServiceDescription");

                }
                else if ("-CSR".Equals(args[0], StringComparison.CurrentCultureIgnoreCase))
                {
                    //UserReport.createAdReport();
                    //UserReport.createADIDReport();
                    //UserReport.createNewBaiduAdReport();
                    //var date = new DateTime(2017, 6, 14);
                    //Console.WriteLine("Date:{0}",date.ToString("yyyy-MM-dd"));
                    //new NewExtendChannelReportExportor(date).Export();
                    //Console.WriteLine("渠道推广报表统计");
                    //lab001:
                    //Console.WriteLine("请输入要统计的时间格式为如：2017-06-02");
                    //var value = Console.ReadLine();
                    //if (string.IsNullOrWhiteSpace(value))
                    //{
                    //    goto lab001;
                    //}

                    //try
                    //{
                    //    var d = new DateTime(2017, 6, 9);
                    //    //var date = Convert.ToDateTime(value.Trim());
                    //    new NewExtendChannelReportExportor(d).Export();
                    //}
                    //catch
                    //{
                    //    Console.WriteLine("输入的时间格式不正确");
                    //    goto lab001;
                    //}
                    //Console.WriteLine("退出请按esc键");
                    //var key = Console.ReadKey();
                    //if (key.Key  != ConsoleKey.Escape)
                    //{
                    //    goto lab001;
                    //}
                }

                Console.Read();

            }
            else
            {
                var name = ConfigHelper.GetAppSettingValue("ServiceName");


                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] {
                    new Service1()
                    {
                        ServiceName = name,
                        CanStop = true,
                        CanShutdown=true,
                    }
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        private static void Report(out System.Threading.Thread _thread)
        {
            _thread = new System.Threading.Thread(() =>
            {
                try
                {
                    Console.WriteLine("Report Starting");
                    //充值报表统计
                    ChargeReport.CreateDailyChargeReport();
                    //用户报表
                    UserReport.CreateRegReport();
                    UserReport.CreateUserReport();
                    UserReport.createSubAccReport();
                    UserReport.createQmallReport();
                    UserReport.createAdReport();
                    //Currency报表和Bean报表
                    MoneyReport.CreateCurrencyReport();
                    MoneyReport.CreateBeanReport();
                    MoneyReport.CreatNewGameReport();

                    //统计工会结果
                    FactionWorker.UpdateGuildInfo();
                    UserReport.createADIDReport();
                    UserReport.createNewBaiduAdReport();

                    Console.WriteLine("Report Complated");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Log.WriteError(ex.StackTrace);
                }

            })
            {
                IsBackground = true
            };

            _thread.Start();
        }



    }


}
