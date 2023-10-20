using System.ComponentModel.DataAnnotations;

namespace PwC.CRM.Service.Dto.Request
{
    public class LoginRequestDto
    {
        /// <summary>
        /// grant type
        /// </summary>
        public string? grant_type { get; set; }
        /// <summary>
        /// client id
        /// </summary>
        public string? client_id { get; set; }
        /// <summary>
        /// client secret
        /// </summary>
        public string? client_secret { get; set; }
        /// <summary>
        /// resource
        /// </summary>
        public string? resource { get; set; }
        /// <summary>
        /// scope
        /// </summary>
        public string? scope { get; set; }
        /// <summary>
        /// user ID
        /// </summary>
        [Required(ErrorMessage = " userID 不可为空")]
        public string? userID { get; set; }

    }
    public class LoginResponseDto
    {
        /// <summary>
        /// token type
        /// </summary>
        public string token_type { get; set; } = "Bearer";
        /// <summary>
        /// expires on
        /// </summary>
        public string expires_on { get; set; }
        /// <summary>
        /// not before
        /// </summary>
        public string not_before { get; set; }
        /// <summary>
        /// resource
        /// </summary>
        public string resource { get; set; } = "";
        /// <summary>
        /// access token
        /// </summary>
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
