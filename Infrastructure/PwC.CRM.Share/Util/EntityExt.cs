using Microsoft.Xrm.Sdk;

namespace PwC.CRM.Share.Util;

public static class EntityExt
{
    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string Str(this Entity entity, string attr) => entity.GetAttributeValue<string>(attr);
    /// <summary>
    /// 获取bool类型
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static bool Bool(this Entity entity, string attr) => entity.GetAttributeValue<bool>(attr);
    /// <summary>
    /// 获取数值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static int Int(this Entity entity, string attr) => entity.GetAttributeValue<int>(attr);
    /// <summary>
    /// 获取数值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static decimal Decimal(this Entity entity, string attr) => entity.GetAttributeValue<decimal>(attr);
    /// <summary>
    /// 获取数值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static double Double(this Entity entity, string attr) => entity.GetAttributeValue<double>(attr);
    /// <summary>
    /// 获取日期时间
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static DateTime DateTime(this Entity entity, string attr) => entity.GetAttributeValue<DateTime>(attr);
    /// <summary>
    /// 获取指定格式时间字符串
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string DateTimeStr(this Entity entity, string attr)
    {
        if (!entity.Attributes.ContainsKey(attr))
        {
            return null;
        }
        else
        {
            return entity.GetAttributeValue<DateTime>(attr).ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
    /// <summary>
    /// 获取查找类型的值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static AliasedValue AliasedValue(this Entity entity, string attr) => entity.GetAttributeValue<AliasedValue>(attr);
    /// <summary>
    /// 获取查找类型的值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string AliasedValueStr(this Entity entity, string attr) => entity.AliasedValue(attr)?.Value.ToString();
    /// <summary>
    /// 获取查找类型的ID
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static Guid? AliasedValueEntityID(this Entity entity, string attr) => (entity.AliasedValue(attr)?.Value as EntityReference)?.Id;
    /// <summary>
    /// 获取查找类型的值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string AliasedValueEntityValue(this Entity entity, string attr) => (entity.AliasedValue(attr)?.Value as EntityReference)?.Name;
    /// <summary>
    /// 获取主实体ID
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static Guid? Id(this Entity entity, string attr) => entity.GetAttributeValue<Guid?>(attr);
    /// <summary>
    /// 获取实体名称
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string RefName(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr)?.Name;
    /// <summary>
    /// 获取引查找实体ID
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static Guid? RefId(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr)?.Id;
    /// <summary>
    /// 获取查找实体Name
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string LogcalName(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr)?.LogicalName;
    /// <summary>
    /// 获取查找实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static EntityReference Ref(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr);
    /// <summary>
    /// 查找FormattedValue
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string FormattedValue(this Entity entity, string attr) => entity.FormattedValues.ContainsKey(attr) ? entity.FormattedValues[attr] : null;
}