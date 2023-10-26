using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.XxlJob
{



    public class XxlJobParameter
    {
        public int jobId { get; set; }
        public string executorHandler { get; set; }
        public string executorParams { get; set; }
        public string executorBlockStrategy { get; set; }
        public int executorTimeout { get; set; }
        public int logId { get; set; }
        public long logDateTime { get; set; }
        public string glueType { get; set; }
        public string glueSource { get; set; }
        public long glueUpdatetime { get; set; }
        public string broadcastIndex { get; set; }
        public int broadcastTotal { get; set; }
    }
    public class XxlJobExecutorParams
    {
        public string Path { get; set; }
        public object Parameter { get; set; }
    }
}
