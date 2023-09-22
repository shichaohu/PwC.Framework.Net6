

## 一、CRM多环境接口路由

### 1.统一标记说明

HK=香港，US=北美，SG=新加坡

### 2.配置文件appsetting.json

```json
"Crm": {
    "HK": {
      "resourceUrl": "https://vaporessodev.api.crm5.dynamics.com",
      "clientId": "417b6275-c68d-4f9d-9f6f-45fa0e7de97a",
      "clientSecret": "mac8Q~9-9dRnNEfi22IATM1G6PMrLMuxIZw.wbT8",
      "tenantId": "3277c91b-811a-401a-b1a1-b769a05aefa7",
      "connectionString": "connectionString",
      "tokenUrl": "login.windows.net"
    },
    "US": {
      "resourceUrl": "https://vaporessosit.api.crm5.dynamics.com",
      "clientId": "417b6275-c68d-4f9d-9f6f-45fa0e7de97a",
      "clientSecret": "mac8Q~9-9dRnNEfi22IATM1G6PMrLMuxIZw.wbT8",
      "tenantId": "3277c91b-811a-401a-b1a1-b769a05aefa7",
      "connectionString": "connectionString",
      "tokenUrl": "login.windows.net"
    },
    "SG": {
      "resourceUrl": "https://vaporessosg.api.crm5.dynamics.com",
      "clientId": "417b6275-c68d-4f9d-9f6f-45fa0e7de97a",
      "clientSecret": "mac8Q~9-9dRnNEfi22IATM1G6PMrLMuxIZw.wbT8",
      "tenantId": "3277c91b-811a-401a-b1a1-b769a05aefa7",
      "connectionString": "connectionString",
      "tokenUrl": "login.windows.net"
    }
  }
```

### 3.场景一：单次请求指向统一服务器

- #### http header添加标记

```javascript
//指向香港服务器
Target-CRM-Service:HK
```

- #### service使用

```c
//a.继承BaseService
//b.使用_cRequest操作CRM数据库
public class PLMService : BaseService, IPLMService
{
    public PLMService(ICommonInjectionObject commonInjectionObject): base(commonInjectionObject)
    {
    }
    public async Task<CrmResponse> UpdateTask(PLMUpdateTaskRequestDto parameter)
    {
        Guid taskId = Guid.Parse(parameter.TaskNum);
        var taskResponse = await _cRequest.QueryRecords<Apv_Task>(taskId, "apv_status");

        return null;
    }
}
```

## 二、系统日志功能介绍

### 1、代码

#### a、代码配置

* ##### program.cs
  
  ```c++
  using PwC.Crm.Service.Share.Log; 
  using PwC.Crm.Service.Share.Log.Interface;
  using PwC.Crm.Share.Log.Serilogs; 
  
  var builder = WebApplication.CreateBuilder(args); 
  //a.注册日志策略 
  builder.Host.UseLogStrategy(builder.Logging, builder.Services, builder.Configuration); 
  
  var app = builder.Build(); 
  //b.添加日志处理管道 
  app.AddLogStrategy(app.Environment);
  ```
  
  

* ##### appsettings.json
  
  ```json
  "Log": {
    "LogStorageType": "LogFile", //日志存储模式：MySql(数据库),LogFile(日志文件)
    "LogStorageTypeWhenDebug": "LogFile", //调试时使用LogFile，用于数据库无法访问的（内网）场景
    "MySql": {
      "DbConnectionString": "mysql数据库连接字符串",
      "TableName": "logs_dev"//数据库表名
    }
  }
  ```

#### b、代码使用

* 控制器使用
  
  ```c++
  public class XxxDemoController : BaseController<XxxDemoController> {
    private readonly ILogger<XxxDemoController> _logger;
  
    public XxxDemoController(ILogger<XxxDemoController> logger) : base(logger)
    {
        _logger = logger;
    }
  
    [HttpPost]
    public async Task<IActionResult> GetXxxs(XxxRequestDto parameter)
    {
        _logger.LogInformation("日志内容");
        return Ok(res);
    }
  }
  ```
  
  

