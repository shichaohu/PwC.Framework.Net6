using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static log4net.Appender.RollingFileAppender;

namespace PwC.CRM.Share.Extensions
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// TimeSpan WithMilliseconds
        /// </summary>
        /// <returns></returns>
        public static long ToTimeSpanWithMilliseconds(this DateTime timeValue)
        {

            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime timeUTC = timeValue.ToUniversalTime();
            TimeSpan ts = (timeUTC - dd);
            return (long)ts.TotalMilliseconds;//精确到毫秒
        }
    }
}
