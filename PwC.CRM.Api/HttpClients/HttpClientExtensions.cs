using PwC.CRM.Api.Extensions.HttpClientHandlers;
using PwC.CRM.Api.HttpClients.HttpClientHandlers;
using System.Runtime.CompilerServices;
using System.Text;

namespace PwC.CRM.Api.HttpClients
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds customer HttpClient to the specified IServiceCollection
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
