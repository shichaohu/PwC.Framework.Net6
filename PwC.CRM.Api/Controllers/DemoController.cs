using PwC.CRM.Service.Core;
using PwC.CRM.Service.Dto.Request;
using PwC.Crm.Share.Log.Serilogs.Attributes;
using Microsoft.AspNetCore.Mvc;
using PwC.CRM.Api.Swagger.SwaggerModel;

namespace PwC.CRM.Api.Controllers
{
    /// <summary>
    /// XxxDemo
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiGroup(ApiGroupNames.Common)]
    [SerilogIgnore]
    public class DemoController : BaseController<DemoController>
    {
        private readonly ILogger<DemoController> _logger;
        private readonly IDemoService _xxxService;

        public DemoController(ILogger<DemoController> logger,
            IDemoService xxxService) : base(logger)
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