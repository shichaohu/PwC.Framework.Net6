using PwC.CRM.Api.Swagger.SwaggerModel;
using PwC.CRM.Service.Core;
using Microsoft.AspNetCore.Mvc;
using PwC.CRM.Share.BaseModel;
using PwC.CRM.Share.Log.Serilogs.Attributes;
using PwC.CRM.Share.Util;

namespace PwC.CRM.Api.Controllers
{
    /// <summary>
    /// 公共接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Common)]
    public class CommonController : BaseController<CommonController>
    {
        private readonly ILogger<CommonController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly ICommonService _commonService;

        public CommonController(ILogger<CommonController> logger, IWebHostEnvironment environment,
            ICommonService commonService) : base(logger)
        {
            _logger = logger;
            _environment = environment;
            _commonService = commonService;
        }

        /// <summary>
        /// 查询系统所有接口信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SerilogIgnore]
        public async Task<CommonResponseDto> GetAllApiInfo()
        {
            var result = new CommonResponseDto<List<ApiBaseInfo>>
            {
                Code = ResponseCodeEnum.Success
            };

            string appName = _environment.ApplicationName;
            string appXmlFilePath = $"";
#if DEBUG
            appXmlFilePath = $"/obj/Debug/net6.0";
#endif
            var apiList = AspNetWebApiHelper.LoadAllApiInfo(appName, appXmlFilePath);
            result.Data = apiList.OrderBy(x => x.ControllerSummary).ThenBy(x => x.ActionSummary).ToList();

            return result;
        }
    }
}
