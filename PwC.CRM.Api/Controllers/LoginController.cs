using PwC.CRM.Api.Swagger.SwaggerModel;
using PwC.CRM.Service.Core;
using PwC.CRM.Service.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PwC.CRM.Api.Controllers
{
    /// <summary>
    /// Login
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiGroup(ApiGroupNames.Common)]
    public class LoginController : BaseController<LoginController>
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;
        public LoginController(ILogger<LoginController> logger, ILoginService loginService) : base(logger)
        {
            _logger = logger;
            _loginService = loginService;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> GetToken(LoginRequestDto request)
        {
            var ret = await _loginService.Login(request);

            if (ret == null)
            {
                _logger.LogInformation($"登录密码错误:{JsonConvert.SerializeObject(request)}");
                var erroRes = new
                {
                    error = "unauthorized_client",
                    error_description = "userid check error",
                    error_codes = new List<int>() { 700016 },
                    timestamp = DateTime.Now.ToString(),
                    trace_id = Guid.NewGuid(),
                    correlation_id = Guid.NewGuid(),
                    error_uri = ""
                };
                return BadRequest(erroRes);
            }
            return Ok(ret);
        }
        /// <summary>
        /// 清空登录用户Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ClearLoginUserToken")]
        public ActionResult ClearLoginUserToken()
        {
            _loginService.ClearLoginUserToken();
            return Ok();
        }
    }

}
