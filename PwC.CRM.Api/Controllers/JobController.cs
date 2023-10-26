using Microsoft.AspNetCore.Mvc;
using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.BaseModel;

namespace PwC.CRM.Api.Controllers
{
    /// <summary>
    /// xxljob
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseController<JobController>
    {
        private readonly ILogger<JobController> _logger;
        public JobController(ILogger<JobController> logger) : base(logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// xxljob 示例
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dosomthing")]
        public async Task<CommonResponseDto<XxxRequestDto>> DoSomthing(XxxRequestDto parameter)
        {
            var result = new CommonResponseDto<XxxRequestDto>
            {
                Code = ResponseCodeEnum.Success
            };
            result.Data = parameter;
            return result;
        }

    }
}
