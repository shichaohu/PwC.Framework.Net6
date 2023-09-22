using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.Util
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 本时区日期时间转时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>long=Int64</returns>
        public static long DateTimeToTimestamp(DateTime datetime)
        {
            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime timeUTC = datetime.ToUniversalTime();
            TimeSpan ts = (timeUTC - dd);
            return (Int64)ts.TotalMilliseconds;//精确到毫秒
        }

        /// <summary>
        /// 时间戳转本时区日期时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(string timeStamp)
        {
            DateTime dd = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Local);
            long longTimeStamp = long.Parse(timeStamp + "0000");
            TimeSpan ts = new TimeSpan(longTimeStamp);
            return dd.Add(ts);
        }
    }
}
