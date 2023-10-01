using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PwC.CRM.Share.BaseModel
{
    public class CommonResponseDto
    {
        /// <summary>
        /// 返回状态的编码
        /// </summary>
        public ResponseCodeEnum Code { get; set; }
        /// <summary>
        /// 详细数据
        /// </summary>
        public string Message { get; set; }
    }
    public class CommonResponseDto<T> : CommonResponseDto
    {
        public T Data { get; set; }

        /// <summary>
        /// 默认成功返回消息实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public CommonResponseDto<T> SuccessResponse<T>(T data, string message = "成功")
        {
            CommonResponseDto<T> responseDto = new CommonResponseDto<T>();
            responseDto.Code = ResponseCodeEnum.Success;
            responseDto.Message = message;
            responseDto.Data = data;
            return responseDto;
        }

        /// <summary>
        /// 默认失败返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public CommonResponseDto<T> FailResponse(string message, ResponseCodeEnum code = ResponseCodeEnum.ParameterError)
        {
            CommonResponseDto<T> responseDto = new CommonResponseDto<T>();
            responseDto.Code = code;
            responseDto.Message = message;
            return responseDto;
        }

    }

    public class TaskResponseDto<T>
    {
        public string AsyncState { get; set; }
        public bool CancellationPending { get; set; }
        //public string CreationOptions { get; set; }
        public string Exception { get; set; }
        public int Id { get; set; }
        public CommonResponseDto<T> Result { get; set; }
        //public string Status { get; set; }
    }
    public enum ResponseCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 返回PO错误状态码
        /// </summary>
        POError = 500,
        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 2000,
        /// <summary>
        /// 内部错误
        /// </summary>
        InternalError = 2010,
        /// <summary>
        /// 其他错误
        /// </summary>
        OtherError = 2020,
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError = 2030,

        BatchCreatError = 2040
    }

}
