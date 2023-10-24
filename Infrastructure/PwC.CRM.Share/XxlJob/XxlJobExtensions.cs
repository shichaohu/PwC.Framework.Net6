using DotXxlJob.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PwC.CRM.Share.XxlJob.JobHandlers;

namespace PwC.CRM.Share.XxlJob
{
    public static class XxlJobExtensions
    {
        public static void AddXxlJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddXxlJobExecutor(configuration);
            //services.AddDefaultXxlJobHandlers();
            services.AddSingleton<IJobHandler, PwCJobHandler>();
            services.AddAutoRegistry(); // 自动注册
        }
        public static IApplicationBuilder UseXxlJobExecutor(this IApplicationBuilder app)
        {
            return app.UseMiddleware<XxlJobExecutorMiddleware>();
        }
    }
}
