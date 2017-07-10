using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MF.Service.Util;
using System.Collections.Generic;

namespace MF.Service.SystemReportWorker
{
    class OldDataCopy
    {
        public static void CopyData()
        {
            #region
            //            //定义数据库
            //            var mfHelper = DbManager.GetDbHelper(UserDbinfo.MF_DB);
            //            var mfWWHelper = DbManager.GetDbHelper(UserDbinfo.MF_WW);
            //            var userHelper =  DbManager.GetDbHelper("user0");

            //            // 关闭自增插入
            //            String closeIdentitySql = "SET IDENTITY_INSERT dbo.users ON";
            //            //处理USER表
            //            String getAllUserSql = "select * from Users order by id asc";
            //            String insertUsersSql = "insert into Users( [ID], Account, [Password], Nickname, Sex, Flag,Lock,Guest,Lv,Exp,ChargeTotal,Currency,Gold,Silver,Bean,RoomCard,Icon,OpenID,OpenPlatform,Name,[Identity],Mobile,Email,Intro,RegistDevice,ADID,RegistArea,PushCode,Relief,Regitime,RegistIp,LastIp,LastLogin,PhoneKey,OnlineTime,LoginCount,LoginDevice,Mission,DeviceCode,GUID,MonthCardOverdue,MonthCardReceive,TodayActive,Master,IsBot, ChannelId,LastGUID, ChargeId, UserType) values(@ID,@Account,@Password,@Nickname,@Sex,@Flag,@Lock,@Guest,@Lv,@Exp,@ChargeTotal,@Currency,@Gold,@Silver,@Bean,@RoomCard,@Icon,@OpenID,@OpenPlatform,@Name,@Identity,@Mobile,@Email,@Intro,@RegistDevice,@ADID,@RegistArea,@PushCode,@Relief,@Regitime,@RegistIp,@LastIp,@LastLogin,@PhoneKey,@OnlineTime,@LoginCount,@LoginDevice,@Mission,@DeviceCode,@GUID,@MonthCardOverdue,@MonthCardReceive,@TodayActive,@Master,@IsBot,@ChannelId,@LastGUID,@ChargeId,@UserType)";

            //            //获取用户数据
            //            DataTable userTable = mfWWHelper.QueryTable(getAllUserSql);
            //            //关闭插入限制
            //            //mfHelper.Execute(closeIdentitySql);
            //            //userHelper.Execute(closeIdentitySql);
            //            //插入数据
            //            foreach (DataRow row in userTable.Rows) {

            //                //定义参数
            //                var userInsertParam = new SqlParameter[] {
            //                    new SqlParameter("@ID",    row["ID"].ToString()),
            //new SqlParameter("@Account",          row["Account"].ToString()),
            //new SqlParameter("@Password",         row["Password"].ToString()),
            //new SqlParameter("@Nickname",         row["Nickname"].ToString()),
            //new SqlParameter("@Sex",              row["Sex"].ToString()),
            //new SqlParameter("@Flag",             row["Flag"].ToString()),
            //new SqlParameter("@Lock",             row["Lock"].ToString()),
            //new SqlParameter("@Guest",            row["Guest"].ToString()),
            //new SqlParameter("@Lv",               row["Lv"].ToString()),
            //new SqlParameter("@Exp",              row["Exp"].ToString()),
            //new SqlParameter("@ChargeTotal",      row["ChargeTotal"].ToString()),
            //new SqlParameter("@Currency",         row["Currency"].ToString()),
            //new SqlParameter("@Gold",             row["Gold"].ToString()),
            //new SqlParameter("@Silver",           row["Silver"].ToString()),
            //new SqlParameter("@Bean",              row["Bean"].ToString()),
            //new SqlParameter("@RoomCard",         row["RoomCard"].ToString()),
            //new SqlParameter("@Icon",             row["Icon"].ToString()),
            //new SqlParameter("@OpenID",           row["OpenID"].ToString()),
            //new SqlParameter("@OpenPlatform",     row["OpenPlatform"].ToString()),
            //new SqlParameter("@Name",             row["Name"].ToString()),
            //new SqlParameter("@Identity",         row["Identity"].ToString()),
            //new SqlParameter("@Mobile",           row["Mobile"].ToString()),
            //new SqlParameter("@Email",            row["Email"].ToString()),
            //new SqlParameter("@Intro",             row["Intro"].ToString()),
            //new SqlParameter("@RegistDevice",     row["RegistDevice"].ToString()),
            //new SqlParameter("@ADID",             row["ADID"].ToString()),
            //new SqlParameter("@RegistArea",       row["RegistArea"].ToString()),
            //new SqlParameter("@PushCode",         row["PushCode"].ToString()),
            //new SqlParameter("@Relief",           row["Relief"].ToString()),
            //new SqlParameter("@Regitime",         row["Regitime"].ToString()),
            //new SqlParameter("@RegistIp",         row["RegistIp"].ToString()),
            //new SqlParameter("@LastIp",           row["LastIp"].ToString()),
            //new SqlParameter("@LastLogin",        row["LastLogin"].ToString()),
            //new SqlParameter("@PhoneKey",         row["PhoneKey"].ToString()),
            //new SqlParameter("@OnlineTime",       row["OnlineTime"].ToString()),
            //new SqlParameter("@LoginCount",       row["LoginCount"].ToString()),
            //new SqlParameter("@LoginDevice",      row["LoginDevice"].ToString()),
            //new SqlParameter("@Mission",          row["Mission"].ToString()),
            //new SqlParameter("@DeviceCode",       row["DeviceCode"].ToString()),
            //new SqlParameter("@GUID",             row["GUID"].ToString()),
            //new SqlParameter("@MonthCardOverdue", row["MonthCardOverdue"].ToString()),
            //new SqlParameter("@MonthCardReceive", row["MonthCardReceive"].ToString()),
            //new SqlParameter("@TodayActive",      row["TodayActive"].ToString()),
            //new SqlParameter("@Master",           row["Master"].ToString()),
            //new SqlParameter("@IsBot",            row["IsBot"].ToString()),
            //new SqlParameter("@ChannelId",        "10A"),
            //new SqlParameter("@LastGUID",         row["GUID"].ToString()),
            //new SqlParameter("@ChargeId",         "10A"+row["ID"].ToString()),
            //new SqlParameter("@UserType",          0)
            //                        };
            //                userHelper.Execute(insertUsersSql, CommandType.Text, userInsertParam);
            //                mfHelper.Execute(insertUsersSql, CommandType.Text, userInsertParam);
            //}


            #endregion
            Log.WriteLog("CopyData start");
            var arry = new IDataExportor[] {
              new UserExportor(UserDbinfo.MF_WW, UserDbinfo.USER0),
              new UserExportor(UserDbinfo.MF_WW, UserDbinfo.MF_DB),
              new ChargeRecordExportor(UserDbinfo.MF_WW, UserDbinfo.MF_DB),
              new GuildExportor(UserDbinfo.MF_WW, UserDbinfo.MF_DB),
              new GuildUserExportor(UserDbinfo.MF_WW, UserDbinfo.MF_DB),
              new GuildApplyRecordExportor(UserDbinfo.MF_WW, UserDbinfo.MF_DB),
              new StrongboxExportor(UserDbinfo.MF_WW, UserDbinfo.MF_DB),
             };

            foreach (var item in arry)
            {
                Log.WriteLog("item start" + item);
                try
                {
                    item.Export();

                }
                catch (Exception ex)
                {
                    Log.WriteError(ex.Message, ex.StackTrace);
                    throw ex;
                }
                Log.WriteLog(" item end" + item);
            }
            Log.WriteLog("CopyData end");

            Console.WriteLine("LOD_AI STAET");
            LoadAi.load();
            Console.WriteLine("LOD_AI END");

        }
    }

