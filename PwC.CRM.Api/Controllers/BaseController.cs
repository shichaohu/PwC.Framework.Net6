﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PwC.CRM.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly ILogger Logger;
        /// <summary>
        /// 此处用来兼容Azure上偶发性丢失日志的问题
        /// </summary>
        /// <param name="logger"></param>
        public BaseController(ILogger<T> logger)
        {
            Logger = logger;

        }
        protected IConfiguration _configuration;
        protected IConfiguration Configuration => _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();

        #region private

        #endregion

    }

}
