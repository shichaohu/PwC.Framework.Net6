namespace APV.CRM.Service.Api.Swagger.SwaggerModel
{
    /// <summary>
    /// 系统分组特性
    /// </summary>
    public class ApiGroupAttribute : Attribute
    {
        public ApiGroupAttribute(params ApiGroupNames[] name)
        {
            GroupName = name;
        }
        public ApiGroupNames[] GroupName { get; set; }
    }
}
