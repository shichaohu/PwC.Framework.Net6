
# WebApi 基础框架设计
- [WebApi 基础框架设计](#webapi-基础框架设计)
  - [基本信息](#基本信息)
    - [使用场景](#使用场景)
    - [框架版本](#框架版本)
    - [源码](#源码)
  - [一、鉴权](#一鉴权)
    - [1.1、配置文件 appsetting.json](#11配置文件-appsettingjson)
    - [1.2、代码开启 program.cs](#12代码开启-programcs)
    - [1.3、示例：Jwt 获取token](#13示例jwt-获取token)
    - [1.4、示例：请求接口“查询数据库日志”](#14示例请求接口查询数据库日志)
    - [1.5、Dataverse 表](#15dataverse-表)
  - [二、设置文件上传大小](#二设置文件上传大小)
    - [2.1. 功能描述](#21-功能描述)
    - [2.2 .net 6代码](#22-net-6代码)
    - [2.3. IIS Web.config 配置](#23-iis-webconfig-配置)
  - [三、Application Insights](#三application-insights)
    - [3.1. 功能描述](#31-功能描述)
    - [3.2. 代码开启program.cs](#32-代码开启programcs)
    - [3.3. 配置文件appsettings.json](#33-配置文件appsettingsjson)
    - [3.4.效果图](#34效果图)
  - [四、CRMClients](#四crmclients)
    - [4.1. 功能描述](#41-功能描述)
      - [CRMClients操作Dataverse的2中方式](#crmclients操作dataverse的2中方式)
      - [使用方式](#使用方式)
    - [4.2. 代码开启program.cs](#42-代码开启programcs)
    - [4.3. 配置文件appsettings.json](#43-配置文件appsettingsjson)
    - [4.4、CRM多环境接口路由](#44crm多环境接口路由)
      - [4.4.1.统一标记说明](#441统一标记说明)
      - [4.4.2.配置文件appsetting.json](#442配置文件appsettingjson)
      - [4.4.3.Demo：单次请求指向单一服务器](#443demo单次请求指向单一服务器)
    - [4.5、事务（TransactionServiceClient）](#45事务transactionserviceclient)
  - [五、依赖注入](#五依赖注入)
      - [5.1 功能描述](#51-功能描述)
    - [5.1 代码开启program.cs](#51-代码开启programcs)
  - [六、api接口入参模型校验](#六api接口入参模型校验)
      - [6.1 功能描述](#61-功能描述)
    - [6.1 代码开启program.cs](#61-代码开启programcs)
  - [七、api跨域支持](#七api跨域支持)
    - [7.1 代码开启program.cs](#71-代码开启programcs)
  - [八、Swagger文档](#八swagger文档)
      - [8.1 功能描述](#81-功能描述)
    - [8.2 代码开启program.cs](#82-代码开启programcs)
    - [8.3 接口分组](#83-接口分组)
    - [8.4 功能截图](#84-功能截图)
  - [九、自定义HttpClient（程序内发起http请求）](#九自定义httpclient程序内发起http请求)
      - [9.1 功能描述](#91-功能描述)
    - [9.2 .net6中HttpClient的3种使用方式](#92-net6中httpclient的3种使用方式)
      - [9.2.1、直接使用方式](#921直接使用方式)
      - [9.2.2、命名式使用方式](#922命名式使用方式)
      - [9.2.3、类型化使用方式](#923类型化使用方式)
  - [十、系统日志](#十系统日志)
    - [10.1功能描述](#101功能描述)
    - [10.2、代码](#102代码)
      - [a、数据库脚本](#a数据库脚本)
      - [b、开启代码](#b开启代码)
      - [c、代码使用](#c代码使用)
    - [10.3、查询](#103查询)
    - [10.4 html+JS查询数据库日志](#104-htmljs查询数据库日志)
      - [10.4.1 功能说明](#1041-功能说明)
      - [10.4.2 截图](#1042-截图)
  - [十一、部署](#十一部署)
    - [11.1 部署架构](#111-部署架构)
    - [11.2 IIS部署](#112-iis部署)
    - [11.3 Windows Servers 部署](#113-windows-servers-部署)
    - [11.4 Docker部署](#114-docker部署)

## 基本信息
### 使用场景
- 场景一：基于.net 6.0框架开发WebApi
  - 此种场景需要注释掉下文中第四点（CRMClients）的功能
- 场景二：D365与客户内部系统通过WebApi（代理角色）实现交互
  - D365部署在外网，而客户内部系统只能内网访问 
  - 交互过程中存在复杂的逻辑
### 框架版本
  ```xml
  <TargetFramework>net6.0</TargetFramework>
  ```
### 源码
  ```
  github: https://github.com/shichaohu/PwC.Framework.Net6
  gitee: https://gitee.com/TheCustodian/PwC.Framework.Net6
  ```
  
- 架构图
## 一、鉴权
<details> 
    <summary>展开设计</summary>

  **鉴权方式**
  - Jwt
    - 使用场景：外网访问webapi时，强制使用
      - 加强系统访问的安全性
    - 使用方式
      - 1、调用任意接口前，先通过"/api/Login/getToken"接口获取access_token，access_token有效时间为24小时，可在配置文件中设置，有效时间可通过access_token中的expires_on（UTC时间戳，精确到秒）计算出
      - 2、调用接口时，在header 中传入参数
        ```C#
        Authorization:Bearer+空格+access_token
        ```
  - Basic
    - 使用场景：内部系统访问
      - 减少系统设计复杂度
      - 但相当于明文传输账号和密码
    - 使用方式
      - 调用接口时，在header 中传入参数
        ```C#
        Authorization:Basic+空格+base64密文
        其中，base64密文=appsetting.json中的Jwt.Basic下的Account和Password按照bast64编码(Account+":"+Password)计算出，详见1.1、配置文件 appsetting.json
        ```

### 1.1、配置文件 appsetting.json

```json
{
    "Jwt": {
    "Bearer": { //Bearer认证参数
      "Issuer": "Pwc",//颁发人
      "Audience": "PwC.CRM.Api",//颁发给
      "SecretKey": "FDSFQ21232113#fdsfds1310dsfdsfIOPMMvf1238*&^%fdsfdsfdsfdsfdsfdsfcs23fds3A@#^!fdsf<>,?",//签名秘钥
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

### 1.2、代码开启 program.cs

``` C#
using PwC.Crm.Share.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
```
- $\color{#dd0000} {builder.Services.AddAuthentication方法是自定义实现的，具体实现请参阅源代码}$


### 1.3、示例：Jwt 获取token

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

### 1.4、示例：请求接口“查询数据库日志”
```javascript
POST http://localhost:7108/api/LogOperations/QueryDBLogs
header 
    Authorization:bearer+空格+access_token
或者 Authorization:Basic Q1JNX1BPOlcxMVEyenhjNDU=Basic
request body:
{
  "HttpHost": "",
  "HttpPath": "/api/Demo/GetXxxs",
  "HttpRequestId": "d711873d3d9a423f8d009ba4c5c4b0d8",
  "SourceContext": "SerilogMiddleware",
  "Level": 2,
  "Message": "",
  "TimeStart": "",
  "TimeEnd": "",
  "Limit": 20
}
```

### 1.5、Dataverse 表
pwc_apiusers（pwc_name,pwc_clientid,pwc_clientsecret,pwc_scope,pwc_roles）
- 用于存储Jwt方式鉴权场景中的接口“获取token”的请求参数
- 表结构如下：
  ``` C#
  public class ApiUser
  {
      public string? pwc_name { get; set; }
      public string? pwc_clientid { get; set; }
      public string? pwc_clientsecret { get; set; }
      public string? pwc_scope { get; set; }
      public string? pwc_roles { get; set; }

  }
  ```

</details>

## 二、设置文件上传大小
<details> 
    <summary>展开设计</summary>

### 2.1. 功能描述
- .net 6 默认上传文件大小限制是30M
- IIS 默认上传文件大小限制是30M
- 如果存在上传大于30M文件的接口，必须配置此功能
- 配置后最大支持约2G
### 2.2 .net 6代码
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.UseFileUpload(builder.WebHost);
```
-  $\color{#dd0000} {builder.Services.UseFileUpload方法是自定义实现的，具体实现请参阅源代码}$
  
### 2.3. IIS Web.config 配置
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\PwC.CRM.Api.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="PRD" />
        </environmentVariables>
      </aspNetCore>
      <serverRuntime uploadReadAheadSize="2147483647" />
    </system.webServer>
    <system.web>
      <httpRuntime maxRequestLength="2147483647" useFullyQualifiedRedirectUrl="true" executionTimeout="3600" />
    </system.web>
    <system.webServer>
      <security>
        <requestFiltering>
          <requestLimits maxAllowedContentLength="2147483647" />
        </requestFiltering>
      </security>
    </system.webServer>
  </location>
</configuration>

```

</details>

## 三、Application Insights
<details> 
    <summary>展开设计</summary>

### 3.1. 功能描述

[Application Insights 概述](https://learn.microsoft.com/zh-cn/azure/azure-monitor/app/app-insights-overview?tabs=net)

[github上的topic]( https://github.com/topics/application-insights)
```
Application Insights是微软基于Azure平台所提供的一个应用程序性能管理 (APM) 服务(类似Skywalking)。
使用它可以监视实时 Web 应用程序，自动检测性能异常。 
其中包含强大的分析工具来帮助诊断问题，了解用户在应用中实际执行了哪些操作。
同时也对js/java/python等之类非微软家的产品它也提供支持。
```
- 支持语言
  - ASP.NET、Java、Node.js、Python、ASP.NET Core
- 功能
  - 收集描述应用程序活动与运行状况的指标
    - 实时指标：实时观察部署的应用程序的活动，而不影响主机环境。
  - 收集描述应用程序的遥测数据 
    - 可用性：也称为综合事务监视。 探测应用程序的外部终结点，以测试一段时间内的整体可用性和响应能力。
    - 使用情况：了解哪些功能受用户欢迎，以及用户如何与应用程序交互和使用应用程序。
    - 智能检测：通过主动遥测分析来自动检测故障和异常。
  - 收集并存储应用程序跟踪日志记录数据
### 3.2. 代码开启program.cs
```C#
var builder = WebApplication.CreateBuilder(args);

//enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.Configure<TelemetryConfiguration>(x =>
    x.DisableTelemetry = bool.TryParse(builder.Configuration["ApplicationInsights:DisableTelemetry"], out bool disableTelemetry) && disableTelemetry
);

```

-  $\color{#dd0000} {方法内部实现请参考阅读官方文档[Application Insights 概述]}$
  
### 3.3. 配置文件appsettings.json
```json
{
  "ApplicationInsights": {
    "DisableTelemetry": true,//true表示禁用
    "ConnectionString": "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://xxx.applicationinsights.azure.com/;LiveEndpoint=https://xxx.livediagnostics.monitor.azure.com/"
  }
}

```
### 3.4.效果图
![效果图1](resource/image/applicationInsights_01.png)
![效果图2](resource/image/applicationInsights_02.png)
![效果图3](resource/image/applicationInsights_03.png)
</details>

## 四、CRMClients
### 4.1. 功能描述
- CRMClients可以看作是仓储层，封装了操作数据库Dataverse的一些常用方法
#### CRMClients操作Dataverse的2中方式
- 方式1：IODataHttpClient
  - 底层实现：http请求
  - 配置文件
  ```json
  "Crm": {
      "Default": {
        "resourceUrl": "https://vaporessodev.api.crm5.dynamics.com",
        "clientId": "417b6275-c68d-4f9d-9f6f-45fa0e7de97a",
        "clientSecret": "mac8Q~9-9dRnNEfi22IATM1G6PMrLMuxIZw.wbT8",
        "tenantId": "3277c91b-811a-401a-b1a1-b769a05aefa7",
        "tokenUrl": "login.windows.net"
      }
    }
  ```
- 方式2：TransactionServiceClient
  - 底层实现：基于连接(wcf方式)，支持事务
  - 配置文件
  ```json
  "Crm": {
      "Default": {
        "connectionString": "connectionString"
      }
    }
  ```
#### 使用方式
- 见 4.4.3.Demo：单次请求指向单一服务器
- 见 4.5.1 Demo：在事务中CRUD
### 4.2. 代码开启program.cs
```C#
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCRMClients(builder.Configuration);
```
-  $\color{#dd0000} {AddCRMClients方法是自定义实现的，具体实现请参阅源代码}$
  
### 4.3. 配置文件appsettings.json
- 见 4.4.2.配置文件appsetting.json (CRM多环境接口路由第4.2点)


### 4.4、CRM多环境接口路由
<details> 
    <summary>展开设计</summary>

```
多环境路由：将不同的请求的数据读/写至不同的Dataverse
```
#### 4.4.1.统一标记说明

Default=默认，HK=香港，US=北美

#### 4.4.2.配置文件appsetting.json

```json
"Crm": {
    "Default": {
      "resourceUrl": "https://vaporessosg.api.crm5.dynamics.com",
      "clientId": "417b6275-c68d-4f9d-9f6f-45fa0e7de97a",
      "clientSecret": "mac8Q~9-9dRnNEfi22IATM1G6PMrLMuxIZw.wbT8",
      "tenantId": "3277c91b-811a-401a-b1a1-b769a05aefa7",
      "connectionString": "connectionString",
      "tokenUrl": "login.windows.net"
    },
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
    }
  }
```

#### 4.4.3.Demo：单次请求指向单一服务器

- ##### http header添加标记

```javascript
//指向香港服务器
Target-CRM-Service:HK
```

- ##### service使用

```C#
//a.继承BaseService
//b.使用_oDataHttpClient操作Dataverse
//c.使用TransactionServiceClient操作Dataverse
public class DemoService : BaseService, IDemoService
{
    public DemoService(ICommonInjectionObject commonInjectionObject) : base(commonInjectionObject)
    {
    }
  public async Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter)
  {
      string fetchXml = $@"
          <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
            <entity name='systemuser'>
              <all-attributes />
              <filter>
                <condition attribute='employeeid' operator='eq' value='S1121072' />
              </filter>
            </entity>
          </fetch>";
      //IODataHttpClient fetchXml查询
      var res1 = await _oDataHttpClient.QueryRecords<Systemuser>(fetchXml);
      
      //TransactionServiceClient fetchXml查询
      var tranSvcClient = TransactionServiceClient;
      EntityCollection entColl = tranSvcClient.RetrieveMultiple(new FetchExpression(fetchXml2));
      var res2 = entColl.Entities.ToModelList<Link_Apv_Message>();

      //TransactionServiceClient 单表基本查询
      var users3 = tranSvcClient.Retrieve("systemuser", user.systemuserid.Value, new ColumnSet(true));
      var res3 = users3.ToModel<Systemuser>();   

      return new List<Systemuser>();
  }
}
```
</details>

### 4.5、事务（TransactionServiceClient）

<details> 
    <summary>展开设计</summary>

- 核心代码：using (var tranSvcClient = TransactionServiceClient){}
- 提交事务，如果需要读取事务提交结果，则使用“var tranResponse=tranSvcClient.CommitTransaction();”显示提交，并获取结果
- 如不关心提交结果，则忽略此行代码，代码会自动提交
- TransactionServiceClient默认支功能“持多环境接口路由”

- #### 4.5.1 Demo：在事务中CRUD
  ```C#

  public class DemoService : BaseService, IDemoService
  {
    public DemoService(ICommonInjectionObject commonInjectionObject) : base(commonInjectionObject)
    {

    }

    public async Task<List<Systemuser>> GetBusinessunit(XxxRequestDto parameter)
    {
      
      Apv_Message message = new()
      {
          apv_name = $"schtest_可删除_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
          apv_content = "",
          apv_messgtype = EnumMessgtype.PLM系统通知,
          ownerid = new CRM.Share.CRMClients.OData.Models.EntityReference(user.systemuserid),
          apv_uniquemarkcode = DateTime.Now.ToString("yyyyMMddHHmmss")
      };

      //事务批量创建、更新，需将提交逻辑放在using语句块里面
      using (var tranSvcClient = TransactionServiceClient)
      {
          //fetchXml inner join查询
          string fetchXml2 = $@"
                              <fetch xmlns:generator='MarkMpn.SQL4CDS' top='1'>
                                <entity name='apv_message'>
                                  <attribute name='apv_messageid' />
                                  <link-entity name='systemuser' to='ownerid' from='systemuserid' alias='link_owner' link-type='inner'>
                                    <all-attributes />
                                    <filter>
                                      <condition attribute='systemuserid' operator='eq' value='573e9425-9deb-ed11-8849-6045bd20a09e' />
                                    </filter>
                                  </link-entity>
                                </entity>
                              </fetch>
                              ";
          FetchExpression query = new(fetchXml2);
          EntityCollection results = tranSvcClient.RetrieveMultiple(query);
          var userList = results.Entities.ToModelList<Link_Apv_Message>();
          var link_owner = userList[0].link_owner;

          //查询单条记录
          var userxx = tranSvcClient.Retrieve("systemuser", user.systemuserid.Value, new ColumnSet(true));
          var user = userxx.ToModel<Systemuser>();

          //事务内创建
          tranSvcClient.CreateInTransaction(message);
          message.apv_content = "list";
          tranSvcClient.CreateInTransaction(new List<Apv_Message> { message });

          //事务内更新
          tranSvcClient.UpdateInTransaction(user);
          user.address1_name = "schtest02";
          tranSvcClient.UpdateInTransaction(new List<Systemuser> { user });

          //事务内删除
          tranSvcClient.DeleteInTransaction("apv_message", new Guid("6e83ccae-3560-ee11-8df0-000d3aa08d08"));

          //提交事务，不写时，会自动提交
          //如果需要读取并使用事务提交结果，则使用此行代码显示提交，并获取结果
          //如不关心提交结果，则忽略此行代码
          //var tranResponse = tranSvcClient.CommitTransaction();
      };

      //查询自定义接口
      //object paramList = new object();
      //_oDataHttpClient.Execute<Systemuser>("api名称", paramList);


      return new List<Systemuser>();
    }
  }
  ``` 
</details>

## 五、依赖注入

#### 5.1 功能描述
- PwC.CRM.Service层中，所有需要自动依赖注入的接口对象，都必须实现接口 IDependency
- 注入对象的生命周期全是：Scoped

### 5.1 代码开启program.cs
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoDependency("PwC.CRM.Service");
```
-  $\color{#dd0000} {AddAutoDependency方法是自定义实现的，具体实现请参阅源代码}$

## 六、api接口入参模型校验

#### 6.1 功能描述
- 用于请求路由至api控制器前做接口的请求参数校验，包括参数类型、参数必填、参数长度

### 6.1 代码开启program.cs
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddModelStateVrify();
```
-  $\color{#dd0000} {AddModelStateVrify方法是自定义实现的，具体实现请参阅源代码}$
## 七、api跨域支持

### 7.1 代码开启program.cs
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("CorsPolicy");
```
## 八、Swagger文档

#### 8.1 功能描述
- 支持Knife4UI（校Swagger更直观第展示接口信息）
- 支持在线调试
- 支持接口分组
- 程序启动时默认展示Knife4UI的主页

### 8.2 代码开启program.cs
```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerDoc(builder.Configuration);


var app = builder.Build();
app.UseSwaggerUi(builder.Configuration, app.Environment);
```
-  $\color{#dd0000} {AddSwaggerDoc方法和UseSwaggerUi方法是自定义实现的，具体实现请参阅源代码}$

### 8.3 接口分组

- 需在控制器上添加特性:ApiGroup

```C#
/// <summary>
/// Demo
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
[ApiGroup(ApiGroupNames.Common)]
public class DemoController : BaseController<DemoController>
{
}
```
- ApiGroupNames 可以根据项目需求自定义

### 8.4 功能截图

![swagger](resource/image/swagger01.png)
![swagger02](resource/image/swagger02.png)


## 九、自定义HttpClient（程序内发起http请求）

<details> 
    <summary>展开设计</summary>

#### 9.1 功能描述
- HttpClient用于在程序内向外部系统发起http请求
- 微软在.NET Core 2.1 开始引入的 HttpClientFactory 解决了HttpClient的所有痛点，包括HttpClient不能立即关闭连接、性能消耗严重等
- HttpClientFactory解决了高并发情况下HttpClient频繁Dispose时，内存消耗严重的问题

### 9.2 .net6中HttpClient的3种使用方式
#### 9.2.1、直接使用方式
```C#
builder.Services.AddHttpClient();

public class ValuesController:BaseController
{
  private readonly IHttpClientFactory _httpClientFactory;
  public ValuesController(IHttpClientFactory httpClientFactory)
  {
      _httpClientFactory = httpClientFactory;
  }
  [HttpGet]
  public async Task<ActionResult> Get()
  {
      var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri("http://www.xxx.com");
      string result = await client.GetStringAsync("/");
      return Ok(result);
  }
}
```

#### 9.2.2、命名式使用方式
```C#
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("github",
    c => {
        c.BaseAddress = new Uri("https://api.github.com/"); 
        c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); 
        c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
    }
);
```
```C#
public class ValuesController : BaseController 
{ 
    private readonly IHttpClientFactory _httpClientFactory; 
    public ValuesController(IHttpClientFactory httpClientFactory) 
    { 
        _httpClientFactory = httpClientFactory;
    }
    [HttpGet] 
    public async Task<ActionResult> Get() 
    { 
        var client = _httpClientFactory.CreateClient("github"); 
        string result = await client.GetStringAsync("/"); 
        return Ok(result);
    }
}
```
#### 9.2.3、类型化使用方式
  - 最简洁的使用方式，当前框架使用此种方式
  - 请求开始前会将Request.Body写入日志
  - 请求结束后会将Response.Body写入日志
  
  - a、代码开启program.cs
    ```C#
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCustomerHttpClient(builder.Configuration);
    ```
  - b、构造器获取注入的 HttpClient
    ```C#
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DemoController : BaseController<DemoController>
    {
        private readonly ILogger<DemoController> _logger;
        private readonly SRDMHttpClient _sRDMHttpClient;

        public DemoController(ILogger<DemoController> logger, SRDMHttpClient sRDMHttpClient) : base(logger)
        {
            _logger = logger;
            _sRDMHttpClient = sRDMHttpClient;
        }

        /// <summary>
        /// 获取Xxx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetXxxs([FromBody] XxxRequestDto parameter)
        {
            var response = await _sRDMHttpClient.SendAsync<CommonResponseDto>("getxxx", new
            {
                param1 = "",
                param2 = ""
            });
            _logger.LogInformation("日志内容");
            return Ok(res);
        }

    }
    ```
  - c、AddCustomerHttpClient的实现代码
  ```C#
  public static void AddCustomerHttpClient(this IServiceCollection services, IConfiguration configuration)
  {
      services.AddHttpClient<SRDMHttpClient>(httpClient =>
          {
              httpClient.BaseAddress = new Uri(configuration["ExternalApiUrl:SRDM:Url"]);
          })
          .AddHttpMessageHandler(provider =>
          {
              return new LogHttpMessageHandler<SRDMHttpClient>(provider);
          })
          .SetHandlerLifetime(TimeSpan.FromMinutes(10));
  }
  ```
  - d、SRDMHttpClient的实现代码
    ```C#
    /// <summary>
    /// SRDM的HttpClient
    /// </summary>
    public class SRDMHttpClient : BaseHttpClient, IHttpClient
    {
        public SRDMHttpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// 给SRDM发送Http请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<CommonResponseDto<T>> SendAsync(string url, object parameters)
        {
            var header = new Dictionary<string, string>();
            header["Content-Type"] = "application/json";
            var result = await PostAsync<T>(url, parameters, header);
            return result;
        }
    }
    ```

</details>   


## 十、系统日志

### 10.1功能描述

<details> 
    <summary>展开设计</summary>
目前只支持日志两种持久化方式，文件和MySql

  - dubug及以上的日志级别才会写日志
  - 每条请求进入后会自动写3种日志：
    - 请求的body内容（Request.Body）
    - 本次请求所用时长，精确到毫秒
    - 响应body内容（Response.Body）
  - 忽略日志时，只需在controller或者action上加上特性 SerilogIgnore
    - 忽略后仍然会写一下两种日志
      - 本次请求所用时长，精确到毫秒
      - 错误日志
  
    ```C#
    [ApiController]
    [SerilogIgnore]
    public class DemoController : BaseController<DemoController>
    {
        [HttpPost]
        public async Task<IActionResult> GetXxxs(XxxRequestDto parameter)
        {
            return Ok(res);
        }

    }
    ```

  - 文件日志
    - 默认情况下将日志写入文件
    - 文件名按天滚动，文件夹名称为日期，
    - 单个文件最大10M，超过10M后另起一个新文件，如20230812.log,20230812_001.log
  - MySql日志
    - 需要手动开启，开启方式见10.2配置文件
    - 程序启动自动生成日志表，日志表的前缀可配置,规则："logs_"+环境，如dev环境=logs_dev
    - 日志表按照周数分表，如logs_dev_36（dev环境第36周的日志表）
  - 其他持久化方式
    - 如SqlServer，MongoDB， Elasticsearch
    - 尚未实现，可扩展
</details> 

### 10.2、代码
<details> 
    <summary>展开设计</summary>

#### a、数据库脚本
```sql
-- mysql
create database if not exists `pwc_crm_logs` default character set utf8mb4 collate utf8mb4_unicode_ci;
```
#### b、开启代码

* ##### program.cs
  
  ```C#
  using PwC.Crm.Service.Share.Log; 
  using PwC.Crm.Service.Share.Log.Interface;
  using PwC.Crm.Share.Log.Serilogs; 
  
  var builder = WebApplication.CreateBuilder(args); 
  //a.添加日志策略 
  builder.Host.AddLogStrategy(builder.Logging, builder.Services, builder.Configuration); 
  
  var app = builder.Build(); 
  //b.添加日志处理管道 
  app.UseLog(app.Environment);
  ```
-  $\color{#dd0000} {AddLogStrategy方法和UseLog方法是自定义实现的，具体实现请参阅源代码}$
  
* ##### 配置文件appsettings.json
  
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

#### c、代码使用

* 控制器使用
  
  ```C#
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
  
  ```C#
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
  
  ```
  
</details> 

### 10.3、查询
<details> 
    <summary>展开设计</summary>

  - 目前查询只支持mysql
  * #### 查询条件
    - mysql
      - 查询条件的TimeStart和TimeEnd可以跨周，支持跨周查询日志
      - 查询条件的Limit只能在0~50之间，每周可查50条，超出无效
    - 至需要查询属于某个请求的日志，请出入查询参数HttpRequestId
    - 查询接口按照日志的写入时间倒序排序

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

* #### 查询数据库日志
  
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
| HttpRequestId | string | 精确匹配 | 否    | d711873d3d9a423f8d009ba4c5c4b0d8 <br>(GUID去掉中间的“-”) | 请求唯一id                                                                            |
| SourceContext | string | 模糊查询     | 否    | Middlewares.SerilogMiddleware                       | 写日志的触发类的全名                                                                        |
| Level         | int    | 精确匹配 | 否    | 2                                                   | 日志级别，为空不过滤此条件, <br> 0=Verbose,1=Debug,2=Information,<br>3=Warning,4=Error,5=Fatal |
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
     "Data": 200 
   }
  ```
  
  
</details> 

### 10.4 html+JS查询数据库日志
#### 10.4.1 功能说明
- 任何人可在D365页面上查询某个接口的请求日志
- 资源文件目录：/resource/log_query_d365/
  - html/ShowLogs.html
  - css/showlog.css
  - css/bootstrap.min.css
  - css/bootstrap-datetimepicker.min.css
  - js/jquery-3.7.0.min.js
  - js/GlobalUtil.js
  - js/Tools.js
  - js/CallCrmApi.js
  - js/showlog.js
  - js/bootstrap-datetimepicker.min.js

#### 10.4.2 截图

![查询接口日志](resource/image/D365_querylog.png)

## 十一、部署

<details> 
    <summary>展开设计</summary>

### 11.1 部署架构
- window服务器简单部署
- 微服务部署
  
![微服务部署架构图](resource/architecture/deploy_microservice.png)

### 11.2 IIS部署
- 根据实际情况更改ASPNETCORE_ENVIRONMENT的值，如：PRD表示生产环境，会自动去读appsettings.PRD.json配置文件的内容
- web.config
  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <location path="." inheritInChildApplications="false">
      <system.webServer>
        <handlers>
          <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
        </handlers>
        <aspNetCore processPath="dotnet" arguments=".\APV.CRM.Service.Api.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess">
          <environmentVariables>
            <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="PRD" />
          </environmentVariables>
        </aspNetCore>
        <serverRuntime uploadReadAheadSize="2147483647" />
      </system.webServer>
      <system.web>
        <httpRuntime maxRequestLength="2147483647" useFullyQualifiedRedirectUrl="true" executionTimeout="3600" />
      </system.web>
      <system.webServer>
        <security>
          <requestFiltering>
            <requestLimits maxAllowedContentLength="2147483647" />
          </requestFiltering>
        </security>
      </system.webServer>
    </location>
  </configuration>
  ```
### 11.3 Windows Servers 部署
- 发布和IIS一样，但无需web.config文件，且需要在program.cs中支持kerstrel，kerstrel有如下实现方式
  - 编码方式 
    ```C#
    webBuilder.UseKestrel(options =>
    {
        // TCP 8001
        options.ListenLocalhost(8001, builder =>
        {
            builder.UseConnectionHandler<MyEchoConnectionHandler>();
        });

        // HTTP 5000
        options.ListenLocalhost(5000);

        // HTTPS 5001
        options.ListenLocalhost(5001, builder =>
        {
            builder.UseHttps();
        });
    });
    ```
  - UseUrls方式
    ```C#
    webBuilder.UseUrls("http://localhost:5000;http://localhost:5001")
    ```
  - 配置文件方式
    ```C#
    {
      "urls": "http://*:5000;"
    }
    ```
- 设置windows服务开机启动
  
### 11.4 Docker部署
- 需要linux服务器
- 需要在program.cs中支持kerstrel（同10.2.Windows Servers 部署）
- 制作基础镜像（hub.pwc.cn/dotnet/aspnet:6.0, .net技术栈公用）
- 添加docker file,继承基础镜像，将环境变量ASPNETCORE_ENVIRONMENT传入dot net run 命令行的environment参数，并推送至代码仓库。docker file 如下：
  
  ```Shell
  FROM hub.pwc.cn/dotnet/aspnet:6.0 AS base
  WORKDIR /app
  EXPOSE 80
  FROM hub.pwc.cn/dotnet/sdk:6.0 AS build
  #ARG BUILD_ENV
  WORKDIR /src
  COPY . .
  RUN dotnet tool install --tool-path /tools dotnet-dump
  RUN dotnet restore "PwC.CRM.Api/PwC.CRM.Api.csproj" -s http://nexus.pwc.cn/repository/nuget-group/index.json
  WORKDIR "/src/PwC.CRM.Api"
  #RUN dotnet build "PwC.CRM.Api.csproj" -c ${BUILD_ENV:-Debug} -o /app/build
  FROM build AS publish
  ARG BUILD_ENV
  RUN dotnet publish "PwC.CRM.Api.csproj" -c ${BUILD_ENV:-Debug} --no-restore -o /app/publish
  FROM base AS final
  WORKDIR /tools
  COPY --from=build /tools .
  WORKDIR /app
  COPY --from=publish /app/publish .
  #ENTRYPOINT dotnet PwC.CRM.Api.dll --environment=${BUILD_ENV:-Debug} --urls=http://*:${SERVER_PORT:-80} --ip=${SERVER_IP} --port=${SERVER_PORT:-80}/
  ```
- 发布服务器去代码仓库拉去源代码，执行docker file，生成程序镜像，并推送至镜像仓库
- 部署节点服务器拉去程序镜像，运行镜像。（docke run img，或者用docker-compose.yml，或者用K8S容器编排）

</details>