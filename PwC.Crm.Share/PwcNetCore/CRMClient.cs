using PwC.Crm.Share.PwcNetCore.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;

namespace PwC.Crm.Share.PwcNetCore
{
    /// <summary>
    ///  CRM Client
    /// </summary>
    public class CRMClient : ICRMClient, IDisposable
    {
        private struct Token
        {
            public AuthenticationResponse authentication;

            public DateTime time;

            public string mess;
        }

        private string resourceUrl = "";

        private string clientId = "";

        private string clientSecret = "";

        private string tenantId = "";

        private string tokenUrl = "";

        private static Type decimalType = typeof(decimal?);

        private static Type doubleType = typeof(double?);

        private static Type intType = typeof(int?);

        private static Type guidType = typeof(Guid?);

        private static Type longType = typeof(long?);

        private static Type stringType = typeof(string);

        private static Type dateTimeType = typeof(DateTime?);

        private static Type entityReferenceType = typeof(EntityReference);

        private static Type boolType = typeof(bool?);

        private HttpClient tokenHttpClient;

        private HttpClient CrmDataPost;

        private Token tokenCaChe;

        public CRMClient(IConfiguration configuration)
        {
            var resourceUrl = configuration["Crm:resourceUrl"];
            if (!resourceUrl.EndsWith("/"))
            {
                resourceUrl += "/";
            }

            string clientId = configuration.GetSection("Crm:clientId").Value;
            string clientSecret = configuration.GetSection("Crm:clientSecret").Value;
            string tenantId = configuration.GetSection("Crm:tenantId").Value;
            string tokenUrl = configuration.GetSection("Crm:tokenUrl").Value;

            init(resourceUrl, clientId, clientSecret, tenantId, tokenUrl);
        }

        private void init(string _resourceUrl, string _clientId, string _clientSecret, string _tenantId, string _tokenUrl, string MSCRMCallerID = "")
        {
            resourceUrl = _resourceUrl;
            clientId = _clientId;
            clientSecret = _clientSecret;
            tenantId = _tenantId;
            tokenUrl = "https://" + _tokenUrl + "/" + tenantId + "/oauth2/token";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            tokenHttpClient = new HttpClient();
            tokenHttpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            CrmDataPost = new HttpClient();
            CrmDataPost.BaseAddress = new Uri(resourceUrl);
            CrmDataPost.Timeout = new TimeSpan(0, 5, 0);
            CrmDataPost.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            CrmDataPost.DefaultRequestHeaders.Add("OData-Version", "4.0");
            if (!string.IsNullOrEmpty(MSCRMCallerID))
            {
                CrmDataPost.DefaultRequestHeaders.Add("MSCRMCallerID", MSCRMCallerID);
            }
        }

        public async Task<CrmResponse> CreateRecords<T>(string entityName, T entity, CrmParameter crmParameter = null)
        {
            return await CreateRecordsFun(entityName, entity);
        }

        private async Task<CrmResponse> CreateRecordsFun<T>(string entityName, T entity, CrmParameter crmParameter = null)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                Type type = entity.GetType();
                MethodInfo method = type.GetMethod("GetEntityKey");
                if (method != null)
                {
                    object obj = method.Invoke(null, null);
                    entityName = obj.ToString();
                }
                else
                {
                    entityName = type.Name.ToLower();
                }
            }

