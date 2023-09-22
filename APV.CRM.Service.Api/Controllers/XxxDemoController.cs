using APV.CRM.Service.Service.Core;
using APV.CRM.Service.Service.Dto.Request;
using PwC.Crm.Share.Log.Serilogs.Attributes;
using Microsoft.AspNetCore.Mvc;
using APV.CRM.Service.Api.Swagger.SwaggerModel;

namespace APV.CRM.Service.Api.Controllers
{
    /// <summary>
    /// XxxDemo
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiGroup(ApiGroupNames.Common)]
    [SerilogIgnore]
    public class XxxDemoController : BaseController<XxxDemoController>
    {
        private readonly ILogger<XxxDemoController> _logger;
        private readonly IXxxDemoService _xxxService;

        public XxxDemoController(ILogger<XxxDemoController> logger,
            IXxxDemoService xxxService) : base(logger)
        {
            _logger = logger;
            _xxxService = xxxService;
        }

        /// <summary>
        /// 获取Xxx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetXxxs([FromBody] XxxRequestDto parameter)
        {
            _logger.LogInformation("日志内容");
            var res = await _xxxService.GetBusinessunit(parameter);
            return Ok(res);
        }

    }

}