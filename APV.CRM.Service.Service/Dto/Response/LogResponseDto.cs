using Serilog.Events;

namespace APV.CRM.Service.Service.Dto.Response
{
    public class LogResponseDto
    {
        public int Id { get; set; }
        //public Guid RequestId { get; set; }
        public string SourceContext { get; set; }
        public LogEventLevel Level { get; set; }
        public string Message { get; set; }
        public string Properties { get; set; }
        public string Exception { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
