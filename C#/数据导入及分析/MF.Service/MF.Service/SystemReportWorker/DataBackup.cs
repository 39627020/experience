using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MF.Service.Util;

namespace MF.Service.SystemReportWorker
{
    class DataBackup
    {
        public static void BackupData()
        {

            List<String> dbs = UserDbinfo.getUserDbinfo();
            foreach (String db in dbs) {
                BackupAndClearSingalData(db);
            }

      
        }
        private static void BackupAndClearSingalData(String dataBase) {
            Log.WriteLog("BackupAndClearSingalData start");
            var insertCurrcrySql = "insert into CurrcryRecordHistory select * from CurrcryRecord";
            var getTopCurrcrySql = "select max(ID) as ID from CurrcryRecordHistory";     
            var deleteCurrcrySql = "delete from CurrcryRecord where Id <= @ID";


            var insertRegistSql = "insert into RegistRecordHistory select * from RegistRecord";
            var getTopRegistSql = "select Max(ID) as ID from RegistRecordHistory";
            var deleteRegistSql = "delete from RegistRecord where ID <= @ID";

            var insertLoginSql = "insert into LoginLogHistory select * from LoginLog";
            var getTopLoginSql = "select Max(ID) as ID from LoginLogHistory";
            var deleteLoginSql = "delete from LoginLog where ID <=@ID";

            var helper = DbManager.GetDbHelper(dataBase);

            //处理CurrcryRecord数据
            helper.Execute(insertCurrcrySql);
            var currcryRow = helper.QueryRow(getTopCurrcrySql);
            if (currcryRow != null) {
                var args = new SqlParameter[] {
                    new SqlParameter("@ID",currcryRow["ID"])
                };
                helper.Execute(deleteCurrcrySql,CommandType.Text ,args);
            }

            //处理RegistRecord数据
            helper.Execute(insertRegistSql);
            var registRow = helper.QueryRow(getTopRegistSql);
            if (registRow != null)
            {
                var args1 = new SqlParameter[] {
                    new SqlParameter("@ID",registRow["ID"]){ SqlDbType = SqlDbType.BigInt }
                };
                helper.Execute(deleteRegistSql, CommandType.Text, args1);
            }

            //处理LoginRecord数据
            helper.Execute(insertLoginSql);
            var loginRow = helper.QueryRow(getTopLoginSql);
            if (registRow != null)
            {
                var args2 = new SqlParameter[] {
                    new SqlParameter("@ID",registRow["ID"]){ SqlDbType = SqlDbType.BigInt }
                };
                helper.Execute(deleteLoginSql, CommandType.Text, args2);
            }
            Log.WriteLog("BackupAndClearSingalData end");
        }
    }
}
