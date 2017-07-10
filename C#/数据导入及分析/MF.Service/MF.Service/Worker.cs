using MF.Service.DataWorker;
using MF.Service.SystemReportWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MF.Service
{

    public interface IWorker
    {
        void Start();
        void Stop();
    }

    public class Worker:IWorker
    {
        private int _state = -1;
        private System.Threading.Thread _thread;

        private int State
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


        public void Start()
        {
            var state = this.State;
            if (state == 1)
            {
                return;
            }
            Console.WriteLine("开始执行");
            this.State = 1;
            this._thread = new System.Threading.Thread(this.StartWork) { IsBackground = true };
            this._thread.Start();
        }

        public void Stop()
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

        private static void DoWork()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message, ex.StackTrace);
            }
        }

        private void StartWork()
        {
            if (this.State == 2)
            {
                return;
            }

            //每天6点生成报表
            var now = DateTime.Now;

            var end = now.Date.AddHours(6);

            if (end < now)
            {
                end = now.Date.AddDays(1).AddHours(6);
            }


            var span = end - now;
            System.Threading.Thread.Sleep(span);
            Log.WriteLog("间隔：", span.TotalMilliseconds.ToString(), "s，开始执行任务");
            Console.WriteLine("间隔：{0}s，开始执行任务", span.Seconds);

            DoWork();


            StartWork();
        }
    }
}
