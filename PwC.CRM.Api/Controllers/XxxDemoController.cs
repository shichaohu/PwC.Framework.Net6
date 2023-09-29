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
        /// ��ȡXxx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetXxxs([FromBody] XxxRequestDto parameter)
        {
            _logger.LogInformation("��־����");
            var res = await _xxxService.GetBusinessunit(parameter);
            return Ok(res);
        }

    }

}