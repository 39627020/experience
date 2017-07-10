using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Service.Util
{
    class TimeUtil
    {
        private static DateTime baseDate = new DateTime(2012, 10, 1, 0, 0, 0);
        public static long getRecordTimeByCurrent() {
            return (DateTime.Now - baseDate).Seconds;
        }

        public static long getRecordTime(DateTime date)
        {
            if (null == date) {
                return 0;
            } else
            {
                return (date - baseDate).Seconds;
            }
        }

        /**获取当天凌晨0点与2012年的差值，单位是秒*/
        public static long getRecordTimeByToday() {
            TimeSpan time = DateTime.Now.Date - baseDate.Date;
            return (long)time.TotalSeconds;
        }

        /**获取昨天凌晨0点与2012年的差值，单位是秒*/
        public static long getRecordTimeByYestorday() {
            return (long)(DateTime.Now.Date.AddDays(-1) - baseDate.Date).TotalSeconds;
        }

        /**获取3日留存率下，凌晨0点与2012年的差值，单位是秒*/
        public static long getRecordTimeByThreeDayLeft()
        {
            return (long)(DateTime.Now.Date.AddDays(-2) - baseDate.Date).TotalSeconds;
        }

        /**获得本周一凌晨0点与2012年的差值，单位是秒*/
        public static long getRecordTimeBythisWeek() {
            DateTime dt = DateTime.Now;
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
            return (long) (startWeek - baseDate.Date).TotalSeconds;
        }


        /**获得本周一凌晨是第几周**/
        public static int getThisWeekOfYear()
        {
            DateTime dt = DateTime.Now;
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }


        /**获得上周一凌晨0点与2012年的差值，单位是秒*/
        public static long getRecordTimeBylastWeek()
        {
            DateTime dt = DateTime.Now;
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))).AddDays(-7);
            return (long)(startWeek - baseDate.Date).TotalSeconds;
        }

        /**获取7日留存率下，凌晨0点与2012年的差值，单位是秒*/
        public static long getRecordTimeBySevenDayLeft()
        {
            return (long)(DateTime.Now.Date.AddDays(-6) - baseDate.Date).TotalSeconds;
        }

        /**获取昨天凌晨0点与2012年的差值，单位是天*/
        public static int getRecordDayByToday() {
            return (DateTime.Now.Date.AddDays(-1) - baseDate).Days;
        }

        /**获取插入数据库当前的时间*/
        public static String getDbDateByNow() {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /**获取昨天的timeStamp*/
        public static String getYestodayTimstamp() {
            return DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /**获取今天的timeStamp*/
        public static String getTodayTimstamp()
        {
            return DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
