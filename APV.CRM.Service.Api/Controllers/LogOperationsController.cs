using APV.CRM.Service.Api.Swagger.SwaggerModel;
using APV.CRM.Service.Service.Core.LogRepositorys;
using APV.CRM.Service.Service.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using PwC.Crm.Share.BaseModel;
using PwC.Crm.Share.Log.Serilogs.Attributes;
using PwC.Crm.Share.Log.Serilogs.Models;

namespace APV.CRM.Service.Api.Controllers
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
                string path = Directory.GetCurrentDirectory() + "\\log";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Directory.GetFiles(path).ToList();
                if (request.logfilename != null && request.logfilename.Count > 0)
                {
                    file = file.Where(w => request.logfilename.Any(c => w.EndsWith(c))).ToList();
                }

                file.ForEach(v =>
                {
                    var allLine = System.IO.File.ReadAllLines(v);
                    for (int i = 0; i < allLine.Length; i++)
                    {

                        if (request.relationship == "or" && retObjcet.Data.Count < 3000)
                        {
                            if (request.search.Any(w => allLine[i].Contains(w)))
                            {
                                retObjcet.Data.Add(allLine[i]);

                            }
                        }

                        else if (request.relationship == "and" && retObjcet.Data.Count < 3000)
                        {
                            if (request.search.All(w => allLine[i].Contains(w)))
                            {
                                retObjcet.Data.Add(allLine[i]);

                            }
                        }


                    }
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
        public List<string> search { get; set; }
        public List<string>? logfilename { get; set; }
        public string? relationship { get; set; } = "or";
    }
}
