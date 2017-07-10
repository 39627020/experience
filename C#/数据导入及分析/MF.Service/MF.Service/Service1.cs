using System;
using System.Configuration;
using System.ServiceProcess;


namespace MF.Service
{
    public partial class Service1 : ServiceBase
    {
        private IWorker _worker;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var workertype = ConfigHelper.GetAppSettingValue("WorkerType");

                if (string.IsNullOrWhiteSpace(workertype))
                {
                    this._worker = new Worker();
                }
                else
                {
                    var type = Type.GetType(workertype);
                    this._worker = Activator.CreateInstance(type) as IWorker;
                }
                this._worker.Start();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message, ex.StackTrace);
                throw ex;
            }
            Log.WriteLog("Started Service");

        }

        protected override void OnStop()
        {
            this._worker.Stop();
            Log.WriteLog("Stoped Service");
            //Qimen_Timer.Stop();
        }
    }
}
