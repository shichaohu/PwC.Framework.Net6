using PwC.CRM.Api.Extensions.HttpClientHandlers;
using NPOI.SS.Formula.Functions;
using PwC.CRM.Share.BaseModel;

namespace PwC.CRM.Api.HttpClients.HttpClientHandlers
{
    /// <summary>
    /// SRDM的HttpClient
    /// </summary>
    public class SRDMHttpClient : BaseHttpClient, IHttpClient
    {
        public SRDMHttpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// 给SRDM发送Http请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<CommonResponseDto<T>> SendAsync(string url, object parameters)
        {
            var header = new Dictionary<string, string>();
            header["Content-Type"] = "application/json";
            var result = await PostAsync<T>(url, parameters, header);
            return result;
        }
    }

}