    internal interface IDataExportor
    {
        void Export();
    }

    internal class UserExportor : IDataExportor
    {
        private string _source;
        private string _target;
        public UserExportor(string source, string target)
        {
            this._source = source;
            this._target = target;
        }

        private SqlDataReader GetSouraceReader(string db)
        {
            /**
              数据变更规则 
              1.原始数据的金卷和银票不在使用直接本身 *100变更为元宝,然后把金卷和银票清零
              2.金豆将原始值除以10最为新的数据
              3.机器人不导入新库  
              4.以ahvuj开头的的账号不导入新库
            **/
            var sql = @"SELECT ID,Account,Password,Nickname,Sex,Flag,Lock,Guest,Lv,Exp,ChargeTotal,Currency+(Gold+Silver)*100 AS Currency,0 AS Gold,0 AS Silver
                        ,(Bean/10) AS Bean,RoomCard,Icon,OpenID
                        ,OpenPlatform,Name,[Identity],Mobile,Email,Intro,RegistDevice,ADID,RegistArea,PushCode,Relief,Regitime,RegistIp,LastIp,LastLogin
                        ,PhoneKey,OnlineTime,LoginCount,LoginDevice,Mission,DeviceCode,GUID,MonthCardOverdue,MonthCardReceive,TodayActive,Master,IsBot
                        ,'10A' AS ChannelId,'10A'+RIGHT('000000000'+CAST(ID as VARCHAR),9) AS ChargeId,0 AS UserType,'' AS LastGUID
                        FROM Users WITH(NOLOCK) WHERE IsBot=0 AND Account NOT LIKE 'ahvuj%' ORDER BY ID ASC";
            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.ExecuteReader(sql, CommandType.Text, null);
        }



        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {
            mapping.Add("ID", "ID");
            mapping.Add("Account", "Account");
            mapping.Add("Password", "Password");
            mapping.Add("Nickname", "Nickname");
            mapping.Add("Sex", "Sex");
            mapping.Add("Flag", "Flag");
            mapping.Add("Lock", "Lock");
            mapping.Add("Guest", "Guest");
            mapping.Add("Lv", "Lv");
            mapping.Add("Exp", "Exp");
            mapping.Add("ChargeTotal", "ChargeTotal");
            mapping.Add("Currency", "Currency");
            mapping.Add("Gold", "Gold");
            mapping.Add("Silver", "Silver");
            mapping.Add("Bean", "Bean");
            mapping.Add("RoomCard", "RoomCard");
            mapping.Add("Icon", "Icon");
            mapping.Add("OpenID", "OpenID");
            mapping.Add("OpenPlatform", "OpenPlatform");
            mapping.Add("Name", "Name");
            mapping.Add("Identity", "Identity");
            mapping.Add("Mobile", "Mobile");
            mapping.Add("Email", "Email");
            mapping.Add("Intro", "Intro");
            mapping.Add("RegistDevice", "RegistDevice");
            mapping.Add("ADID", "ADID");
            mapping.Add("RegistArea", "RegistArea");
            mapping.Add("PushCode", "PushCode");
            mapping.Add("Relief", "Relief");
            mapping.Add("Regitime", "Regitime");
            mapping.Add("RegistIp", "RegistIp");
            mapping.Add("LastIp", "LastIp");
            mapping.Add("LastLogin", "LastLogin");
            mapping.Add("PhoneKey", "PhoneKey");
            mapping.Add("OnlineTime", "OnlineTime");
            mapping.Add("LoginCount", "LoginCount");
            mapping.Add("LoginDevice", "LoginDevice");
            mapping.Add("Mission", "Mission");
            mapping.Add("DeviceCode", "DeviceCode");
            mapping.Add("GUID", "GUID");
            mapping.Add("MonthCardOverdue", "MonthCardOverdue");
            mapping.Add("MonthCardReceive", "MonthCardReceive");
            mapping.Add("TodayActive", "TodayActive");
            mapping.Add("Master", "Master");
            mapping.Add("IsBot", "IsBot");

            mapping.Add("ChannelId", "ChannelId");
            mapping.Add("ChargeId", "ChargeId");
            mapping.Add("UserType", "UserType");
            mapping.Add("LastGUID", "LastGUID");

        }

