using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VDMMutiline.Core.Helper
{
    public class ReflectionHelper
    {
        public static void BeforeClean(object obj)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                PropertyInfo[] properties = obj.GetType().GetProperties();
                if (obj.GetType().GetProperty("PList") != null)
                {
                    object obj2 = type.GetMethod("Init_BeforeClean").Invoke(obj, new object[0]);
                }
            }
        }

        public static T CleanEntity<T>(T entity) where T : class
        {
            return (RemoveCollection(entity) as T);
        }

        public static List<T> CleanList<T>(List<T> list) where T : class
        {
            list.ForEach(delegate (T c)
            {
                c = RemoveCollection(c) as T;
            });
            return list;
        }

        public static T ConvertEntity<T>(T entity, bool isRemoveCollection) where T : class
        {
            T local = InspectItemDeleteable(entity) as T;
            if (isRemoveCollection)
            {
                local = RemoveCollection(local) as T;
            }
            return local;
        }

        public static bool ExsitExclude(List<string> exludes, string propertyName)
        {
            if (exludes.Count > 0)
            {
                foreach (string str in exludes)
                {
                    if (str.Equals(propertyName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public static List<AuditDetail> GetAllPropertyToAudit(object values, string keyName, string keyValue)
        //{
        //    List<string> exclude = GetExclude(values);
        //    List<AuditDetail> list2 = new List<AuditDetail>();
        //    if (values != null)
        //    {
        //        Type type = values.GetType();
        //        if (((type.IsPrimitive || (type == typeof(DateTime?))) || (type == typeof(DateTime))) || (type == typeof(string)))
        //        {
        //            return list2;
        //        }
        //        IList<PropertyInfo> list3 = new List<PropertyInfo>(type.GetProperties());
        //        foreach (PropertyInfo info in list3)
        //        {
        //            if ((((info.PropertyType.IsPrimitive || (info.PropertyType == typeof(DateTime?))) || (info.PropertyType == typeof(DateTime))) || (info.PropertyType == typeof(string))) && !ExsitExclude(exclude, info.Name))
        //            {
        //                object obj2 = info.GetValue(values);
        //                if (info.Name.Equals(keyName))
        //                {
        //                    obj2 = keyValue;
        //                }
        //                AuditDetail item = new AuditDetail
        //                {
        //                    PropertyName = info.Name,
        //                    OldValue = "null"
        //                };
        //                if (obj2 != null)
        //                {
        //                    item.NewValue = obj2.ToString();
        //                }
        //                else
        //                {
        //                    item.NewValue = "null";
        //                }
        //                list2.Add(item);
        //            }
        //        }
        //    }
        //    return list2;
        //}

        //public static List<AuditDetail> GetDiff(object oldValues, object newValues)
        //{
        //    List<AuditDetail> list = new List<AuditDetail>();
        //    if ((oldValues != null) && (newValues != null))
        //    {
        //        List<string> exclude = GetExclude(oldValues);
        //        Type type = oldValues.GetType();
        //        if (((type.IsPrimitive || (type == typeof(DateTime?))) || (type == typeof(DateTime))) || (type == typeof(string)))
        //        {
        //            return list;
        //        }
        //        IList<PropertyInfo> list3 = new List<PropertyInfo>(type.GetProperties());
        //        foreach (PropertyInfo info in list3)
        //        {
        //            if (((info.PropertyType.IsPrimitive || (info.PropertyType == typeof(DateTime?))) || (info.PropertyType == typeof(DateTime))) || (info.PropertyType == typeof(string)))
        //            {
        //                object obj2 = info.GetValue(oldValues);
        //                object obj3 = info.GetValue(newValues);
        //                if (((((obj2 != null) && (obj3 == null)) || ((obj2 == null) && (obj3 != null))) || (((obj2 != null) && (obj3 != null)) && !obj2.Equals(obj3))) && !ExsitExclude(exclude, info.Name))
        //                {
        //                    AuditDetail item = new AuditDetail
        //                    {
        //                        PropertyName = info.Name
        //                    };
        //                    if (obj2 == null)
        //                    {
        //                        item.OldValue = "null";
        //                    }
        //                    else
        //                    {
        //                        item.OldValue = obj2.ToString();
        //                    }
        //                    if (obj3 == null)
        //                    {
        //                        item.NewValue = "null";
        //                    }
        //                    else
        //                    {
        //                        item.NewValue = obj3.ToString();
        //                    }
        //                    list.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    return list;
        //}

        public static List<string> GetExclude(object obj)
        {
            List<string> list = new List<string>();
            if (obj != null)
            {
                PropertyInfo property = obj.GetType().GetProperty("ExcludedProperties", BindingFlags.Public | BindingFlags.Static);
                if (property != null)
                {
                    string[] collection = (property.GetValue(obj, null) as string).Split(new char[] { ',' });
                    list.AddRange(collection);
                }
            }
            return list;
        }

        public static object GetItemOfEntity(object obj, string propertyName)
        {
            if (obj != null)
            {
                PropertyInfo property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    return property.GetValue(obj, null);
                }
            }
            return null;
        }

        public static object InspectItemDeleteable(object obj)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                PropertyInfo property = obj.GetType().GetProperty("IsDeleteable");
                if (property != null)
                {
                    property.SetValue(obj, Convert.ChangeType(true, property.PropertyType), null);
                    if ((!type.IsPrimitive && (type != typeof(DateTime?))) && !(type == typeof(DateTime)))
                    {
                        IList<PropertyInfo> list = new List<PropertyInfo>(type.GetProperties());
                        foreach (PropertyInfo info2 in list)
                        {
                            Type tColl;
                            if ((!info2.PropertyType.IsPrimitive && (info2.PropertyType != typeof(DateTime?))) && (info2.PropertyType != typeof(DateTime)))
                            {
                                tColl = typeof(ICollection<>);
                                Type propertyType = info2.PropertyType;
                                if ((propertyType.IsGenericType && tColl.IsAssignableFrom(propertyType.GetGenericTypeDefinition())) || propertyType.GetInterfaces().Any<Type>(x => (x.IsGenericType && (x.GetGenericTypeDefinition() == tColl))))
                                {
                                    PropertyInfo info3 = propertyType.GetProperty("Count");
                                    object obj2 = info2.GetValue(obj, null);
                                    if (obj2 != null)
                                    {
                                        int num = (int)info3.GetValue(obj2, null);
                                        if (num > 0)
                                        {
                                            property.SetValue(obj, Convert.ChangeType(false, property.PropertyType), null);
                                            return obj;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return obj;
                }
            }
            return null;
        }

        public static List<T> InspectListDeleteable<T>(List<T> list) where T : class
        {
            if ((list != null) && (list.Count != 0))
            {
                list.ForEach(delegate (T c)
                {
                    c = InspectItemDeleteable(c) as T;
                });
            }
            return list;
        }

        public static T InspectSingleDeleteable<T>(T entity) where T : class
        {
            return (InspectItemDeleteable(entity) as T);
        }

        public static bool IsDeleteAble(object obj)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                if ((type.IsPrimitive || (type == typeof(DateTime?))) || (type == typeof(DateTime)))
                {
                    return true;
                }
                IList<PropertyInfo> list = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo info in list)
                {
                    Type tColl;
                    if ((!info.PropertyType.IsPrimitive && (info.PropertyType != typeof(DateTime?))) && (info.PropertyType != typeof(DateTime)))
                    {
                        tColl = typeof(ICollection<>);
                        Type propertyType = info.PropertyType;
                        if ((propertyType.IsGenericType && tColl.IsAssignableFrom(propertyType.GetGenericTypeDefinition())) || propertyType.GetInterfaces().Any<Type>(x => (x.IsGenericType && (x.GetGenericTypeDefinition() == tColl))))
                        {
                            PropertyInfo property = propertyType.GetProperty("Count");
                            object obj2 = info.GetValue(obj, null);
                            if (obj2 != null)
                            {
                                int num = (int)property.GetValue(obj2, null);
                                if (num > 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static object RemoveCollection(object obj)
        {
            BeforeClean(obj);
            if (obj == null)
            {
                return null;
            }
            Type type = obj.GetType();
            if (((!type.IsPrimitive && (type != typeof(DateTime?))) && (type != typeof(DateTime))) && (type != typeof(string)))
            {
                IList<PropertyInfo> list = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo info in list)
                {
                    Type tColl;
                    if (((!info.PropertyType.IsPrimitive && (info.PropertyType != typeof(DateTime?))) && (info.PropertyType != typeof(DateTime))) && (info.PropertyType != typeof(string)))
                    {
                        tColl = typeof(ICollection<>);
                        Type type2 = typeof(IEnumerable<>);
                        Type type3 = typeof(HashSet<>);
                        Type propertyType = info.PropertyType;
                        if ((((propertyType.IsGenericType && type2.IsAssignableFrom(propertyType.GetGenericTypeDefinition())) || (propertyType.IsGenericType && type3.IsAssignableFrom(propertyType.GetGenericTypeDefinition()))) || (propertyType.IsGenericType && tColl.IsAssignableFrom(propertyType.GetGenericTypeDefinition()))) || propertyType.GetInterfaces().Any<Type>(x => (x.IsGenericType && (x.GetGenericTypeDefinition() == tColl))))
                        {
                            info.SetValue(obj, Convert.ChangeType(null, info.PropertyType), null);
                        }
                        else
                        {
                            object obj2 = RemoveCollection(info.GetValue(obj));
                        }
                    }
                }
            }
            return obj;
        }

        public static object SetItemOfEntity(object obj, string propertyName, object value)
        {
            if (obj != null)
            {
                PropertyInfo property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    property.SetValue(obj, value, null);
                    return obj;
                }
            }
            return null;
        }
    }
}
