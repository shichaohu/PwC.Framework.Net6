using PwC.CRM.Api.Swagger.SwaggerModel;
using PwC.CRM.Service.Core.LogRepositorys;
using PwC.CRM.Service.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using PwC.CRM.Share.BaseModel;
using PwC.CRM.Share.Log.Serilogs.Attributes;
using PwC.CRM.Share.Log.Serilogs.Models;

namespace PwC.CRM.Api.Controllers
{
    /// <summary>
    /// 日志
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Common)]
    [SerilogIgnore]
    public class LogOperationsController : BaseController<LogOperationsController>
    {
        private readonly IMySqlLogRepository _mySqlLogRepository;

        public LogOperationsController(ILogger<LogOperationsController> logger,
            IMySqlLogRepository mySqlLogRepository) : base(logger)
        {
            _mySqlLogRepository = mySqlLogRepository;
        }

        /// <summary>
        /// 查询文件日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryFileLogs")]
        public async Task<IActionResult> QueryFileLogs(SearchLogsModel request)
        {
            CommonResponseDto<List<string>> retObjcet = new CommonResponseDto<List<string>>()
            {
                Code = ResponseCodeEnum.Success,
                Message = "ok",
                Data = new List<string>()
            };
            try
            {
                string path = Directory.GetCurrentDirectory() + "\\logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Directory.GetFiles(path).ToList();
                if (request.LogfileNamePrefix != null && request.LogfileNamePrefix.Count > 0)
                {
                    file = file.Where(w => request.LogfileNamePrefix.Any(c => w.StartsWith(c))).ToList();
                }

                file.ForEach(v =>
                {
                    var allLine = System.IO.File.ReadAllLines(v).ToList(); 
                    retObjcet.Data.AddRange(allLine);                    
                });
            }
            catch (Exception e)
            {

            }
            return Ok(retObjcet);
        }

        /// <summary>
        /// 查询数据库日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryDBLogs")]
        public async Task<CommonResponseDto<List<LoggerSimpleDO>>> QueryDBLogs(LogRequestDto request)
        {

            var result = new CommonResponseDto<List<LoggerSimpleDO>>
            {
                Code = ResponseCodeEnum.Success
            };
            var resp = await _mySqlLogRepository.QueryLogsAsync(request);

            result.Data = resp;

            return result;

        }




    }


    public class SearchLogsModel
    {
        /// <summary>
        /// 日志文件名称前缀
        /// </summary>
        public List<string>? LogfileNamePrefix { get; set; }
    }
}
