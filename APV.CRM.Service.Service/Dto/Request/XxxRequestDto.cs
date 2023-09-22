namespace APV.CRM.Service.Service.Dto.Request
{
    /// <summary>
    /// Xxx实体
    /// </summary>
    public class XxxRequestDto
    {
        /// <summary>
        /// 单次查询返回行数
        /// </summary>
        public int Limit { get; set; } = 5000;
        /// <summary>
        /// 数据更新时间
        /// </summary>
        public string Modifiedon { get; set; }
    }
}
