using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APV.CRM.Service.Service.Dto.Request
{
    public class LogRequestDto
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public string HttpHost { get; set; }
        /// <summary>
        /// http path
        /// </summary>
        public string HttpPath { get; set; }
        /// <summary>
        /// http 请求id
        /// </summary>
        public string HttpRequestId { get; set; }
        /// <summary>
        /// 日志触发写入的上下文
        /// </summary>
        public string SourceContext { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogEventLevel? Level { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 日志写入的开始时间
        /// </summary>
        public DateTime? TimeStart { get; set; }
        /// <summary>
        /// 日志写入的结束时间
        /// </summary>
        public DateTime? TimeEnd { get; set; }
        /// <summary>
        /// 查询数量
        /// </summary>
        public int Limit { get; set; }
    }
}
