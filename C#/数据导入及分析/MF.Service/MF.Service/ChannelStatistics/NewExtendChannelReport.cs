using System;

namespace MF.Service.ChannelStatistics
{
    [Serializable]
    public class NewExtendChannelReport
    {
        /// <summary>
        /// Day
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Channel
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// ChannelNum
        /// </summary>
        public string ChannelNum { get; set; }

        /// <summary>
        /// PCLoad
        /// </summary>
        public long PCLoad { get; set; }

        /// <summary>
        /// AndroidLoad
        /// </summary>
        public long AndroidLoad { get; set; }

        /// <summary>
        /// iOSLoad
        /// </summary>
        public long iOSLoad { get; set; }

        /// <summary>
        /// LoadTimeAvg
        /// </summary>
        public long LoadTimeAvg { get; set; }

        /// <summary>
        /// Stay
        /// </summary>
        public long Stay { get; set; }

        /// <summary>
        /// PCDown
        /// </summary>
        public long PCDown { get; set; }

        /// <summary>
        /// AndroidDown
        /// </summary>
        public long AndroidDown { get; set; }

        /// <summary>
        /// iOSDown
        /// </summary>
        public long iOSDown { get; set; }

        /// <summary>
        /// PCFirstActive
        /// </summary>
        public long PCFirstActive { get; set; }

        /// <summary>
        /// AndroidFirstActive
        /// </summary>
        public long AndroidFirstActive { get; set; }

        /// <summary>
        /// iOSFirstActive
        /// </summary>
        public long iOSFirstActive { get; set; }

        /// <summary>
        /// SecondDown
        /// </summary>
        public long SecondDown { get; set; }

        /// <summary>
        /// SecondDownTimeAvg
        /// </summary>
        public long SecondDownTimeAvg { get; set; }


        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 注册次数
        /// </summary>
        public long Register { get; set; }

    }
}
