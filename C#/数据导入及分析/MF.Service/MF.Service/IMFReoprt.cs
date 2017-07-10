using System.Data;

namespace MF.Service
{
    /// <summary>
    /// MF报表接口
    /// </summary>
    public interface IMFReoprt
    {
        DataTable Report();

        DataSet ReportMany();
    }
}
