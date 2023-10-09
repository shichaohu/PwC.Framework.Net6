using Microsoft.Xrm.Sdk;
using System.Reflection;
using EntityReference = Microsoft.Xrm.Sdk.EntityReference;

namespace PwC.CRM.Share.Extensions;

public static class EntityExtensions
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

    /// <summary>
    /// 将entity转换成model
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static T ToModel<T>(this Entity entity) where T : class, new()
    {
        var obj = Activator.CreateInstance(typeof(T));
        PropertyInfo[] modelPropertys = typeof(T).GetProperties();//获取目的对象的属性
        foreach (var mPro in modelPropertys)
        {
            try
            {
                //如果目标属性无set权限，放弃对他的操作
                if (mPro.CanWrite == false)
                {
                    continue;
                }
                if (mPro.PropertyType != typeof(string) && mPro.PropertyType.IsClass)//link对象
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
    /// 将entity list转换成model list
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static List<T> ToModelList<T>(this IList<Entity> entities) where T : class, new()
    {
        var res = entities.Select(x => x.ToModel<T>()).ToList();
        return res;
    }

    /// <summary>
    /// 转换成目标类型
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
        //当目标类型为可null类型时，需要获取到对应的基础类型
        if (proType.IsGenericType
                && proType.GetGenericTypeDefinition() == typeof(Nullable<>)
                && proType.GenericTypeArguments.Length > 0) //如果要转类型，判断是否是可null类型
        {
            proType = proType.GenericTypeArguments[0]; // 如果目的为可null类型必须转换为强类型才能进行赋值
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