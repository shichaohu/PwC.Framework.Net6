using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PwC.Crm.Share.Util;
using PwC.CRM.Service.Dto.Request;

namespace PwC.CRM.Job.BaseData.UpdateCustomerBalanceReceivable
{
    public class UpdateCustomerBalanceReceivableService
    {
        private readonly IConfigurationRoot _configuration;
        private readonly string _baseUrl;

        public UpdateCustomerBalanceReceivableService(IConfigurationRoot _configuration)
        {
            _baseUrl = _configuration["CRMInterface:BasreUrl"];
        }

        public async Task Execute()
        {
            string url = $"{_baseUrl}/api/BaseData/QueryBalanceReceivable";
            string token = GetToken();
            Dictionary<string, string> headers = new()
            {
                { "Authorization", token }
            };
            var param = new
            {
                KUNNR = "",
                BSUNT = ""
            };
            string postData = JsonConvert.SerializeObject(param);
            var result = HttpHelperUtil.HttpWebRequestPost(url, postData, headers, timeOut: 600000);
        }

        private string GetToken()
        {
            string strTokenUrl = $"{_baseUrl}/api/Login/GetToken";

            Dictionary<string, string> headers = new Dictionary<string, string>();
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            loginRequestDto.grant_type = _configuration["CRMInterface:TokenParams:GrantType"];
            loginRequestDto.client_id = _configuration["CRMInterface:TokenParams:ClientId"];
            loginRequestDto.client_secret = _configuration["CRMInterface:TokenParams:ClientSecret"];
            loginRequestDto.scope = _configuration["CRMInterface:TokenParams:Scope"];
            loginRequestDto.resource = _configuration["CRMInterface:TokenParams:Resource"];
            loginRequestDto.userID = _configuration["CRMInterface:TokenParams:UserId"];
            string strBody = JsonConvert.SerializeObject(loginRequestDto);
            var strResponse = HttpHelperUtil.HttpWebRequestPost(strTokenUrl, strBody, headers);
            var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(strResponse);
            return $"{loginResponseDto.token_type} {loginResponseDto.access_token}";
        }

    }
}
