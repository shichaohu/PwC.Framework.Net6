namespace APV.CRM.Service.Api.Swagger.SwaggerModel
{
    /// <summary>
    /// 系统分组枚举值
    /// </summary>
    public enum ApiGroupNames
    {
        /// <summary>
        /// All 所有
        /// </summary>
        [GroupInfo(Title = "All 所有", Description = "All接口", Version = "")]
        All = 0,
        /// <summary>
        /// NoGroup 未分组
        /// </summary>
        [GroupInfo(Title = "NoGroup 未分组", Description = "尚未分组的接口", Version = "")]
        NoGroup,
        /// <summary>
        /// Developing 开发中
        /// </summary>
        [GroupInfo(Title = "Developing 开发中", Description = "开发中", Version = "")]
        Developing,
        /// <summary>
        /// 公共接口
        /// </summary>
        [GroupInfo(Title = "公共接口", Description = "公共接口", Version = "")]
        Common
    }
}
