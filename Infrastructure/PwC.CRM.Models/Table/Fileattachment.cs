using Newtonsoft.Json;
using PwC.CRM.Share.CRMClients.OData.Models;

namespace PwC.CRM.Models.Table
{
    /// <summary>
    /// FileAttachment
    /// fileattachment
    /// </summary>
    public class Fileattachment
    {


        ///<summary>
        /// Body
        ///</summary>    
        public string body
        {
            get
            {
                return _body;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("body");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("body");
                }
                _body = value;
            }
        }

        ///<summary>
        /// 创建日期
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
        /// FileAttachmentId
        ///</summary>    
        public Guid? fileattachmentid
        {
            get
            {
                return _fileattachmentid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("fileattachmentid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("fileattachmentid");
                }
                _fileattachmentid = value;
            }
        }

        ///<summary>
        /// 文件名
        ///</summary>    
        public string filename
        {
            get
            {
                return _filename;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("filename");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("filename");
                }
                _filename = value;
            }
        }

        ///<summary>
        /// 文件指针
        ///</summary>    
        public string filepointer
        {
            get
            {
                return _filepointer;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("filepointer");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("filepointer");
                }
                _filepointer = value;
            }
        }

        ///<summary>
        /// 文件大小(字节)
        ///</summary>    
        public long? filesizeinbytes
        {
            get
            {
                return _filesizeinbytes;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("filesizeinbytes");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("filesizeinbytes");
                }
                _filesizeinbytes = value;
            }
        }

        ///<summary>
        /// IsCommitted
        ///</summary>    
        public bool? iscommitted
        {
            get
            {
                return _iscommitted;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("iscommitted");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("iscommitted");
                }
                _iscommitted = value;
            }
        }

        ///<summary>
        /// MIME 类型
        ///</summary>    
        public string mimetype
        {
            get
            {
                return _mimetype;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("mimetype");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("mimetype");
                }
                _mimetype = value;
            }
        }

        ///<summary>
        /// 相关项
        ///</summary>   
        [CFieldType(EntityName = "account")]
        [JsonProperty(PropertyName = "objectid@odata.bind")]
        public EntityReference objectid
        {
            get
            {
                return _objectid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("objectid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("objectid");
                }
                _objectid = value;
            }
        }

        ///<summary>
        /// 对象 ID 类型代码
        ///</summary>    
        public string objectidtypecode
        {
            get
            {
                return _objectidtypecode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("objectidtypecode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("objectidtypecode");
                }
                _objectidtypecode = value;
            }
        }

        ///<summary>
        /// 对象类型 
        ///</summary>    
        public string objecttypecode
        {
            get
            {
                return _objecttypecode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("objecttypecode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("objecttypecode");
                }
                _objecttypecode = value;
            }
        }

        ///<summary>
        /// 前缀
        ///</summary>    
        public string prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("prefix");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("prefix");
                }
                _prefix = value;
            }
        }

        ///<summary>
        /// 关系属性架构名称
        ///</summary>    
        public string regardingfieldname
        {
            get
            {
                return _regardingfieldname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("regardingfieldname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("regardingfieldname");
                }
                _regardingfieldname = value;
            }
        }

        ///<summary>
        /// 存储指针
        ///</summary>    
        public string storagepointer
        {
            get
            {
                return _storagepointer;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("storagepointer");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("storagepointer");
                }
                _storagepointer = value;
            }
        }

        private HashSet<string> _modifyList = new HashSet<string>(); private string _body = null; private DateTime? _createdon = null; private Guid? _fileattachmentid = null; private string _filename = null; private string _filepointer = null; private long? _filesizeinbytes = null; private bool? _iscommitted = null; private string _mimetype = null; private EntityReference _objectid = null; private string _objectidtypecode = null; private string _objecttypecode = null; private string _prefix = null; private string _regardingfieldname = null; private string _storagepointer = null;
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
            return "fileattachment";
        }
    }

}
