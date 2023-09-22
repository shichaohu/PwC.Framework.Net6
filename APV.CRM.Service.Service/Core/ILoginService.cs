using APV.CRM.Service.Service.Dto.Request;
using PwC.Crm.Share.CommonCode;

namespace APV.CRM.Service.Service.Core
{
    public interface ILoginService : IDependency
    {
        Task<LoginResponseDto> Login(LoginRequestDto request);

        Task ClearLoginUserToken();
    }
}
