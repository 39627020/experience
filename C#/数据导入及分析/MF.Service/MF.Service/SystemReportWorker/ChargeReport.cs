using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MF.Service.Util;
using System.Data;

namespace MF.Service.SystemReportWorker
{
    class ChargeReport
    {


        //转化为key,int value的形式
        private static Dictionary<String, int> ListToMap(DataTable table)
        {
            Dictionary<String, int> result = new Dictionary<String, int>();
            foreach (DataRow row in table.Rows)
            {
                String channelId = row["ChannelId"].ToString();
                int num = 0;
                int.TryParse(row["count"].ToString(), out num);
                if (null != channelId && channelId.Length != 0)
                {
                    result.Add(channelId, num);
                }
            }
            return result;
        }

        //转化为key,DataRow的形式
        private static Dictionary<String, DataRow> ListToRowMap(DataTable table)
        {
            Dictionary<String, DataRow> result = new Dictionary<String, DataRow>();
            foreach (DataRow row in table.Rows)
            {
                String channelId = row["ChannelId"].ToString();
                if (null != channelId && channelId.Length != 0)
                {
                    result.Add(channelId, row);
                }
            }
            return result;
        }

        public static Boolean CreateDailyChargeReport() {
            Log.WriteLog("CreateDailyChargeReport start");
            long todaySecond = TimeUtil.getRecordTimeByToday();
            long yesterdaySecond = TimeUtil.getRecordTimeByYestorday();

            //每⽇日充值订单⽣生成数量量（根据充值渠道区分） 根据充值记录表统计当天充值订单的数量量
            String getCreateOrderSql = "select count(1) as count , sum(sumbitmoney) as sumbitmoney, LEFT(chargeId,3) as ChannelId from  ChargeRecord with(nolock) where createDate BETWEEN @yesterdaySecond and @todaySecond and PayChannel = @PayChannel group by LEFT(chargeId,3)";

            //每⽇日充值订单完成数量量（根据充值渠道区分
            //根据充值记录表统计当天充值订单完成加钱的 数量量 每⽇日充值⾦金金额数量量（根据充值渠道区分）
            //根据充值记录表统计⽀支付成功的⾦金金额 每⽇日充值⽤用户数量量（根据充值渠道区分）
            String getFinishOrderSql = "select count(1) as count, sum(sumbitmoney) as sumbitmoney , count(distinct(account)) as acccountNum ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 and PayChannel = @PayChannel  group by LEFT(chargeId,3)";

            //根据充值记录表统计⽤用户数量量 每⽇日新增充值⽤用户数量量：（⾸首次充值的⽤用户）根据充值记录表的account计算当⽇日注册的玩家 数量量
            String getNewChargeUserAccSql = "select count(1) as count ,t1.ChannelId from  (select distinct(account) as accnow , LEFT(chargeId,2) as channelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 ) as t1 left join (select distinct(account) as asaccbefore from ChargeRecord with(nolock) where paydate < @yesterdaySecond and flag = 1 ) as t2 on  t1.accnow = t2.asaccbefore where t2.asaccbefore is null group by t1.channelId ";

            //每⽇日充值额度的分布：（根据充值价格和数量量做构成的饼状图）5元-
            //5 - 10元
            //10 - 20元
            //20 - 50元
            //50 - 100元
            //100元 +
            String getPayNum_5 = "select count(1) as count,LEFT(chargeId,3) as ChannelId from  ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond  and flag = 1 and PayChannel = @PayChannel and  SumbitMoney < 5 group by LEFT(chargeId,3)";
            String getPayNum_5_10 = "select count(1) as count  ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 and PayChannel = @PayChannel and SumbitMoney >= 5 and SumbitMoney < 10 group by LEFT(chargeId,3)";
            String getPayNum_10_20 = "select count(1) as count ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 and PayChannel = @PayChannel  and SumbitMoney >= 10 and SumbitMoney < 20 group by LEFT(chargeId,3)";
            String getPayNum_20_50 = "select count(1) as count ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 and PayChannel = @PayChannel   and SumbitMoney >= 20 and SumbitMoney < 50 group by LEFT(chargeId,3)";
            String getPayNum_50_100 = "select count(1) as count ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 and PayChannel = @PayChannel   and SumbitMoney >= 50 and SumbitMoney < 100 group by LEFT(chargeId,3)";
            String getPayNum_100 = "select count(1) as count ,LEFT(chargeId,3) as ChannelId from ChargeRecord with(nolock) where paydate BETWEEN @yesterdaySecond and @todaySecond and flag = 1 and PayChannel = @PayChannel   and SumbitMoney >= 100 group by LEFT(chargeId,3)";


            //插入数据库SQL
            String insertChargeRecord = "insert into newChargeReport(Day,SubmitNum,SubmitMoney,PayNum,PayMoney,SubmitUserNum,FirstSubmitUserNum,ChannelId,PayChannel,Created,PayNum_5,PayNum_5_10,PayNum_10_20,PayNum_20_50,PayNum_50_100,PayNum_100 ) values (@Day,@SubmitNum,@SubmitMoney,@PayNum,@PayMoney,@SubmitUserNum,@FirstSubmitUserNum,@ChannelId,@PayChannel,@Created,@PayNum_5,@PayNum_5_10,@PayNum_10_20,@PayNum_20_50,@PayNum_50_100,@PayNum_100)";

            //
            var readHelper = DbManager.GetDbHelper(UserDbinfo.MF_DB);
            var recordHelper = DbManager.GetDbHelper(UserDbinfo.MF_RECORD_DB);

            //计算当天统计结果
            foreach (int payChannelId in UserDbinfo.getPayChannelInfo())
            {
                var topCreateOrder = new SqlParameter[] {
                             new SqlParameter("@todaySecond",todaySecond),
                             new SqlParameter("@yesterdaySecond",yesterdaySecond),
                             new SqlParameter("@PayChannel",payChannelId)
                    };
                Log.WriteLog("topCreateOrderTable start");
                DataTable topCreateOrderTable = readHelper.QueryParamTable(getCreateOrderSql, topCreateOrder);
                Log.WriteLog("topCreateOrderTable end"); 
                Dictionary<String, DataRow> createOrder = ListToRowMap(topCreateOrderTable);
                Log.WriteLog("getFinishOrderSql start");
                Dictionary<String, DataRow> finishOrder = ListToRowMap(readHelper.QueryParamTable(getFinishOrderSql, topCreateOrder));
                Log.WriteLog("getFinishOrderSql end");
                Dictionary<String, int> newChargeUserAcc = ListToMap(readHelper.QueryParamTable(getNewChargeUserAccSql, topCreateOrder));
                Log.WriteLog("getNewChargeUserAccSql end");
                Dictionary<String, int> payNum_5 = ListToMap(readHelper.QueryParamTable(getPayNum_5, topCreateOrder));
                Log.WriteLog("getPayNum_5 end");
                Dictionary<String, int> payNum_5_10 = ListToMap(readHelper.QueryParamTable(getPayNum_5_10, topCreateOrder));
                Log.WriteLog("getPayNum_5_10 end");
                Dictionary<String, int> payNum_10_20 = ListToMap(readHelper.QueryParamTable(getPayNum_10_20, topCreateOrder));
                Log.WriteLog("getPayNum_10_20 end");
                Dictionary<String, int> payNum_20_50 = ListToMap(readHelper.QueryParamTable(getPayNum_20_50, topCreateOrder));
                Log.WriteLog("getPayNum_20_50 end");
                Dictionary<String, int> payNum_50_100 = ListToMap(readHelper.QueryParamTable(getPayNum_50_100, topCreateOrder));
                Log.WriteLog("getPayNum_50_100 end");
                Dictionary<String, int> payNum_100 = ListToMap(readHelper.QueryParamTable(getPayNum_100, topCreateOrder));
                Log.WriteLog("getPayNum_100 end");
                foreach (DataRow row in topCreateOrderTable.Rows) {
                    String channelId = row["ChannelId"].ToString();
                    if (null != channelId && channelId.Length != 0)
                    {
                        //计算主报表
                        int createOrderCount = 0;
                        int createSubmitMoney = 0;
                        int finishOrderCount = 0;
                        int finishOrderMoney = 0;
                        int finishUserCount = 0;
                        int firstFinishUserCount = 0;
                        //计算充值区间报表
                        int PayNum_5 = 0;
                        int PayNum_5_10 = 0;
                        int PayNum_10_20 = 0;
                        int PayNum_20_50 = 0;
                        int PayNum_50_100 = 0;
                        int PayNum_100 = 0;

                        DataRow createOrderRow = null;
                        createOrder.TryGetValue(channelId, out createOrderRow);
                        int.TryParse(createOrderRow["count"].ToString(), out createOrderCount);
                        int.TryParse(createOrderRow["sumbitmoney"].ToString(), out createSubmitMoney);

                        DataRow finishOrderRow = null;
                        finishOrder.TryGetValue(channelId, out finishOrderRow);
                        if(null != finishOrderRow) { 
                            int.TryParse(finishOrderRow["count"].ToString(), out finishOrderCount);
                            int.TryParse(finishOrderRow["sumbitmoney"].ToString(), out finishOrderMoney);
                            int.TryParse(finishOrderRow["acccountNum"].ToString(), out finishUserCount);
                        }
                        newChargeUserAcc.TryGetValue(channelId, out firstFinishUserCount);
                        payNum_5.TryGetValue(channelId, out PayNum_5);
                        payNum_5_10.TryGetValue(channelId, out PayNum_5_10);
                        payNum_10_20.TryGetValue(channelId, out PayNum_10_20);
                        payNum_20_50.TryGetValue(channelId, out PayNum_20_50);
                        payNum_50_100.TryGetValue(channelId, out PayNum_50_100);
                        payNum_100.TryGetValue(channelId, out PayNum_100);





                        int.TryParse(createOrderRow["count"].ToString(), out createOrderCount);
                        int.TryParse(createOrderRow["sumbitmoney"].ToString(), out createSubmitMoney);


                        //插入数据库
                        var ChannelReport = new SqlParameter[] {
                            new SqlParameter("@Day",TimeUtil.getRecordDayByToday()),
                            new SqlParameter("@SubmitNum",createOrderCount),
                            new SqlParameter("@SubmitMoney",createSubmitMoney),
                            new SqlParameter("@PayNum",finishOrderCount),
                            new SqlParameter("@PayMoney",finishOrderMoney),
                            new SqlParameter("@SubmitUserNum",finishUserCount),
                            new SqlParameter("@FirstSubmitUserNum",firstFinishUserCount),
                            new SqlParameter("@ChannelId",channelId),
                            new SqlParameter("@PayChannel",payChannelId),
                            new SqlParameter("@Created",TimeUtil.getDbDateByNow()),
                            new SqlParameter("@PayNum_5",PayNum_5),
                            new SqlParameter("@PayNum_5_10",PayNum_5_10),
                            new SqlParameter("@PayNum_10_20",PayNum_10_20),
                            new SqlParameter("@PayNum_20_50",PayNum_20_50),
                            new SqlParameter("@PayNum_50_100",PayNum_50_100),
                            new SqlParameter("@PayNum_100",PayNum_100)
                         };
                        Log.WriteLog("insertChargeRecord end" + ChannelReport +":: start");
                        recordHelper.Execute(insertChargeRecord, CommandType.Text, ChannelReport);
                        Log.WriteLog("insertChargeRecord end" + ChannelReport + ":: end");
                    }


                }

            }
            Log.WriteLog("CreateDailyChargeReport end");
            return true;
        }
       

    }
}
