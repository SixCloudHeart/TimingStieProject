using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core.Common.Extention
{
    public static partial class Extention
    {
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }
        public static string ObjToString(this object obj)
        {
            if (obj != null) return obj.ToString().Trim();
            return "";
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static bool ToBool(this object obj)
        {
            bool reval = false;
            if (obj != null && obj != DBNull.Value && bool.TryParse(obj.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        public static T DeepCopyByReflection<T>(this T obj)
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
                }
                catch { }
            }

            return (T)retval;
        }
    }
}
