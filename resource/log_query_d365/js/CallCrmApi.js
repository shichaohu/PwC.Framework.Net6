/**
 * 前端JS调用api
 */
"use strict";
var pwc = window.pwc || {};
(function () {
  let self = pwc;

  // 获取配置
  this.getConfig = function () {
    let xml = `
        <fetch xmlns:generator='MarkMpn.SQL4CDS'>
  <entity name='environmentvariablevalue'>
    <attribute name='value' />
    <link-entity name='environmentvariabledefinition' to='environmentvariabledefinitionid' from='environmentvariabledefinitionid' alias='d' link-type='inner'>
      <attribute name='schemaname' />
      <filter>
        <condition attribute='schemaname' operator='in'>
          <value>pwc_ApiBaseUrl</value>
          <value>apv_ImportUrl</value>
          <value>apv_ApiUser</value>
          <value>apv_TokenUrl</value>
        </condition>
      </filter>
    </link-entity>
    <filter>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
  </entity>
</fetch>`;
    let enValues = tool.get("environmentvariablevalue", xml);
    let config = {};
    for (let item of enValues) {
      let schemaname = item["d.schemaname"];
      switch (schemaname) {
        case "pwc_ApiBaseUrl":
          {
            config.baseUrl = item.value;
          }
          break;
        case "apv_ImportUrl":
          {
            config.importApiUrl = item.value;
          }
          break;
        case "apv_ApiUser":
          {
            config.apiUser = item.value;
          }
          break;
        case "apv_TokenUrl":
          {
            config.tokenUrl = item.value;
          }
          break;
      }
    }
    return config;
  };

  // 获取请求认证头
  this.getToken = function (config) {
    return new Promise(async (resolve, reject) => {
      let data = self.getApiUser(config.apiUser);
      if (!data) {
        let err = `failed to get api user named [${config.apiUser}]`;
        console.error(err);
        reject(err);
      }

      let response = await fetch(`${config.baseUrl}${config.tokenUrl}`, {
        method: "POST",
        headers: { "content-type": "application/json" },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        let json = await response.json();
        if (json.error) {            
          reject(json.error_description);
        } else {
            resolve(json);
        }
      } else {
        reject(`response error,${response.statusText}`);
      }
    });
  };

  // 获取接口用户
  this.getApiUser = function (userName) {
    let xml = `<fetch xmlns:generator='MarkMpn.SQL4CDS'>
  <entity name='pwc_apiuser'>
    <attribute name='pwc_name' />
    <attribute name='pwc_clientid' />
    <attribute name='pwc_clientsecret' />
    <attribute name='pwc_scope' />
    <filter>
      <condition attribute='pwc_name' operator='eq' value='${userName}' />
    </filter>
  </entity>
</fetch>`;
    let user = tool.getOne("pwc_apiuser", xml);
    if (!user) return null;

    return {
      client_id: user.pwc_clientid,
      client_secret: user.pwc_clientsecret,
      scope: user.pwc_scope,
      userID: user.pwc_name,
    };
  };

  /**
   * 调用接口，发送Post请求
   *
   * @param {String} url 接口地址，相对路径
   * @param {Object} data 接口参数
   *
   * @returns
   */
  this.post = function (url, data) {
    debugger;
    return self.call("POST", url, data);
  };

  /**
   * 调用接口，发送Post请求
   *
   * @param {String} url 接口地址，相对路径
   *
   * @returns
   */
  this.get = function (url) {
    debugger;
    return self.call("GET", url);
  };
 
  // 发起http请求
  this.call = function (method, url, data) {
    return new Promise(async (resolve, reject) => {
      let config = self.getConfig();
      if (!config) {
        let err = "failed to get environmentvariable value";
        console.error(err);
        reject(err);
      }

      let token = await self.getToken(config);
      if (!token) {
        let err = "failed to get token";
        console.error(err);
        reject(err);
      }

      let option = {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token.access_token}`,
        },
      };

      if (method == "GET") {
        option.method = "GET";
      } else {
        option.method = "POST";
        option.body = JSON.stringify(data);
      }

      let response = await fetch(`${config.baseUrl}${url}`, option);
      if (response.ok) {
        let json = await response.json();
        if (json.Code == 200) {
          resolve(json);
        } else {
          reject(json.Message);
        }
      } else {
        reject(`response error,${response.statusText}`);
      }
    });
  };
}).call(pwc);
