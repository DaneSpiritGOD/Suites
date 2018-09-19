using Suites.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TimeStamp
    {
        public static long Now
        {
            get
            {
                NativeMethods.QueryPerformanceFrequency(out long freq);
                NativeMethods.QueryPerformanceCounter(out long count);
                return count * 1000 / freq;
            }
        }

        /// <summary>
        /// 支持24小时内的时间戳
        /// </summary>
        /// <returns></returns>
        public static int NowInDay()
        {
            return (int)Now;
        }

        public static string Get(TimeType type, string splitter = "")
        {
            return DateTime.Now.Get(type, splitter);
        }

        public static string Get(this DateTime time, TimeType type, string splitter = "")
        {
            switch (type)
            {
                case TimeType.ByYear:
                    return string.Format("{0:yy}", time);
                case TimeType.ByMonth:
                    return time.ToString(string.Format("yy{0}MM", splitter));
                case TimeType.ByDay:
                    return time.ToString(string.Format("yy{0}MM{1}dd", splitter, splitter));
                case TimeType.ByHour:
                    return time.ToString(string.Format("yy{0}MM{1}dd{2}HH", splitter, splitter, splitter));
                case TimeType.ByMinute:
                    return time.ToString(string.Format("yy{0}MM{1}dd{2}HH{3}mm", splitter, splitter, splitter, splitter));
                case TimeType.BySecond:
                    return time.ToString(string.Format("yy{0}MM{1}dd{2}HH{3}mm{4}ss", splitter, splitter, splitter, splitter, splitter));
                case TimeType.ByMillisecond:
                    return time.ToString(string.Format("yy{0}MM{1}dd{2}HH{3}mm{4}ss{5}fff", splitter, splitter, splitter, splitter, splitter, splitter));
                default:
                    return string.Format("{0:yyMMddHHmmss}", time);
            }
        }

        public static string DetailNow => Get(TimeType.ByMillisecond);
    }

    public enum TimeType
    {
        ByYear,
        ByMonth,
        ByDay,
        ByHour,
        ByMinute,
        BySecond,
        ByMillisecond
    }
}
