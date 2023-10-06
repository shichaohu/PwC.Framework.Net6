using Newtonsoft.Json;
using PwC.CRM.Share.CRMClients.OData.Models;

namespace PwC.CRM.Models.Table
{
    /// <summary>
    /// 货币
    /// transactioncurrency
    /// </summary>
    public class Transactioncurrency
    {


        ///<summary>
        /// 创建者
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "createdby@odata.bind")]
        public EntityReference createdby
        {
            get
            {
                return _createdby;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("createdby");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("createdby");
                }
                _createdby = value;
            }
        }

        ///<summary>
        /// 创建时间
        ///</summary>    
        public DateTime? createdon
        {
            get
            {
                return _createdon;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("createdon");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("createdon");
                }
                _createdon = value;
            }
        }

        ///<summary>
        /// 货币名称
        ///</summary>    
        public string currencyname
        {
            get
            {
                return _currencyname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("currencyname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("currencyname");
                }
                _currencyname = value;
            }
        }

        ///<summary>
        /// 货币精度
        ///</summary>    
        public int? currencyprecision
        {
            get
            {
                return _currencyprecision;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("currencyprecision");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("currencyprecision");
                }
                _currencyprecision = value;
            }
        }

        ///<summary>
        /// 货币符号
        ///</summary>    
        public string currencysymbol
        {
            get
            {
                return _currencysymbol;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("currencysymbol");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("currencysymbol");
                }
                _currencysymbol = value;
            }
        }

        ///<summary>
        /// 实体图像
        ///</summary>    
        [CFieldType(FieldType = "Virtual")]
        public string entityimage
        {
            get
            {
                return _entityimage;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("entityimage");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("entityimage");
                }
                _entityimage = value;
            }
        }

        ///<summary>
        /// undefined
        ///</summary>    
        public long? entityimage_timestamp
        {
            get
            {
                return _entityimage_timestamp;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("entityimage_timestamp");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("entityimage_timestamp");
                }
                _entityimage_timestamp = value;
            }
        }

        ///<summary>
        /// undefined
        ///</summary>    
        public string entityimage_url
        {
            get
            {
                return _entityimage_url;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("entityimage_url");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("entityimage_url");
                }
                _entityimage_url = value;
            }
        }

        ///<summary>
        /// 实体图像 ID
        ///</summary>    
        public Guid? entityimageid
        {
            get
            {
                return _entityimageid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("entityimageid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("entityimageid");
                }
                _entityimageid = value;
            }
        }

        ///<summary>
        /// 汇率
        ///</summary>    
        public decimal? exchangerate
        {
            get
            {
                return _exchangerate;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("exchangerate");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("exchangerate");
                }
                _exchangerate = value;
            }
        }

        ///<summary>
        /// 货币代码
        ///</summary>    
        public string isocurrencycode
        {
            get
            {
                return _isocurrencycode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("isocurrencycode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("isocurrencycode");
                }
                _isocurrencycode = value;
            }
        }

        ///<summary>
        /// 修改者
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "modifiedby@odata.bind")]
        public EntityReference modifiedby
        {
            get
            {
                return _modifiedby;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("modifiedby");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("modifiedby");
                }
                _modifiedby = value;
            }
        }

        ///<summary>
        /// 修改时间
        ///</summary>    
        public DateTime? modifiedon
        {
            get
            {
                return _modifiedon;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("modifiedon");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("modifiedon");
                }
                _modifiedon = value;
            }
        }

        ///<summary>
        /// 修改者(代理)
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "modifiedonbehalfby@odata.bind")]
        public EntityReference modifiedonbehalfby
        {
            get
            {
                return _modifiedonbehalfby;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("modifiedonbehalfby");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("modifiedonbehalfby");
                }
                _modifiedonbehalfby = value;
            }
        }

        ///<summary>
        /// 组织
        ///</summary>   
        [CFieldType(EntityName = "organization")]
        [JsonProperty(PropertyName = "organizationid@odata.bind")]
        public EntityReference organizationid
        {
            get
            {
                return _organizationid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("organizationid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("organizationid");
                }
                _organizationid = value;
            }
        }

        ///<summary>
        /// 状态
        ///</summary>   
        [CFieldType(FieldType = "State")]
        public int? statecode
        {
            get
            {
                return _statecode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("statecode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("statecode");
                }
                _statecode = value;
            }
        }

        ///<summary>
        /// 状态描述
        ///</summary>   
        [CFieldType(FieldType = "Status")]
        public int? statuscode
        {
            get
            {
                return _statuscode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("statuscode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("statuscode");
                }
                _statuscode = value;
            }
        }

        ///<summary>
        /// 交易币种
        ///</summary>    
        public Guid? transactioncurrencyid
        {
            get
            {
                return _transactioncurrencyid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("transactioncurrencyid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("transactioncurrencyid");
                }
                _transactioncurrencyid = value;
            }
        }

        ///<summary>
        /// UniqueDscId
        ///</summary>    
        public Guid? uniquedscid
        {
            get
            {
                return _uniquedscid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("uniquedscid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("uniquedscid");
                }
                _uniquedscid = value;
            }
        }

        private HashSet<string> _modifyList = new HashSet<string>(); private EntityReference _createdby = null; private DateTime? _createdon = null; private string _currencyname = null; private int? _currencyprecision = null; private string _currencysymbol = null; private string _entityimage = null; private long? _entityimage_timestamp = null; private string _entityimage_url = null; private Guid? _entityimageid = null; private decimal? _exchangerate = null; private string _isocurrencycode = null; private EntityReference _modifiedby = null; private DateTime? _modifiedon = null; private EntityReference _modifiedonbehalfby = null; private EntityReference _organizationid = null; private int? _statecode = null; private int? _statuscode = null; private Guid? _transactioncurrencyid = null; private Guid? _uniquedscid = null;
        public HashSet<string> GetModifyList()
        {
            return _modifyList;
        }
        public static bool GetIsActivity()
        {
            return false;
        }
        public static string GetEntityKey()
        {
            return "transactioncurrency";
        }
    }

    /// <summary>
    /// 货币
    /// transactioncurrency
    /// </summary>
    public class Link_Transactioncurrency : Transactioncurrency
    {

    }
}
