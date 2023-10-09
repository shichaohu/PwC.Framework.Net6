using Newtonsoft.Json;
using PwC.CRM.Share.CRMClients.OData.Models;

namespace PwC.CRM.Models.Table
{
    /// <summary>
    /// 消息通知
    /// apv_message
    /// </summary>
    public class Apv_Message
    {


        ///<summary>
        /// 内容
        ///</summary>    
        public string apv_content
        {
            get
            {
                return _apv_content;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_content");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_content");
                }
                _apv_content = value;
            }
        }

        ///<summary>
        /// 修改者（自定义）
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "apv_editor@odata.bind")]
        public EntityReference apv_editor
        {
            get
            {
                return _apv_editor;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_editor");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_editor");
                }
                _apv_editor = value;
            }
        }

        ///<summary>
        /// 关联洞察任务
        ///</summary>   
        [CFieldType(EntityName = "apv_insighttask")]
        [JsonProperty(PropertyName = "apv_insighttask_r1@odata.bind")]
        public EntityReference apv_insighttask_r1
        {
            get
            {
                return _apv_insighttask_r1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_insighttask_r1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_insighttask_r1");
                }
                _apv_insighttask_r1 = value;
            }
        }

        ///<summary>
        /// 是否已读
        ///</summary>    
        public bool? apv_isread
        {
            get
            {
                return _apv_isread;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_isread");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_isread");
                }
                _apv_isread = value;
            }
        }

        ///<summary>
        /// 消息通知
        ///</summary>    
        public Guid? apv_messageid
        {
            get
            {
                return _apv_messageid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_messageid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_messageid");
                }
                _apv_messageid = value;
            }
        }

        ///<summary>
        /// 通知类型
        ///</summary>    
        [CFieldType(EnumType = typeof(EnumMessgtype))]
        public EnumMessgtype? apv_messgtype
        {
            get
            {
                return _apv_messgtype;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_messgtype");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_messgtype");
                }
                _apv_messgtype = value;
            }
        }

        ///<summary>
        /// 标题
        ///</summary>    
        public string apv_name
        {
            get
            {
                return _apv_name;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_name");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_name");
                }
                _apv_name = value;
            }
        }

        ///<summary>
        /// 关联项目
        ///</summary>   
        [CFieldType(EntityName = "apv_project")]
        [JsonProperty(PropertyName = "apv_project_r1@odata.bind")]
        public EntityReference apv_project_r1
        {
            get
            {
                return _apv_project_r1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_project_r1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_project_r1");
                }
                _apv_project_r1 = value;
            }
        }

        ///<summary>
        /// 备注
        ///</summary>    
        public string apv_remark
        {
            get
            {
                return _apv_remark;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_remark");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_remark");
                }
                _apv_remark = value;
            }
        }

        ///<summary>
        /// 发件人
        ///</summary>    
        public string apv_sender
        {
            get
            {
                return _apv_sender;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_sender");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_sender");
                }
                _apv_sender = value;
            }
        }

        ///<summary>
        /// 关联任务
        ///</summary>   
        [CFieldType(EntityName = "apv_task")]
        [JsonProperty(PropertyName = "apv_task_r1@odata.bind")]
        public EntityReference apv_task_r1
        {
            get
            {
                return _apv_task_r1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_task_r1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_task_r1");
                }
                _apv_task_r1 = value;
            }
        }

        ///<summary>
        /// 消息唯一标记码
        ///</summary>    
        public string apv_uniquemarkcode
        {
            get
            {
                return _apv_uniquemarkcode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_uniquemarkcode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_uniquemarkcode");
                }
                _apv_uniquemarkcode = value;
            }
        }

        ///<summary>
        /// 抄送
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "apv_user_r1@odata.bind")]
        public EntityReference apv_user_r1
        {
            get
            {
                return _apv_user_r1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_user_r1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_user_r1");
                }
                _apv_user_r1 = value;
            }
        }

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
        /// 负责人
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "ownerid@odata.bind")]
        public EntityReference ownerid
        {
            get
            {
                return _ownerid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("ownerid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("ownerid");
                }
                _ownerid = value;
            }
        }

        ///<summary>
        /// 负责的业务部门
        ///</summary>   
        [CFieldType(EntityName = "businessunit")]
        [JsonProperty(PropertyName = "owningbusinessunit@odata.bind")]
        public EntityReference owningbusinessunit
        {
            get
            {
                return _owningbusinessunit;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("owningbusinessunit");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("owningbusinessunit");
                }
                _owningbusinessunit = value;
            }
        }

        ///<summary>
        /// 负责团队
        ///</summary>   
        [CFieldType(EntityName = "team")]
        [JsonProperty(PropertyName = "owningteam@odata.bind")]
        public EntityReference owningteam
        {
            get
            {
                return _owningteam;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("owningteam");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("owningteam");
                }
                _owningteam = value;
            }
        }

        ///<summary>
        /// 负责人用户
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "owninguser@odata.bind")]
        public EntityReference owninguser
        {
            get
            {
                return _owninguser;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("owninguser");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("owninguser");
                }
                _owninguser = value;
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

        private HashSet<string> _modifyList = new HashSet<string>(); private string _apv_content = null; private EntityReference _apv_editor = null; private EntityReference _apv_insighttask_r1 = null; private bool? _apv_isread = null; private Guid? _apv_messageid = null; private EnumMessgtype? _apv_messgtype = null; private string _apv_name = null; private EntityReference _apv_project_r1 = null; private string _apv_remark = null; private string _apv_sender = null; private EntityReference _apv_task_r1 = null; private string _apv_uniquemarkcode = null; private EntityReference _apv_user_r1 = null; private EntityReference _createdby = null; private DateTime? _createdon = null; private EntityReference _modifiedby = null; private DateTime? _modifiedon = null; private EntityReference _modifiedonbehalfby = null; private EntityReference _ownerid = null; private EntityReference _owningbusinessunit = null; private EntityReference _owningteam = null; private EntityReference _owninguser = null; private int? _statecode = null; private int? _statuscode = null;
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
            return "apv_message";
        }
    }

    /// <summary>
    /// 客户账户信息
    /// apv_accountinfo
    /// </summary>
    public class Link_Apv_Message : Apv_Message
    {

        ///<summary>
        /// 客户付款信息
        ///</summary>    
        public Systemuser link_owner { get; set; }

    }

}
