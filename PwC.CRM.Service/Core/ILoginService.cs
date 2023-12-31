﻿using PwC.CRM.Service.Dto.Request;
using PwC.CRM.Share.CommonCode;

namespace PwC.CRM.Service.Core
{
    public interface ILoginService : IDependency
    {
        Task<LoginResponseDto> Login(LoginRequestDto request);

        Task ClearLoginUserToken();
    }
}