            CrmResponse crmRet = new CrmResponse();
            try
            {
                string objectToString = GetObjectToString(entityName, entity, isBatch: false);
                await PostData(CommonFun.GetPluralForm(entityName), objectToString, crmParameter, async delegate (HttpResponseMessage response)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string originalString = response.Headers.Location!.OriginalString;
                        crmRet.Id = Guid.Parse(originalString.Substring(originalString.Length - 37, 36));
                        crmRet.code = ResultCode.Success;
                    }
                    else
                    {
                        string value = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(value))
                        {
                            crmRet.code = ResultCode.OtherError;
                            crmRet.message = response.ReasonPhrase;
                        }
                        else
                        {
                            crmRet = JsonConvert.DeserializeObject<CrmResponse>(value);
                            object obj2 = ((dynamic)JsonConvert.DeserializeObject<object>(value)).error;
                            crmRet.code = ResultCode.OtherError;
                            crmRet.message = ((dynamic)obj2)?.message?.Value;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                crmRet.code = ResultCode.OtherError;
                crmRet.message = ex.Message;
                crmRet.stacktrace = ex.StackTrace;
                crmRet.location = "Plug-in";
            }

            return crmRet;
        }

        public async Task<CrmResponse> CreateRecords<T>(T entity, CrmParameter crmParameter = null)
        {
            return await CreateRecordsFun("", entity);
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(string fetchXml, bool include)
        {
            return await QueryRecords<T>(fetchXml, new CrmParameter
            {
                include = include
            });
        }

        public async Task<CrmResponse<T>> QueryRecords<T>(string fetchXml, CrmParameter crmParameter = null)
        {
            fetchXml = fetchXml?.TrimStart();
            Type typeT = typeof(T);
            if (fetchXml.StartsWith("<fetch"))
            {
                MethodInfo method = typeT.GetMethod("GetEntityKey");
                if (method != null)
                {
                    object obj2 = method.Invoke(null, null);
                    fetchXml = CommonFun.GetPluralForm((string)obj2) + "?fetchXml=" + HttpUtility.UrlEncode(fetchXml, Encoding.UTF8);
                }
                else
                {
                    fetchXml = CommonFun.GetPluralForm(typeT.Name.ToLower()) + "?fetchXml=" + HttpUtility.UrlEncode(fetchXml, Encoding.UTF8);
                }
            }

            if (crmParameter != null && !crmParameter.include)
            {
                MethodInfo method2 = typeT.GetMethod("GetIsActivity");
                if (method2 != null && (bool)method2.Invoke(null, null))
                {
                    crmParameter.include = true;
                }
            }

            CrmResponse<string> crmResponse = await GetData(fetchXml, crmParameter);
            if (crmResponse.code != ResultCode.Success)
            {
                return new CrmResponse<T>
                {
                    code = crmResponse.code,
                    message = crmResponse.message,
                    value = new List<T>()
                };
            }

            try
            {
                JArray jArray = ((dynamic)JsonConvert.DeserializeObject<object>(crmResponse.value[0])).value;
                List<T> list = new List<T>();
                if (typeT.Name.StartsWith("Dictionary"))
                {
                    for (int i = 0; i < jArray.Count; i++)
                    {
                        JObject obj3 = (JObject)jArray[i];
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        foreach (KeyValuePair<string, JToken> item2 in obj3)
                        {
                            if (item2.Value != null)
                            {
                                if (item2.Value.Type == JTokenType.Integer)
                                {
                                    dictionary.Add(item2.Key, item2.Value.Value<long>());
                                }
                                else if (item2.Value.Type == JTokenType.String)
                                {
                                    dictionary.Add(item2.Key, item2.Value.Value<string>());
                                }
                                else if (item2.Value.Type == JTokenType.Boolean)
                                {
                                    dictionary.Add(item2.Key, (item2.Value.Value<bool>() ? ((byte)1) : ((byte)0)) != 0);
                                }
                                else if (item2.Value.Type == JTokenType.Float)
                                {
                                    dictionary.Add(item2.Key, item2.Value.Value<decimal>());
                                }
                                else if (item2.Value.Type == JTokenType.Date)
                                {
                                    dictionary.Add(item2.Key, item2.Value.Value<DateTime>());
                                }
                                else if (item2.Value.Type == JTokenType.Guid)
                                {
                                    dictionary.Add(item2.Key, item2.Value.Value<string>());
                                }
                                else
                                {
                                    dictionary.Add(item2.Key, item2.Value.Value<object>());
                                }
                            }
                            else
                            {
                                dictionary.Add(item2.Key, null);
                            }
                        }

                        list.Add((T)(object)dictionary);
                    }

                    return new CrmResponse<T>
                    {
                        value = list,
                        code = ResultCode.Success
                    };
                }

                for (int j = 0; j < jArray.Count; j++)
                {
                    object obj = Activator.CreateInstance(typeT);
                    Type type = obj.GetType();
                    JObject jObject = (JObject)jArray[j];
                    List<Tuple<string, string, JToken>> lookupList = new List<Tuple<string, string, JToken>>();
                    foreach (KeyValuePair<string, JToken> item3 in jObject)
                    {
                        if (!item3.Key.Contains("@") && item3.Key.Contains("."))
                        {
                            string[] array = item3.Key.Split(".");
                            _ = item3.Value;
                            Tuple<string, string, JToken> item = new Tuple<string, string, JToken>(array[0], array[1], item3.Value);
                            lookupList.Add(item);
                        }
                        else
                        {
                            SetObjctValue(item3, jObject, type, ref obj);
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
                                     likeType.SetValue(obj, attJob);
                                 }
                                 else
                                 {
                                     list2.ForEach(delegate (Tuple<string, string, JToken> att)
                                     {
                                         SetObjctValue(new KeyValuePair<string, JToken>(att.Item2, att.Item3), attJob, likeType.PropertyType, ref attrObj);
                                     });
                                     likeType.SetValue(obj, attrObj);
                                 }
                             }
                         });
                    }

                    list.Add((T)obj);
                }