        public void Export()
        {
            Console.WriteLine("Exec UserExportor Export");
            DbManager.GetDbHelper(this._target).Execute("truncate table Users");

            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
            using (var reader = GetSouraceReader(this._source))
            {
                var bcp = DbManager.GetDbHelper(this._target).GetBCP();
                this.ColumnMapping(bcp.ColumnMappings);
                bcp.DestinationTableName = "Users";
                bcp.WriteToServer(reader);
            }
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),this._source, this._target);
        }
    }

    internal class ChargeRecordExportor : IDataExportor
    {
        private string _source;
        private string _target;
        public ChargeRecordExportor(string source,string target)
        {
            this._source = source;
            this._target = target;
        }

        private SqlDataReader GetSouraceReader(string db)
        {
            var sql = @"SELECT ID,OrderNo,Account,CreateDate,CreateIp,SumbitMoney,Flag,PayDate,PayMoney,PlatformTransId,PayChannel,PayMode,Device,Channel,MissionType,Gold
                        ,CreateDate AS EndTime,'(SYSTEM)于'+CONVERT(VARCHAR,GETDATE(),20)+'导入' AS Remark,'10A'+RIGHT('000000000'+CAST(ID as VARCHAR),9) AS ChargeId
                        FROM ChargeRecord WITH(NOLOCK) ORDER BY ID ASC";

            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.ExecuteReader(sql, CommandType.Text, null);
        }

        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {
            mapping.Add("ID", "ID");
            mapping.Add("OrderNo", "OrderNo");
            mapping.Add("Account", "Account");
            mapping.Add("CreateDate", "CreateDate");
            mapping.Add("CreateIp", "CreateIp");
            mapping.Add("SumbitMoney", "SumbitMoney");
            mapping.Add("Flag", "Flag");
            mapping.Add("PayDate", "PayDate");
            mapping.Add("PayMoney", "PayMoney");
            mapping.Add("PlatformTransId", "PlatformTransId");
            mapping.Add("PayChannel", "PayChannel");
            mapping.Add("PayMode", "PayMode");
            mapping.Add("Device", "Device");
            mapping.Add("Channel", "Channel");
            mapping.Add("MissionType", "MissionType");
            mapping.Add("Gold", "Gold");

            mapping.Add("Endtime", "Endtime");
            mapping.Add("Remark", "Remark");
            mapping.Add("ChargeId", "ChargeId");
        }

        public void Export()
        {
            Console.WriteLine("Exec ChargeRecordExportor Export");
            DbManager.GetDbHelper(this._target).Execute("truncate table ChargeRecord");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
            using (var reader = GetSouraceReader(this._source))
            {
                var bcp = DbManager.GetDbHelper(this._target).GetBCP();
                this.ColumnMapping(bcp.ColumnMappings);
                bcp.DestinationTableName = "ChargeRecord";
                bcp.WriteToServer(reader);
            }
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
        }
    }


    internal class GuildExportor : IDataExportor
    {
        private string _source;
        private string _target;
        public GuildExportor(string source, string target)
        {
            this._source = source;
            this._target = target;
        }

        private SqlDataReader GetSouraceReader(string db)
        {
            var sql = @"SELECT ID,CreateTime,IsActive,Exp,Master,Name,Introduction,Notice,HelloMsgID,UserCount,ActiveUserNumOfLastWeek,ActiveUserNumOfCurrentWeek,Weixin,QQ,Phone,IsSendMail,MaxUser,ActiveUserOfYestoday,ActiveUserOfToday,Score,IsoWeek,History
                        FROM Guild WITH(NOLOCK) ORDER BY ID ASC";

            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.ExecuteReader(sql, CommandType.Text, null);
        }

        private DataTable GetSourceDataTable(string db)
        {
            var sql = @"SELECT ID,CreateTime,IsActive,Exp,Master,Name,Introduction,Notice,HelloMsgID,UserCount,ActiveUserNumOfLastWeek,ActiveUserNumOfCurrentWeek,Weixin,QQ,Phone,IsSendMail,MaxUser,ActiveUserOfYestoday,ActiveUserOfToday,Score,IsoWeek,History
                        FROM Guild WITH(NOLOCK) ORDER BY ID ASC";

            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.QueryTable(sql);
        }

        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {

            mapping.Add("ID", "ID");
            mapping.Add("CreateTime", "CreateTime");
            mapping.Add("IsActive", "IsActive");
            mapping.Add("Exp", "Exp");
            mapping.Add("Master", "Master");
            mapping.Add("Name", "Name");
            mapping.Add("Introduction", "Introduction");
            mapping.Add("Notice", "Notice");
            mapping.Add("HelloMsgID", "HelloMsgID");
            mapping.Add("UserCount", "UserCount");
            mapping.Add("ActiveUserNumOfLastWeek", "ActiveUserNumOfLastWeek");
            mapping.Add("ActiveUserNumOfCurrentWeek", "ActiveUserNumOfCurrentWeek");
            mapping.Add("Weixin", "Weixin");
            mapping.Add("QQ", "QQ");
            mapping.Add("Phone", "Phone");
            mapping.Add("IsSendMail", "IsSendMail");
            mapping.Add("MaxUser", "MaxUser");
            mapping.Add("ActiveUserOfYestoday", "ActiveUserOfYestoday");
            mapping.Add("ActiveUserOfToday", "ActiveUserOfToday");
            mapping.Add("Score", "Score");
            mapping.Add("IsoWeek", "IsoWeek");
            mapping.Add("History", "History");

        }


        private DataTable GuildRead()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "guild.xlsx");
            var connstring = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
            using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(connstring))
            {
                connection.Open();
                var sql = "select * from [Sheet1$]";
                System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(sql, connection);
                adapter.Fill(dt);
            }

            var sb = new StringBuilder();
            sb.AppendLine("Dictionary<string, string> dic = new Dictionary<string, string>();");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine(string.Format("dic.Add(\"{0}\",\"{1}\");", row["ID"], row["Name"]));
            }
            var value =  sb.ToString();

            return dt;
        }


        private Dictionary<string, string> GuildDic()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("8", "天下英豪");
            dic.Add("19", "童童商号");
            dic.Add("23", "诚信天下");
            dic.Add("24", "米奇财富");
            dic.Add("25", "乐乐商会");
            dic.Add("27", "可儿网络");
            dic.Add("30", "天地英豪");
            dic.Add("32", "中国富豪");
            dic.Add("42", "相逢笑网络");
            dic.Add("44", "妞妞萌萌哒");
            dic.Add("51", "乐悠网络");
            dic.Add("53", "富豪联盟");
            dic.Add("55", "￥仙山网络￥");
            dic.Add("61", "$妞妞网络￥");
            dic.Add("62", "￥丫丫超市＄");
            dic.Add("63", "至尊宝庄");
            dic.Add("64", "仙儿网络");
            dic.Add("65", "￥仙山财富￥");
            dic.Add("66", "保利诚信");
            dic.Add("67", "财神贺喜");
            dic.Add("71", "￥仙山游戏中心＄");
            dic.Add("72", "＄仙山超市￥");
            dic.Add("73", "纵横网路");
            dic.Add("74", "￥诚信快捷￥");
            dic.Add("75", "￥乐儿中心￥");
            dic.Add("79", "￥纵横财富￥");
            dic.Add("81", "￥仙山宝库￥");
            dic.Add("83", "￥琪琪便利店￥");
            dic.Add("86", "钻石国度");
            dic.Add("91", "￥￥纵横财富￥￥");
            dic.Add("100", "游戏猎人");
            dic.Add("104", "＄大富翁联盟＄");
            dic.Add("109", "大美妞");
            dic.Add("110", "安妮贝儿");
            dic.Add("111", "万发聚英庄");
            dic.Add("116", "嘟嘟游戏中心");
            return dic;
        }

        public void Export()
        {
            Console.WriteLine("Exec GuildExportor Export");
            DbManager.GetDbHelper(this._target).Execute("TRUNCATE TABLE Guild");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);

            var table = this.GetSourceDataTable(this._source);

            var dic = this.GuildDic();

            foreach (var kv in dic)
            {
                var filter = string.Format("ID={0}", kv.Key);
                var rows = table.Select(filter);
                if (rows != null && rows.Length > 0)
                {
                    rows[0]["Name"] = kv.Value;
                }
            }


            var bcp = DbManager.GetDbHelper(this._target).GetBCP();
            this.ColumnMapping(bcp.ColumnMappings);
            bcp.DestinationTableName = "Guild";
            bcp.WriteToServer(table);

            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);

        }


        /*
        public void Export()
        {
            Console.WriteLine("Exec GuildExportor Export");
            DbManager.GetDbHelper(this._target).Execute("TRUNCATE TABLE Guild");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
            using (var reader = GetSouraceReader(this._source))
            {
                var bcp = DbManager.GetDbHelper(this._target).GetBCP();
                this.ColumnMapping(bcp.ColumnMappings);
                bcp.DestinationTableName = "Guild";
                bcp.WriteToServer(reader);
            }
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
        }
        */
    }

    internal class GuildUserExportor : IDataExportor
    {
        private string _source;
        private string _target;
        public GuildUserExportor(string source, string target)
        {
            this._source = source;
            this._target = target;
        }

        private SqlDataReader GetSouraceReader(string db)
        {
            var sql = @"SELECT Account,JoinDate,LastOnLine,MissionList,MaxMoney,ActiveRec,LastWeekActiveDay,OldGuildIDList,GuildID,ID,Nickname,Icon,MissionState,IsoWeek,ActiveNum
FROM GuildUser WITH(NOLOCK) ORDER BY ID ASC";

            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.ExecuteReader(sql, CommandType.Text, null);
        }

        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {

            mapping.Add("Account", "Account");
            mapping.Add("JoinDate", "JoinDate");
            mapping.Add("LastOnLine", "LastOnLine");
            mapping.Add("MissionList", "MissionList");
            mapping.Add("MaxMoney", "MaxMoney");
            mapping.Add("ActiveRec", "ActiveRec");
            mapping.Add("LastWeekActiveDay", "LastWeekActiveDay");
            mapping.Add("OldGuildIDList", "OldGuildIDList");
            mapping.Add("GuildID", "GuildID");
            mapping.Add("ID", "ID");
            mapping.Add("Nickname", "Nickname");
            mapping.Add("Icon", "Icon");
            mapping.Add("MissionState", "MissionState");
            mapping.Add("IsoWeek", "IsoWeek");
            mapping.Add("ActiveNum", "ActiveNum");

        }

        public void Export()
        {
            Console.WriteLine("Exec GuildUserExportor Export");
            DbManager.GetDbHelper(this._target).Execute("TRUNCATE TABLE GuildUser");
            DbManager.GetDbHelper(this._target).Execute("SET IDENTITY_INSERT GuildUser ON");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
            using (var reader = GetSouraceReader(this._source))
            {
                var bcp = DbManager.GetDbHelper(this._target).GetBCP();
                this.ColumnMapping(bcp.ColumnMappings);
                bcp.DestinationTableName = "GuildUser";
                bcp.WriteToServer(reader);
            }
            DbManager.GetDbHelper(this._target).Execute("SET IDENTITY_INSERT GuildUser OFF");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
        }
    }



    internal class GuildApplyRecordExportor : IDataExportor
    {
        private string _source;
        private string _target;
        public GuildApplyRecordExportor(string source, string target)
        {
            this._source = source;
            this._target = target;
        }

        private SqlDataReader GetSouraceReader(string db)
        {
            var sql = @"SELECT ID,OrderNo,Account,Flag,CreateDate,Money,GuildID,GuildName,PayDate,IP,TransId,ApplyRefundTime,RefundTime,RefundUser,AlipayAccount,AlipayName,Memo
FROM GuildApplyRecord WITH(NOLOCK) ORDER BY ID ASC";

            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.ExecuteReader(sql, CommandType.Text, null);
        }

        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {

            mapping.Add("ID", "ID");
            mapping.Add("OrderNo", "OrderNo");
            mapping.Add("Account", "Account");
            mapping.Add("Flag", "Flag");
            mapping.Add("CreateDate", "CreateDate");
            mapping.Add("Money", "Money");
            mapping.Add("GuildID", "GuildID");
            mapping.Add("GuildName", "GuildName");
            mapping.Add("PayDate", "PayDate");
            mapping.Add("IP", "IP");
            mapping.Add("TransId", "TransId");
            mapping.Add("ApplyRefundTime", "ApplyRefundTime");
            mapping.Add("RefundTime", "RefundTime");
            mapping.Add("RefundUser", "RefundUser");
            mapping.Add("AlipayAccount", "AlipayAccount");
            mapping.Add("AlipayName", "AlipayName");
            mapping.Add("Memo", "Memo");

        }

        public void Export()
        {
            Console.WriteLine("Exec GuildApplyRecordExportor Export");
            DbManager.GetDbHelper(this._target).Execute("TRUNCATE TABLE GuildApplyRecord");
            DbManager.GetDbHelper(this._target).Execute("SET IDENTITY_INSERT GuildApplyRecord ON");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
            using (var reader = GetSouraceReader(this._source))
            {
                var bcp = DbManager.GetDbHelper(this._target).GetBCP();
                this.ColumnMapping(bcp.ColumnMappings);
                bcp.DestinationTableName = "GuildApplyRecord";
                bcp.WriteToServer(reader);
            }
            DbManager.GetDbHelper(this._target).Execute("SET IDENTITY_INSERT GuildApplyRecord OFF");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
        }
    }


    internal class StrongboxExportor : IDataExportor
    {
        private string _source;
        private string _target;
        public StrongboxExportor(string source, string target)
        {
            this._source = source;
            this._target = target;
        }

        private SqlDataReader GetSouraceReader(string db)
        {
            var sql = @"SELECT ID,Password,UserId,UserAccount,Currency,CreateDate,OperateTime,'10A'+RIGHT('000000000'+CAST(UserId as VARCHAR),9) AS ChargeId
FROM Strongbox WITH(NOLOCK) ORDER BY ID ASC";

            var dbhelper = DbManager.GetDbHelper(db);
            return dbhelper.ExecuteReader(sql, CommandType.Text, null);
        }

        private void ColumnMapping(SqlBulkCopyColumnMappingCollection mapping)
        {

            mapping.Add("ID", "ID");
            mapping.Add("Password", "Password");
            mapping.Add("UserId", "UserId");
            mapping.Add("UserAccount", "UserAccount");
            mapping.Add("Currency", "Currency");
            mapping.Add("CreateDate", "CreateDate");
            mapping.Add("OperateTime", "OperateTime");
            mapping.Add("ChargeId", "ChargeId");

        }

        public void Export()
        {
            Console.WriteLine("Exec StrongboxExportor Export");
            DbManager.GetDbHelper(this._target).Execute("TRUNCATE TABLE Strongbox");
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
            using (var reader = GetSouraceReader(this._source))
            {
                var bcp = DbManager.GetDbHelper(this._target).GetBCP();
                this.ColumnMapping(bcp.ColumnMappings);
                bcp.DestinationTableName = "Strongbox";
                bcp.WriteToServer(reader);
            }
            Console.WriteLine("{0} Source:{1},Target:{2} Exprot Complated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this._source, this._target);
        }
    }
}
