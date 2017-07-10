using System;
using System.Data;
using System.Data.SqlClient;

namespace MF.Service.SystemReportWorker
{
    /// <summary>
    /// 留存统计
    /// </summary>
    public enum DayLeave
    {
        /// <summary>
        /// 次日留存
        /// </summary>
        Yesterday,
        /// <summary>
        /// 3日留存
        /// </summary>
        _3Day,
        /// <summary>
        /// 7日留存
        /// </summary>
        _7Day
    }

    /// <summary>
    /// 留存报表
    /// </summary>
    public class MFDayLeaveReport : IMFReoprt
    {

        private readonly DbHelper _dbhelper;
        private readonly SqlParameter[] _parameters;
        private readonly string _sql;

        public MFDayLeaveReport(string db, DateTime regStart, DateTime regEnd, DateTime loginStart, DateTime loginEnd)
        {
            this._dbhelper = DbManager.GetDbHelper(db);
            this._sql = GetSql();
            this._parameters = new[] {
                new SqlParameter("@RegTimeStart", regStart) { SqlDbType = SqlDbType.DateTime },
                new SqlParameter("@RegTimeEnd", regEnd) { SqlDbType = SqlDbType.DateTime },
                new SqlParameter("@LoginTimeStart", loginStart) { SqlDbType = SqlDbType.DateTime },
                new SqlParameter("@LoginTimeEnd", loginEnd) { SqlDbType = SqlDbType.DateTime }
                };
        }

        protected virtual string GetSql()
        {

            #region 陈鹏飞原始的语句()
            /*
            //获得次日留存数量
           var getOneDayLeftSql = "SELECT count(distinct(t2.LastGUID)) as count , LEFT(t1.chargeId,3) as ChannelId from LoginLog as t1 join Users as t2 on t1.Account = t2.Account where t1.LoginTime BETWEEN @Yestoday  and @Today  and t2.Regitime < @yesterdaySecond and t2.Regitime > " + (TimeUtil.getRecordTimeByYestorday() - 86400)+" group by LEFT(t1.chargeId,3)";

            //获得3日留存数量
            var getThreeDayLeftSql = "SELECT count(distinct(t2.LastGUID)) as count , LEFT(t1.chargeId,3) as ChannelId from LoginLog as t1 join Users as t2 on t1.Account = t2.Account where t1.LoginTime BETWEEN @Yestoday  and @Today  and t2.Regitime < @threedaySecond and t2.Regitime > " + (TimeUtil.getRecordTimeByThreeDayLeft() - 86400) + " group by LEFT(t1.chargeId,3)";


            //获得7日留存数量
            var getSevenDayLeftSql = "SELECT count(distinct(t2.LastGUID)) as count , LEFT(t1.chargeId,3) as ChannelId from LoginLog as t1 join Users as t2 on t1.Account = t2.Account where t1.LoginTime BETWEEN @Yestoday  and @Today and t2.Regitime < @sevendaySecond and t2.Regitime > " + (TimeUtil.getRecordTimeBySevenDayLeft()-86400)  + " group by LEFT(t1.chargeId,3)" ;
            */
            #endregion

            /*
            select count(DISTINCT RegistRecord.Guid) from RegistRecord where  [RegTime] > N'2017-06-10 00:00:00' and [RegTime] < N'2017-06-11 00:00:00'
            and RegistRecord.Guid in (select LoginLog.Guid from LoginLog where [LoginTime] > N'2017-06-11 00:00:00' and [LoginTime] < N'2017-06-12 00:00:00')
            */
            //根据需要使用土豆的sql条件作为标准(上面是土豆的参考sql语句)需要变更来源（赵凯）修改日期2017-05-15 by lwb
            var sql = string.Format(@"SELECT COUNT(DISTINCT a.Guid) AS count,LEFT(a.chargeId,3) AS ChannelId
                        FROM RegistRecord a
                        INNER JOIN LoginLog b ON a.[Guid]=b.[GUID]
                        WHERE  a.[RegTime] > @RegTimeStart AND a.[RegTime] < @RegTimeEnd
                        AND b.[LoginTime] > @LoginTimeStart and b.[LoginTime] < @LoginTimeEnd
                        GROUP BY LEFT(a.chargeId,3)");
            return sql;
        }

        public DataTable Report()
        {
            return this._dbhelper.QueryParamTable(this._sql, this._parameters);
        }

        public DataSet ReportMany()
        {
            throw new NotSupportedException();
        }
    }

    public class MFDayLeaveReportAdid : MFDayLeaveReport
    {
        public MFDayLeaveReportAdid(string db, DateTime regStart, DateTime regEnd, DateTime loginStart, DateTime loginEnd) : base(db, regStart, regEnd, loginStart, loginEnd)
        {

        }

        protected override string GetSql()
        {
            var sql = string.Format(@"SELECT COUNT(DISTINCT a.Guid) AS count,LEFT(a.chargeId,3) AS ChannelId,c.ADID
                        FROM RegistRecord a
                        INNER JOIN LoginLog b ON a.[Guid]=b.[GUID]
                        INNER JOIN Users c on b.Account=c.Account
                        WHERE a.[RegTime] > @RegTimeStart AND a.[RegTime] < @RegTimeEnd 
                        AND b.[LoginTime] > @LoginTimeStart and b.[LoginTime] < @LoginTimeEnd AND c.ADID <> ''
                        GROUP BY LEFT(a.chargeId,3),c.ADID");
            return sql;
        }
    }

    public class _MFDayLeaveReportHelper
    {
        /// <summary>
        /// 根据留存枚举获取对于的流程报表
        /// </summary>
        /// <param name="db">要查询数据的数据映射名称</param>
        /// <param name="leave">流程统计</param>
        /// <returns>一个对于的报表实列</returns>
        public static IMFReoprt MFDayLeaveReport(string db,DayLeave leave)
        {
            var date = DateTime.Now.Date.AddDays(-1);
            var endLogin = date.AddDays(1).AddSeconds(-1);
            switch (leave)
            {
                case DayLeave.Yesterday:
                    return new MFDayLeaveReport(db, date.AddDays(-1), date.AddSeconds(-1), date, endLogin);
                case DayLeave._3Day:
                    return new MFDayLeaveReport(db, date.AddDays(-2), date.AddDays(-1).AddSeconds(-1), date, endLogin);
                case DayLeave._7Day:
                    return new MFDayLeaveReport(db, date.AddDays(-6), date.AddDays(-5).AddSeconds(-1), date, endLogin);
                default:
                    throw new NotSupportedException();
            }
        }


        /// <summary>
        /// 根据留存枚举获取对于的流程报表
        /// </summary>
        /// <param name="db">要查询数据的数据映射名称</param>
        /// <param name="leave">流程统计</param>
        /// <returns>一个对于的报表实列</returns>
        public static IMFReoprt MFDayLeaveReportAdid(string db, DayLeave leave)
        {
            var date = DateTime.Now.Date.AddDays(-1);
            var endLogin = date.AddDays(1).AddSeconds(-1);
            switch (leave)
            {
                case DayLeave.Yesterday:
                    return new MFDayLeaveReportAdid(db, date.AddDays(-1), date.AddSeconds(-1), date, endLogin);
                case DayLeave._3Day:
                    return new MFDayLeaveReportAdid(db, date.AddDays(-2), date.AddDays(-1).AddSeconds(-1), date, endLogin);
                case DayLeave._7Day:
                    return new MFDayLeaveReportAdid(db, date.AddDays(-6), date.AddDays(-5).AddSeconds(-1), date, endLogin);
                default:
                    throw new NotSupportedException();
            }
        }


    }
}
