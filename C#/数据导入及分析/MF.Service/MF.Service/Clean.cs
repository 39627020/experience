using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace MF.Service
{
    public class Clean
    {
        public static void day_clean()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("172.16.1.14:7020");
            IDatabase db = redis.GetDatabase();
            string sql = "select id,account,nickname,guid from users where"
                         + " (Identity='' and moble='' and phonekey='')"
                         + " and ((exp = 0 and LastLogin < datediff(s, '2012-10-01', dateadd(d, -7, getdate())))"
                         + " or(Currency < 2000 and LastLogin < datediff(s, '2012-10-01', dateadd(d, -7, getdate())))"
                         + " or(guest = 1 and LastLogin < datediff(s, '2012-10-01', dateadd(d, -30, getdate()))))";
            var helperMf = DbManager.GetDbHelper("mf");
            var args = new SqlParameter[] {
                new SqlParameter("@account","jiangzd2020")
            };
            int res = helperMf.Execute("mf_P_Clean", args);
            Console.WriteLine(res);
            var helperRecord = DbManager.GetDbHelper("record");
            for (int i = 0; i < 1; i++)
            {
                var helperUser = DbManager.GetDbHelper("user" + i);
                DataTable dt = helperUser.QueryTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    string sqlCharge = "select top 1 id from chargerecord where account='" + dr["account"] + "'";
                    DataRow drCharge = helperUser.QueryRow(sqlCharge);
                    if (drCharge != null)
                    {
                        break;
                    }
                    db.KeyDelete("ACCOUNT_SECURITY_TOKNE_" + dr["account"]);
                    db.KeyDelete("USER_IUKEY_" + dr["account"]);
                    db.KeyDelete("X_ID_" + dr["id"]);
                    db.KeyDelete("Y_ACC_" + dr["account"]);
                    db.KeyDelete("Z_NICK_" + dr["nickname"]);
                    db.KeyDelete("ACCOUNT_USERS_CACHE_" + dr["account"]);
                    db.KeyDelete("ACCOUNT_USERS_CHANNEL_CACHE_" + dr["account"]);
                    db.KeyDelete("SUBACC_COUNT_" + dr["account"]);
                    db.KeyDelete("ACCOUNT_USERS_SUB_LIST_" + dr["account"]);
                    db.KeyDelete("USER_GUILD_INFO_" + dr["account"]);
                    db.KeyDelete("USERCUT_-_" + dr["account"]);
                    db.KeyDelete("USER_MESSEGE_" + dr["account"]);
                    db.KeyDelete("USER_CHARGE_MAX_MONEY_EVERYDAY_" + dr["account"]);
                    db.KeyDelete("USER_APPPLE_CHARGE_MAX_MONEY_EVERYDAY_" + dr["account"]);
                    db.KeyDelete("USER_APPPLE_CHARGE_MAX_MONEY_EVERYDAY_" + dr["account"]);
                    db.StringDecrement("GUID_USER_" + dr["guid"]);
                    db.KeyDelete("ACC_BOX_INUSE" + dr["account"]);
                    db.KeyDelete("USER_FIRST_RECHARGE_REWARD_Rule_" + dr["account"]);
                    db.KeyDelete("USER_FIRST_RECHARGE_REWARD_TAKE_FLAG_" + dr["account"]);
                    db.KeyDelete("USER_CHARGE_COUNT_" + dr["account"]);
                    db.KeyDelete("USER_CHARGE_APPLE_COUNT" + dr["account"]);
                    db.KeyDelete("ACCOUNT_CAMPAIN1_" + dr["account"]);
                    db.KeyDelete("ACCOUNT_CAMPAIN2_" + dr["account"]);

                    string sqlCurrency = "delete from users where account='" + dr["account"] + "'";
                    string sqlGuildUser = "delete from guilduser where account='" + dr["account"] + "'";
                    string sqlFriend = "delete from friend where id=" + dr["id"] + " or friend_id=" + dr["id"];
                    string sqlGroupmember = "delete from groupmember where group_id in (select id from group where founder=" + dr["id"] + ")";
                    string sqlGroup = "delete from group where founder=" + dr["id"];
                    string sqlUser = "delete from users where account='" + dr["account"] + "'";
                }
            }
        }
    }
}
