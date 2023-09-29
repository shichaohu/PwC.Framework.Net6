
using Newtonsoft.Json;
using PwcNetCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CrmModels
{
    /// <summary>
    /// 用户
    /// systemuser
    /// </summary>
    public class Systemuser
    {


        ///<summary>
        /// 访问模式
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumAccessmode))]
        //public EnumAccessmode? accessmode
        //{
        //    get
        //    {
        //        return _accessmode;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("accessmode");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("accessmode");
        //        }
        //        _accessmode = value;
        //    }
        //}

        ///<summary>
        /// Active Directory GUID
        ///</summary>    
        public Guid? activedirectoryguid
        {
            get
            {
                return _activedirectoryguid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("activedirectoryguid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("activedirectoryguid");
                }
                _activedirectoryguid = value;
            }
        }

        ///<summary>
        /// 地址 1: ID
        ///</summary>    
        public Guid? address1_addressid
        {
            get
            {
                return _address1_addressid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_addressid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_addressid");
                }
                _address1_addressid = value;
            }
        }

        ///<summary>
        /// 地址 1: 地址类型
        ///</summary>    
        [CFieldType(EnumType = typeof(EnumAddress1_addresstypecode))]
        public EnumAddress1_addresstypecode? address1_addresstypecode
        {
            get
            {
                return _address1_addresstypecode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_addresstypecode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_addresstypecode");
                }
                _address1_addresstypecode = value;
            }
        }

        ///<summary>
        /// 市/县
        ///</summary>    
        public string address1_city
        {
            get
            {
                return _address1_city;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_city");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_city");
                }
                _address1_city = value;
            }
        }

        ///<summary>
        /// 地址
        ///</summary>    
        public string address1_composite
        {
            get
            {
                return _address1_composite;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_composite");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_composite");
                }
                _address1_composite = value;
            }
        }

        ///<summary>
        /// 国家/地区
        ///</summary>    
        public string address1_country
        {
            get
            {
                return _address1_country;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_country");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_country");
                }
                _address1_country = value;
            }
        }

        ///<summary>
        /// 地址 1: 县
        ///</summary>    
        public string address1_county
        {
            get
            {
                return _address1_county;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_county");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_county");
                }
                _address1_county = value;
            }
        }

        ///<summary>
        /// 地址 1: 传真
        ///</summary>    
        public string address1_fax
        {
            get
            {
                return _address1_fax;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_fax");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_fax");
                }
                _address1_fax = value;
            }
        }

        ///<summary>
        /// 地址 1: 纬度
        ///</summary>    
        public double? address1_latitude
        {
            get
            {
                return _address1_latitude;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_latitude");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_latitude");
                }
                _address1_latitude = value;
            }
        }

        ///<summary>
        /// 街道 1
        ///</summary>    
        public string address1_line1
        {
            get
            {
                return _address1_line1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_line1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_line1");
                }
                _address1_line1 = value;
            }
        }

        ///<summary>
        /// 街道 2
        ///</summary>    
        public string address1_line2
        {
            get
            {
                return _address1_line2;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_line2");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_line2");
                }
                _address1_line2 = value;
            }
        }

        ///<summary>
        /// 街道 3
        ///</summary>    
        public string address1_line3
        {
            get
            {
                return _address1_line3;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_line3");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_line3");
                }
                _address1_line3 = value;
            }
        }

        ///<summary>
        /// 地址 1: 经度
        ///</summary>    
        public double? address1_longitude
        {
            get
            {
                return _address1_longitude;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_longitude");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_longitude");
                }
                _address1_longitude = value;
            }
        }

        ///<summary>
        /// 地址 1: 名称
        ///</summary>    
        public string address1_name
        {
            get
            {
                return _address1_name;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_name");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_name");
                }
                _address1_name = value;
            }
        }

        ///<summary>
        /// 邮政编码
        ///</summary>    
        public string address1_postalcode
        {
            get
            {
                return _address1_postalcode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_postalcode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_postalcode");
                }
                _address1_postalcode = value;
            }
        }

        ///<summary>
        /// 地址 1: 邮政信箱
        ///</summary>    
        public string address1_postofficebox
        {
            get
            {
                return _address1_postofficebox;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_postofficebox");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_postofficebox");
                }
                _address1_postofficebox = value;
            }
        }

        ///<summary>
        /// 地址 1: 送货方式
        ///</summary>    
        [CFieldType(EnumType = typeof(EnumAddress1_shippingmethodcode))]
        public EnumAddress1_shippingmethodcode? address1_shippingmethodcode
        {
            get
            {
                return _address1_shippingmethodcode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_shippingmethodcode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_shippingmethodcode");
                }
                _address1_shippingmethodcode = value;
            }
        }

        ///<summary>
        /// 省/市/自治区
        ///</summary>    
        public string address1_stateorprovince
        {
            get
            {
                return _address1_stateorprovince;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_stateorprovince");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_stateorprovince");
                }
                _address1_stateorprovince = value;
            }
        }

        ///<summary>
        /// 主要电话
        ///</summary>    
        public string address1_telephone1
        {
            get
            {
                return _address1_telephone1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_telephone1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_telephone1");
                }
                _address1_telephone1 = value;
            }
        }

        ///<summary>
        /// 其他电话
        ///</summary>    
        public string address1_telephone2
        {
            get
            {
                return _address1_telephone2;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_telephone2");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_telephone2");
                }
                _address1_telephone2 = value;
            }
        }

        ///<summary>
        /// 寻呼机
        ///</summary>    
        public string address1_telephone3
        {
            get
            {
                return _address1_telephone3;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_telephone3");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_telephone3");
                }
                _address1_telephone3 = value;
            }
        }

        ///<summary>
        /// 地址 1: UPS 区域
        ///</summary>    
        public string address1_upszone
        {
            get
            {
                return _address1_upszone;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_upszone");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_upszone");
                }
                _address1_upszone = value;
            }
        }

        ///<summary>
        /// 地址 1: UTC 时差
        ///</summary>    
        public int? address1_utcoffset
        {
            get
            {
                return _address1_utcoffset;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address1_utcoffset");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address1_utcoffset");
                }
                _address1_utcoffset = value;
            }
        }

        ///<summary>
        /// 地址 2: ID
        ///</summary>    
        public Guid? address2_addressid
        {
            get
            {
                return _address2_addressid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_addressid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_addressid");
                }
                _address2_addressid = value;
            }
        }

        ///<summary>
        /// 地址 2: 地址类型
        ///</summary>    
        [CFieldType(EnumType = typeof(EnumAddress2_addresstypecode))]
        public EnumAddress2_addresstypecode? address2_addresstypecode
        {
            get
            {
                return _address2_addresstypecode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_addresstypecode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_addresstypecode");
                }
                _address2_addresstypecode = value;
            }
        }

        ///<summary>
        /// 其他市/县
        ///</summary>    
        public string address2_city
        {
            get
            {
                return _address2_city;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_city");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_city");
                }
                _address2_city = value;
            }
        }

        ///<summary>
        /// 其他地址
        ///</summary>    
        public string address2_composite
        {
            get
            {
                return _address2_composite;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_composite");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_composite");
                }
                _address2_composite = value;
            }
        }

        ///<summary>
        /// 其他国家/地区
        ///</summary>    
        public string address2_country
        {
            get
            {
                return _address2_country;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_country");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_country");
                }
                _address2_country = value;
            }
        }

        ///<summary>
        /// 地址 2: 县
        ///</summary>    
        public string address2_county
        {
            get
            {
                return _address2_county;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_county");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_county");
                }
                _address2_county = value;
            }
        }

        ///<summary>
        /// 地址 2: 传真
        ///</summary>    
        public string address2_fax
        {
            get
            {
                return _address2_fax;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_fax");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_fax");
                }
                _address2_fax = value;
            }
        }

        ///<summary>
        /// 地址 2: 纬度
        ///</summary>    
        public double? address2_latitude
        {
            get
            {
                return _address2_latitude;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_latitude");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_latitude");
                }
                _address2_latitude = value;
            }
        }

        ///<summary>
        /// 其他街道 1
        ///</summary>    
        public string address2_line1
        {
            get
            {
                return _address2_line1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_line1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_line1");
                }
                _address2_line1 = value;
            }
        }

        ///<summary>
        /// 其他街道 2
        ///</summary>    
        public string address2_line2
        {
            get
            {
                return _address2_line2;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_line2");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_line2");
                }
                _address2_line2 = value;
            }
        }

        ///<summary>
        /// 其他街道 3
        ///</summary>    
        public string address2_line3
        {
            get
            {
                return _address2_line3;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_line3");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_line3");
                }
                _address2_line3 = value;
            }
        }

        ///<summary>
        /// 地址 2: 经度
        ///</summary>    
        public double? address2_longitude
        {
            get
            {
                return _address2_longitude;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_longitude");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_longitude");
                }
                _address2_longitude = value;
            }
        }

        ///<summary>
        /// 地址 2: 名称
        ///</summary>    
        public string address2_name
        {
            get
            {
                return _address2_name;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_name");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_name");
                }
                _address2_name = value;
            }
        }

        ///<summary>
        /// 其他邮政编码
        ///</summary>    
        public string address2_postalcode
        {
            get
            {
                return _address2_postalcode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_postalcode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_postalcode");
                }
                _address2_postalcode = value;
            }
        }

        ///<summary>
        /// 地址 2: 邮政信箱
        ///</summary>    
        public string address2_postofficebox
        {
            get
            {
                return _address2_postofficebox;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_postofficebox");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_postofficebox");
                }
                _address2_postofficebox = value;
            }
        }

        ///<summary>
        /// 地址 2: 送货方式
        ///</summary>    
        [CFieldType(EnumType = typeof(EnumAddress2_shippingmethodcode))]
        public EnumAddress2_shippingmethodcode? address2_shippingmethodcode
        {
            get
            {
                return _address2_shippingmethodcode;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_shippingmethodcode");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_shippingmethodcode");
                }
                _address2_shippingmethodcode = value;
            }
        }

        ///<summary>
        /// 其他省/市/自治区
        ///</summary>    
        public string address2_stateorprovince
        {
            get
            {
                return _address2_stateorprovince;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_stateorprovince");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_stateorprovince");
                }
                _address2_stateorprovince = value;
            }
        }

        ///<summary>
        /// 地址 2: 电话 1
        ///</summary>    
        public string address2_telephone1
        {
            get
            {
                return _address2_telephone1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_telephone1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_telephone1");
                }
                _address2_telephone1 = value;
            }
        }

        ///<summary>
        /// 地址 2: 电话 2
        ///</summary>    
        public string address2_telephone2
        {
            get
            {
                return _address2_telephone2;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_telephone2");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_telephone2");
                }
                _address2_telephone2 = value;
            }
        }

        ///<summary>
        /// 地址 2: 电话 3
        ///</summary>    
        public string address2_telephone3
        {
            get
            {
                return _address2_telephone3;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_telephone3");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_telephone3");
                }
                _address2_telephone3 = value;
            }
        }

        ///<summary>
        /// 地址 2: UPS 区域
        ///</summary>    
        public string address2_upszone
        {
            get
            {
                return _address2_upszone;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_upszone");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_upszone");
                }
                _address2_upszone = value;
            }
        }

        ///<summary>
        /// 地址 2: UTC 时差
        ///</summary>    
        public int? address2_utcoffset
        {
            get
            {
                return _address2_utcoffset;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("address2_utcoffset");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("address2_utcoffset");
                }
                _address2_utcoffset = value;
            }
        }

        ///<summary>
        /// 应用程序 ID
        ///</summary>    
        public Guid? applicationid
        {
            get
            {
                return _applicationid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("applicationid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("applicationid");
                }
                _applicationid = value;
            }
        }

        ///<summary>
        /// 应用程序 ID URI
        ///</summary>    
        public string applicationiduri
        {
            get
            {
                return _applicationiduri;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("applicationiduri");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("applicationiduri");
                }
                _applicationiduri = value;
            }
        }

        ///<summary>
        /// 区域
        ///</summary>   
        [CFieldType(EntityName = "apv_area")]
        [JsonProperty(PropertyName = "apv_area_r1@odata.bind")]
        public EntityReference apv_area_r1
        {
            get
            {
                return _apv_area_r1;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_area_r1");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_area_r1");
                }
                _apv_area_r1 = value;
            }
        }

        ///<summary>
        /// 业务角色
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumBusinessrole))]
        //public EnumBusinessrole? apv_businessrole
        //{
        //    get
        //    {
        //        return _apv_businessrole;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("apv_businessrole");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("apv_businessrole");
        //        }
        //        _apv_businessrole = value;
        //    }
        //}

        ///<summary>
        /// 纷享系统ID
        ///</summary>    
        public string apv_fxid
        {
            get
            {
                return _apv_fxid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_fxid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_fxid");
                }
                _apv_fxid = value;
            }
        }

        ///<summary>
        /// 性别
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumGender))]
        //public EnumGender? apv_gender
        //{
        //    get
        //    {
        //        return _apv_gender;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("apv_gender");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("apv_gender");
        //        }
        //        _apv_gender = value;
        //    }
        //}

        ///<summary>
        /// 门店信息完整度%
        ///</summary>    
        public decimal? apv_informationintegrity
        {
            get
            {
                return _apv_informationintegrity;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_informationintegrity");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_informationintegrity");
                }
                _apv_informationintegrity = value;
            }
        }

        ///<summary>
        /// 是否兼职
        ///</summary>    
        public bool? apv_isparttime
        {
            get
            {
                return _apv_isparttime;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_isparttime");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_isparttime");
                }
                _apv_isparttime = value;
            }
        }

        ///<summary>
        /// 入职日期
        ///</summary>   
        [JsonConverter(typeof(ShortDateFormat))]
        public DateTime? apv_onboardingdate
        {
            get
            {
                return _apv_onboardingdate;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_onboardingdate");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_onboardingdate");
                }
                _apv_onboardingdate = value;
            }
        }

        ///<summary>
        /// 门店数汇总
        ///</summary>    
        public int? apv_shopcount
        {
            get
            {
                return _apv_shopcount;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_shopcount");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_shopcount");
                }
                _apv_shopcount = value;
            }
        }

        ///<summary>
        /// 门店数汇总 (上次更新日期)
        ///</summary>    
        public DateTime? apv_shopcount_date
        {
            get
            {
                return _apv_shopcount_date;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_shopcount_date");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_shopcount_date");
                }
                _apv_shopcount_date = value;
            }
        }

        ///<summary>
        /// 门店数汇总 (状态)
        ///</summary>    
        public int? apv_shopcount_state
        {
            get
            {
                return _apv_shopcount_state;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_shopcount_state");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_shopcount_state");
                }
                _apv_shopcount_state = value;
            }
        }

        ///<summary>
        /// 门店完整度汇总
        ///</summary>    
        public decimal? apv_sumintegrity
        {
            get
            {
                return _apv_sumintegrity;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_sumintegrity");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_sumintegrity");
                }
                _apv_sumintegrity = value;
            }
        }

        ///<summary>
        /// 门店完整度汇总 (上次更新日期)
        ///</summary>    
        public DateTime? apv_sumintegrity_date
        {
            get
            {
                return _apv_sumintegrity_date;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_sumintegrity_date");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_sumintegrity_date");
                }
                _apv_sumintegrity_date = value;
            }
        }

        ///<summary>
        /// 门店完整度汇总 (状态)
        ///</summary>    
        public int? apv_sumintegrity_state
        {
            get
            {
                return _apv_sumintegrity_state;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("apv_sumintegrity_state");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("apv_sumintegrity_state");
                }
                _apv_sumintegrity_state = value;
            }
        }

        ///<summary>
        /// Azure AD 对象 ID
        ///</summary>    
        public Guid? azureactivedirectoryobjectid
        {
            get
            {
                return _azureactivedirectoryobjectid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("azureactivedirectoryobjectid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("azureactivedirectoryobjectid");
                }
                _azureactivedirectoryobjectid = value;
            }
        }

        ///<summary>
        /// Azure 删除时间
        ///</summary>    
        public DateTime? azuredeletedon
        {
            get
            {
                return _azuredeletedon;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("azuredeletedon");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("azuredeletedon");
                }
                _azuredeletedon = value;
            }
        }

        ///<summary>
        /// Azure 状态
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumAzurestate))]
        //public EnumAzurestate? azurestate
        //{
        //    get
        //    {
        //        return _azurestate;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("azurestate");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("azurestate");
        //        }
        //        _azurestate = value;
        //    }
        //}

        ///<summary>
        /// 业务部门
        ///</summary>   
        [CFieldType(EntityName = "businessunit")]
        [JsonProperty(PropertyName = "businessunitid@odata.bind")]
        public EntityReference businessunitid
        {
            get
            {
                return _businessunitid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("businessunitid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("businessunitid");
                }
                _businessunitid = value;
            }
        }

        ///<summary>
        /// 日历
        ///</summary>   
        [CFieldType(EntityName = "calendar")]
        [JsonProperty(PropertyName = "calendarid@odata.bind")]
        public EntityReference calendarid
        {
            get
            {
                return _calendarid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("calendarid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("calendarid");
                }
                _calendarid = value;
            }
        }

        ///<summary>
        /// 许可证类型
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumCaltype))]
        //public EnumCaltype? caltype
        //{
        //    get
        //    {
        //        return _caltype;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("caltype");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("caltype");
        //        }
        //        _caltype = value;
        //    }
        //}

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
        /// 已填充默认筛选器
        ///</summary>    
        public bool? defaultfilterspopulated
        {
            get
            {
                return _defaultfilterspopulated;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("defaultfilterspopulated");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("defaultfilterspopulated");
                }
                _defaultfilterspopulated = value;
            }
        }

        ///<summary>
        /// 邮箱
        ///</summary>   
        [CFieldType(EntityName = "mailbox")]
        [JsonProperty(PropertyName = "defaultmailbox@odata.bind")]
        public EntityReference defaultmailbox
        {
            get
            {
                return _defaultmailbox;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("defaultmailbox");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("defaultmailbox");
                }
                _defaultmailbox = value;
            }
        }

        ///<summary>
        /// 默认 OneDrive for Business 文件夹名称
        ///</summary>    
        public string defaultodbfoldername
        {
            get
            {
                return _defaultodbfoldername;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("defaultodbfoldername");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("defaultodbfoldername");
                }
                _defaultodbfoldername = value;
            }
        }

        ///<summary>
        /// “已删除”状态
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumDeletestate))]
        //public EnumDeletestate? deletedstate
        //{
        //    get
        //    {
        //        return _deletedstate;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("deletedstate");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("deletedstate");
        //        }
        //        _deletedstate = value;
        //    }
        //}

        ///<summary>
        /// 禁用的原因
        ///</summary>    
        public string disabledreason
        {
            get
            {
                return _disabledreason;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("disabledreason");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("disabledreason");
                }
                _disabledreason = value;
            }
        }

        ///<summary>
        /// 在服务视图上显示
        ///</summary>    
        public bool? displayinserviceviews
        {
            get
            {
                return _displayinserviceviews;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("displayinserviceviews");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("displayinserviceviews");
                }
                _displayinserviceviews = value;
            }
        }

        ///<summary>
        /// 用户名
        ///</summary>    
        public string domainname
        {
            get
            {
                return _domainname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("domainname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("domainname");
                }
                _domainname = value;
            }
        }

        ///<summary>
        /// 主要电子邮件状态
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumEmailrouteraccessapproval))]
        //public EnumEmailrouteraccessapproval? emailrouteraccessapproval
        //{
        //    get
        //    {
        //        return _emailrouteraccessapproval;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("emailrouteraccessapproval");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("emailrouteraccessapproval");
        //        }
        //        _emailrouteraccessapproval = value;
        //    }
        //}

        ///<summary>
        /// 员工
        ///</summary>    
        public string employeeid
        {
            get
            {
                return _employeeid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("employeeid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("employeeid");
                }
                _employeeid = value;
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
        /// 名
        ///</summary>    
        public string firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("firstname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("firstname");
                }
                _firstname = value;
            }
        }

        ///<summary>
        /// 全名
        ///</summary>    
        public string fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("fullname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("fullname");
                }
                _fullname = value;
            }
        }

        ///<summary>
        /// 身份证
        ///</summary>    
        public string governmentid
        {
            get
            {
                return _governmentid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("governmentid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("governmentid");
                }
                _governmentid = value;
            }
        }

        ///<summary>
        /// 住宅电话
        ///</summary>    
        public string homephone
        {
            get
            {
                return _homephone;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("homephone");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("homephone");
                }
                _homephone = value;
            }
        }

        ///<summary>
        /// 唯一的用户标识 ID
        ///</summary>    
        public int? identityid
        {
            get
            {
                return _identityid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("identityid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("identityid");
                }
                _identityid = value;
            }
        }

        ///<summary>
        /// 接收电子邮件的传送方式
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumIncomingemaildeliverymethod))]
        //public EnumIncomingemaildeliverymethod? incomingemaildeliverymethod
        //{
        //    get
        //    {
        //        return _incomingemaildeliverymethod;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("incomingemaildeliverymethod");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("incomingemaildeliverymethod");
        //        }
        //        _incomingemaildeliverymethod = value;
        //    }
        //}

        ///<summary>
        /// 主要电子邮件
        ///</summary>    
        public string internalemailaddress
        {
            get
            {
                return _internalemailaddress;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("internalemailaddress");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("internalemailaddress");
                }
                _internalemailaddress = value;
            }
        }

        ///<summary>
        /// 邀请状态
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumInvitestatuscode))]
        //public EnumInvitestatuscode? invitestatuscode
        //{
        //    get
        //    {
        //        return _invitestatuscode;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("invitestatuscode");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("invitestatuscode");
        //        }
        //        _invitestatuscode = value;
        //    }
        //}

        ///<summary>
        /// 是 Active Directory 用户
        ///</summary>    
        public bool? isactivedirectoryuser
        {
            get
            {
                return _isactivedirectoryuser;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("isactivedirectoryuser");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("isactivedirectoryuser");
                }
                _isactivedirectoryuser = value;
            }
        }

        ///<summary>
        /// 状态
        ///</summary>    
        public bool? isdisabled
        {
            get
            {
                return _isdisabled;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("isdisabled");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("isdisabled");
                }
                _isdisabled = value;
            }
        }

        ///<summary>
        /// 电子邮件地址 O365 管理审批状态
        ///</summary>    
        public bool? isemailaddressapprovedbyo365admin
        {
            get
            {
                return _isemailaddressapprovedbyo365admin;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("isemailaddressapprovedbyo365admin");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("isemailaddressapprovedbyo365admin");
                }
                _isemailaddressapprovedbyo365admin = value;
            }
        }

        ///<summary>
        /// 集成用户模式
        ///</summary>    
        public bool? isintegrationuser
        {
            get
            {
                return _isintegrationuser;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("isintegrationuser");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("isintegrationuser");
                }
                _isintegrationuser = value;
            }
        }

        ///<summary>
        /// 用户已获许可
        ///</summary>    
        public bool? islicensed
        {
            get
            {
                return _islicensed;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("islicensed");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("islicensed");
                }
                _islicensed = value;
            }
        }

        ///<summary>
        /// 已同步用户
        ///</summary>    
        public bool? issyncwithdirectory
        {
            get
            {
                return _issyncwithdirectory;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("issyncwithdirectory");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("issyncwithdirectory");
                }
                _issyncwithdirectory = value;
            }
        }

        ///<summary>
        /// 职务
        ///</summary>    
        public string jobtitle
        {
            get
            {
                return _jobtitle;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("jobtitle");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("jobtitle");
                }
                _jobtitle = value;
            }
        }

        ///<summary>
        /// 姓
        ///</summary>    
        public string lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("lastname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("lastname");
                }
                _lastname = value;
            }
        }

        ///<summary>
        /// 上次用户更新时间
        ///</summary>    
        public DateTime? latestupdatetime
        {
            get
            {
                return _latestupdatetime;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("latestupdatetime");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("latestupdatetime");
                }
                _latestupdatetime = value;
            }
        }

        ///<summary>
        /// 中间名
        ///</summary>    
        public string middlename
        {
            get
            {
                return _middlename;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("middlename");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("middlename");
                }
                _middlename = value;
            }
        }

        ///<summary>
        /// 手机通知电子邮件
        ///</summary>    
        public string mobilealertemail
        {
            get
            {
                return _mobilealertemail;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("mobilealertemail");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("mobilealertemail");
                }
                _mobilealertemail = value;
            }
        }

        ///<summary>
        /// Mobile Offline 配置文件
        ///</summary>   
        [CFieldType(EntityName = "mobileofflineprofile")]
        [JsonProperty(PropertyName = "mobileofflineprofileid@odata.bind")]
        public EntityReference mobileofflineprofileid
        {
            get
            {
                return _mobileofflineprofileid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("mobileofflineprofileid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("mobileofflineprofileid");
                }
                _mobileofflineprofileid = value;
            }
        }

        ///<summary>
        /// 移动电话
        ///</summary>    
        public string mobilephone
        {
            get
            {
                return _mobilephone;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("mobilephone");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("mobilephone");
                }
                _mobilephone = value;
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
        /// 用户类型
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumSystemuser_msdyn_agenttype))]
        //public EnumSystemuser_msdyn_agenttype? msdyn_agentType
        //{
        //    get
        //    {
        //        return _msdyn_agentType;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("msdyn_agentType");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("msdyn_agentType");
        //        }
        //        _msdyn_agentType = value;
        //    }
        //}

        ///<summary>
        /// 机器人应用程序 ID
        ///</summary>    
        public string msdyn_botapplicationid
        {
            get
            {
                return _msdyn_botapplicationid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_botapplicationid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_botapplicationid");
                }
                _msdyn_botapplicationid = value;
            }
        }

        ///<summary>
        /// 说明
        ///</summary>    
        public string msdyn_botdescription
        {
            get
            {
                return _msdyn_botdescription;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_botdescription");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_botdescription");
                }
                _msdyn_botdescription = value;
            }
        }

        ///<summary>
        /// 终结点
        ///</summary>    
        public string msdyn_botendpoint
        {
            get
            {
                return _msdyn_botendpoint;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_botendpoint");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_botendpoint");
                }
                _msdyn_botendpoint = value;
            }
        }

        ///<summary>
        /// 机器人句柄
        ///</summary>    
        public string msdyn_bothandle
        {
            get
            {
                return _msdyn_bothandle;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_bothandle");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_bothandle");
                }
                _msdyn_bothandle = value;
            }
        }

        ///<summary>
        /// 机器人提供程序
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumSystemuser_msdyn_botprovider))]
        //public EnumSystemuser_msdyn_botprovider? msdyn_botprovider
        //{
        //    get
        //    {
        //        return _msdyn_botprovider;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("msdyn_botprovider");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("msdyn_botprovider");
        //        }
        //        _msdyn_botprovider = value;
        //    }
        //}

        ///<summary>
        /// 密钥
        ///</summary>    
        public string msdyn_botsecretkeys
        {
            get
            {
                return _msdyn_botsecretkeys;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_botsecretkeys");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_botsecretkeys");
                }
                _msdyn_botsecretkeys = value;
            }
        }

        ///<summary>
        /// 产能
        ///</summary>    
        public int? msdyn_capacity
        {
            get
            {
                return _msdyn_capacity;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_capacity");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_capacity");
                }
                _msdyn_capacity = value;
            }
        }

        ///<summary>
        /// 默认状态
        ///</summary>   
        [CFieldType(EntityName = "msdyn_presence")]
        [JsonProperty(PropertyName = "msdyn_defaultpresenceiduser@odata.bind")]
        public EntityReference msdyn_defaultpresenceiduser
        {
            get
            {
                return _msdyn_defaultpresenceiduser;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_defaultpresenceiduser");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_defaultpresenceiduser");
                }
                _msdyn_defaultpresenceiduser = value;
            }
        }

        ///<summary>
        /// GDPR 选择退出
        ///</summary>    
        public bool? msdyn_gdproptout
        {
            get
            {
                return _msdyn_gdproptout;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_gdproptout");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_gdproptout");
                }
                _msdyn_gdproptout = value;
            }
        }

        ///<summary>
        /// 网格包装器控件字段
        ///</summary>    
        public string msdyn_gridwrappercontrolfield
        {
            get
            {
                return _msdyn_gridwrappercontrolfield;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_gridwrappercontrolfield");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_gridwrappercontrolfield");
                }
                _msdyn_gridwrappercontrolfield = value;
            }
        }

        ///<summary>
        /// 已为专家启用召集
        ///</summary>    
        public bool? msdyn_isexpertenabledforswarm
        {
            get
            {
                return _msdyn_isexpertenabledforswarm;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_isexpertenabledforswarm");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_isexpertenabledforswarm");
                }
                _msdyn_isexpertenabledforswarm = value;
            }
        }

        ///<summary>
        /// 负责环境 ID
        ///</summary>    
        public string msdyn_owningenvironmentid
        {
            get
            {
                return _msdyn_owningenvironmentid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("msdyn_owningenvironmentid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("msdyn_owningenvironmentid");
                }
                _msdyn_owningenvironmentid = value;
            }
        }

        ///<summary>
        /// 类型
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumSystemuser_msdyn_usertype))]
        //public EnumSystemuser_msdyn_usertype? msdyn_usertype
        //{
        //    get
        //    {
        //        return _msdyn_usertype;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("msdyn_usertype");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("msdyn_usertype");
        //        }
        //        _msdyn_usertype = value;
        //    }
        //}

        ///<summary>
        /// 昵称
        ///</summary>    
        public string nickname
        {
            get
            {
                return _nickname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("nickname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("nickname");
                }
                _nickname = value;
            }
        }

        ///<summary>
        /// 组织 
        ///</summary>    
        public Guid? organizationid
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
        /// 外发的电子邮件的传送方式
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumOutgoingemaildeliverymethod))]
        //public EnumOutgoingemaildeliverymethod? outgoingemaildeliverymethod
        //{
        //    get
        //    {
        //        return _outgoingemaildeliverymethod;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("outgoingemaildeliverymethod");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("outgoingemaildeliverymethod");
        //        }
        //        _outgoingemaildeliverymethod = value;
        //    }
        //}

        ///<summary>
        /// 经理
        ///</summary>   
        [CFieldType(EntityName = "systemuser")]
        [JsonProperty(PropertyName = "parentsystemuserid@odata.bind")]
        public EntityReference parentsystemuserid
        {
            get
            {
                return _parentsystemuserid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("parentsystemuserid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("parentsystemuserid");
                }
                _parentsystemuserid = value;
            }
        }

        ///<summary>
        /// Passport Hi
        ///</summary>    
        public int? passporthi
        {
            get
            {
                return _passporthi;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("passporthi");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("passporthi");
                }
                _passporthi = value;
            }
        }

        ///<summary>
        /// Passport Lo
        ///</summary>    
        public int? passportlo
        {
            get
            {
                return _passportlo;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("passportlo");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("passportlo");
                }
                _passportlo = value;
            }
        }

        ///<summary>
        /// 电子邮件 2
        ///</summary>    
        public string personalemailaddress
        {
            get
            {
                return _personalemailaddress;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("personalemailaddress");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("personalemailaddress");
                }
                _personalemailaddress = value;
            }
        }

        ///<summary>
        /// 照片 URL
        ///</summary>    
        public string photourl
        {
            get
            {
                return _photourl;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("photourl");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("photourl");
                }
                _photourl = value;
            }
        }

        ///<summary>
        /// 位置
        ///</summary>   
        [CFieldType(EntityName = "position")]
        [JsonProperty(PropertyName = "positionid@odata.bind")]
        public EntityReference positionid
        {
            get
            {
                return _positionid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("positionid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("positionid");
                }
                _positionid = value;
            }
        }

        ///<summary>
        /// 首选地址
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumPreferredaddresscode))]
        //public EnumPreferredaddresscode? preferredaddresscode
        //{
        //    get
        //    {
        //        return _preferredaddresscode;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("preferredaddresscode");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("preferredaddresscode");
        //        }
        //        _preferredaddresscode = value;
        //    }
        //}

        ///<summary>
        /// 首选电子邮件
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumPreferredemailcode))]
        //public EnumPreferredemailcode? preferredemailcode
        //{
        //    get
        //    {
        //        return _preferredemailcode;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("preferredemailcode");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("preferredemailcode");
        //        }
        //        _preferredemailcode = value;
        //    }
        //}

        ///<summary>
        /// 首选电话
        ///</summary>    
        //[CFieldType(EnumType = typeof(EnumPreferredphonecode))]
        //public EnumPreferredphonecode? preferredphonecode
        //{
        //    get
        //    {
        //        return _preferredphonecode;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            _modifyList.Add("preferredphonecode");
        //        }
        //        else if (_modifyList.Count > 0)
        //        {

        //            _modifyList.Remove("preferredphonecode");
        //        }
        //        _preferredphonecode = value;
        //    }
        //}

        ///<summary>
        /// 流程
        ///</summary>    
        public Guid? processid
        {
            get
            {
                return _processid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("processid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("processid");
                }
                _processid = value;
            }
        }

        ///<summary>
        /// 默认队列
        ///</summary>   
        [CFieldType(EntityName = "queue")]
        [JsonProperty(PropertyName = "queueid@odata.bind")]
        public EntityReference queueid
        {
            get
            {
                return _queueid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("queueid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("queueid");
                }
                _queueid = value;
            }
        }

        ///<summary>
        /// 称呼
        ///</summary>    
        public string salutation
        {
            get
            {
                return _salutation;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("salutation");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("salutation");
                }
                _salutation = value;
            }
        }

        ///<summary>
        /// 限制访问模式
        ///</summary>    
        public bool? setupuser
        {
            get
            {
                return _setupuser;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("setupuser");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("setupuser");
                }
                _setupuser = value;
            }
        }

        ///<summary>
        /// SharePoint 电子邮件地址
        ///</summary>    
        public string sharepointemailaddress
        {
            get
            {
                return _sharepointemailaddress;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("sharepointemailaddress");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("sharepointemailaddress");
                }
                _sharepointemailaddress = value;
            }
        }

        ///<summary>
        /// 场所
        ///</summary>   
        [CFieldType(EntityName = "site")]
        [JsonProperty(PropertyName = "siteid@odata.bind")]
        public EntityReference siteid
        {
            get
            {
                return _siteid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("siteid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("siteid");
                }
                _siteid = value;
            }
        }

        ///<summary>
        /// 技能
        ///</summary>    
        public string skills
        {
            get
            {
                return _skills;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("skills");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("skills");
                }
                _skills = value;
            }
        }

        ///<summary>
        /// (已弃用)流程阶段
        ///</summary>    
        public Guid? stageid
        {
            get
            {
                return _stageid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("stageid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("stageid");
                }
                _stageid = value;
            }
        }

        ///<summary>
        /// 用户
        ///</summary>    
        public Guid? systemuserid
        {
            get
            {
                return _systemuserid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("systemuserid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("systemuserid");
                }
                _systemuserid = value;
            }
        }

        ///<summary>
        /// 区域
        ///</summary>   
        [CFieldType(EntityName = "territory")]
        [JsonProperty(PropertyName = "territoryid@odata.bind")]
        public EntityReference territoryid
        {
            get
            {
                return _territoryid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("territoryid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("territoryid");
                }
                _territoryid = value;
            }
        }

        ///<summary>
        /// 标题
        ///</summary>    
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("title");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("title");
                }
                _title = value;
            }
        }

        ///<summary>
        /// 货币
        ///</summary>   
        [CFieldType(EntityName = "transactioncurrency")]
        [JsonProperty(PropertyName = "transactioncurrencyid@odata.bind")]
        public EntityReference transactioncurrencyid
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
        /// (已弃用)遍历的路径
        ///</summary>    
        public string traversedpath
        {
            get
            {
                return _traversedpath;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("traversedpath");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("traversedpath");
                }
                _traversedpath = value;
            }
        }

        ///<summary>
        /// 用户许可证类型
        ///</summary>    
        public int? userlicensetype
        {
            get
            {
                return _userlicensetype;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("userlicensetype");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("userlicensetype");
                }
                _userlicensetype = value;
            }
        }

        ///<summary>
        /// 用户 PUID
        ///</summary>    
        public string userpuid
        {
            get
            {
                return _userpuid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("userpuid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("userpuid");
                }
                _userpuid = value;
            }
        }

        ///<summary>
        /// Windows Live ID
        ///</summary>    
        public string windowsliveid
        {
            get
            {
                return _windowsliveid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("windowsliveid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("windowsliveid");
                }
                _windowsliveid = value;
            }
        }

        ///<summary>
        /// Yammer 电子邮件
        ///</summary>    
        public string yammeremailaddress
        {
            get
            {
                return _yammeremailaddress;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("yammeremailaddress");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("yammeremailaddress");
                }
                _yammeremailaddress = value;
            }
        }

        ///<summary>
        /// Yammer 用户 ID
        ///</summary>    
        public string yammeruserid
        {
            get
            {
                return _yammeruserid;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("yammeruserid");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("yammeruserid");
                }
                _yammeruserid = value;
            }
        }

        ///<summary>
        /// Yomi 名
        ///</summary>    
        public string yomifirstname
        {
            get
            {
                return _yomifirstname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("yomifirstname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("yomifirstname");
                }
                _yomifirstname = value;
            }
        }

        ///<summary>
        /// Yomi 全名
        ///</summary>    
        public string yomifullname
        {
            get
            {
                return _yomifullname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("yomifullname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("yomifullname");
                }
                _yomifullname = value;
            }
        }

        ///<summary>
        /// Yomi 姓
        ///</summary>    
        public string yomilastname
        {
            get
            {
                return _yomilastname;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("yomilastname");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("yomilastname");
                }
                _yomilastname = value;
            }
        }

        ///<summary>
        /// Yomi 中间名
        ///</summary>    
        public string yomimiddlename
        {
            get
            {
                return _yomimiddlename;
            }
            set
            {
                if (value == null)
                {
                    _modifyList.Add("yomimiddlename");
                }
                else if (_modifyList.Count > 0)
                {

                    _modifyList.Remove("yomimiddlename");
                }
                _yomimiddlename = value;
            }
        }

        private HashSet<string> _modifyList = new HashSet<string>();  private Guid? _activedirectoryguid = null; private Guid? _address1_addressid = null; private EnumAddress1_addresstypecode? _address1_addresstypecode = null; private string _address1_city = null; private string _address1_composite = null; private string _address1_country = null; private string _address1_county = null; private string _address1_fax = null; private double? _address1_latitude = null; private string _address1_line1 = null; private string _address1_line2 = null; private string _address1_line3 = null; private double? _address1_longitude = null; private string _address1_name = null; private string _address1_postalcode = null; private string _address1_postofficebox = null; private EnumAddress1_shippingmethodcode? _address1_shippingmethodcode = null; private string _address1_stateorprovince = null; private string _address1_telephone1 = null; private string _address1_telephone2 = null; private string _address1_telephone3 = null; private string _address1_upszone = null; private int? _address1_utcoffset = null; private Guid? _address2_addressid = null; private EnumAddress2_addresstypecode? _address2_addresstypecode = null; private string _address2_city = null; private string _address2_composite = null; private string _address2_country = null; private string _address2_county = null; private string _address2_fax = null; private double? _address2_latitude = null; private string _address2_line1 = null; private string _address2_line2 = null; private string _address2_line3 = null; private double? _address2_longitude = null; private string _address2_name = null; private string _address2_postalcode = null; private string _address2_postofficebox = null; private EnumAddress2_shippingmethodcode? _address2_shippingmethodcode = null; private string _address2_stateorprovince = null; private string _address2_telephone1 = null; private string _address2_telephone2 = null; private string _address2_telephone3 = null; private string _address2_upszone = null; private int? _address2_utcoffset = null; private Guid? _applicationid = null; private string _applicationiduri = null; private EntityReference _apv_area_r1 = null; private string _apv_fxid = null; private decimal? _apv_informationintegrity = null; private bool? _apv_isparttime = null; private DateTime? _apv_onboardingdate = null; private int? _apv_shopcount = null; private DateTime? _apv_shopcount_date = null; private int? _apv_shopcount_state = null; private decimal? _apv_sumintegrity = null; private DateTime? _apv_sumintegrity_date = null; private int? _apv_sumintegrity_state = null; private Guid? _azureactivedirectoryobjectid = null; private DateTime? _azuredeletedon = null; private EntityReference _businessunitid = null; private EntityReference _calendarid = null;  private EntityReference _createdby = null; private DateTime? _createdon = null; private bool? _defaultfilterspopulated = null; private EntityReference _defaultmailbox = null; private string _defaultodbfoldername = null;  private string _disabledreason = null; private bool? _displayinserviceviews = null; private string _domainname = null;  private string _employeeid = null; private string _entityimage = null; private long? _entityimage_timestamp = null; private string _entityimage_url = null; private Guid? _entityimageid = null; private decimal? _exchangerate = null; private string _firstname = null; private string _fullname = null; private string _governmentid = null; private string _homephone = null; private int? _identityid = null; private string _internalemailaddress = null; private bool? _isactivedirectoryuser = null; private bool? _isdisabled = null; private bool? _isemailaddressapprovedbyo365admin = null; private bool? _isintegrationuser = null; private bool? _islicensed = null; private bool? _issyncwithdirectory = null; private string _jobtitle = null; private string _lastname = null; private DateTime? _latestupdatetime = null; private string _middlename = null; private string _mobilealertemail = null; private EntityReference _mobileofflineprofileid = null; private string _mobilephone = null; private EntityReference _modifiedby = null; private DateTime? _modifiedon = null; private EntityReference _modifiedonbehalfby = null;  private string _msdyn_botapplicationid = null; private string _msdyn_botdescription = null; private string _msdyn_botendpoint = null; private string _msdyn_bothandle = null;  private string _msdyn_botsecretkeys = null; private int? _msdyn_capacity = null; private EntityReference _msdyn_defaultpresenceiduser = null; private bool? _msdyn_gdproptout = null; private string _msdyn_gridwrappercontrolfield = null; private bool? _msdyn_isexpertenabledforswarm = null; private string _msdyn_owningenvironmentid = null;  private string _nickname = null; private Guid? _organizationid = null; private EntityReference _parentsystemuserid = null; private int? _passporthi = null; private int? _passportlo = null; private string _personalemailaddress = null; private string _photourl = null; private EntityReference _positionid = null; private Guid? _processid = null; private EntityReference _queueid = null; private string _salutation = null; private bool? _setupuser = null; private string _sharepointemailaddress = null; private EntityReference _siteid = null; private string _skills = null; private Guid? _stageid = null; private Guid? _systemuserid = null; private EntityReference _territoryid = null; private string _title = null; private EntityReference _transactioncurrencyid = null; private string _traversedpath = null; private int? _userlicensetype = null; private string _userpuid = null; private string _windowsliveid = null; private string _yammeremailaddress = null; private string _yammeruserid = null; private string _yomifirstname = null; private string _yomifullname = null; private string _yomilastname = null; private string _yomimiddlename = null;
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
            return "systemuser";
        }
    }

}
