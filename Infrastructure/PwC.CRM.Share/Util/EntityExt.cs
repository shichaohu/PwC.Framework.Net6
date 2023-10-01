using Microsoft.Xrm.Sdk;

namespace PwC.CRM.Share.Util;

public static class EntityExt
{
    /// <summary>
    /// ��ȡ�ַ���
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string Str(this Entity entity, string attr) => entity.GetAttributeValue<string>(attr);
    /// <summary>
    /// ��ȡbool����
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static bool Bool(this Entity entity, string attr) => entity.GetAttributeValue<bool>(attr);
    /// <summary>
    /// ��ȡ��ֵ
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static int Int(this Entity entity, string attr) => entity.GetAttributeValue<int>(attr);
    /// <summary>
    /// ��ȡ��ֵ
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static decimal Decimal(this Entity entity, string attr) => entity.GetAttributeValue<decimal>(attr);
    /// <summary>
    /// ��ȡ��ֵ
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static double Double(this Entity entity, string attr) => entity.GetAttributeValue<double>(attr);
    /// <summary>
    /// ��ȡ����ʱ��
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static DateTime DateTime(this Entity entity, string attr) => entity.GetAttributeValue<DateTime>(attr);
    /// <summary>
    /// ��ȡָ����ʽʱ���ַ���
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
    /// ��ȡ�������͵�ֵ
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static AliasedValue AliasedValue(this Entity entity, string attr) => entity.GetAttributeValue<AliasedValue>(attr);
    /// <summary>
    /// ��ȡ�������͵�ֵ
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string AliasedValueStr(this Entity entity, string attr) => entity.AliasedValue(attr)?.Value.ToString();
    /// <summary>
    /// ��ȡ�������͵�ID
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static Guid? AliasedValueEntityID(this Entity entity, string attr) => (entity.AliasedValue(attr)?.Value as EntityReference)?.Id;
    /// <summary>
    /// ��ȡ�������͵�ֵ
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string AliasedValueEntityValue(this Entity entity, string attr) => (entity.AliasedValue(attr)?.Value as EntityReference)?.Name;
    /// <summary>
    /// ��ȡ��ʵ��ID
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static Guid? Id(this Entity entity, string attr) => entity.GetAttributeValue<Guid?>(attr);
    /// <summary>
    /// ��ȡʵ������
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string RefName(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr)?.Name;
    /// <summary>
    /// ��ȡ������ʵ��ID
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static Guid? RefId(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr)?.Id;
    /// <summary>
    /// ��ȡ����ʵ��Name
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string LogcalName(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr)?.LogicalName;
    /// <summary>
    /// ��ȡ����ʵ��
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static EntityReference Ref(this Entity entity, string attr) => entity.GetAttributeValue<EntityReference>(attr);
    /// <summary>
    /// ����FormattedValue
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static string FormattedValue(this Entity entity, string attr) => entity.FormattedValues.ContainsKey(attr) ? entity.FormattedValues[attr] : null;
}