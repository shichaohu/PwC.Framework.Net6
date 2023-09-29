
using PwC.CRM.Job.BaseData.UpdateCustomerBalanceReceivable;
using PwC.Crm.Share.Util;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Drawing.Text;


Stopwatch watch = new();
try
{
    var config = InitConfiguration();
    var crequest = InitCRequest(config);

    SerilLogHelper.WriteLog("Job执行开始。", SerilLogLogLevel.Info);
    watch.Start();

    UpdateCustomerBalanceReceivableService service = new(config);
    await service.Execute();
}
catch (Exception ex)
{
    SerilLogHelper.WriteLog($"发生异常：{ex.Message}", SerilLogLogLevel.Error);
}
finally
{
    if (watch.IsRunning) watch.Stop();
    SerilLogHelper.WriteLog($"Job执行结束，业务执行总耗时：{watch.ElapsedMilliseconds} 毫秒。");
}


static IConfigurationRoot InitConfiguration()
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
    return config;

}

/// <summary>
/// 初始化CRequest
/// </summary>
static PwcNetCore.CRequest InitCRequest(IConfigurationRoot config)
{
    return new PwcNetCore.CRequest(
            config["Crm:resourceUrl"],
            config["Crm:clientId"],
            config["Crm:clientSecret"],
            config["Crm:tenantId"],
            config["Crm:tokenUrl"]);

}