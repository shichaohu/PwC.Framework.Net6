using System.ComponentModel.DataAnnotations;

namespace APV.CRM.Service.Service.Dto.Request
{
    public class LoginRequestDto
    {
        public string? grant_type { get; set; }
        public string? client_id { get; set; }
        public string? client_secret { get; set; }
        public string? resource { get; set; }
        public string? scope { get; set; }
        [Required(ErrorMessage = " userID 不可为空")]
        public string? userID { get; set; }

    }
    public class LoginResponseDto
    {
        public string token_type { get; set; } = "Bearer";
        public string expires_on { get; set; }
        public string not_before { get; set; }

        public string resource { get; set; } = "";
        public string access_token { get; set; }
    }

    public class ApiUser
    {
        public string? pwc_name { get; set; }
        public string? pwc_clientid { get; set; }
        public string? pwc_clientsecret { get; set; }
        public string? pwc_scope { get; set; }
        public string? pwc_roles { get; set; }

    }
}
