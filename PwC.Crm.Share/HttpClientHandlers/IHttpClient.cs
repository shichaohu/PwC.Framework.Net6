using PwC.Crm.Share.BaseModel;

namespace APV.CRM.Service.Api.Extensions.HttpClientHandlers
{
    public interface IHttpClient
    {
        Task<T> GetAsync<T>(string method);
        Task<string> GetAsync(string method);
        Task<CommonResponseDto<T>> PostAsync<T>(string method, object parameters, Dictionary<string, string> headers = null);
    }
}
