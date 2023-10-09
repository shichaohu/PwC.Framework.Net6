using Microsoft.Xrm.Sdk;
using System.Reflection;
using EntityReference = Microsoft.Xrm.Sdk.EntityReference;

namespace PwC.CRM.Share.Extensions;

public static class EntityExtensions
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

    /// <summary>
    /// ��entityת����model
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static T ToModel<T>(this Entity entity) where T : class, new()
    {
        var obj = Activator.CreateInstance(typeof(T));
        PropertyInfo[] modelPropertys = typeof(T).GetProperties();//��ȡĿ�Ķ��������
        foreach (var mPro in modelPropertys)
        {
            try
            {
                //���Ŀ��������setȨ�ޣ����������Ĳ���
                if (mPro.CanWrite == false)
                {
                    continue;
                }
                if (mPro.PropertyType != typeof(string) && mPro.PropertyType.IsClass)//link����
                {
                    var linkObj = Activator.CreateInstance(mPro.PropertyType);
                    PropertyInfo[] linkPropertys = mPro.PropertyType.GetProperties();
                    foreach (var linkPro in linkPropertys)
                    {
                        try
                        {
                            var linkProvalue = entity.GetAttributeValue<AliasedValue>($"{mPro.Name}.{linkPro.Name}")?.Value;
                            var convertLinkValue = GetValueToDestinationType(linkProvalue, linkPro);
                            linkPro.SetValue(linkObj, convertLinkValue, null);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error happend when get value of [{typeof(T).Name}.{mPro.Name}.{linkPro.Name}], {ex.Message}");
                        }
                    }

                    mPro.SetValue(obj, linkObj, null);
                }
                else
                {
                    var proValue = GetAttributeValue(entity, mPro.Name);
                    var convertProValue = GetValueToDestinationType(proValue, mPro);
                    mPro.SetValue(obj, convertProValue, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error happend when get value of [{typeof(T).Name}.{mPro.Name}], {ex.Message}");
            }
        }

        return (T)obj;
    }


    /// <summary>
    /// ��entity listת����model list
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static List<T> ToModelList<T>(this IList<Entity> entities) where T : class, new()
    {
        var res = entities.Select(x => x.ToModel<T>()).ToList();
        return res;
    }

    /// <summary>
    /// ת����Ŀ������
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    private static object GetValueToDestinationType(object value, PropertyInfo propertyInfo)
    {
        Type proType = propertyInfo.PropertyType;
        if (value == null)
        {
            return value;
        }
        //��Ŀ������Ϊ��null����ʱ����Ҫ��ȡ����Ӧ�Ļ�������
        if (proType.IsGenericType
                && proType.GetGenericTypeDefinition() == typeof(Nullable<>)
                && proType.GenericTypeArguments.Length > 0) //���Ҫת���ͣ��ж��Ƿ��ǿ�null����
        {
            proType = proType.GenericTypeArguments[0]; // ���Ŀ��Ϊ��null���ͱ���ת��Ϊǿ���Ͳ��ܽ��и�ֵ
        }

        if (proType.Name == "EntityReference")
        {
            if (value is EntityReference efVal)
            {
                var ef = new CRMClients.OData.Models.EntityReference(efVal.Id)
                {
                    logicalName = efVal.LogicalName
                };
                value = ef;
            }
        }
        else if (proType.IsEnum)
        {
            if (value is OptionSetValue osValue)
            {
                value = Enum.Parse(proType, osValue.Value.ToString(), true);
            }
            else
            {
                value = Enum.Parse(proType, value.ToString(), true);
            }
        }
        else
        {
            value = Convert.ChangeType(value, proType);
        }
        return value;
    }


    private static object GetAttributeValue(Entity entity, string attributeLogicalName)
    {
        if (string.IsNullOrWhiteSpace(attributeLogicalName))
        {
            throw new ArgumentNullException("attributeLogicalName");
        }

        if (!entity.Contains(attributeLogicalName))
        {
            return null;
        }

        return entity[attributeLogicalName];
    }
}