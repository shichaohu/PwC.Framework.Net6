# XxlJob分布式任务调度平台
- [XxlJob分布式任务调度平台](#xxljob分布式任务调度平台)
  - [一、简介](#一简介)
    - [1.1概述](#11概述)
    - [1.2特性](#12特性)
    - [1.3 环境](#13-环境)
    - [1.4 源码结构](#14-源码结构)
  - [二、功能使用](#二功能使用)
    - [2.1初始化“调度数据库”](#21初始化调度数据库)
    - [2.2配置部署“调度中心”](#22配置部署调度中心)
      - [2.2.1调度中心配置：](#221调度中心配置)
      - [2.2.2调度中心部署](#222调度中心部署)
    - [2.3 .net6执行器](#23-net6执行器)
      - [Program.cs](#programcs)
      - [appsetting.json](#appsettingjson)

## 一、简介
### 1.1概述
- XXL-JOB是一个分布式任务调度平台，开发迅速、轻量级、易扩展、开箱即用
- [官方文档](https://www.xuxueli.com/xxl-job/)
- 源码仓库地址	
    - https://github.com/xuxueli/xxl-job
    - http://gitee.com/xuxueli0323/xxl-job
### 1.2特性
- **动态**：支持动态修改任务状态、启动/停止任务，以及终止运行中任务，即时生效
- **调度中心HA（中心式）**：调度采用中心式设计，“调度中心”自研调度组件并支持集群部署，可保证调度中心HA；
- **执行器HA（分布式）**：任务分布式执行，任务”执行器”支持集群部署，可保证任务执行HA；
- **注册中心**: 执行器会周期性自动注册任务, 调度中心将会自动发现注册的任务并触发执行。同时，也支持手动录入执行器地址；
- **弹性扩容缩容**：一旦有新执行器机器上线或者下线，下次调度时将会重新分配任务；
- **触发策略**：提供丰富的任务触发策略，包括：Cron触发、固定间隔触发、固定延时触发、API（事件）触发、人工触发、父子任务触发；
- **调度过期策略**：调度中心错过调度时间的补偿处理策略，包括：忽略、立即补偿触发一次等；
- **阻塞处理策略**：调度过于密集执行器来不及处理时的处理策略，策略包括：单机串行（默认）、丢弃后续调度、覆盖之前调度；
- **任务失败重试**：支持自定义任务失败重试次数，当任务失败时将会按照预设的失败重试次数主动进行重试；其中分片任务支持分片粒度的失败重试；
- **任务失败告警**；默认提供邮件方式失败告警，同时预留扩展接口，可方便的扩展短信、钉钉等告警方式；
- **路由策略**：执行器集群部署时提供丰富的路由策略，包括：第一个、最后一个、轮询、随机、一致性HASH、最不经常使用、最近最久未使用、故障转移、忙碌转移等；
### 1.3 环境
- Maven3+
- Jdk1.8+
- Mysql5.7+
### 1.4 源码结构
- **xxl-job-admin：调度中心（单独部署）**
- xxl-job-core：公共依赖
- **xxl-job-executor-samples：执行器Sample示例（.net6基础框架中已实现.net6版的执行器）**
    - xxl-job-executor-sample-springboot：Springboot版本；
    - xxl-job-executor-sample-frameless：无框架版本；


## 二、功能使用
### 2.1初始化“调度数据库”
请下载项目源码并解压，获取 “调度数据库初始化SQL脚本” 并执行即可。
“调度数据库初始化SQL脚本” 位置为:
```
/xxl-job/doc/db/tables_xxl_job.sql
```
调度中心支持集群部署，集群情况下各节点务必连接同一个mysql实例;
如果mysql做主从,调度中心集群节点务必强制走主库;
### 2.2配置部署“调度中心”
- 调度中心项目：xxl-job-admin
- 作用：统一管理任务调度平台上调度任务，负责触发调度执行，并且提供任务管理平台。

#### 2.2.1调度中心配置：
- 调度中心配置文件地址：
```
/xxl-job/xxl-job-admin/src/main/resources/application.properties
```
- 调度中心配置内容说明：
```java
### 调度中心JDBC链接：链接地址请保持和 2.1章节 所创建的调度数据库的地址一致
spring.datasource.url=jdbc:mysql://127.0.0.1:3306/xxl_job?useUnicode=true&characterEncoding=UTF-8&autoReconnect=true&serverTimezone=Asia/Shanghai
spring.datasource.username=root
spring.datasource.password=root_pwd
spring.datasource.driver-class-name=com.mysql.jdbc.Driver
### 报警邮箱
spring.mail.host=smtp.qq.com
spring.mail.port=25
spring.mail.username=xxx@qq.com
spring.mail.password=xxx
spring.mail.properties.mail.smtp.auth=true
spring.mail.properties.mail.smtp.starttls.enable=true
spring.mail.properties.mail.smtp.starttls.required=true
spring.mail.properties.mail.smtp.socketFactory.class=javax.net.ssl.SSLSocketFactory
### 调度中心通讯TOKEN [选填]：非空时启用；
xxl.job.accessToken=
### 调度中心国际化配置 [必填]： 默认为 "zh_CN"/中文简体, 可选范围为 "zh_CN"/中文简体, "zh_TC"/中文繁体 and "en"/英文；
xxl.job.i18n=zh_CN
## 调度线程池最大线程配置【必填】
xxl.job.triggerpool.fast.max=200
xxl.job.triggerpool.slow.max=100
### 调度中心日志表数据保存天数 [必填]：过期日志自动清理；限制大于等于7时生效，否则, 如-1，关闭自动清理功能；
xxl.job.logretentiondays=30
```

#### 2.2.2调度中心部署
- 如果已经正确进行上述配置，可将项目编译打包部署。
- 调度中心访问地址：http://localhost:8080/xxl-job-admin (该地址执行器将会使用到，作为回调地址)
- 默认登录账号 “admin/123456”
  

### 2.3 .net6执行器
#### Program.cs
  ```C#
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddXxlJob(builder.Configuration);

    var app = builder.Build();  
    app.UseXxlJobExecutor();
  ```
#### appsetting.json
  ```json
  {
    "xxlJob": {
        "adminAddresses": "http://localhost:8080/xxl-job-admin",//调度中心
        "appName": "xxl-job-executor-dotnet",//.net6执行器名称
        "specialBindAddress": "http://localhost",//.net6执行器部署后的ip
        "port": 7018,//.net6执行器部署后的端口
        "autoRegistry": true,
        "accessToken": "",
        "logRetentionDays": 30
    }
  }
  ```