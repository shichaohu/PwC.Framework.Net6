﻿using DotXxlJob.Core;
using DotXxlJob.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PwC.CRM.Share.Util;
using System.Text;

namespace PwC.CRM.Share.XxlJob
{
    public class XxlJobExecutorMiddleware
    {
        private readonly IServiceProvider _provider;
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<XxlJobExecutorMiddleware> _logger;
        private readonly XxlRestfulServiceHandler _rpcService;
        public XxlJobExecutorMiddleware(IServiceProvider provider, RequestDelegate next, IConfiguration configuration, ILogger<XxlJobExecutorMiddleware> logger)
        {
            this._provider = provider;
            this._next = next;
            this._configuration = configuration;
            this._logger = logger;
            this._rpcService = _provider.GetRequiredService<XxlRestfulServiceHandler>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue)
            {
                string[] paths = new string[] { "beat", "idleBeat", "run", "kill", "log" };
                string requestPath = context.Request.Path.Value.Trim('/').ToLower();
                if (paths.Contains(requestPath))
                {
                    //await _rpcService.HandlerAsync(context.Request, context.Response);
                    //return;

                    switch (requestPath)
                    {
                        case "beat":
                        case "idleBeat":
                        case "kill":
                            context.Response.StatusCode = 200;
                            return;
                        case "log":
                            var logRes = ReturnT.Success("");
                            logRes.Content = new LogResult(1, 2, "查询job执行日志，请使用接口：/api/LogOperations/QueryDBLogs", true);
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(logRes));
                            context.Response.StatusCode = 200;
                            return;
                    }
                    if (requestPath == "run")
                    {
                        try
                        {
                            string bodyStr = string.Empty;

                            context.Request.EnableBuffering();
                            context.Request.Body.Seek(0, SeekOrigin.Begin);
                            using (var ms = new MemoryStream())
                            {
                                context.Request.Body.CopyTo(ms);
                                var param = ms.ToArray();
                                bodyStr = Encoding.UTF8.GetString(param);
                            };
                            context.Request.EnableBuffering();
                            context.Request.Body.Seek(0, SeekOrigin.Begin);

                            var body = JsonConvert.DeserializeObject<XxlJobParameter>(bodyStr);
                            string executorParamsStr = body?.executorParams;
                            string actuallyParam = string.Empty;
                            if (!string.IsNullOrEmpty(executorParamsStr))
                            {
                                var executorParams = JsonConvert.DeserializeObject<XxlJobExecutorParams>(executorParamsStr);
                                if (!string.IsNullOrWhiteSpace(executorParams?.Path))
                                {
                                    actuallyParam = JsonConvert.SerializeObject(executorParams.Parameter);
                                    var buff = Encoding.UTF8.GetBytes(actuallyParam);
                                    var ms = new MemoryStream();
                                    ms.Seek(0, SeekOrigin.Begin);
                                    ms.Write(buff);

                                    context.Request.Body = ms;
                                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                                    context.Request.Path = executorParams.Path;

                                    _logger.LogInformation($@"XxlJobExecutor begin run path:{executorParams.Path}{Environment.NewLine}Request.Body:{actuallyParam}");
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            string message = $"XxlJobExecutor run error:{ex.Message}";
                            _logger.LogError(message);
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(ReturnT.Failed(message)));
                            context.Response.StatusCode = 200;
                            return;
                        }
                        finally
                        {
                            string authorization = StringHelper.StringToBase64($"{_configuration["Jwt:Basic:Account"]}:{_configuration["Jwt:Basic:Password"]}");
                            context.Request.Headers.Authorization = $"Basic {authorization}";
                        }

                    }

                }
            }

            await _next.Invoke(context);
        }
    }

}