
namespace CrmModels
{
    /// <summary>
    /// 地址 1: 地址类型
    /// contact_address1_addresstypecode
    /// </summary>
    public enum EnumAddress1_addresstypecode
    {
        帐单邮寄地址 = 1,
        送货地址 = 2,
        主要地址 = 3,
        其他 = 4
    }

    /// <summary>
    /// 地址 1: 送货方式
    /// contact_address1_shippingmethodcode
    /// </summary>
    public enum EnumAddress1_shippingmethodcode
    {
        航空运输 = 1,
        邮递 = 5,
        铁路运输 = 10,
        海运 = 11,
        送货上门 = 12,
        特快专递 = 35
    }

    /// <summary>
    /// 地址 2: 地址类型
    /// contact_address2_addresstypecode
    /// </summary>
    public enum EnumAddress2_addresstypecode
    {
        默认值 = 1
    }

    /// <summary>
    /// 地址 2: 送货方式 
    /// contact_address2_shippingmethodcode
    /// </summary>
    public enum EnumAddress2_shippingmethodcode
    {
        默认值 = 1
    }
}
