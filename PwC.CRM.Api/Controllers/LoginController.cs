using PwC.CRM.Api.Swagger.SwaggerModel;
using PwC.CRM.Service.Core;
using PwC.CRM.Service.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PwC.CRM.Share.BaseModel;
using PwC.CRM.Share.Log.Serilogs.Models;

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
        public async Task<CommonResponseDto<LoginResponseDto>> GetToken(LoginRequestDto request)
        {
            var result = new CommonResponseDto<LoginResponseDto>
            {
                Code = ResponseCodeEnum.Success
            };
            var ret = await _loginService.Login(request);

            if (ret == null)
            {
                result.Code = ResponseCodeEnum.ParameterError;
                result.Message = "unauthorized client,incorrect input parameter";
                _logger.LogInformation($"unauthorized client,incorrect input parameter:{JsonConvert.SerializeObject(request)}");

            }
            else
                result.Data = ret;

            return result;
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