                return new CrmResponse<T>
                {
                    value = list,
                    code = ResultCode.Success
                };
            }
            catch (Exception ex)
            {
                return new CrmResponse<T>
                {
                    code = ResultCode.InternalError,
                    message = ex.Message,
                    stacktrace = ex.StackTrace,
                    location = "Plug-in"
                };
            }
        }

        private void SetObjctValue(KeyValuePair<string, JToken> kp, JObject currObject, Type objType, ref object obj)
        {
            if (kp.Key.StartsWith("_") && kp.Key.EndsWith("_value"))
            {
                string name = kp.Key.Substring(1, kp.Key.Length - 7);
                PropertyInfo property = objType.GetProperty(name);
                if (!(property != null) || !(property.PropertyType == entityReferenceType))
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

            if (property2.PropertyType == stringType)
            {
                property2.SetValue(obj, kp.Value.Value<string>());
                return;
            }

            if (property2.PropertyType == decimalType)
            {
                property2.SetValue(obj, kp.Value.Value<decimal>());
                return;
            }

            if (property2.PropertyType == doubleType)
            {
                property2.SetValue(obj, kp.Value.Value<double>());
                return;
            }

            if (property2.PropertyType == intType)
            {
                property2.SetValue(obj, kp.Value.Value<int>());
                return;
            }

            if (property2.PropertyType == longType)
            {
                property2.SetValue(obj, kp.Value.Value<long>());
                return;
            }

            if (property2.PropertyType == dateTimeType)
            {
                property2.SetValue(obj, kp.Value.Value<DateTime>());
                return;
            }

            if (property2.PropertyType == guidType)
            {
                property2.SetValue(obj, Guid.Parse(kp.Value.Value<string>()));
                return;
            }

            if (property2.PropertyType == boolType)
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
            else if (property2.PropertyType == entityReferenceType)
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
                stringBuilder.Append(" <attribute name='" + fieldList[i] + "' />");
            }

            Type typeFromHandle = typeof(T);
            if (string.IsNullOrWhiteSpace(entityName))
            {
                MethodInfo method = typeFromHandle.GetMethod("GetEntityKey");
                if (method != null)
                {
                    object obj = method.Invoke(null, null);
                    entityName = (string)obj;
                }
                else
                {
                    entityName = typeFromHandle.Name.ToLower();
                }
            }

            MethodInfo method2 = typeFromHandle.GetMethod("GetIsActivity");
            string text = entityName + "id";
            if (method2 != null && (bool)method2.Invoke(null, null))
            {
                text = "activityid";
            }

            string str = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>\r\n  <entity name='" + entityName + "'>\r\n   " + stringBuilder.ToString() + "\r\n    <filter type='and'><condition attribute='" + text + "' operator='eq' value='" + Id.ToString() + "' />   </filter>\r\n  </entity>\r\n</fetch>";
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

        public async Task<CrmResponse> UpdateRecords<T>(string entityName, Guid? Id, T entity)
        {
            return await UpdateRecordsFun(entityName, Id, entity);
        }

        public async Task<CrmResponse> UpdateRecords<T>(string entityName, Guid? Id, T entity, CrmParameter crmParameter)
        {
            return await UpdateRecordsFun(entityName, Id, entity, crmParameter);
        }

        public async Task<CrmResponse> UpdateRecords<T>(Guid? Id, T entity, CrmParameter crmParameter)
        {
            return await UpdateRecordsFun("", Id, entity, crmParameter);
        }

        private async Task<CrmResponse> UpdateRecordsFun<T>(string entityName, Guid? Id, T entity, CrmParameter crmParameter = null)
        {
            Type type = entity.GetType();
            if (string.IsNullOrEmpty(entityName))
            {
                entityName = type.Name.ToLower();
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
                        crmRet.code = ResultCode.Success;
                    }
                    else
                    {
                        string value = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(value))
                        {
                            crmRet.code = ResultCode.OtherError;
                            crmRet.message = response.ReasonPhrase;
                        }
                        else
                        {
                            object obj = ((dynamic)JsonConvert.DeserializeObject<object>(value)).error;
                            crmRet.code = ResultCode.OtherError;
                            crmRet.message = ((dynamic)obj)?.message?.Value;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                crmRet.code = ResultCode.OtherError;
                crmRet.message = ex.Message;
                crmRet.stacktrace = ex.StackTrace;
                crmRet.location = "Plug-in";
            }

            return crmRet;
        }

        public async Task<CrmResponse> UpdateRecords<T>(Guid? Id, T entity)
        {
            return await UpdateRecordsFun("", Id, entity);
        }

        public async Task<CrmResponse<string>> ExecuteBatch(List<BatchContainer> data, CrmParameter crmParameter = null)
        {
            return await ExecuteBatchFun(data, crmParameter);
        }

        public async Task<CrmResponse> DeleteRecords(string entityName, Guid? Id)
        {
            return await DeleteRecordsFun(entityName, Id);
        }

        private async Task<CrmResponse> DeleteRecordsFun(string entityName, Guid? Id, CrmParameter crmParameter = null)
        {
            CrmResponse ret = new CrmResponse
            {
                code = ResultCode.Success
            };
            Token token = await GetAuthenticationResponse();
            HttpMethod delete = HttpMethod.Delete;
            string[] obj = new string[5]
            {
                "api/data/v9.2/",
                CommonFun.GetPluralForm(entityName),
                "(",
                null,
                null
            };
            Guid? guid = Id;
            obj[3] = guid.ToString();
            obj[4] = ")";
            HttpRequestMessage request = new HttpRequestMessage(delete, string.Concat(obj));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("If-Match", "*");
            SetCrmParameter(crmParameter, request);
            try
            {
                HttpResponseMessage response = await CrmDataPost.SendAsync(request);
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
                    ret.message = ((dynamic)jToken["error"]).message.Value;
                }
                else
                {
                    ret.message = response.ReasonPhrase;
                }

                response.Dispose();
                ret.code = ResultCode.OtherError;
                return ret;
            }
            catch (HttpRequestException ex)
            {
                ret.message = ex.Message;
                ret.code = ResultCode.OtherError;
                return ret;
            }
        }

        public async Task<CrmResponse> DeleteRecords(string entityName, Guid? Id, CrmParameter crmParameter)
        {
            return await DeleteRecordsFun(entityName, Id, crmParameter);
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
            if (tokenCaChe.authentication != null && DateTime.Now.Subtract(tokenCaChe.time).TotalMinutes < 30.0)
            {
                return tokenCaChe;
            }

            try
            {
                await GetAuthenticationRequest();
                if (tokenCaChe.authentication == null)
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

            return tokenCaChe;
        }

        private async Task GetAuthenticationRequest()
        {
            _ = 2;
            try
            {
                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("resource", resourceUrl),
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_secret", clientSecret)
                });
                HttpResponseMessage httpResponseMessage = await tokenHttpClient.PostAsync(tokenUrl, content);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    AuthenticationResponse authentication = JsonConvert.DeserializeObject<AuthenticationResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
                    tokenCaChe.authentication = authentication;
                    tokenCaChe.time = DateTime.Now;
                }
                else
                {
                    JToken jToken = JsonConvert.DeserializeObject<JToken>(await httpResponseMessage.Content.ReadAsStringAsync());
                    tokenCaChe.mess = jToken["error_description"].Value<string>();
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task UpdateData<T>(string entity, Guid? Id, T sendObject, CrmParameter crmParameter = null, Action<HttpResponseMessage> t = null)
        {
            Token token = await GetAuthenticationResponse();
            try
            {
                string content = ((!(typeof(T) == typeof(string))) ? JsonConvert.SerializeObject(sendObject) : Convert.ToString(sendObject));
                HttpMethod patch = HttpMethod.Patch;
                string[] obj = new string[5] { "api/data/v9.2/", entity, "(", null, null };
                Guid? guid = Id;
                obj[3] = guid.ToString();
                obj[4] = ")";
                HttpRequestMessage request = new HttpRequestMessage(patch, string.Concat(obj));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
                request.Content = new StringContent(content);
                request.Content!.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request.Headers.Add("Preference-Applied", "odata.include-annotations=\"*\"");
                request.Headers.Add("If-Match", "*");
                SetCrmParameter(crmParameter, request);
                HttpResponseMessage httpResponseMessage = await CrmDataPost.SendAsync(request);
                t?.Invoke(httpResponseMessage);
                httpResponseMessage.Dispose();
                request.Dispose();
            }
            catch (Exception ex)
            {
                if (t != null)
                {
                    HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    var value = new
                    {
                        error = new
                        {
                            message = new
                            {
                                Value = ex.Message
                            }
                        }
                    };
                    httpResponseMessage2.Content = new StringContent(JsonConvert.SerializeObject(value));
                    t(httpResponseMessage2);
                    httpResponseMessage2.Dispose();
                }
            }
        }

        private async Task PostData<T>(string entity, T sendObject, CrmParameter crmParameter = null, Action<HttpResponseMessage> t = null)
        {
            Token token = await GetAuthenticationResponse();
            string content = ((!(typeof(T) == typeof(string))) ? JsonConvert.SerializeObject(sendObject) : Convert.ToString(sendObject));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/data/v9.2/" + entity);
            request.Content = new StringContent(content);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Content!.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Headers.Add("Preference-Applied", "odata.include-annotations=\"*\"");
            SetCrmParameter(crmParameter, request);
            HttpResponseMessage httpResponseMessage = await CrmDataPost.SendAsync(request);
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

        private async Task<CrmResponse<string>> ExecuteBatchFun(List<BatchContainer> data, CrmParameter crmParameter)
        {
            CrmResponse<string> ret = new CrmResponse<string>
            {
                code = ResultCode.Success
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
                    requestBody.Append("POST " + resourceUrl + "api/data/v9.2/" + CommonFun.GetPluralForm(entityName) + " HTTP/1.1");
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
                    requestBody.Append($"PATCH {resourceUrl}api/data/v9.2/{CommonFun.GetPluralForm(entityName)}({data[i].Id}) HTTP/1.1");
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
                    requestBody.Append($"DELETE {resourceUrl}api/data/v9.2/{CommonFun.GetPluralForm(entityName)}({data[i].Id}) HTTP/1.1");
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
            HttpResponseMessage response = await CrmDataPost.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                CrmResponse<string> crmResponse = ret;
                List<string> list = new List<string>();
                List<string> list2 = list;
                list2.Add(await response.Content.ReadAsStringAsync());
                crmResponse.value = list;
            }
            else
            {
                CrmResponse<string> crmResponse = ret;
                crmResponse.message = await response.Content.ReadAsStringAsync();
                ret.code = ResultCode.DataError;
            }

            request.Dispose();
            response.Dispose();
            return ret;
        }

        private async Task<CrmResponse<string>> GetData(string queryString, CrmParameter crmParameter = null)
        {
            Token token = await GetAuthenticationResponse();
            CrmResponse<string> ret = new CrmResponse<string>();
            if (token.authentication == null)
            {
                ret.value = new List<string>();
                ret.code = ResultCode.ParameterError;
                ret.message = token.mess;
                return ret;
            }

            string item;
            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "api/data/v9.2/" + queryString);
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
                SetCrmParameter(crmParameter, httpRequestMessage);
                HttpResponseMessage resonse = await CrmDataPost.SendAsync(httpRequestMessage);
                if (!resonse.IsSuccessStatusCode)
                {
                    ret.code = ResultCode.InternalError;
                    CrmResponse<string> crmResponse = ret;
                    crmResponse.message = await resonse.Content.ReadAsStringAsync();
                    ret.value = new List<string>();
                    return ret;
                }

                item = await resonse.Content.ReadAsStringAsync();
                resonse.Dispose();
                httpRequestMessage.Dispose();
            }
            catch (Exception ex)
            {
                ret.code = ResultCode.InternalError;
                ret.message = ex.Message;
                ret.stacktrace = ex.StackTrace;
                ret.location = "Plug-in";
                ret.value = new List<string>();
                return ret;
            }

            ret.code = ResultCode.Success;
            ret.message = "OK";
            ret.value = new List<string> { item };
            return ret;
        }

        public async Task<CrmResponse> Associate(string entityName, Guid entityId, string relationship, EntityReference entityReferences, CrmParameter crmParameter = null)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            CrmResponse crmResponse = new CrmResponse();
            dictionary.Add("@odata.id", resourceUrl + "api/data/v9.2/" + CommonFun.GetPluralForm(entityReferences.logicalName) + "(" + entityReferences.Id.ToString() + ")");
            CRMClient cRequest = this;
            string[] obj = new string[6]
            {
                CommonFun.GetPluralForm(entityName),
                "(",
                null,
                null,
                null,
                null
            };
            Guid guid = entityId;
            obj[2] = guid.ToString();
            obj[3] = ")/";
            obj[4] = relationship;
            obj[5] = "/$ref";
            await cRequest.PostData(string.Concat(obj), dictionary, crmParameter, delegate (HttpResponseMessage r)
            {
                if (r.IsSuccessStatusCode)
                {
                    crmResponse.code = ResultCode.Success;
                    crmResponse.message = r.ReasonPhrase;
                }
                else
                {
                    crmResponse.code = ResultCode.OtherError;
                    crmResponse.message = r.ReasonPhrase;
                }
            });
            return crmResponse;
        }

        public async Task<CrmResponse> Disassociate(string entityName, Guid entityId, string relationship, EntityReference entityReferences, CrmParameter crmParameter = null)
        {
            new Dictionary<string, string>();
            CrmResponse crmResponse = new CrmResponse();
            Token token = await GetAuthenticationResponse();
            string[] obj = new string[9]
            {
                "api/data/v9.2/",
                CommonFun.GetPluralForm(entityName),
                "(",
                null,
                null,
                null,
                null,
                null,
                null
            };
            Guid guid = entityId;
            obj[3] = guid.ToString();
            obj[4] = ")/";
            obj[5] = relationship;
            obj[6] = "(";
            obj[7] = entityReferences.Id.ToString();
            obj[8] = ")/$ref";
            string requestUri = string.Concat(obj);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.authentication.access_token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("If-Match", "*");
            SetCrmParameter(crmParameter, request);
            HttpResponseMessage httpResponseMessage = await CrmDataPost.SendAsync(request);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                crmResponse.code = ResultCode.Success;
                crmResponse.message = httpResponseMessage.ReasonPhrase;
            }
            else
            {
                crmResponse.code = ResultCode.OtherError;
                crmResponse.message = httpResponseMessage.ReasonPhrase;
            }

            request.Dispose();
            return crmResponse;
        }

        public async Task<CrmResponse<T>> Execute<T>(string operationName, object boundParameter, CrmParameter crmParameter = null)
        {
            CrmResponse<T> crmRet = new CrmResponse<T>
            {
                value = new List<T>()
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
                                crmRet.value.Add((T)(object)text);
                            }
                            else if (typeFromHandle == typeof(int))
                            {
                                crmRet.value.Add((T)(object)Convert.ToInt32(text));
                            }
                            else
                            {
                                crmRet.value = new List<T>();
                                crmRet.value.Add(JsonConvert.DeserializeObject<T>(text));
                            }
                        }

                        crmRet.code = ResultCode.Success;
                    }
                    else if (string.IsNullOrEmpty(text))
                    {
                        crmRet.code = ResultCode.OtherError;
                        crmRet.message = response.ReasonPhrase;
                    }
                    else
                    {
                        object obj = ((dynamic)JsonConvert.DeserializeObject<object>(text)).error;
                        crmRet.code = ResultCode.OtherError;
                        crmRet.message = ((dynamic)obj)?.message?.Value;
                    }
                });
            }
            catch (Exception ex)
            {
                crmRet.code = ResultCode.OtherError;
                crmRet.message = ex.Message;
                crmRet.stacktrace = ex.StackTrace;
                crmRet.location = "Plug-in";
            }

            return crmRet;
        }

        public async Task<CrmResponse> ClearField<T>(string entityName, Guid? Id, List<string> field)
        {
            Token authResult = await GetAuthenticationResponse();
            CrmResponse ret = new CrmResponse
            {
                code = ResultCode.Success,
                message = "ok"
            };
            for (int i = 0; i < field.Count; i++)
            {
                HttpMethod delete = HttpMethod.Delete;
                string[] obj = new string[6]
                {
                    "api/data/v9.2/",
                    CommonFun.GetPluralForm(entityName),
                    "(",
                    null,
                    null,
                    null
                };
                Guid? guid = Id;
                obj[3] = guid.ToString();
                obj[4] = ")/";
                obj[5] = field[i];
                HttpRequestMessage request = new HttpRequestMessage(delete, string.Concat(obj));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.authentication.access_token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("If-Match", "*");
                HttpResponseMessage response = await CrmDataPost.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string value = await (response.Content?.ReadAsStringAsync());
                    if (string.IsNullOrEmpty(value))
                    {
                        ret.code = ResultCode.OtherError;
                        ret.message = response.ReasonPhrase;
                    }
                    else
                    {
                        object obj2 = ((dynamic)JsonConvert.DeserializeObject<object>(value)).error;
                        ret.code = ResultCode.OtherError;
                        ret.message = ((dynamic)obj2)?.message?.Value;
                    }

                    return ret;
                }

                response.Dispose();
                request.Dispose();
            }

            return ret;
        }

        public void Dispose()
        {
            tokenHttpClient.Dispose();
            CrmDataPost.Dispose();
        }
    }
}
