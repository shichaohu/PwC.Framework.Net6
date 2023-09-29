using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PwC.Crm.Share.CRMClients.OData.Models;
using PwC.Crm.Share.Util;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;

namespace PwC.Crm.Share.CRMClients.OData
{
    /// <summary>
    ///  CRM Client
    /// </summary>
    public class ODataHttpClient : IODataHttpClient, IDisposable
    {
        private struct Token
        {
            public AuthenticationResponse authentication;

            public DateTime expires_on;

            public DateTime not_before;

            public string errorDescription;
        }

        private string _resourceUrl = "";

        private string _clientId = "";

        private string _clientSecret = "";

        private string _tenantId = "";

        private string _tokenUrl = "";

        private static Type _decimalType = typeof(decimal?);

        private static Type _doubleType = typeof(double?);

        private static Type _intType = typeof(int?);

        private static Type _guidType = typeof(Guid?);

        private static Type _longType = typeof(long?);

        private static Type _stringType = typeof(string);

        private static Type _dateTimeType = typeof(DateTime?);

        private static Type _entityReferenceType = typeof(EntityReference);

        private static Type _boolType = typeof(bool?);

        private HttpClient _tokenHttpClient;

        private HttpClient _crmDataPost;

        private Token _tokenCaChe;

        public ODataHttpClient(string resourceUrl, string clientId, string clientSecret, string tenantId, string tokenUrl, string MSCRMCallerID = "")
        {
            init(resourceUrl, clientId, clientSecret, tenantId, tokenUrl, MSCRMCallerID);
        }

        private void init(string resourceUrl, string clientId, string clientSecret, string tenantId, string tokenUrl, string MSCRMCallerID = "")
        {
            _resourceUrl = resourceUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tenantId = tenantId;
            _tokenUrl = "https://" + tokenUrl + "/" + tenantId + "/oauth2/token";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            _tokenHttpClient = new HttpClient();
            _tokenHttpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            _crmDataPost = new HttpClient();
            _crmDataPost.BaseAddress = new Uri(resourceUrl);
            _crmDataPost.Timeout = new TimeSpan(0, 5, 0);
            _crmDataPost.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _crmDataPost.DefaultRequestHeaders.Add("OData-Version", "4.0");
            if (!string.IsNullOrEmpty(MSCRMCallerID))
            {
                _crmDataPost.DefaultRequestHeaders.Add("MSCRMCallerID", MSCRMCallerID);
            }
        }

        #region Create
        public async Task<CrmResponse> CreateRecord<T>(string entityName, T entity, CrmParameter crmParameter = null)
        {
            return await CreateRecordFun(entityName, entity);
        }
        public async Task<CrmResponse> CreateRecord<T>(T entity, CrmParameter crmParameter = null)
        {
            return await CreateRecordFun("", entity);
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(string fetchXml, bool include)
        {
            return await QueryRecords<T>(fetchXml, new CrmParameter
            {
                include = include
            });
        }
        private async Task<CrmResponse> CreateRecordFun<T>(string entityName, T entity, CrmParameter crmParameter = null)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                entityName = GetEntityName<T>();
            }

            CrmResponse result = new();
            try
            {
                string objectToString = GetObjectToString(entityName, entity, isBatch: false);
                await PostData(CommonFun.GetPluralForm(entityName), objectToString, crmParameter, async delegate (HttpResponseMessage response)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string originalString = response.Headers.Location!.OriginalString;
                        result.Id = Guid.Parse(originalString.Substring(originalString.Length - 37, 36));
                        result.Code = ResultCode.Success;
                    }
                    else
                    {
                        string value = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(value))
                        {
                            result.Code = ResultCode.OtherError;
                            result.Message = response.ReasonPhrase;
                        }
                        else
                        {
                            result = JsonConvert.DeserializeObject<CrmResponse>(value);
                            object objError = ((dynamic)JsonConvert.DeserializeObject<object>(value)).error;
                            result.Code = ResultCode.OtherError;
                            result.Message = ((dynamic)objError)?.Message?.Value;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                result.Code = ResultCode.OtherError;
                result.Message = ex.Message;
                result.StackTrace = ex.StackTrace;
                result.Location = "Plug-in";
            }

            return result;
        }


        #endregion

