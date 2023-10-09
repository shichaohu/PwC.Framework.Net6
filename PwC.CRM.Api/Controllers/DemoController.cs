using PwC.CRM.Service.Core;
using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.Log.Serilogs.Attributes;
using Microsoft.AspNetCore.Mvc;
using PwC.CRM.Api.Swagger.SwaggerModel;
using PwC.CRM.Api.HttpClients.HttpClientHandlers;
using PwC.CRM.Share.BaseModel;

namespace PwC.CRM.Api.Controllers
{
    /// <summary>
    /// Demo
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiGroup(ApiGroupNames.Common)]
    [SerilogIgnore]
    public class DemoController : BaseController<DemoController>
    {
        private readonly ILogger<DemoController> _logger;
        private readonly SRDMHttpClient _sRDMHttpClient;
        private readonly IDemoService _xxxService;

        public DemoController(ILogger<DemoController> logger, SRDMHttpClient sRDMHttpClient,
            IDemoService xxxService) : base(logger)
        {
            _logger = logger;
            _sRDMHttpClient = sRDMHttpClient;
            _xxxService = xxxService;
        }

        /// <summary>
        /// 获取Xxx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetXxxs([FromBody] XxxRequestDto parameter)
        {
            var response = await _sRDMHttpClient.PostAsync<CommonResponseDto>("getxxx", new
            {
                param1 = "",
                param2 = ""
            });
            _logger.LogInformation("日志内容");
            var res = await _xxxService.GetBusinessunit(parameter);
            return Ok(res);
        }

    }

}