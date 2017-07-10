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
    class UserReport
    {

        //DbtableToMap
        private static Dictionary<String, int> ListToMap(DataTable table)
        {
            Dictionary<String, int> result = new Dictionary<String, int>();
            foreach (DataRow row in table.Rows)
            {
                String channelId = row["ChannelId"].ToString();
                int num = 0;
                int.TryParse(row["userAccount"].ToString(), out num);
                if (null != channelId && channelId.Length != 0)
                {
                    result.Add(channelId, num);
                }
            }
            return result;
        }
        public static Boolean CreateRegReport()
        {
            Log.WriteLog("CreateRegReport start");

            //新增用户总数
            String newUserSQL = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where  t2.master =''  and t1.RegTime BETWEEN @Yestoday and @Today and t1.type != 1  GROUP by lEFT(t1.chargeId,3)";
            //Iphone注册总数
            String iphoneSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where t2.master =''  and t1.ClientType = 5 and t1.RegTime BETWEEN @Yestoday and @Today and t1.type != 1 GROUP by lEFT(t1.chargeId,3)";
            //android注册总数
            String androidSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where t2.master ='' and t1.ClientType = 6 and t1.RegTime BETWEEN @Yestoday and @Today and t1.type != 1 GROUP by lEFT(t1.chargeId,3)";
            //ipad注册总数
            String ipadSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where t2.master ='' and t1.ClientType = 3 and t1.RegTime BETWEEN @Yestoday and @Today and t1.type != 1 GROUP by lEFT(t1.chargeId,3)";
            //PC注册总数
            String PCSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where t2.master =''  and t1.ClientType = 2 and t1.RegTime BETWEEN @Yestoday and @Today and t1.type != 1 GROUP by lEFT(t1.chargeId,3)";
            //游客转正总数
            String touristToUserSql = "select count(DISTINCT(Guid)) as userAccount,lEFT(chargeId,3)  as ChannelId from Users with(nolock) where regitime BETWEEN @BeginSec and @EndSec and master ='' and guest=2 GROUP by lEFT(chargeId,3)";
            //微信注册总数
            String weixinSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where t2.master ='' and t1.type = 3 and t1.RegTime BETWEEN @Yestoday and @Today GROUP by lEFT(t1.chargeId,3)";
            //游客总数
            String touirstSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where t2.master ='' and t1.type = 1 and t1.RegTime BETWEEN @Yestoday and @Today GROUP by lEFT(t1.chargeId,3)";
            //子账号总数
            String subAccSql = "SELECT count(DISTINCT(t1.Guid)) as userAccount, lEFT(t1.chargeId,3)  as ChannelId from RegistRecord t1 lEFT join Users t2 on t1.account = t2.account where  t2.master != '' AND t2.master is not null and t1.type = 4 and t1.RegTime BETWEEN @Yestoday and @Today GROUP by lEFT(t1.chargeId,3)";
            //总用户数量
            String allUserSql = "SELECT count(DISTINCT(GUID)) as userAccount, lEFT(chargeId,3) as ChannelId from users with(nolock) where master =''  and flag = 1 GROUP by lEFT(chargeId,3)";
            //领取救济人数
            String getRelifSQL = "SELECT count(DISTINCT(t1.Account)) as userAccount, lEFT(t1.chargeId,3) as ChannelId  from CurrcryRecord t1  lEFT Join Users t2 on t1.account = t2.account where t2.master =''  and t1.Type = 16 and t1.Time between @BeginSec and @EndSec GROUP by lEFT(t1.chargeId,3)";

            //插入数据
            String insertInfo = "insert into NewRegReport (day,NewUser,NewVisitor,ClientUser,IphoneUser,AndroidUser,IpadUser,TouristToUser,SubAccTotal,AccTotal,ChannelId,Created,Modified,Relief,WeixinUser) values(@day, @NewUser, @NewVisitor, @ClientUser, @IphoneUser, @AndroidUser, @IpadUser, @TouristToUser, @SubAccTotal, @AccTotal, @ChannelId, @Created, @Modified, @Relief, @WeixinUser)";
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
            var userParam = new SqlParameter[] {
                             new SqlParameter("@Yestoday",TimeUtil.getYestodayTimstamp()),
                             new SqlParameter("@Today",TimeUtil.getTodayTimstamp())
                    };
            var relifParam = new SqlParameter[] {
                             new SqlParameter("@BeginSec",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@EndSec",TimeUtil.getRecordTimeByToday())
                             };
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);
                Dictionary<String, int> newUser = ListToMap(readHelper.QueryParamTable(newUserSQL, userParam));
                Dictionary<String, int> iphone = ListToMap(readHelper.QueryParamTable(iphoneSql, userParam));
                Dictionary<String, int> android = ListToMap(readHelper.QueryParamTable(androidSql, userParam));
                Dictionary<String, int> ipad = ListToMap(readHelper.QueryParamTable(ipadSql, userParam));
                Dictionary<String, int> PC = ListToMap(readHelper.QueryParamTable(PCSql, userParam));
                Dictionary<String, int> touristToUser = ListToMap(readHelper.QueryParamTable(touristToUserSql, relifParam));
                Dictionary<String, int> weixin = ListToMap(readHelper.QueryParamTable(weixinSql, userParam));
                Dictionary<String, int> touirst = ListToMap(readHelper.QueryParamTable(touirstSql, userParam));
                DataTable allUserTable = readHelper.QueryParamTable(allUserSql, userParam);
                Dictionary<String, int> allUser = ListToMap(allUserTable);
                Dictionary<String, int> subAcc = ListToMap(readHelper.QueryParamTable(subAccSql, userParam));

                Dictionary<String, int> getRelif = ListToMap(readHelper.QueryParamTable(getRelifSQL, relifParam));
                foreach (DataRow row in allUserTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        int newUserNum = 0;
                        int newVisitorNum = 0;
                        int clientUserNum = 0;
                        int iphoneUserNum = 0;
                        int androidUserNum = 0;
                        int ipadUserNum = 0;
                        int touristToUserNum = 0;
                        int subAccTotalNum = 0;
                        int weixinNum = 0;
                        int allUserNum = 0;
                        int reliefNum = 0;
                        newUser.TryGetValue(channelId, out newUserNum);
                        touirst.TryGetValue(channelId, out newVisitorNum);
                        PC.TryGetValue(channelId, out clientUserNum);
                        iphone.TryGetValue(channelId, out iphoneUserNum);
                        android.TryGetValue(channelId, out androidUserNum);
                        ipad.TryGetValue(channelId, out ipadUserNum);
                        touristToUser.TryGetValue(channelId, out touristToUserNum);
                        subAcc.TryGetValue(channelId, out subAccTotalNum);
                        allUser.TryGetValue(channelId, out allUserNum);
                        getRelif.TryGetValue(channelId, out reliefNum);
                        weixin.TryGetValue(channelId, out weixinNum);

                        var newRegReportParam = new SqlParameter[] {
                             new SqlParameter("@day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@NewUser",newUserNum),
                             new SqlParameter("@NewVisitor",newVisitorNum),
                             new SqlParameter("@ClientUser",clientUserNum),
                             new SqlParameter("@IphoneUser",iphoneUserNum),
                             new SqlParameter("@AndroidUser",androidUserNum),
                             new SqlParameter("@IpadUser",ipadUserNum),
                             new SqlParameter("@TouristToUser",touristToUserNum),
                             new SqlParameter("@SubAccTotal",subAccTotalNum),
                             new SqlParameter("@AccTotal",allUserNum),
                             new SqlParameter("@ChannelId",channelId),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Relief",reliefNum),
                             new SqlParameter("@WeixinUser",weixinNum)
                        };
                        recordHelper.Execute(insertInfo, CommandType.Text, newRegReportParam);

                    }

                }



            }
            Log.WriteLog("CreateRegReport end");
            return true;
        }
        public static Boolean CreateUserReport()
        {
            Log.WriteLog("CreateUserReport start");
            //登录用户数量统计
            String loginNumSql = "SELECT count(DISTINCT(Guid)) as userAccount, lEFT(chargeId,3)  as ChannelId ,LoginDevice as device from LoginLog where LoginTime BETWEEN @Yestoday and @Today group by lEFT(chargeId,3),LoginDevice ";
            //注册用户数量统计
            String regNumSql = "SELECT count(DISTINCT(Guid)) as userAccount, lEFT(chargeId,3)  as ChannelId ,ClientType as device from RegistRecord where Regtime BETWEEN @Yestoday and @Today group by lEFT(chargeId,3),ClientType";
            //插入数据
            String insertSql = "insert into newUserReport(regNum,loginNum,device,channelId,day,created) values(@regNum,@loginNum,@device,@channelId,@day,@created)";

            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
            var userParam = new SqlParameter[] {
                             new SqlParameter("@Yestoday",TimeUtil.getYestodayTimstamp()),
                             new SqlParameter("@Today",TimeUtil.getTodayTimstamp())
                    };
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);
                DataTable loginTable = readHelper.QueryParamTable(loginNumSql, userParam);
                DataTable regTable = readHelper.QueryParamTable(regNumSql, userParam);
                //以login为主记录数据
                foreach (DataRow loginRow in loginTable.Rows)
                {
                    String channelId = loginRow["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0) {
                        int loginNum = 0;
                        int.TryParse(loginRow["userAccount"].ToString(), out loginNum);
                        int device = -1;
                        int.TryParse(loginRow["device"].ToString(), out device);
                        int regNum = 0;
                        foreach (DataRow regRow in regTable.Rows) {
                            String innerChannelId = regRow["ChannelId"].ToString();
                            if (null != innerChannelId && innerChannelId.Length != 0) {
                                int innerDevice = -1;
                                int.TryParse(regRow["device"].ToString(), out innerDevice);
                                if (innerDevice == device) {
                                    int.TryParse(regRow["userAccount"].ToString(), out regNum);
                                    break;
                                }
                            }
                        }
                        //处理完成以后插入数据
                        var newRegReportParam = new SqlParameter[] {
                             new SqlParameter("@regNum",regNum),
                             new SqlParameter("@loginNum",loginNum),
                             new SqlParameter("@device",device),
                             new SqlParameter("@channelId",channelId),
                             new SqlParameter("@day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@created",TimeUtil.getDbDateByNow())
                        };
                        recordHelper.Execute(insertSql, CommandType.Text, newRegReportParam);
                    }

                }
                //以Reg为主记录数据
                foreach (DataRow regRow in regTable.Rows)
                {
                    String channelId = regRow["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        int regNum = 0;
                        int.TryParse(regRow["userAccount"].ToString(), out regNum);
                        int device = -1;
                        int.TryParse(regRow["device"].ToString(), out device);
                        int loginNum = 0;
                        foreach (DataRow loginRow in loginTable.Rows)
                        {
                            String innerChannelId = loginRow["ChannelId"].ToString();
                            if (null != innerChannelId && innerChannelId.Length != 0)
                            {
                                int innerDevice = -1;
                                int.TryParse(loginRow["device"].ToString(), out innerDevice);
                                if (innerDevice == device)
                                {
                                    int.TryParse(loginRow["userAccount"].ToString(), out loginNum);
                                    break;
                                }
                            }
                        }
                        //这么做就可以避免出现当Login数据为0时没有插入Reg数据
                        if (loginNum == 0) {
                            //处理完成以后插入数据
                            var newRegReportParam = new SqlParameter[] {
                             new SqlParameter("@regNum",regNum),
                             new SqlParameter("@loginNum",loginNum),
                             new SqlParameter("@device",device),
                             new SqlParameter("@channelId",channelId),
                             new SqlParameter("@day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@created",TimeUtil.getRecordTimeByCurrent())
                        };
                            recordHelper.Execute(insertSql, CommandType.Text, newRegReportParam);
                        }
                    }

                }
            }
            Log.WriteLog("CreateUserReport end");
            return true;
        }

        public static void createSubAccReport()
        {
            Log.WriteLog("CreateUserReport start");
            var getSubAccSql = "select sum(currency) as money, count(1) as count ,LEFT(chargeId,3) as ChannelId from Users with(nolock) where len(master) = 0 group by LEFT(chargeId,3) ";
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);
                DataTable subAccTable = readHelper.QueryTable(getSubAccSql);
                foreach (DataRow row in subAccTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        long subAccNum = 0;
                        long subAccCurrency = 0;

                        long.TryParse(row["money"].ToString(), out subAccCurrency);
                        long.TryParse(row["count"].ToString(), out subAccNum);

                        //定义参数
                        var newSubAccReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@SubAccNum",subAccNum),
                             new SqlParameter("@SubAccCurrency",subAccCurrency),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@ChannelId",channelId),
                        };

                        //插入SQL
                        var insertSql = "insert into NewSubAccReport(Day,SubAccNum,SubAccCurrency,Created,Modified,ChannelId) values (@Day,@SubAccNum,@SubAccCurrency,@Created,@Modified,@ChannelId)";
                        recordHelper.Execute(insertSql, CommandType.Text, newSubAccReportParam);

                    }
                }
            }
            Log.WriteLog("CreateUserReport end");
        }

        public static void createADIDReport() {
            Log.WriteLog("createADIDReport start");
            //添加设备去重(distinct([GUID]) by lwb
            var getADIDSql = "select count(distinct([GUID]) as count,LEFT(ChargeId,3) as ChannelId,ADID from Users with(nolock) where regitime between @yesterdaySecond and  @todaySecond group by LEFT(ChargeId,3), ADID";
            var insertSql = "insert into NewADIDReport (day,ADID,Num,ChannelId,created,modified) values (@day,@ADID,@Num,@ChannelId,@created,@modified)";
            //用户表参数
            var userParam = new SqlParameter[] {
                             new SqlParameter("@Yestoday",TimeUtil.getYestodayTimstamp()),
                             new SqlParameter("@Today",TimeUtil.getTodayTimstamp()),
                             new SqlParameter("@yesterdaySecond",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@todaySecond",TimeUtil.getRecordTimeByToday()),
                             new SqlParameter("@threedaySecond",TimeUtil.getRecordTimeByThreeDayLeft()),
                             new SqlParameter("@sevendaySecond",TimeUtil.getRecordTimeBySevenDayLeft()),
                    };

            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);
                DataTable ADIDTable = readHelper.QueryParamTable(getADIDSql, userParam);
                foreach (DataRow row in ADIDTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        int num = 0;
                        int.TryParse(row["count"].ToString(), out num);
                        //定义参数
                        var newADIDReportParam = new SqlParameter[] {
                             new SqlParameter("@day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@ADID",row["ADID"].ToString()),
                             new SqlParameter("@Num",num),
                             new SqlParameter("@ChannelId",channelId),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                            
                        };
                        recordHelper.Execute(insertSql, CommandType.Text, newADIDReportParam);
                    }
                }
            }

                        Log.WriteLog("createADIDReport end");
        }


        public static void createQmallReport() {
            Log.WriteLog("createQmallReport start");
            var userParam = new SqlParameter[] {
                             new SqlParameter("@Yestoday",TimeUtil.getYestodayTimstamp()),
                             new SqlParameter("@Today",TimeUtil.getTodayTimstamp())
                    };
            var getProductInfoSql = "select count(1) as SellNum, product_id as product, LEFT(charge_id,3) as ChannelId from QmallRecord where create_date between @Yestoday and @Today  group by  LEFT(charge_id,3),product_id";
////            var getRoomCartNumSql = "select sum(RoomCard) as money, count(1) as count ,LEFT(chargeId,3) as ChannelId from Users with(nolock) where len(master) = 0 group by LEFT(chargeId,3)";
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);
                DataTable productInfoTable = readHelper.QueryParamTable(getProductInfoSql, userParam);
                foreach (DataRow row in productInfoTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        long product = 0;
                        long sellNum = 0;


                        long.TryParse(row["product"].ToString(), out product);
                        long.TryParse(row["SellNum"].ToString(), out sellNum);


                        //定义参数
                        var newQmallReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@Product",product),
                              new SqlParameter("@SellNum",sellNum),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@ChannelId",channelId),
                        };

                        //插入SQL
                        var insertSql = "insert into NewQmallReport(Day,Product,SellNum,Created,Modified,ChannelId) values (@Day,@Product,@SellNum,@Created,@Modified,@ChannelId)";
                        recordHelper.Execute(insertSql, CommandType.Text, newQmallReportParam);

                    }
                }
            }
            Log.WriteLog("createQmallReport end");

        }




        public static void createAdReport() {
            Log.WriteLog("createAdReport start");
            //获取活跃用户SQL
            var getActUserNumSql = "SELECT count(distinct(t2.LastGUID)) as count , LEFT(t1.chargeId,3) as ChannelId from CurrcryRecord as t1 join Users as t2 on t1.Account = t2.Account where t1.time BETWEEN @yesterdaySecond  and @todaySecond and t1.type in(6,7,30,9,26)  group by LEFT(t1.chargeId,3)";

            //登录用户数量统计
            var loginNumSql = "SELECT count(DISTINCT(Guid)) as count, lEFT(chargeId,3)  as ChannelId from LoginLog where LoginTime BETWEEN @Yestoday and @Today group by lEFT(chargeId,3) ";

            //注册用户数量统计
            String regNumSql = "SELECT count(DISTINCT(Guid)) as count, lEFT(chargeId,3)  as ChannelId  from RegistRecord where Regtime BETWEEN @Yestoday and @Today group by lEFT(chargeId,3)";

            

            //每⽇日充值订单完成数量量（根据充值渠道区分
            //根据充值记录表统计当天充值订单完成加钱的 数量量 每⽇日充值⾦金金额数量量（根据充值渠道区分）
            //根据充值记录表统计⽀支付成功的⾦金金额 每⽇日充值⽤用户数量量（根据充值渠道区分）
            String getFinishOrderSql = "select count(1) as count, sum(sumbitmoney) as sumbitmoney , count(distinct(account)) as acccountNum ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where createDate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 group by LEFT(chargeId,3)";



            //用户表参数
            var userParam = new SqlParameter[] {
                             new SqlParameter("@Yestoday",TimeUtil.getYestodayTimstamp()),
                             new SqlParameter("@Today",TimeUtil.getTodayTimstamp()),
                             new SqlParameter("@yesterdaySecond",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@todaySecond",TimeUtil.getRecordTimeByToday()),
                             new SqlParameter("@threedaySecond",TimeUtil.getRecordTimeByThreeDayLeft()),
                             new SqlParameter("@sevendaySecond",TimeUtil.getRecordTimeBySevenDayLeft()),
                    };

            ICacheProvider cache = CacheManager.Provider;
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
            var mfHelper = DbManager.GetDbHelper(UserDbinfo.MF_DB);
            String keyName = "ChannelId";
            Dictionary<String, DataRow> finishOrderMap = UserDbinfo.ListToRowMap(mfHelper.QueryParamTable(getFinishOrderSql, userParam), keyName);

            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);

                DataTable actUserNumTable = readHelper.QueryParamTable(getActUserNumSql, userParam);
                Dictionary<String, long> actUserNumMap = UserDbinfo.ListToMap(actUserNumTable, "count", keyName);

                Dictionary<String, long> oneDayLeftMap = UserDbinfo.ListToMap(_MFDayLeaveReportHelper.MFDayLeaveReport(dbInfo, DayLeave.Yesterday).Report(), "count", keyName);
                Dictionary<String, long> threeDayLeftMap = UserDbinfo.ListToMap(_MFDayLeaveReportHelper.MFDayLeaveReport(dbInfo, DayLeave._3Day).Report(), "count", keyName);
                Dictionary<String, long> sevenDayLeftMap = UserDbinfo.ListToMap(_MFDayLeaveReportHelper.MFDayLeaveReport(dbInfo, DayLeave._7Day).Report(), "count", keyName);

                Dictionary<String, long> loginNumMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(loginNumSql, userParam), "count", keyName);
                Dictionary<String, long> regNumMap = UserDbinfo.ListToMap(readHelper.QueryParamTable(regNumSql, userParam), "count", keyName);


                foreach (DataRow row in actUserNumTable.Rows)
                {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        long loginNum = 0;
                        long actUserNum = 0;
                        long oneDayLeft = 0;
                        long threeDayLeft = 0;
                        long sevenDayLeft = 0;
                        long regNum = 0;

                        decimal ARPU = 0.00m;
                        decimal payRate = 0.00m;
                        decimal ARPPU = 0.00m;
                        decimal chargeUserRate = 0.00m;

                        loginNumMap.TryGetValue(channelId, out loginNum);
                        actUserNumMap.TryGetValue(channelId, out actUserNum);
                        oneDayLeftMap.TryGetValue(channelId, out oneDayLeft);
                        threeDayLeftMap.TryGetValue(channelId, out threeDayLeft);
                        sevenDayLeftMap.TryGetValue(channelId, out sevenDayLeft);
                        regNumMap.TryGetValue(channelId, out regNum);



                        DataRow finishRow = null;
                        finishOrderMap.TryGetValue(channelId, out finishRow);
                        if (null != finishRow) {



                            long chargeNum = 0;
                            long chargeAccNum = 0;
                            long chargeMoney = 0;

                            long.TryParse(finishRow["count"].ToString(),out chargeNum);
                            long.TryParse(finishRow["sumbitmoney"].ToString(), out chargeMoney);
                            long.TryParse(finishRow["acccountNum"].ToString(), out chargeAccNum);

                            //计算数据
                            //这里用来计算如果活跃用户为0，默认设置为1，否则分母无限大
                            if (actUserNum == 0) actUserNum = 1;
                            //总充值数量/活跃⽤用户
                            ARPU = chargeMoney / actUserNum;
                            //付费⽤用户数/登录⽤用户数（去重）
                            if (loginNum > 0) {
                                 payRate = Math.Round((decimal)chargeAccNum / (decimal)loginNum,2);
                            }

                            //总充值数量/付费⽤用户数
                            if (chargeAccNum > 0) {
                                ARPPU = Math.Round((decimal)chargeMoney / (decimal)chargeAccNum,2);
                            }

                            //每⽇日的付费率 总充值⼈人数/活跃⽤用户
                            chargeUserRate = Math.Round((decimal)(chargeAccNum*100) / (decimal)actUserNum,2);
                        }

                        //从redis中获取昨天的key,用于计算最大值
                        String maxUserKey = "CHANNEL_ONLINE_COUNT_MAX" + channelId + DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                        long maxUserNum = 0;
                        long.TryParse(cache.Get(maxUserKey), out maxUserNum);

                        //定义参数
                        var newAdReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@LoginNum",loginNum),
                             new SqlParameter("@MaxNum",maxUserNum),
                             new SqlParameter("@ActUserNum",actUserNum),
                             new SqlParameter("@OneDayLeft",oneDayLeft),
                             new SqlParameter("@ThreeDayLeft",threeDayLeft),
                             new SqlParameter("@SevenDayLeft",sevenDayLeft),
                             new SqlParameter("@ARPU",ARPU),
                             new SqlParameter("@ARPPU",ARPPU),
                             new SqlParameter("@PayRate",payRate),
                             new SqlParameter("@ChargeUserRate",chargeUserRate),
                             new SqlParameter("@ChannelId",channelId),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@regNum",regNum),
                        };
                        var insertSql = "insert into NewAdReport(Day,LoginNum,MaxNum,ActUserNum,OneDayLeft,ThreeDayLeft,SevenDayLeft,ARPU,ARPPU,PayRate,ChargeUserRate,ChannelId,Created,Modified,regNum) values (@Day,@LoginNum,@MaxNum,@ActUserNum,@OneDayLeft,@ThreeDayLeft,@SevenDayLeft,@ARPU,@ARPPU,@PayRate,@ChargeUserRate,@ChannelId,@Created,@Modified,@regNum)";
                        recordHelper.Execute(insertSql, CommandType.Text, newAdReportParam);
                    }
                }
            }
            Log.WriteLog("createAdReport end");
        }


        private static int GetTableCount(DataTable table,string filter)
        {
            var rows = table.Select(filter);
            if (rows == null || rows.Length > 0) return 0;
            var obj = rows[0]["count"];
            if (obj==null||obj==DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt32(obj);
        }

        public static void createNewBaiduAdReport()
        {
            Log.WriteLog("createNewBaiduAdReport start");
            //获取活跃用户SQL
            var getActUserNumSql = @"SELECT count(distinct(t2.GUID)) as count , LEFT(t1.chargeId,3) as ChannelId,t2.ADID 
                                    from CurrcryRecord as t1 join Users as t2 on t1.Account = t2.Account 
                                    where t1.time BETWEEN @yesterdaySecond  and @todaySecond and t1.type in(6,7,30,9,26) and t2.ADID <> '' 
                                    group by LEFT(t1.chargeId,3),t2.ADID";

            ////获得次日留存数量
            //var getOneDayLeftSql = "SELECT count(distinct(t2.GUID)) as count , LEFT(t1.chargeId,3) as ChannelId from LoginLog as t1 join Users as t2 on t1.Account = t2.Account where t1.LoginTime BETWEEN @Yestoday  and @Today  and t2.ADID <> '' and  t2.Regitime < @yesterdaySecond and t2.Regitime > " + (TimeUtil.getRecordTimeByYestorday() - 86400) + " group by LEFT(t1.chargeId,3)";

            ////获得3日留存数量
            //var getThreeDayLeftSql = "SELECT count(distinct(t2.GUID)) as count , LEFT(t1.chargeId,3) as ChannelId from LoginLog as t1 join Users as t2 on t1.Account = t2.Account where t1.LoginTime BETWEEN @Yestoday  and @Today  and t2.ADID <> '' and t2.Regitime < @threedaySecond and t2.Regitime > " + (TimeUtil.getRecordTimeByThreeDayLeft() - 86400) + " group by LEFT(t1.chargeId,3)";


            ////获得7日留存数量
            //var getSevenDayLeftSql = "SELECT count(distinct(t2.GUID)) as count , LEFT(t1.chargeId,3) as ChannelId from LoginLog as t1 join Users as t2 on t1.Account = t2.Account where t1.LoginTime BETWEEN @Yestoday  and @Today  and t2.ADID <> '' and t2.Regitime < @sevendaySecond and t2.Regitime > " + (TimeUtil.getRecordTimeBySevenDayLeft() - 86400) + " group by LEFT(t1.chargeId,3)";

            //登录用户数量统计
            var loginNumSql = @"SELECT count(DISTINCT(t1.Guid)) as count, lEFT(t1.chargeId,3)  as ChannelId,t2.ADID 
                                from LoginLog as t1 join Users as t2 on t1.Account = t2.Account 
                                where t1.LoginTime BETWEEN @Yestoday and @Today and t2.ADID <> '' 
                                group by lEFT(t1.chargeId,3),t2.ADID";

            //注册用户数量统计
            String regNumSql = @"SELECT count(DISTINCT(t1.Guid)) as count, lEFT(t1.chargeId,3)  as ChannelId,t2.ADID  
                                 from RegistRecord as t1 join Users as t2 on t1.Account = t2.Account 
                                 where Regtime BETWEEN @Yestoday and @Today and t2.ADID <> '' 
                                 group by lEFT(t1.chargeId,3),t2.ADID";



 


            //用户表参数
            var userParam = new SqlParameter[] {
                             new SqlParameter("@Yestoday",TimeUtil.getYestodayTimstamp()),
                             new SqlParameter("@Today",TimeUtil.getTodayTimstamp()),
                             new SqlParameter("@yesterdaySecond",TimeUtil.getRecordTimeByYestorday()),
                             new SqlParameter("@todaySecond",TimeUtil.getRecordTimeByToday()),
                             new SqlParameter("@threedaySecond",TimeUtil.getRecordTimeByThreeDayLeft()),
                             new SqlParameter("@sevendaySecond",TimeUtil.getRecordTimeBySevenDayLeft()),
                    };


            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);
 
            foreach (String dbInfo in UserDbinfo.getUserDbinfo())
            {
                var readHelper = DbManager.GetDbHelper(dbInfo);

                DataTable actUserNumTable = readHelper.QueryParamTable(getActUserNumSql, userParam);


                DataTable loginTable = readHelper.QueryParamTable(loginNumSql, userParam);
                DataTable regTable = readHelper.QueryParamTable(regNumSql, userParam);

                var yt = _MFDayLeaveReportHelper.MFDayLeaveReportAdid(dbInfo, DayLeave.Yesterday).Report();
                var _3t = _MFDayLeaveReportHelper.MFDayLeaveReportAdid(dbInfo, DayLeave._3Day).Report();
                var _7t = _MFDayLeaveReportHelper.MFDayLeaveReportAdid(dbInfo, DayLeave._7Day).Report();


                foreach (DataRow row in actUserNumTable.Rows)
                {
                    string channelId = row["ChannelId"].ToString();
                    string adid = row["ADID"].ToString();

                    if (null != channelId && channelId.Length != 0)
                    {
                        long loginNum = 0;
                        long actUserNum = 0;
                        long oneDayLeft = 0;
                        long threeDayLeft = 0;
                        long sevenDayLeft = 0;
                        long regNum = 0;

                        var filter = string.Format("ChannelId='{0}' and ADID='{1}'", channelId, adid);

                        loginNum = GetTableCount(loginTable, filter);
                        var c = row["count"];
                        actUserNum = (c == null || c == DBNull.Value) ? 0 : Convert.ToInt32(c);

                       
                        oneDayLeft = GetTableCount(yt,filter);
                        threeDayLeft = GetTableCount(_3t, filter);
                        sevenDayLeft = GetTableCount(_7t, filter);

                        regNum = GetTableCount(regTable, filter);


  

                        //定义参数
                        var newAdReportParam = new SqlParameter[] {
                             new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                             new SqlParameter("@LoginNum",loginNum),
                             new SqlParameter("@ActUserNum",actUserNum),
                             new SqlParameter("@OneDayLeft",oneDayLeft),
                             new SqlParameter("@ThreeDayLeft",threeDayLeft),
                             new SqlParameter("@SevenDayLeft",sevenDayLeft),
                             new SqlParameter("@ChannelId",channelId),
                             new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@Modified",TimeUtil.getDbDateByNow()),
                             new SqlParameter("@regNum",regNum),
                             new SqlParameter("@ADID",adid){SqlDbType= SqlDbType.NVarChar,Size=50}
                        };

                        var insertSql = "insert into NewBaiduAdReport(Day,LoginNum,ActUserNum,OneDayLeft,ThreeDayLeft,SevenDayLeft,ChannelId,Created,Modified,regNum,ADID) values (@Day,@LoginNum,@ActUserNum,@OneDayLeft,@ThreeDayLeft,@SevenDayLeft,@ChannelId,@Created,@Modified,@regNum,@ADID)";
                        recordHelper.Execute(insertSql, CommandType.Text, newAdReportParam);
                    }
                }
            }
            Log.WriteLog("createNewBaiduAdReport end");
        }


    }
}
