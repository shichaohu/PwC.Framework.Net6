using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.Crm.Share.Log.Serilogs.Util
{
    public class SerilogsLogUtil
    {
        /// <summary>
        /// 获取分表后的日志表名
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="tableNamePrefix">日志表前缀(即名称相同部分)</param>
        /// <returns></returns>
        public static string GetPracticalTableName(DateTime dateTime, string tableNamePrefix)
        {
            //创建公历日历对象
            GregorianCalendar gregorianCalendar = new GregorianCalendar();

            //获取指定日期是周数
            // CalendarWeekRule:第一周开始于该年的第一天
            // DayOfWeek:每周第一天是星期几
            int weekOfYear = gregorianCalendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return $"{tableNamePrefix}_week_{weekOfYear}";
        }
    }
}
