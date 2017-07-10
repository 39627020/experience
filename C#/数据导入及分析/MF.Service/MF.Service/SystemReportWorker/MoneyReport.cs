using MF.Service.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Service.SystemReportWorker
{
    class MoneyReport
    {
        public static void CreateCurrencyReport() {
            Log.WriteLog("CreateCurrencyReport start");
            //创建mfReader
            var mfReadHelper = DbManager.GetDbHelper(UserDbinfo.MF_DB);
            //创建record
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);

            var userParam = new SqlParameter[] {
                             new SqlParameter("@yesterdaySecond",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@todaySecond",TimeUtil.getRecordTimeByToday()),
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()-1)
                             };
            String keyName = "ChannelId";

            //每日保险箱剩余数量,每日保险箱剩余自己数量
            var getStrongBoxMoneySql = "select sum(currency) as money , count(1) as count ,LEFT(chargeId,3) as ChannelId   from Strongbox with(nolock) group by LEFT(chargeId,3)";
            Dictionary<String, DataRow> strongBoxMoneyMap = UserDbinfo.ListToRowMap(mfReadHelper.QueryParamTable(getStrongBoxMoneySql, userParam),keyName);

            //每日新建保险箱的数量
            var getCreateStrongBoxSql = "select count(1) as count,LEFT(chargeId,3) as ChannelId from Strongbox with(nolock) where  CreateDate BETWEEN @yesterdaySecond and @todaySecond  group by LEFT(chargeId,3)";
            Dictionary<String, long> createStrongMap = UserDbinfo.ListToMap(mfReadHelper.QueryParamTable(getCreateStrongBoxSql, userParam), "count", keyName);

            //每日销毁保险箱数量 昨日保险箱总数-今日保险箱总数-今日新增保险箱总数
            var getDayBeforYestordayCurrencyReportSql = "select max(StrongBoxCount) as count, ChannelId from NewCurrencyReport where day = @Day group by ChannelId";
            Dictionary<String, long> dayBeforYestordayCurrencyReportMap = UserDbinfo.ListToMap(recordHelper.QueryParamTable(getDayBeforYestordayCurrencyReportSql, userParam), "count", keyName);

            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);
                //每日玩家剩余资金数量
                var getUserMoneySql = "select sum(currency) as money ,LEFT(chargeId,3) as ChannelId from Users with(nolock) group by LEFT(chargeId,3)";
                DataTable userMoneyTable = readHelper.QueryParamTable(getUserMoneySql, userParam);
                Dictionary<String, long> userMoneyMap = UserDbinfo.ListToMap(userMoneyTable, "money", keyName);
                Log.WriteLog("userMoneyMap" + userMoneyMap);
                Log.WriteLog("userMoneyTable" + userMoneyTable);

                //每⽇日抽⽔水总额 让⼟土⾖豆专⻔门建⽴立玩家离场时的抽⽔水字段，根据这个 字段记录总抽⽔水
                var getWaterPullMoneySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 30 group by LEFT(chargeId,3) ";
                Dictionary<String, long> waterPullMoneyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getWaterPullMoneySql, userParam), "money", keyName);

                //每日充值的数量
                var getChargeCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type in (29,26) group by LEFT(chargeId,3) ";
                Dictionary<String, long> chargeCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getChargeCurrencySql, userParam), "money", keyName);

                //每日购买房卡的数量
                var getBuyRoomCurrencySql = "select sum(abs(Num)) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 24 group by LEFT(chargeId,3) ";
                Dictionary<String, long> buyRoomCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getBuyRoomCurrencySql, userParam), "money", keyName);

                //领取救济（type=16），(Currency)  总数
                var getReliefCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 16 group by LEFT(chargeId,3) ";
                Dictionary<String, long> reliefCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getReliefCurrencySql, userParam), "money", keyName);

                //1.系统赠送(type=10)，   (Currency)  总数
                var getSystemDeliveryCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 10 group by LEFT(chargeId,3) ";
                Dictionary<String, long> systemDeliveryCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getSystemDeliveryCurrencySql, userParam), "money", keyName);

                //每⽇日免费产出元宝数 免费比赛 （MatchID type = 94541，95541）（type=6），(Currency)  总数
                var getFreeGameCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 6 and MatchId in (94541,95541) group by LEFT(chargeId,3) ";
                Dictionary<String, long> freeGameCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getFreeGameCurrencySql, userParam), "money", keyName);

                // 每日定时赛实际发元宝总金额 找找到定时赛（MatchID type = 94571，98171，94271）（type=7），(Currency)  总数
                var getResurrectionCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 7 and MatchId in (94571,98171,94271) group by LEFT(chargeId,3) ";
                Dictionary<String, long> resurrectionCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getResurrectionCurrencySql, userParam), "money", keyName);

                // 每日定时赛实际发元宝总金额 找到定时赛（MatchID type = 94571，98171，94271） （type=6），(Currency)  总数
                var getTimingCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 6 and MatchId in (94571,98171,94271) group by LEFT(chargeId,3) ";
                Dictionary<String, long> timingCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getTimingCurrencySql, userParam), "money", keyName);

                //每日商城兑换元宝的数量    商城兑换元宝(type=9)，   (Currency)  总数
                var getExchangeCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 9 group by LEFT(chargeId,3) ";
                Dictionary<String, long> exchangeCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getExchangeCurrencySql, userParam), "money", keyName);

                //每日管理员发放    商城兑换元宝(type=8)，   (Currency)  总数
                var getAdminCurrencySql = "select sum(Num) as money  ,LEFT(chargeId,3) as ChannelId from CurrcryRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond  and type = 27 group by LEFT(chargeId,3) ";
                Dictionary<String, long> adminCurrencyMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getAdminCurrencySql, userParam), "money", keyName);



                foreach (DataRow row in userMoneyTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        long leftCurrency = 0;
                        long takeCurrency = 0;
                        long strongBoxCurrency = 0;
                        long strongBoxCount = 0;
                        long strongBoxCreated = 0;
                        long reliefCurrency = 0;
                        long systemDeliveryCurrency = 0;
                        long freeGameCurrency = 0;
                        long exchangeCurrency = 0;
                        long adminCurrency = 0;
                        long buyRoomCurrency = 0;
                        long timingCurrency = 0;
                        long resurrectionCurrency = 0;
                        long chargeCurrency = 0;
                        long strongBoxCountOfDayBeforeYestoday = 0;

                        userMoneyMap.TryGetValue(channelId, out leftCurrency);
                        waterPullMoneyMap.TryGetValue(channelId, out takeCurrency);
                        DataRow strongBoxRow = null;
                        strongBoxMoneyMap.TryGetValue(channelId, out strongBoxRow);
                        if (null != strongBoxRow) {
                            long.TryParse(strongBoxRow["money"].ToString(), out strongBoxCurrency);
                            long.TryParse(strongBoxRow["count"].ToString(), out strongBoxCount);
                        }
                        createStrongMap.TryGetValue(channelId, out strongBoxCreated);
                        reliefCurrencyMap.TryGetValue(channelId, out reliefCurrency);
                        systemDeliveryCurrencyMap.TryGetValue(channelId, out systemDeliveryCurrency);
                        freeGameCurrencyMap.TryGetValue(channelId, out freeGameCurrency);
                        exchangeCurrencyMap.TryGetValue(channelId, out exchangeCurrency);
                        adminCurrencyMap.TryGetValue(channelId, out adminCurrency);
                        buyRoomCurrencyMap.TryGetValue(channelId, out buyRoomCurrency);
                        timingCurrencyMap.TryGetValue(channelId, out timingCurrency);
                        resurrectionCurrencyMap.TryGetValue(channelId, out resurrectionCurrency);
                        chargeCurrencyMap.TryGetValue(channelId, out chargeCurrency);
                        dayBeforYestordayCurrencyReportMap.TryGetValue(channelId, out strongBoxCountOfDayBeforeYestoday);
                        //昨日保险箱总数-(今日保险箱总数-今日新增保险箱总数)
                        long strongBoxDistory = strongBoxCountOfDayBeforeYestoday - (strongBoxCount - strongBoxCreated);


                        //定义参数
                        var newCurrencyReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@LeftCurrency",leftCurrency+strongBoxCurrency),
                             new SqlParameter("@TakeCurrency",takeCurrency),
                             new SqlParameter("@StrongBoxCurrency",strongBoxCurrency),
                             new SqlParameter("@StrongBoxCount",strongBoxCount),
                             new SqlParameter("@StrongBoxCreated",strongBoxCreated),
                             new SqlParameter("@ReliefCurrency",reliefCurrency),
                             new SqlParameter("@SystemDeliveryCurrency",systemDeliveryCurrency),
                             new SqlParameter("@FreeGameCurrency",freeGameCurrency),
                             new SqlParameter("@ExchangeCurrency",exchangeCurrency),
                             new SqlParameter("@AdminCurrency",adminCurrency),
                             new SqlParameter("@BuyRoomCurrency",buyRoomCurrency),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@ChannelId",channelId),
                             new SqlParameter("@TimingCurrency",timingCurrency),
                             new SqlParameter("@ResurrectionCurrency",resurrectionCurrency),
                             new SqlParameter("@ChargeCurrency",chargeCurrency),
                             new SqlParameter("@StrongBoxDistory",strongBoxDistory)
                        };

                        //插入SQL

                        var insertSql = "insert into NewCurrencyReport (Day,LeftCurrency,TakeCurrency,StrongBoxCurrency,StrongBoxCount,StrongBoxCreated,ReliefCurrency,SystemDeliveryCurrency,FreeGameCurrency,ExchangeCurrency,AdminCurrency,BuyRoomCurrency,Created,Modified,ChannelId,TimingCurrency,ResurrectionCurrency,ChargeCurrency,StrongBoxDistory) values(@Day,@LeftCurrency,@TakeCurrency,@StrongBoxCurrency,@StrongBoxCount,@StrongBoxCreated,@ReliefCurrency,@SystemDeliveryCurrency,@FreeGameCurrency,@ExchangeCurrency,@AdminCurrency,@BuyRoomCurrency,@Created,@Modified,@ChannelId,@TimingCurrency,@ResurrectionCurrency,@ChargeCurrency,@StrongBoxDistory)";
                        recordHelper.Execute(insertSql, CommandType.Text, newCurrencyReportParam);

                    }
                }


            }
            Log.WriteLog("CreateCurrencyReport end");
        }

        public static void CreateBeanReport()
        {
            Log.WriteLog("CreateBeanReport start");
            //创建record
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);

            var userParam = new SqlParameter[] {
                             new SqlParameter("@yesterdaySecond",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@todaySecond",TimeUtil.getRecordTimeByToday())
                             };
            String keyName = "ChannelId";

            //每⽇日剩余金豆总数
            var getLeftBeanSql = "select sum(bean) as money ,LEFT(chargeId,3) as ChannelId from Users with(nolock) group by LEFT(chargeId,3)";
            //每日金豆消耗总数 （type = 9）
            var getShopExchangeBeanSql = "select sum(num) as money,LEFT(chargeId,3) as ChannelId from beanRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and type =9 group by LEFT(chargeId,3) ";
            //每日定时赛实际发金豆总金额 找到定时赛（MatchID type = 94571，98171，94271） （type = 6）
            var getTimingBeanSql = "select sum(num) as money,LEFT(chargeId,3) as ChannelId from beanRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and type =6 and  matchId in (94571,98171,94271) group by LEFT(chargeId,3) ";
            //每日话费比赛统计
            var getTelephoneFareBeanSql = "select sum(num) as money,LEFT(chargeId,3) as ChannelId from beanRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and type =6 and  matchId in (94521,94321,97321,96921,95521,94221,98121,98221,94121) group by LEFT(chargeId,3) ";

            //每日管理员发放
            var getAdminBeanSql = "select sum(num) as money,LEFT(chargeId,3) as ChannelId from beanRecord with(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and type =27 group by LEFT(chargeId,3) ";
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);

                DataTable leftBeanTable = readHelper.QueryParamTable(getLeftBeanSql, userParam);
                Dictionary<String, long> leftBeanMap = UserDbinfo.ListToMap(leftBeanTable, "money", keyName);

                Dictionary<String, long> shopExchangeBeanMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getShopExchangeBeanSql, userParam), "money", keyName);
                Dictionary<String, long> timingBeanMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getTimingBeanSql, userParam), "money", keyName);
                Dictionary<String, long> adminBeanMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getAdminBeanSql, userParam), "money", keyName);
                Dictionary<String, long> telephoneFareBeanMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(getTelephoneFareBeanSql, userParam), "money", keyName);

                foreach (DataRow row in leftBeanTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        long leftBean = 0;
                        long shopExchangeBean = 0;
                        long timingBean = 0;
                        long adminBean = 0;
                        long telephoneFareBean = 0;


                        leftBeanMap.TryGetValue(channelId, out leftBean);
                        shopExchangeBeanMap.TryGetValue(channelId, out shopExchangeBean);
                        timingBeanMap.TryGetValue(channelId, out timingBean);
                        adminBeanMap.TryGetValue(channelId, out adminBean);
                        telephoneFareBeanMap.TryGetValue(channelId, out telephoneFareBean);

                        //定义参数
                        var newBeanReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@LeftBean",leftBean),
                             new SqlParameter("@ShopExchangeBean",shopExchangeBean),
                             new SqlParameter("@TimingBean",timingBean),
                             new SqlParameter("@TelephoneFareBean",telephoneFareBean),
                             new SqlParameter("@AdminBean",adminBean),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@ChannelId",channelId),
                        };

                        var insertSql = "insert into NewBeanReport(Day,LeftBean,ShopExchangeBean,TimingBean,TelephoneFareBean,AdminBean,Created,ChannelId) values(@Day,@LeftBean,@ShopExchangeBean,@TimingBean,@TelephoneFareBean,@AdminBean,@Created,@ChannelId)";
                        recordHelper.Execute(insertSql, CommandType.Text, newBeanReportParam);
                    }
                }
            }
            Log.WriteLog("CreateBeanReport end");
        }

        public static void CreatNewGameReport() {
            Log.WriteLog("CreatNewGameReport start");

            //创建record
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);

            var userParam = new SqlParameter[] {
                             new SqlParameter("@yesterdaySecond",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@todaySecond",TimeUtil.getRecordTimeByToday())
                             };
            String keyName = "ChannelId";

            //7带⼊入为正数
            var getGameInfoSql = "select t1.ChannelId as ChannelId, t1.GameId as GameId,t1.MatchId as MatchId, t1.RuleId as RuleId,t1.count as Actives, t2.money as Win,  t5.money as WinBean, t3.money as Lose , t6.money as LoseBean,t4.money as Shrink from   (select count(distinct account) as count ,LEFT(chargeId,3) as ChannelId, GameId,MatchId,RuleId from CurrcryRecord WITH(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and Type in (6,7,30) group by LEFT(chargeId,3),GameId,MatchId,RuleId) as t1     left JOIN (select sum(abs(num)) as money ,LEFT(chargeId,3) as ChannelId, GameId,MatchId,RuleId from CurrcryRecord WITH(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and  Type = 7 group by LEFT(chargeId,3),GameId,MatchId,RuleId) as t2  on  t1.GameId = t2.GameId and t1.MatchId = t2.MatchId and t1.ChannelId = t2.ChannelId and t1.RuleId = t2.RuleId left JOIN (select sum(abs(num)) as money ,LEFT(chargeId,3) as ChannelId, GameId,MatchId,RuleId from CurrcryRecord WITH(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and Type = 6 group by LEFT(chargeId,3),GameId,MatchId,RuleId) as t3  ON t1.GameId = t3.GameId and t1.MatchId = t3.MatchId and t1.ChannelId = t3.ChannelId and t1.RuleId = t3.RuleId  left JOIN (select sum(abs(num)) as money ,LEFT(chargeId,3) as ChannelId, GameId,MatchId,RuleId from CurrcryRecord WITH(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and Type = 30 group by LEFT(chargeId,3),GameId,MatchId,RuleId) as t4  ON t1.GameId = t4.GameId and t1.MatchId = t4.MatchId and t1.ChannelId = t4.ChannelId and t1.RuleId = t4.RuleId    left JoIN (select sum(abs(num)) as money ,LEFT(chargeId,3) as ChannelId, GameId,MatchId,RuleId from BeanRecord With(nolock)  where time BETWEEN @yesterdaySecond  and @todaySecond and Type = 7 group by LEFT(chargeId,3),GameId,MatchId,RuleId) as t5  ON t1.GameId = t5.GameId and t1.MatchId = t5.MatchId and t1.ChannelId = t5.ChannelId and t1.RuleId = t5.RuleId left JoIN (select sum(abs(num)) as money ,LEFT(chargeId,3) as ChannelId, GameId,MatchId,RuleId from BeanRecord With(nolock) where time BETWEEN @yesterdaySecond  and @todaySecond and Type = 6 group by LEFT(chargeId,3),GameId,MatchId,RuleId) as t6 ON t1.GameId = t6.GameId and t1.MatchId = t6.MatchId and t1.ChannelId = t6.ChannelId and t1.RuleId = t6.RuleId ";

            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);

                DataTable activesTable = readHelper.QueryParamTable(getGameInfoSql, userParam);
                foreach (DataRow row in activesTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        long matchId = 0;
                        long gameId = 0;
                        long win = 0;
                        long winBean = 0;
                        long lose = 0;
                        long loseBean = 0;
                        long shrink = 0;
                        long actives = 0;
                        long ruleId = 0;

                        long.TryParse(row["MatchId"].ToString(), out matchId);
                        long.TryParse(row["GameId"].ToString(), out gameId);
                        long.TryParse(row["Win"].ToString(), out win);
                        long.TryParse(row["WinBean"].ToString(), out winBean);
                        long.TryParse(row["Lose"].ToString(), out lose);
                        long.TryParse(row["LoseBean"].ToString(), out loseBean);
                        long.TryParse(row["Shrink"].ToString(), out shrink);
                        long.TryParse(row["Actives"].ToString(), out actives);
                        long.TryParse(row["RuleId"].ToString(), out ruleId);


                        //定义参数
                        var newGameReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@MatchId",matchId),
                             new SqlParameter("@GameId",gameId),
                             new SqlParameter("@Win",win+winBean),
                             new SqlParameter("@Lose",lose+loseBean),
                             new SqlParameter("@Shrink",shrink),
                             new SqlParameter("@ChannelId",channelId),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Actives",actives),
                             new SqlParameter("@RuleId",ruleId),
                        };

                        var insertSql = "insert into NewGameReport (Day,MatchId,GameId,Win,Lose,Shrink,ChannelId,Created,Modified,Actives,RuleId) values(@Day,@MatchId,@GameId,@Win,@Lose,@Shrink,@ChannelId,@Created,@Modified,@Actives,@RuleId)";
                        recordHelper.Execute(insertSql, CommandType.Text, newGameReportParam);



                    }
                }



                    }
            Log.WriteLog("CreatNewGameReport end");
        }


    }
}
