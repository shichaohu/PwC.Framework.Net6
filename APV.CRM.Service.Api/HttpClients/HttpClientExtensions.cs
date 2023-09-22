using APV.CRM.Service.Api.Extensions.HttpClientHandlers;
using APV.CRM.Service.Api.HttpClients.HttpClientHandlers;
using System.Runtime.CompilerServices;
using System.Text;

namespace APV.CRM.Service.Api.HttpClients
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// 注册ECMHttpClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCustomerHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<SRDMHttpClient>(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(configuration["ExternalApiUrl:SRDM:Url"]);
                })
                .AddHttpMessageHandler(provider =>
                {
                    return new LogHttpMessageHandler<SRDMHttpClient>(provider);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));
        }

    }
}
