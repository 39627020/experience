using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Service.Util
{
    class UserDbinfo
    {

        public static String MF_DB = "mf";
        public static String MF_RECORD_DB = "mf_record";
        public static String MF_MAIN = "mf_main";
        public static String MF_WW = "mf_ww";
        public static String USER0 = "user0";






        //获得UserDB的数字，目前可以使用user0,今后多渠道可以直接使用到user0 到user 6
        private static List<String> userDbInfo = null;
        public static List<String> getUserDbinfo() {

            if (null == userDbInfo) {
                userDbInfo = new List<String>();
                userDbInfo.Add("user0");
            }
            return userDbInfo;
        
        }

        //获得DB对应的渠道ID号
        private static Dictionary<String, int> channelDictionary = null;

        public static Dictionary<String, int> getChannelDictionary() {
            if (null == channelDictionary) {
                channelDictionary = new Dictionary<String, int>();
                channelDictionary.Add("user0", 0);
                channelDictionary.Add("user1", 0);
                channelDictionary.Add("user2", 0);
                channelDictionary.Add("user3", 0);
                channelDictionary.Add("user4", 0);
                channelDictionary.Add("user5", 0);
                channelDictionary.Add("user6", 0);
                channelDictionary.Add("user7", 0);
                channelDictionary.Add("user8", 0);
                channelDictionary.Add("user9", 0);
                channelDictionary.Add("user10", 0);
                channelDictionary.Add("user11", 0);
                channelDictionary.Add("user12", 0);
                channelDictionary.Add("user13", 0);
                channelDictionary.Add("user14", 0);
                channelDictionary.Add("user15", 0);
                channelDictionary.Add("user16", 0);
            }
            return channelDictionary;
        }


        //0:官方,10:支付宝_网站,11:支付宝_App;2:易宝，3:苹果，40:微信_Web,41:微信_App,。。。其他联营渠道需要时定义
        private static List<int> payChannelInfo = null;
        public static List<int> getPayChannelInfo() {
            if (null == payChannelInfo) {
                payChannelInfo = new List<int>();
                payChannelInfo.Add(0);
                payChannelInfo.Add(10);
                payChannelInfo.Add(11);
                payChannelInfo.Add(2);
                payChannelInfo.Add(3);
                payChannelInfo.Add(40);
                payChannelInfo.Add(41);

            }
            return payChannelInfo;
        }

        //ChannelId,目前有0,1,2,3,4,5,6 7个不同的channel,但是现在只是会使用1个channel就是0
        private static List<int> channelInfo = null;
        public static List<int> getChannelInfo() {
            if (null == channelInfo) {
                channelInfo = new List<int>();
                channelInfo.Add(0);
            }
            return channelInfo;
        }


        //转化为key,int value的形式
        public static Dictionary<String, long> ListToMap(DataTable table,String rowName,String KeyName)
        {
            Dictionary<String, long> result = new Dictionary<String, long>();
            foreach (DataRow row in table.Rows)
            {
                String channelId = row[KeyName].ToString();
                long num = 0;
                long.TryParse(row[rowName].ToString(), out num);
                if (null != channelId && channelId.Length != 0)
                {
                    result.Add(channelId, num);
                }
            }
            return result;
        }



        //转化为key,DataRow的形式
        public static Dictionary<String, DataRow> ListToRowMap(DataTable table,String KeyName)
        {
            Dictionary<String, DataRow> result = new Dictionary<String, DataRow>();
            foreach (DataRow row in table.Rows)
            {
                String channelId = row[KeyName].ToString();
                if (null != channelId && channelId.Length != 0)
                {
                    result.Add(channelId, row);
                }
            }
            return result;
        }
    }
}
