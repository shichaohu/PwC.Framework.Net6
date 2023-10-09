using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PwC.CRM.Share.BaseModel;
using PwC.CRM.Share.CommonCode;
using System.Reflection;

namespace PwC.CRM.Share.Extensions;

public static class AspDotNetExtensions
{

    /// <summary>
    /// �Զ�ע��projectName��ʵ��IDependency�Ľӿ�
    /// </summary>
    /// <param name="services"></param>
    /// <param name="projectName">��Ŀ����ǰ׺��һ����������ע��㣬��service</param>
    public static void AddAutoDependency(this IServiceCollection services, string projectName)
    {
        var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
            .Where(a => a.FullName.StartsWith(projectName));

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.Where(t => t.IsAssignableTo(typeof(IDependency))))
                .AsImplementedInterfaces(x =>
                {
                    return x.IsAssignableTo(typeof(IDependency));
                })
        .WithScopedLifetime());
    }


    /// <summary>
    /// Adds the model state vrify service to the specified IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    public static void AddModelStateVrify(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                //��ȡ��֤ʧ�ܵ�ģ���ֶ� 
                var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => e.Value.Errors.First().ErrorMessage)
                .ToList();
                var str = string.Join("|", errors);
                //���÷�������
                var result = new CommonResponseDto
                {
                    Code = ResponseCodeEnum.ParameterError,
                    Message = str
                };
                var res = new BadRequestObjectResult(result)
                {
                    StatusCode = 200
                };
                return res;
            };
        });
    }

    /// <summary>
    /// �����ļ��ϴ���С 4G
    /// </summary>
    /// <param name="services"></param>
    /// <param name="webHostBuilder"></param>
    public static void UseFileUpload(this IServiceCollection services, IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureKestrel((context, options) =>
        {
            //�������4G��1073741824*4=4,294,967,296  byte��
            options.Limits.MaxRequestBodySize = 4_294_967_296;
        });
        services.Configure<FormOptions>(option =>
        {
            //�������4G��1073741824*4=4,294,967,296  byte��
            option.MultipartBodyLengthLimit = 4_294_967_296;
        });
    }
}

