using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
//using Log4NetAdapter;

namespace MF.Service
{
    public class LoadUser
    {
        public static void load()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("172.16.0.14:7020");
            IDatabase db = redis.GetDatabase();

            try
            {
                var sql = "select id,account,nickname,guid from users where isBot=0";
                var helperUser = DbManager.GetDbHelper("user0");
                DataTable dt = helperUser.QueryTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    //db.StringSet("X_ID_" + dr["id"]);
                    //db.StringSet("Y_ACC_" + dr["account"]);
                    //db.StringSet("Z_NICK_" + dr["nickname"]);
                    //db.StringSet("ACCOUNT_USERS_CACHE_" + dr["account"]);
                    //db.StringSet("ACCOUNT_USERS_CHANNEL_CACHE_" + dr["account"]);
                }
            }
            catch (Exception e)
            {
                Log.WriteError(e.ToString());
            }
        }
    }
}
