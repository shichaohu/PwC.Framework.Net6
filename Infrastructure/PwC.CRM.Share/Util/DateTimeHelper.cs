using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.Util
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 本时区日期时间转Utc时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>Utc时间戳(精确到秒)</returns>
        public static long DateTimeToTimestamp(DateTime datetime)
        {
            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime timeUTC = datetime.ToUniversalTime();
            TimeSpan ts = (timeUTC - dd);
            return (long)ts.TotalSeconds;//精确到秒
        }

        /// <summary>
        /// Utc时间戳转本时区时间
        /// </summary>
        /// <param name="timeStamp">Utc时间戳(精确到秒)</param>
        /// <returns>本地时间</returns>
        public static DateTime TimestampToDateTime(long timeStamp)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var utcTtime = startTime.AddSeconds(timeStamp);

            var time = TimeZoneInfo.ConvertTimeFromUtc(utcTtime, TimeZoneInfo.Local);
            return time;
        }
    }
}
