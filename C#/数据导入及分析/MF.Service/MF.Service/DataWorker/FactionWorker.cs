using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MF.Service.Util;
using System.Data;

namespace MF.Service.DataWorker
{
    class FactionWorker
    {

        public static Boolean UpdateGuildInfo()
        {
            Log.WriteLog("createAdReport start");
            //如果不是每周一，不会统计这个值，不会更新工会数据
            if (!"Sunday".Equals(DateTime.Now.DayOfWeek.ToString())) {
                Log.WriteLog("createAdReport do not need work");
                return false;
            }
            var getGuildInfoSql = "select ID as guildId, UserCount, ActiveUserNumOfLastWeek , ActiveUserNumOfCurrentWeek from guild where isActive = 1";
            var getGuildAccount = "select Account from GuildUser where GuildID = @GuildId";
            var updateGuildInfo = "update Guild set ActiveUserNumOfLastWeek = @ActiveUserNumOfLastWeek,ActiveUserNumOfCurrentWeek=@ActiveUserNumOfCurrentWeek,Exp=Exp+@Exp ,IsoWeek = @IsoWeek where id = @GuildId";



            var readMfHelper = DbManager.GetDbHelper(UserDbinfo.MF_DB);
            var recordMfMainHelper = DbManager.GetDbHelper(UserDbinfo.MF_MAIN);

            DataTable guildInfoTale = readMfHelper.QueryTable(getGuildInfoSql);
            var timeParam = new SqlParameter[] {
                          new SqlParameter("@LastWeekSecond",TimeUtil.getRecordTimeBylastWeek()),
                          new SqlParameter("@ThisWeekSecond",TimeUtil.getRecordTimeBythisWeek())
                       };
            foreach (DataRow row in guildInfoTale.Rows) {
                int guildId = 0;
                int userCount = 0;
                int activeUserNumOfCurrentWeek = 0;
                int.TryParse(row["guildId"].ToString(), out guildId);
                int.TryParse(row["UserCount"].ToString(), out userCount);
                int.TryParse(row["ActiveUserNumOfCurrentWeek"].ToString(), out activeUserNumOfCurrentWeek);

                if (guildId > 0 && userCount>0) {
                    var guildIdParam = new SqlParameter[] {
                          new SqlParameter("@GuildId",guildId)
                       };
                    DataTable guildUserTable = readMfHelper.QueryParamTable(getGuildAccount, guildIdParam);
                    String userAccounts = "";
                    foreach (DataRow accRow in guildUserTable.Rows) {
                        if (userAccounts.Length == 0){
                            userAccounts = "'"+accRow["Account"].ToString()+"'";
                        }
                        else {
                            userAccounts = userAccounts + ",'" + accRow["Account"].ToString() + "'";
                        }
                    }
                    var getChannelGuildCoung = "select(count(DISTINCT(account))) as count from (select top 100 sum(num) as money, account from CurrcryRecord where Account in (" + userAccounts + ") and time between @LastWeekSecond and @ThisWeekSecond  and type = 30 group by account) as t1 where t1.money>20000";
                    int totolActiveUserNum = 0;
                    foreach (String dbInfo in UserDbinfo.getUserDbinfo())
                    {
                        var readHelper = DbManager.GetDbHelper(dbInfo);
                        int currencyChannelUserCount = 0;
                        DataRow countInfo = readHelper.QueryParamRow(getChannelGuildCoung, timeParam);
                        int.TryParse(countInfo["count"].ToString(),out currencyChannelUserCount);
                        totolActiveUserNum += currencyChannelUserCount;
                    }
                    //只有当两个数值的比例大于0.7才会添加工会等级
                    int guildlevelAdd = 0;
                    if (totolActiveUserNum / userCount > 0.7) {
                        guildlevelAdd = getExpLev(totolActiveUserNum);
                    }
                    
                    var guildParam = new SqlParameter[] {
                          new SqlParameter("@ActiveUserNumOfLastWeek",activeUserNumOfCurrentWeek),
                          new SqlParameter("@ActiveUserNumOfCurrentWeek",totolActiveUserNum),
                          new SqlParameter("@IsoWeek",TimeUtil.getThisWeekOfYear()),
                          new SqlParameter("@Exp",guildlevelAdd),
                          new SqlParameter("@GuildId",guildId)
                       };

                    recordMfMainHelper.Execute(updateGuildInfo, CommandType.Text, guildParam);
                  

                }
            }


            Log.WriteLog("createAdReport end");

            return false;
        }

         private static int getExpLev(int ActiveCount) {
            if (ActiveCount < 40) return 2;
            if (ActiveCount >=40 && ActiveCount< 80) return 3;
            if (ActiveCount >= 80 && ActiveCount < 120) return 4;
            if (ActiveCount >= 120 && ActiveCount < 160) return 5;
            if (ActiveCount >= 160 && ActiveCount < 250) return 6;
            if (ActiveCount >= 250 && ActiveCount < 300) return 7;
            if (ActiveCount >= 300 && ActiveCount < 350) return 8;
            if (ActiveCount >= 350 && ActiveCount < 400) return 9;
            if (ActiveCount >= 400) return 10;
            return 0;
        }
    }
}