        #region Query
        public async Task<CrmResponse<T>> QueryRecords<T>(string entityName, string fetchXml, bool include)
        {
            return await QueryRecords<T>(entityName, fetchXml, new CrmParameter
            {
                include = include
            });
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(string entityName, string fetchXml, CrmParameter crmParameter = null)
        {
            return await QueryRecords<T>(CommonFun.GetPluralForm(entityName) + "?fetchXml=" + HttpUtility.UrlEncode(fetchXml, Encoding.UTF8), crmParameter);
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(string entityName, Guid? Id, string fields, CrmParameter crmParameter = null)
        {
            return await QueryRecords<T>(entityName, Id, fields.Split(","), crmParameter);
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(Guid? Id, string fields, CrmParameter crmParameter = null)
        {
            return await QueryRecords<T>("", Id, fields.Split(","), crmParameter);
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(Guid? Id, string[] fieldList, CrmParameter crmParameter = null)
        {
            return await QueryRecords<T>("", Id, fieldList, crmParameter);
        }
        public async Task<CrmResponse<T>> QueryRecords<T>(string entityName, Guid? Id, string[] fieldList, CrmParameter crmParameter = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < fieldList.Length; i++)
            {
                stringBuilder.Append($" <attribute name='{fieldList[i]}' /> ");
            }

            Type typeFromHandle = typeof(T);
            if (string.IsNullOrWhiteSpace(entityName))
            {
                entityName = GetEntityName<T>();
            }

            MethodInfo methodGetIsActivity = typeFromHandle.GetMethod("GetIsActivity");
            string text = entityName + "id";
            if (methodGetIsActivity != null && (bool)methodGetIsActivity.Invoke(null, null))
            {
                text = "activityid";
            }

            string str = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>
                                <entity name='{entityName}'>
                                    {stringBuilder}
                                    <filter type='and'><condition attribute='{text}' operator='eq' value='{Id}' /></filter>
                                </entity>
                            </fetch>";
            if (crmParameter == null)
            {
                crmParameter = new CrmParameter
                {
                    include = true
                };
            }
            else
            {
                crmParameter.include = true;
            }

            return await QueryRecords<T>(CommonFun.GetPluralForm(entityName) + "?fetchXml=" + HttpUtility.UrlEncode(str, Encoding.UTF8), crmParameter);
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(string fetchXml, CrmParameter crmParameter = null)
        {
            fetchXml = fetchXml?.TrimStart();
            Type typeT = typeof(T);
            if (fetchXml.StartsWith("<fetch"))
            {
                string entityName = GetEntityName<T>();
                fetchXml = CommonFun.GetPluralForm(entityName) + "?fetchXml=" + HttpUtility.UrlEncode(fetchXml, Encoding.UTF8);
            }

            if (crmParameter != null && !crmParameter.include)
            {
                MethodInfo methodGetIsActivity = typeT.GetMethod("GetIsActivity");
                if (methodGetIsActivity != null && (bool)methodGetIsActivity.Invoke(null, null))
                {
                    crmParameter.include = true;
                }
            }

            CrmResponse<string> crmResponse = await GetData(fetchXml, crmParameter);
            if (crmResponse.Code != ResultCode.Success)
            {
                return new CrmResponse<T>
                {
                    Code = crmResponse.Code,
                    Message = crmResponse.Message,
                    Data = new List<T>()
                };
            }

            try
            {
                JArray jArray = ((dynamic)JsonConvert.DeserializeObject<object>(crmResponse.Data[0])).value;
                List<T> list = new List<T>();
                if (typeT.Name.StartsWith("Dictionary"))
                {
                    for (int i = 0; i < jArray.Count; i++)
                    {
                        JObject jObject = (JObject)jArray[i];
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        foreach (KeyValuePair<string, JToken> item in jObject)
                        {
                            if (item.Value != null)
                            {
                                if (item.Value.Type == JTokenType.Integer)
                                {
                                    dictionary.Add(item.Key, item.Value.Value<long>());
                                }
                                else if (item.Value.Type == JTokenType.String)
                                {
                                    dictionary.Add(item.Key, item.Value.Value<string>());
                                }
                                else if (item.Value.Type == JTokenType.Boolean)
                                {
                                    dictionary.Add(item.Key, (item.Value.Value<bool>() ? ((byte)1) : ((byte)0)) != 0);
                                }
                                else if (item.Value.Type == JTokenType.Float)
                                {
                                    dictionary.Add(item.Key, item.Value.Value<decimal>());
                                }
                                else if (item.Value.Type == JTokenType.Date)
                                {
                                    dictionary.Add(item.Key, item.Value.Value<DateTime>());
                                }
                                else if (item.Value.Type == JTokenType.Guid)
                                {
                                    dictionary.Add(item.Key, item.Value.Value<string>());
                                }
                                else
                                {
                                    dictionary.Add(item.Key, item.Value.Value<object>());
                                }
                            }
                            else
                            {
                                dictionary.Add(item.Key, null);
                            }
                        }

                        list.Add((T)(object)dictionary);
                    }

                    return new CrmResponse<T>
                    {
                        Data = list,
                        Code = ResultCode.Success
                    };
                }

                for (int j = 0; j < jArray.Count; j++)
                {
                    object model = Activator.CreateInstance(typeT);
                    JObject jObject = (JObject)jArray[j];
                    List<Tuple<string, string, JToken>> lookupList = new List<Tuple<string, string, JToken>>();
                    foreach (KeyValuePair<string, JToken> item in jObject)
                    {
                        if (!item.Key.Contains("@") && item.Key.Contains("."))
                        {
                            string[] array = item.Key.Split(".");
                            lookupList.Add(new Tuple<string, string, JToken>(array[0], array[1], item.Value));
                        }
                        else
                        {
                            SetObjctValue(item, jObject, typeT, ref model);
                        }
                    }

                    if (lookupList.Count > 0)
                    {
                        (from w in lookupList
                         group w by w.Item1 into w
                         select w.Key).ToList().ForEach(delegate (string g)
                         {
                             PropertyInfo likeType = typeT.GetProperty(g);
                             if (likeType != null)
                             {
                                 object attrObj = Activator.CreateInstance(likeType.PropertyType);
                                 List<Tuple<string, string, JToken>> list2 = lookupList.Where((Tuple<string, string, JToken> w) => w.Item1 == g).ToList();
                                 JObject attJob = new JObject();
                                 list2.ForEach(delegate (Tuple<string, string, JToken> att)
                                 {
                                     attJob.Add(att.Item2, att.Item3);
                                 });
                                 if (likeType.PropertyType.Name == "Object")
                                 {
                                     likeType.SetValue(model, attJob);
                                 }
                                 else
                                 {
                                     list2.ForEach(delegate (Tuple<string, string, JToken> att)
                                     {
                                         SetObjctValue(new KeyValuePair<string, JToken>(att.Item2, att.Item3), attJob, likeType.PropertyType, ref attrObj);
                                     });
                                     likeType.SetValue(model, attrObj);
                                 }
                             }
                         });
                    }

                    list.Add((T)model);
                }

                return new CrmResponse<T>
                {
                    Data = list,
                    Code = ResultCode.Success
                };
            }
            catch (Exception ex)
            {
                return new CrmResponse<T>
                {
                    Code = ResultCode.InternalError,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Location = "Plug-in"
                };
            }
        }

        private void SetObjctValue(KeyValuePair<string, JToken> kp, JObject currObject, Type objType, ref object obj)
        {
            if (kp.Key.StartsWith("_") && kp.Key.EndsWith("_value"))
            {
                string name = kp.Key.Substring(1, kp.Key.Length - 7);
                PropertyInfo property = objType.GetProperty(name);
                if (!(property != null) || !(property.PropertyType == _entityReferenceType))
                {
                    return;
                }

                EntityReference entityReference = new EntityReference();
                string text = kp.Value.Value<string>();
                if (text == null)
                {
                    return;
                }

                entityReference.Id = Guid.Parse(text);
                if (currObject[kp.Key + "@OData.Community.Display.V1.FormattedValue"] != null)
                {
                    entityReference.Name = currObject[kp.Key + "@OData.Community.Display.V1.FormattedValue"].Value<string>();
                }

                if (currObject[kp.Key + "@Microsoft.Dynamics.CRM.lookuplogicalname"] != null)
                {
                    entityReference.logicalName = currObject[kp.Key + "@Microsoft.Dynamics.CRM.lookuplogicalname"].Value<string>();
                }
                else
                {
                    object[] customAttributes = property.GetCustomAttributes(typeof(CFieldType), inherit: true);
                    if (customAttributes.Length != 0)
                    {
                        entityReference.logicalName = ((CFieldType[])customAttributes)[0].EntityName;
                    }
                }

                property.SetValue(obj, entityReference);
                return;
            }

            PropertyInfo property2 = objType.GetProperty(kp.Key);
            if (property2 == null || kp.Value.Type == JTokenType.Null)
            {
                return;
            }

            if (property2.PropertyType.IsEnum)
            {
                property2.SetValue(obj, Enum.Parse(property2.PropertyType, kp.Value.Value<string>()));
                return;
            }

            if (property2.PropertyType == _stringType)
            {
                property2.SetValue(obj, kp.Value.Value<string>());
                return;
            }

            if (property2.PropertyType == _decimalType)
            {
                property2.SetValue(obj, kp.Value.Value<decimal>());
                return;
            }

            if (property2.PropertyType == _doubleType)
            {
                property2.SetValue(obj, kp.Value.Value<double>());
                return;
            }

            if (property2.PropertyType == _intType)
            {
                property2.SetValue(obj, kp.Value.Value<int>());
                return;
            }

            if (property2.PropertyType == _longType)
            {
                property2.SetValue(obj, kp.Value.Value<long>());
                return;
            }

            if (property2.PropertyType == _dateTimeType)
            {
                property2.SetValue(obj, kp.Value.Value<DateTime>());
                return;
            }

            if (property2.PropertyType == _guidType)
            {
                property2.SetValue(obj, Guid.Parse(kp.Value.Value<string>()));
                return;
            }

            if (property2.PropertyType == _boolType)
            {
                property2.SetValue(obj, (kp.Value.Value<bool>() ? ((byte)1) : ((byte)0)) != 0);
                return;
            }

            object[] customAttributes2 = property2.GetCustomAttributes(typeof(CFieldType), inherit: true);
            if (customAttributes2.Length != 0 && ((CFieldType[])customAttributes2)[0].EnumType != null)
            {
                CFieldType eno = ((CFieldType[])customAttributes2)[0];
                if (eno.FieldType == "MultiSelectPicklistType")
                {
                    List<string> list = kp.Value.Value<string>()!.Split(",").ToList();
                    Type type = typeof(List<>).MakeGenericType(eno.EnumType);
                    dynamic ls = Activator.CreateInstance(type);
                    list.ForEach(delegate (string v)
                    {
                        ls.Add((dynamic)Enum.Parse(eno.EnumType, v));
                    });
                    property2.SetValue(obj, ls);
                }
                else
                {
                    property2.SetValue(obj, Enum.Parse(eno.EnumType, kp.Value.Value<string>()));
                }
            }
            else if (property2.PropertyType == _entityReferenceType)
            {
                EntityReference entityReference2 = new EntityReference();
                entityReference2.Id = Guid.Parse(kp.Value.Value<string>());
                if (currObject[kp.Key + "@OData.Community.Display.V1.FormattedValue"] != null)
                {
                    entityReference2.Name = currObject[kp.Key + "@OData.Community.Display.V1.FormattedValue"].Value<string>();
                }

                object[] customAttributes3 = property2.GetCustomAttributes(typeof(CFieldType), inherit: true);
                if (customAttributes3.Length != 0)
                {
                    entityReference2.logicalName = ((CFieldType[])customAttributes3)[0].EntityName;
                }

                property2.SetValue(obj, entityReference2);
            }
            else
            {
                objType.GetProperty(kp.Key)!.SetValue(obj, kp.Value.Value<object>());
            }
        }

        #endregion

        #region Update
        public async Task<CrmResponse> UpdateRecord<T>(string entityName, Guid? Id, T entity)
        {
            return await UpdateRecordFun(entityName, Id, entity);
        }

        public async Task<CrmResponse> UpdateRecord<T>(string entityName, Guid? Id, T entity, CrmParameter crmParameter)
        {
            return await UpdateRecordFun(entityName, Id, entity, crmParameter);
        }

        public async Task<CrmResponse> UpdateRecord<T>(Guid? Id, T entity, CrmParameter crmParameter)
        {
            return await UpdateRecordFun("", Id, entity, crmParameter);
        }

        public async Task<CrmResponse> UpdateRecord<T>(Guid? Id, T entity)
        {
            return await UpdateRecordFun("", Id, entity);
        }
        private async Task<CrmResponse> UpdateRecordFun<T>(string entityName, Guid? Id, T entity, CrmParameter crmParameter = null)
        {
            Type type = entity.GetType();
            if (string.IsNullOrEmpty(entityName))
            {
                entityName = GetEntityName<T>();
            }

            CrmResponse crmRet = new CrmResponse();
            try
            {
                string text = GetObjectToString(entityName, entity, isBatch: false);
                MethodInfo method = type.GetMethod("GetModifyList");
                if (method != null)
                {
                    Activator.CreateInstance(type);
                    HashSet<string> hashSet = (HashSet<string>)method.Invoke(entity, null);
                    if (hashSet.Count > 0)
                    {
                        JToken jToken = JsonConvert.DeserializeObject<JToken>(text);
                        List<string> list = hashSet.ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (jToken[list[i]] == null)
                            {
                                jToken[list[i]] = null;
                            }
                        }

                        text = JsonConvert.SerializeObject(jToken);
                    }
                }

                await UpdateData(CommonFun.GetPluralForm(entityName), Id, text, crmParameter, async delegate (HttpResponseMessage response)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string originalString = response.Headers.Location!.OriginalString;
                        crmRet.Id = Guid.Parse(originalString.Substring(originalString.Length - 37, 36));
                        crmRet.Code = ResultCode.Success;
                    }
                    else
                    {
                        string value = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(value))
                        {
                            crmRet.Code = ResultCode.OtherError;
                            crmRet.Message = response.ReasonPhrase;
                        }
                        else
                        {
                            object obj = ((dynamic)JsonConvert.DeserializeObject<object>(value)).error;
                            crmRet.Code = ResultCode.OtherError;
                            crmRet.Message = ((dynamic)obj)?.Message?.Value;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                crmRet.Code = ResultCode.OtherError;
                crmRet.Message = ex.Message;
                crmRet.StackTrace = ex.StackTrace;
                crmRet.Location = "Plug-in";
            }

            return crmRet;
        }

        #endregion

        #region Delete
        public async Task<CrmResponse> DeleteRecords(string entityName, Guid? Id)
        {
            return await DeleteRecordsFun(CommonFun.GetPluralForm(entityName), Id);
        }
        public async Task<CrmResponse> DeleteRecords(string entityName, Guid? Id, CrmParameter crmParameter)
        {
            return await DeleteRecordsFun(CommonFun.GetPluralForm(entityName), Id, crmParameter);
        }

        private async Task<CrmResponse> DeleteRecordsFun(string entityName, Guid? Id, CrmParameter crmParameter = null)
        {
            CrmResponse ret = new CrmResponse
            {
                Code = ResultCode.Success
            };
            Token token = await GetAuthenticationResponse();

            string requestUrl = $"api/data/v9.2/{entityName}({Id})";
            HttpRequestMessage request = new(HttpMethod.Delete, requestUrl);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("If-Match", "*");
            SetCrmParameter(crmParameter, request);
            try
            {
                HttpResponseMessage response = await _crmDataPost.SendAsync(request);
                request.Dispose();
                if (response.IsSuccessStatusCode)
                {
                    response.Dispose();
                    return ret;
                }

                string value = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    JToken jToken = JsonConvert.DeserializeObject<JToken>(value);
                    ret.Message = ((dynamic)jToken["error"]).Message.Value;
                }
                else
                {
                    ret.Message = response.ReasonPhrase;
                }

                response.Dispose();
                ret.Code = ResultCode.OtherError;
                return ret;
            }
            catch (HttpRequestException ex)
            {
                ret.Message = ex.Message;
                ret.Code = ResultCode.OtherError;
                ret.Location = "Plug-in";
                return ret;
            }
        }
        #endregion

        #region Execute
        public async Task<CrmResponse<T>> Execute<T>(string operationName, object boundParameter, CrmParameter crmParameter = null)
        {
            CrmResponse<T> crmRet = new CrmResponse<T>
            {
                Data = new List<T>()
            };
            try
            {
                await PostData(operationName, boundParameter, crmParameter, async delegate (HttpResponseMessage response)
                {
                    string text = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        Type typeFromHandle = typeof(T);
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            if (typeFromHandle == typeof(string))
                            {
                                crmRet.Data.Add((T)(object)text);
                            }
                            else if (typeFromHandle == typeof(int))
                            {
                                crmRet.Data.Add((T)(object)Convert.ToInt32(text));
                            }
                            else
                            {
                                crmRet.Data.Add(JsonConvert.DeserializeObject<T>(text));
                            }
                        }

                        crmRet.Code = ResultCode.Success;
                    }
                    else if (string.IsNullOrEmpty(text))
                    {
                        crmRet.Code = ResultCode.OtherError;
                        crmRet.Message = response.ReasonPhrase;
                    }
                    else
                    {
                        object obj = ((dynamic)JsonConvert.DeserializeObject<object>(text)).error;
                        crmRet.Code = ResultCode.OtherError;
                        crmRet.Message = ((dynamic)obj)?.Message?.Value;
                    }
                });
            }
            catch (Exception ex)
            {
                crmRet.Code = ResultCode.OtherError;
                crmRet.Message = ex.Message;
                crmRet.StackTrace = ex.StackTrace;
                crmRet.Location = "Plug-in";
            }

            return crmRet;
        }

        public async Task<CrmResponse<string>> ExecuteBatch(List<BatchContainer> data, CrmParameter crmParameter = null)
        {
            return await ExecuteBatchFun(data, crmParameter);
        }
        private async Task<CrmResponse<string>> ExecuteBatchFun(List<BatchContainer> data, CrmParameter crmParameter)
        {
            CrmResponse<string> ret = new CrmResponse<string>
            {
                Code = ResultCode.Success
            };
            decimal batchId = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
            Guid changesetId = Guid.NewGuid();
            StringBuilder requestBody = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                string entityName;
                if (!string.IsNullOrEmpty(data[i].entityName))
                {
                    entityName = data[i].entityName;
                }
                else
                {
                    Type type = data[i].data.GetType();
                    MethodInfo method = type.GetMethod("GetEntityKey");
                    entityName = ((!(method != null)) ? type.Name.ToLower() : method.Invoke(null, null)!.ToString());
                }

                if (data[i].Operate == PerformOperations.Create)
                {
                    requestBody.Append($"--batch_{batchId}");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Type: application/http");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Transfer-Encoding: binary");
                    requestBody.Append("\n");
                    requestBody.Append("\n");
                    requestBody.Append("POST " + _resourceUrl + "api/data/v9.2/" + CommonFun.GetPluralForm(entityName) + " HTTP/1.1");
                    requestBody.Append("\n");
                    requestBody.Append("Accept: application/json");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Type: application/json");
                    requestBody.Append("\n");
                    requestBody.Append("\n");
                    requestBody.Append(GetObjectToString(entityName, data[i].data, isBatch: true));
                    requestBody.Append("\n");
                }
                else if (data[i].Operate == PerformOperations.Update)
                {
                    requestBody.Append($"--batch_{batchId}");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Type: application/http");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Transfer-Encoding: binary");
                    requestBody.Append("\n");
                    requestBody.Append("\n");
                    requestBody.Append($"PATCH {_resourceUrl}api/data/v9.2/{CommonFun.GetPluralForm(entityName)}({data[i].Id}) HTTP/1.1");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Type: application/json");
                    requestBody.Append("\n");
                    requestBody.Append("\n");
                    requestBody.Append(GetObjectToString(entityName, data[i].data, isBatch: true));
                    requestBody.Append("\n");
                }
                else if (data[i].Operate == PerformOperations.Delete)
                {
                    requestBody.Append($"--batch_{batchId}");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Type: application/http");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Transfer-Encoding: binary");
                    requestBody.Append("\n");
                    requestBody.Append("\n");
                    requestBody.Append($"DELETE {_resourceUrl}api/data/v9.2/{CommonFun.GetPluralForm(entityName)}({data[i].Id}) HTTP/1.1");
                    requestBody.Append("\n");
                    requestBody.Append("If-match: *");
                    requestBody.Append("\n");
                    requestBody.Append("Content-Type: application/json");
                    requestBody.Append("\n");
                    requestBody.Append("Prefer: odata.include-annotations=\"*\"");
                    requestBody.Append("\n");
                    requestBody.Append("\n");
                }
            }

            requestBody.Append($"--batch_{batchId}--\r\n\0");
            Token token = await GetAuthenticationResponse();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/data/v9.2/$batch");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            MultipartContent multipartContent = new MultipartContent("mixed", "batch_" + batchId.ToString().Replace("\"", ""));
            StringContent stringContent = new StringContent(requestBody.ToString());
            stringContent.Headers.Clear();
            stringContent.Headers.Add("Content-Type", "multipart/mixed;boundary=" + changesetId);
            multipartContent.Add(stringContent);
            request.Content = multipartContent;
            SetCrmParameter(crmParameter, request);
            HttpResponseMessage response = await _crmDataPost.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                CrmResponse<string> crmResponse = ret;
                List<string> list = new List<string>();
                List<string> list2 = list;
                list2.Add(await response.Content.ReadAsStringAsync());
                crmResponse.Data = list;
            }
            else
            {
                CrmResponse<string> crmResponse = ret;
                crmResponse.Message = await response.Content.ReadAsStringAsync();
                ret.Code = ResultCode.DataError;
                ret.Location = "Plug-in";
            }

            request.Dispose();
            response.Dispose();
            return ret;
        }

        #endregion

        #region other
        public async Task<CrmResponse> Associate(string entityName, Guid entityId, string relationship, EntityReference entityReferences, CrmParameter crmParameter = null)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            CrmResponse crmResponse = new();

            dictionary.Add("@odata.id", _resourceUrl + "api/data/v9.2/" + CommonFun.GetPluralForm(entityReferences.logicalName) + "(" + entityReferences.Id.ToString() + ")");

            string requestUrl = $"{CommonFun.GetPluralForm(entityName)}({entityId})/{relationship}/$ref";
            await PostData(requestUrl, dictionary, crmParameter, delegate (HttpResponseMessage r)
            {
                if (r.IsSuccessStatusCode)
                {
                    crmResponse.Code = ResultCode.Success;
                    crmResponse.Message = r.ReasonPhrase;
                }
                else
                {
                    crmResponse.Code = ResultCode.OtherError;
                    crmResponse.Message = r.ReasonPhrase;
                    crmResponse.Location = "Plug-in";
                }
            });
            return crmResponse;
        }

        public async Task<CrmResponse> Disassociate(string entityName, Guid entityId, string relationship, EntityReference entityReferences, CrmParameter crmParameter = null)
        {
            new Dictionary<string, string>();
            CrmResponse crmResponse = new CrmResponse();
            Token token = await GetAuthenticationResponse();

            string requestUrl = $"api/data/v9.2/{CommonFun.GetPluralForm(entityName)}({entityId}/{relationship}({entityReferences.Id}/)/$ref";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("If-Match", "*");
            SetCrmParameter(crmParameter, request);
            HttpResponseMessage httpResponseMessage = await _crmDataPost.SendAsync(request);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                crmResponse.Code = ResultCode.Success;
                crmResponse.Message = httpResponseMessage.ReasonPhrase;
            }
            else
            {
                crmResponse.Code = ResultCode.OtherError;
                crmResponse.Message = httpResponseMessage.ReasonPhrase;
                crmResponse.Location = "Plug-in";
            }

            request.Dispose();
            return crmResponse;
        }

        public async Task<CrmResponse> ClearField<T>(string entityName, Guid? Id, List<string> field)
        {
            Token authResult = await GetAuthenticationResponse();
            CrmResponse ret = new CrmResponse
            {
                Code = ResultCode.Success,
                Message = "ok"
            };
            for (int i = 0; i < field.Count; i++)
            {
                string requestUrl = $"api/data/v9.2/{CommonFun.GetPluralForm(entityName)}({Id})/{field[i]}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.authentication.access_token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("If-Match", "*");
                HttpResponseMessage response = await _crmDataPost.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string value = await (response.Content?.ReadAsStringAsync());
                    if (string.IsNullOrEmpty(value))
                    {
                        ret.Code = ResultCode.OtherError;
                        ret.Message = response.ReasonPhrase;
                    }
                    else
                    {
                        object obj2 = ((dynamic)JsonConvert.DeserializeObject<object>(value)).error;
                        ret.Code = ResultCode.OtherError;
                        ret.Message = ((dynamic)obj2)?.Message?.Value;
                    }

                    return ret;
                }

                response.Dispose();
                request.Dispose();
            }

            return ret;
        }
        #endregion

        #region private
        private string GetEntityName<T>()
        {
            Type type = typeof(T);
            MethodInfo method = type.GetMethod("GetEntityKey");
            string entityName;
            if (method != null)
            {
                object obj = method.Invoke(null, null);
                entityName = obj.ToString();
            }
            else
            {
                entityName = type.Name.ToLower();
            }

            return entityName;
        }
        private string GetObjectToString<T>(string entityName, T entity, bool isBatch)
        {
            List<PropertyInfo> list = (from w in (isBatch ? entity.GetType() : typeof(T)).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                       where w.PropertyType.Name == "EntityReference"
                                       select w).ToList();
            string text = "";
            for (int i = 0; i < list.Count; i++)
            {
                object value = list[i].GetValue(entity);
                if (value == null)
                {
                    continue;
                }

                object[] customAttributes = list[i].GetCustomAttributes(typeof(CFieldType), inherit: true);
                if (customAttributes.Length == 0)
                {
                    continue;
                }

                EntityReference entityReference = (EntityReference)value;
                if (list[i].Name == "ownerid")
                {
                    if (string.IsNullOrEmpty(entityReference.logicalName))
                    {
                        string text4 = (entityReference.logicalName = (entityReference.PluralForm = "systemuser"));
                    }

                    if (entityReference.logicalName != "systemusers")
                    {
                        entityReference.PluralForm = CommonFun.GetPluralForm(((EntityReference)value).logicalName);
                    }

                    list[i].SetValue(entity, value);
                }
                else if ("regardingobjectid" == list[i].Name)
                {
                    text = entityReference.logicalName;
                    ((CFieldType[])customAttributes)[0].EntityName = entityReference.logicalName;
                    entityReference.PluralForm = CommonFun.GetPluralForm(entityReference.logicalName);
                }
                else
                {
                    entityReference.PluralForm = CommonFun.GetPluralForm(((CFieldType[])customAttributes)[0].EntityName);
                    list[i].SetValue(entity, value);
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(entity, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    Converters = new List<JsonConverter>
                    {
                        new EntityReferenceConverter()
                    }
                }));
                if (dictionary.ContainsKey("regardingobjectid"))
                {
                    dynamic val = dictionary["regardingobjectid"];
                    dictionary.Remove("regardingobjectid");
                    dictionary.Add("regardingobjectid_" + text + "_" + entityName + "@odata.bind", val);
                }

                return JsonConvert.SerializeObject(dictionary, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            }

            return JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Converters = new List<JsonConverter>
                {
                    new EntityReferenceConverter()
                }
            });
        }

        private async Task<Token> GetAuthenticationResponse()
        {
            if (_tokenCaChe.authentication != null && _tokenCaChe.not_before.Subtract(DateTime.Now).TotalMinutes > 10d)
            {
                return _tokenCaChe;
            }

            try
            {
                await GetAuthenticationRequest();
                if (_tokenCaChe.authentication == null)
                {
                    await Task.Delay(1000);
                    await GetAuthenticationRequest();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    await Task.Delay(1000);
                    await GetAuthenticationRequest();
                }
                catch (Exception)
                {
                }
            }

            return _tokenCaChe;
        }

        private async Task GetAuthenticationRequest()
        {
            try
            {
                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("resource", _resourceUrl),
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_secret", _clientSecret)
                });
                HttpResponseMessage httpResponseMessage = await _tokenHttpClient.PostAsync(_tokenUrl, content);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    AuthenticationResponse authentication = JsonConvert.DeserializeObject<AuthenticationResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
                    _tokenCaChe.authentication = authentication;
                    _tokenCaChe.expires_on = DateTimeHelper.TimestampToDateTime(authentication.expires_on);
                    _tokenCaChe.not_before = DateTimeHelper.TimestampToDateTime(authentication.not_before);

                }
                else
                {
                    JToken jToken = JsonConvert.DeserializeObject<JToken>(await httpResponseMessage.Content.ReadAsStringAsync());
                    _tokenCaChe.errorDescription = jToken["error_description"].Value<string>();
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task UpdateData<T>(string entityName, Guid? Id, T sendObject, CrmParameter crmParameter = null, Action<HttpResponseMessage> action = null)
        {
            Token token = await GetAuthenticationResponse();
            try
            {
                string content = ((!(typeof(T) == typeof(string))) ? JsonConvert.SerializeObject(sendObject) : Convert.ToString(sendObject));
                string requestUrl = $"api/data/v9.2/{entityName}({Id})";
                HttpRequestMessage request = new(HttpMethod.Patch, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
                request.Content = new StringContent(content);
                request.Content!.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request.Headers.Add("Preference-Applied", "odata.include-annotations=\"*\"");
                request.Headers.Add("If-Match", "*");
                SetCrmParameter(crmParameter, request);
                HttpResponseMessage httpResponseMessage = await _crmDataPost.SendAsync(request);
                action?.Invoke(httpResponseMessage);
                httpResponseMessage.Dispose();
                request.Dispose();
            }
            catch (Exception ex)
            {
                if (action != null)
                {
                    HttpResponseMessage httpResponseMessage = new(HttpStatusCode.BadRequest);
                    var value = new
                    {
                        error = new
                        {
                            Message = new
                            {
                                Value = ex.Message
                            }
                        }
                    };
                    httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(value));
                    action(httpResponseMessage);
                    httpResponseMessage.Dispose();
                }
            }
        }

        private async Task PostData<T>(string entityName, T sendObject, CrmParameter crmParameter = null, Action<HttpResponseMessage> t = null)
        {
            if (entityName is null)
            {
                throw new ArgumentNullException(nameof(entityName));
            }

            Token token = await GetAuthenticationResponse();
            string content = ((!(typeof(T) == typeof(string))) ? JsonConvert.SerializeObject(sendObject) : Convert.ToString(sendObject));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/data/v9.2/" + entityName);
            request.Content = new StringContent(content);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Content!.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Headers.Add("Preference-Applied", "odata.include-annotations=\"*\"");
            SetCrmParameter(crmParameter, request);
            HttpResponseMessage httpResponseMessage = await _crmDataPost.SendAsync(request);
            t?.Invoke(httpResponseMessage);
            httpResponseMessage.Dispose();
            request.Dispose();
        }

        private void SetCrmParameter(CrmParameter crmParameter, HttpRequestMessage request)
        {
            if (crmParameter != null)
            {
                if (!string.IsNullOrEmpty(crmParameter.MSCRMCallerID))
                {
                    request.Headers.Add("MSCRMCallerID", crmParameter.MSCRMCallerID);
                }

                if (crmParameter.BypassCustomPluginExecution)
                {
                    request.Headers.Add("MSCRM.BypassCustomPluginExecution", "true");
                }

                if (crmParameter.include)
                {
                    request.Headers.Add("Prefer", "odata.include-annotations=\"*\"");
                }
            }
        }

        private async Task<CrmResponse<string>> GetData(string queryString, CrmParameter crmParameter = null)
        {
            Token token = await GetAuthenticationResponse();
            CrmResponse<string> ret = new CrmResponse<string>();
            if (token.authentication == null)
            {
                ret.Data = new List<string>();
                ret.Code = ResultCode.ParameterError;
                ret.Message = token.errorDescription;
                return ret;
            }

            string item;
            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "api/data/v9.2/" + queryString);
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
                SetCrmParameter(crmParameter, httpRequestMessage);
                HttpResponseMessage resonse = await _crmDataPost.SendAsync(httpRequestMessage);
                if (!resonse.IsSuccessStatusCode)
                {
                    ret.Code = ResultCode.InternalError;
                    CrmResponse<string> crmResponse = ret;
                    crmResponse.Message = await resonse.Content.ReadAsStringAsync();
                    ret.Data = new List<string>();
                    return ret;
                }

                item = await resonse.Content.ReadAsStringAsync();
                resonse.Dispose();
                httpRequestMessage.Dispose();
            }
            catch (Exception ex)
            {
                ret.Code = ResultCode.InternalError;
                ret.Message = ex.Message;
                ret.StackTrace = ex.StackTrace;
                ret.Location = "Plug-in";
                ret.Data = new List<string>();
                return ret;
            }

            ret.Code = ResultCode.Success;
            ret.Message = "OK";
            ret.Data = new List<string> { item };
            return ret;
        }
        #endregion


        public void Dispose()
        {
            _tokenHttpClient.Dispose();
            _crmDataPost.Dispose();
        }

        public override bool Equals(object? obj)
        {
            return obj is ODataHttpClient client &&
                   EqualityComparer<HttpClient>.Default.Equals(_tokenHttpClient, client._tokenHttpClient);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_tokenHttpClient);
        }
    }
}
