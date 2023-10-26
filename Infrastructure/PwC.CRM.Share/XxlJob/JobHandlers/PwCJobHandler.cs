using DotXxlJob.Core;
using DotXxlJob.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PwC.CRM.Share.XxlJob.JobHandlers
{
    [JobHandler("PwCJobHandler")]
    public class PwCJobHandler : AbstractJobHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PwCJobHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public override async Task<ReturnT> Execute(JobExecuteContext context)
        {
            if (string.IsNullOrEmpty(context.JobParameter))
            {
                return ReturnT.Failed("Job parameter is empty");
            }

            var jobParameter = JsonConvert.DeserializeObject<XxlJobExecutorParams>(context.JobParameter);
            if (jobParameter == null || string.IsNullOrWhiteSpace(jobParameter.Path))
            {
                return ReturnT.Failed("Path in job parameter is not valid");
            }

            using HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri($"{_configuration["xxlJob:specialBindAddress"]}:{_configuration["xxlJob:port"]}");

            string postJsonData = JsonConvert.SerializeObject(jobParameter.Parameter);

            var httpContent = new StringContent(postJsonData, Encoding.UTF8, "application/json");
            httpContent.Headers.TryAddWithoutValidation(HeaderNames.Accept, "application/json");

            try
            {
                HttpResponseMessage httpResponseMessage = await client.PostAsync(jobParameter.Path, httpContent);
                if (httpResponseMessage == null)
                {
                    context.JobLogger.Log("call remote error,response is null");
                    return ReturnT.Failed("call remote error,response is null");
                }

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    context.JobLogger.Log("call remote error,response statusCode ={0}", httpResponseMessage.StatusCode);
                    return ReturnT.Failed("call remote error,response statusCode =" + httpResponseMessage.StatusCode);
                }

                string text = await httpResponseMessage.Content.ReadAsStringAsync();
                context.JobLogger.Log("<br/> call remote success ,response is : <br/> {0}", text);
                return ReturnT.Success(text);
            }
            catch (Exception ex)
            {
                context.JobLogger.LogError(ex);
                return ReturnT.Failed(ex.Message);
            }
        }

    }
}
