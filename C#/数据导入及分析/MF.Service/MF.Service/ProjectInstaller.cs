using System.ComponentModel;
using System.Configuration.Install;
using System.Configuration;
using System.ServiceProcess;
namespace MF.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Committed += ProjectInstaller_Committed;
        }

        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {
            var name = ConfigHelper.GetAppSettingValue("ServiceName");
            ServiceController sc;

            if (string.IsNullOrWhiteSpace(name))
            {
                sc = new ServiceController("MF.Service");
            }
            else
            {
                sc = new ServiceController(name);
            }
            sc.Start();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
