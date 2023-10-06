using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.CRMClients;
using PwC.CRM.Share.Util;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PwC.CRM.Service.Core.Implement
{
    public class LoginService : BaseService, ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly LocalCachelper _cache;
        private string _loginUsersCacheKey = "APV.CRM.Service.LoginUsers";

        public LoginService(ILogger<LoginService> logger, LocalCachelper cache, ICommonInjectionObject commonInjectionObject)
            : base(commonInjectionObject)
        {
            string targetCRMService = commonInjectionObject.HttpContextAccessor.HttpContext.Request.Headers["Target-CRM-Service"];
            if (Enum.TryParse(targetCRMService, out CRMClientTypeEnum crmClientTypeEnum))
            {
                _loginUsersCacheKey = _loginUsersCacheKey + crmClientTypeEnum.ToString();
            }
            else
            {
                _loginUsersCacheKey = _loginUsersCacheKey + "HK";
            }
            _logger = logger;
            _cache = cache;
        }

        public Task ClearLoginUserToken()
        {
            _cache.RemoveKey(_loginUsersCacheKey);
            return Task.CompletedTask;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            var allApiUsers = _cache.GetValue<List<ApiUser>>(_loginUsersCacheKey);
            if (allApiUsers == null || allApiUsers.Count == 0)
            {
                allApiUsers = await LoadAllApiUsersToCache();
            }
            ApiUser myUser = null;
            if (string.IsNullOrEmpty(request.scope))
            {
                myUser = allApiUsers.FirstOrDefault(w => w.pwc_clientsecret == request.client_secret && w.pwc_clientid == request.client_id);
            }
            else
            {
                myUser = allApiUsers.FirstOrDefault(w => w.pwc_scope == request.scope);
            }
            if (myUser == null)
            {
                return null;
            }

            List<Claim> claims1 = new() { };
            claims1.Add(new Claim(JwtRegisteredClaimNames.Sub, myUser.pwc_clientid));
            claims1.Add(new Claim(JwtRegisteredClaimNames.Name, request.userID));
            if (!string.IsNullOrWhiteSpace(myUser.pwc_roles))
            {
                var sp = myUser.pwc_roles.Split(',');
                for (int i = 0; i < sp.Length; i++)
                {
                    if (!string.IsNullOrEmpty(sp[i]))
                    {
                        claims1.Add(new Claim(ClaimTypes.Role, sp[i]));
                    }
                }
            }
            var signingAlogorithm = SecurityAlgorithms.HmacSha256;
            var claims = claims1.ToArray();
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Jwt:Bearer:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlogorithm);
            var isOverseas = true;// await ConfigService.isOverseas(crequest);

            if (!int.TryParse(_configuration["Jwt:Bearer:TokenExpiryHours"], out int tokenExpiryHours))
            {
                tokenExpiryHours = 2;//默认2小时
            }

            var currDate = isOverseas ? DateTime.UtcNow : DateTime.UtcNow.AddHours(8);
            var calcDate = isOverseas ? DateTime.Parse("1970-01-01T00:00:00") : DateTime.Parse("1970-01-01T00:00:00").AddHours(8);
            DateTime notBefore = currDate;
            DateTime expires = currDate.AddHours(tokenExpiryHours);
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Bearer:Issuer"],
                    audience: _configuration["Jwt:Bearer:Audience"],
                    claims: claims,
                    notBefore: notBefore,
                    expires: expires,
                    signingCredentials
                );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            var ret = new LoginResponseDto()
            {
                expires_on = Convert.ToInt64((expires - calcDate).TotalSeconds).ToString(),
                not_before = Convert.ToInt64((notBefore - calcDate).TotalSeconds).ToString(),
                access_token = tokenStr,
                resource = ""
            };
            return ret;
        }

        private async Task<List<ApiUser>> LoadAllApiUsersToCache()
        {
            var rList = await _oDataHttpClient.QueryRecords<ApiUser>("pwc_apiusers?$select=pwc_name,pwc_clientid,pwc_clientsecret,pwc_scope,pwc_roles");
            if (rList.Data == null || rList.Data.Count == 0)
            {
                string msg = $"There is no user in table【pwc_apiusers】" + rList.Message;
                string logMsg = $"LoginService: {msg}";
                _logger.LogError(logMsg);
                throw new Exception(msg);
            }
            _cache.SetValue(_loginUsersCacheKey, rList.Data, TimeSpan.FromHours(24));

            return rList.Data;
        }
    }
}