* service 使用
  
      public class XxxDemoService : IXxxDemoService {
        private readonly ILogger<XxxDemoService> _logger;
        public XxxDemoService(ILogger<XxxDemoService> logger)
        {
            _logger = logger;
        }
        public async Task<List<Businessunit>> GetBusinessunit(XxxRequestDto parameter)
        {
            _logger.LogInformation("日志内容");
            return null;
        }
      }
  
  

### 2、查询

* #### 获取token
  
  ```javascript
  post http://localhost:7108/api/Login/getToken
  request body：
  {
    "grant_type":"bearer",
    "client_id":"4e122431-4134-b1ca-e730-7102b6c1980a",
    "client_secret":"bn8BBA6BNTmZjjTQD47roB28pEo4TH",
    "scope":"eRXctBWeDce0R5N6bmF4t5sb37f687SkihzKFWJKJSHSWCCQse",
    "userID":"apvUser"
  }
  response body
  {
    "token_type": "Bearer",
    "expires_on": "1692614760",
    "not_before": "1692607560",
    "resource": "",
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0ZTEyMjQzMS00MTM0LWIxY2EtZTczMC03MTAyYjZjMTk4MGEiLCJuYW1lIjoiYXB2VXNlciIsIm5iZiI6MTY5MjYwNzU1OSwiZXhwIjoxNjkyNjE0NzU5LCJpc3MiOiJQd2MiLCJhdWQiOiJBUFYuQ1JNLlNlcnZpY2UuQXBpIn0.Z9uFUB5lA9xbf92xq4AAW_kr_h7TNtBcSoba7pNkGC0"
  }
  ```

* #### 请求示例
  
  ```
  POST http://localhost:7108/api/LogOperations/QueryDBLogs
  header Authorization:bearer+空格+access_token
  request body:
  {
    "HttpHost": "",
    "HttpPath": "/api/XxxDemo/GetXxxs",
    "HttpRequestId": "d711873d3d9a423f8d009ba4c5c4b0d8",
    "SourceContext": "SerilogMiddleware",
    "Level": 2,
    "Message": "",
    "TimeStart": "",
    "TimeEnd": "",
    "Limit": 20
  }
  ```

| 参数名           | 数类型    | 过滤方式     | 是否必填 | 举例                                                  | 描述                                                                                |
| ------------- | ------ | -------- | ---- | --------------------------------------------------- | --------------------------------------------------------------------------------- |
| HttpHost      | string | 模糊查询     | 否    | crm-web-api.smooretechtest.com                      | 主机地址                                                                              |
| HttpPath      | string | 模糊查询     | 否    | api/XxxDemo/GetXxxs                                 | 请求路径                                                                              |
| HttpRequestId | string | ==精确匹配== | 否    | d711873d3d9a423f8d009ba4c5c4b0d8 <br>(GUID去掉中间的“-”) | 请求唯一id                                                                            |
| SourceContext | string | 模糊查询     | 否    | Middlewares.SerilogMiddleware                       | 写日志的触发类的全名                                                                        |
| Level         | int    | ==精确匹配== | 否    | 2                                                   | 日志级别，为空不过滤此条件, <br> 0=Verbose,1=Debug,2=Information,<br>3=Warning,4=Error,5=Fatal |
| Message       | string | 模糊查询     | 否    | 日志内容                                                | 日志内容                                                                              |
| TimeStart     | string | 比较大小     | 否    | 2023-08-16 17:01:01                                 | 日志写入的开始时间，为空不过滤此条件                                                                |
| TimeEnd       | string | 比较大小     | 否    | 2023-08-16 17:09:01                                 | 日志写入的结束时间，为空时不过滤此条件                                                               |
| Limit         | int    | 限制数量     | 否    | 10                                                  | 查询条数，<br>为空或<=0时，程序强制设为50                                                         |

