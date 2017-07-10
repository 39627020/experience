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
    public class LoadAi
    {
        public static void load()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("172.16.0.14:7020");
            IDatabase db = redis.GetDatabase();

            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(@"d:\ai.txt"))
                {
                    string str;
                    while ((str = sr.ReadLine()) != null)
                    {
                        db.StringSet("Z_NICK_" + str,"1");
                        Console.WriteLine("Z_NICK_" + str);
                    }
                }
                Console.WriteLine("Over");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Log.WriteError(e.ToString());
            }
        }
    }
}
