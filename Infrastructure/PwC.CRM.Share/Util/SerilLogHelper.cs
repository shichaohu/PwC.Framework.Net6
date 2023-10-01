using Serilog;
using Serilog.Events;

namespace PwC.CRM.Share.Util
{
    /// <summary>
    /// Serilog 记录日志
    /// </summary>
    public static class SerilLogHelper
    {
        private static ILogger logger;

        /// <summary>
        /// 实例化ILogger
        /// </summary>>
        static SerilLogHelper()
        {
            // 为每个线程创建独立的日志记录器，日志文件夹以项目名称命名
            logger = new LoggerConfiguration()

                .MinimumLevel.Debug() // 设置最低日志级别
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .WriteTo.Console()    // 输出日志到控制台
                .WriteTo.Logger(config => ConfigureSerilog(config))
                .CreateLogger();// 构造日志记录器
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        private static void ConfigureSerilog(LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .WriteTo.Logger(configure => configure
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(
                        $"logs/error/.log",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 10_000_000,
                        retainedFileCountLimit: 200,
                        retainedFileTimeLimit: TimeSpan.FromDays(7),
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .WriteTo.File(
                    $"logs/info/.log",
                    rollingInterval: RollingInterval.Day,// 设置日志输出到文件中，文件名按天滚动，文件夹名称为日期加小时
                    rollOnFileSizeLimit: true,// 设置为 true，表示启用日志文件大小限制，当日志文件达到设定的大小后，会自动滚动到新的文件中。
                    fileSizeLimitBytes: 10_000_000, //设置每个日志文件的最大大小，单位是字节。这里的值是 10MB，即 10_000_000 字节。
                    retainedFileCountLimit: 200,//设置保留的日志文件数量上限，这里是 200，即最多保留最新的 200 个日志文件。
                    retainedFileTimeLimit: TimeSpan.FromDays(7),//设置日志文件的最长保留时间，这里是 7 天。
                    shared: true, // 多进程共享文件
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj} {NewLine}{Exception}",// 设置日志输出模板，包括时间戳、日志级别、日志消息、日志来源和异常信息
                    restrictedToMinimumLevel: LogEventLevel.Debug
                );
        }

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="logLevel">日志级别</param>
        public static void WriteLog(string message, SerilLogLogLevel logLevel = SerilLogLogLevel.Info)
        {
            switch (logLevel)
            {
                case SerilLogLogLevel.Info:
                    logger.Information(message);
                    break;
                case SerilLogLogLevel.Warning:
                    logger.Warning(message);
                    break;
                case SerilLogLogLevel.Error:
                    logger.Error(message);
                    break;
                case SerilLogLogLevel.Debug:
                    logger.Debug(message);
                    break;
                case SerilLogLogLevel.Fatal:
                    logger.Fatal(message);
                    break;
            }
        }
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum SerilLogLogLevel
    {
        Info, Warning, Error, Debug, Fatal
    }
}

