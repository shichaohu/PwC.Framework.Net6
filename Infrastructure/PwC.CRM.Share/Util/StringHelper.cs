﻿using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PwC.CRM.Share.Util
{
    public class StringHelper
    {
        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public bool IsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                   !objTwoDotPattern.IsMatch(strNumber) &&
                   !objTwoMinusPattern.IsMatch(strNumber) &&
                   objNumberPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// 是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        /// <summary>
        /// 是否为int类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        /// <summary>
        /// 是否为无符号数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }
        /// <summary>
        /// 数组转换成字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ArrayToString(string[] values)
        {
            if (null == values) return string.Empty;

            StringBuilder buffer = new StringBuilder(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                buffer.Append(values[i]).Append(",");
            }
            if (buffer.Length > 0)
            {
                return buffer.ToString().Substring(0, buffer.Length - 1);
            }
            return string.Empty;
        }
        /// <summary>
        /// object转string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ObjectToString(object obj, string defaultValue = "")
        {
            string result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                string name = obj.GetType().Name;
                switch (name)
                {
                    case "Int32":
                    case "Float":
                    case "Double":
                    case "Decimal":
                        result = obj.ToString();
                        return result;
                    case "DateTime":
                        result = (obj is DateTime ? (DateTime)obj : default).ToString("yyyy-mm-dd HH:MI:ss");
                        return result;
                    case "String":
                        result = string.Concat(obj).Length > 0 ? obj.ToString() : defaultValue;
                        return result;
                }
                result = obj as string;
            }
            return result;
        }

        /// <summary>
        /// 转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(string str)
        {
            return ToInt32(str, 0);
        }

        /// <summary>
        /// 转int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(string str, int defaultValue)
        {
            var value = 0;
            return int.TryParse(str, out value) ? value : defaultValue;
        }
    }
}