* #### 响应示例
  
  ```json
   { 
     "Value": [
           {
            "Id": 1030,
            "HttpHost": "crm-web-api-uat.smooretechtest.com",
            "HttpRemoteAddress": "10.99.9.28",
            "HttpXForwardedFor": "10.98.16.115",
            "HttpPath": "/api/BaseData/create_or_update_material_and_bank_datas",
            "HttpRequestId": "22542768423f446580a57945adb47b0e",
            "SourceContext": "Serilog.AspNetCore.RequestLoggingMiddleware",
            "Timestamp": "2023-08-21 16:57:37",
            "Level": "Information",
            "LevelEnum": 2,
            "Message": "HTTP \"POST\" \"/api/BaseData/create_or_update_material_and_bank_datas\" responded 200 in 71.0317 ms"
        },
        {
            "Id": 1031,
            "HttpHost": "crm-web-api-uat.smooretechtest.com",
            "HttpRemoteAddress": "10.99.9.28",
            "HttpXForwardedFor": "10.98.16.115",
            "HttpPath": "/api/BaseData/create_or_update_material_and_bank_datas",
            "HttpRequestId": "22542768423f446580a57945adb47b0e",
            "SourceContext": "APV.CRM.Service.Share.Log.Serilogs.Middlewares.SerilogMiddleware",
            "Timestamp": "2023-08-21 16:57:37",
            "Level": "Information",
            "LevelEnum": 2,
            "Message": "Response.Body：{\n    \"Value\":true,\n    \"Code\":200,\n    \"Message\":\"成功\"\n    }"
        }
      ], 
     "Code": 200 
   }
  ```
  
  

## 三、鉴权

### 1、appsetting.json

```json
{
    "Jwt": {
    "Bearer": { //Bearer认证参数
      "Issuer": "Pwc",//颁发人
      "Audience": "APV.CRM.Service.Api",//颁发给
      "SecretKey": "FDSFQ21232113#fdsfds1310dsfdsfIOPMMvf1238*&^%fdsfdsfdsfdsfdsfdsfcs23fds3A@#^!fdsf<>,?",//秘钥
      "TokenExpiryHours": 24 //token有效期24小时
    },
    "Basic": { //Basic认证参数
      "AllowIp": "10.121.1,*", //适用的Ip,多个用逗号隔开，10.121.1表示允许以10.121.1开头的ip，*表示允许所有ip
      "Account": "CRM_PO",
      "Password": "W11Q2zxc45" //：bast64编码("CRM_PO:W11Q2zxc45")="Q1JNX1BPOlcxMVEyenhjNDU" 即Basic Q1JNX1BPOlcxMVEyenhjNDU
    }
  }
}
```

### 2、program.cs

```c++
using PwC.Crm.Share.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(builder.Configuration);
```

### 3、获取token

```c++
post http://localhost:7108/api/Login/getToken
request body：
{
  "grant_type":"bearer",
  "client_id":"4e122431-4134-b1ca-e730-7102b6c1980a",
  "client_secret":"bn8BBA6BNTmZjjTQD47roB28pEo4TH",
  "scope":"eRXctBWeDce0R5N6bmF4t5sb37f687SkihzKFWJKJSHSWCCQse",
  "userID":"apvUser"
}
response body
{
  "token_type": "Bearer",
  "expires_on": "1692614760",
  "not_before": "1692607560",
  "resource": "",
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0ZTEyMjQzMS00MTM0LWIxY2EtZTczMC03MTAyYjZjMTk4MGEiLCJuYW1lIjoiYXB2VXNlciIsIm5iZiI6MTY5MjYwNzU1OSwiZXhwIjoxNjkyNjE0NzU5LCJpc3MiOiJQd2MiLCJhdWQiOiJBUFYuQ1JNLlNlcnZpY2UuQXBpIn0.Z9uFUB5lA9xbf92xq4AAW_kr_h7TNtBcSoba7pNkGC0"
}
```

### 4、请求示例

    POST http://localhost:7108/api/LogOperations/QueryDBLogs
    header 
        Authorization:bearer+空格+access_token
    或者 Authorization:Basic Q1JNX1BPOlcxMVEyenhjNDU=Basic
    request body:
    {
      "HttpHost": "",
      "HttpPath": "/api/XxxDemo/GetXxxs",
      "HttpRequestId": "d711873d3d9a423f8d009ba4c5c4b0d8",
      "SourceContext": "SerilogMiddleware",
      "Level": 2,
      "Message": "",
      "TimeStart": "",
      "TimeEnd": "",
      "Limit": 20
    }
