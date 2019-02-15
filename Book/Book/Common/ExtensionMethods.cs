using System.Data;
using System.Linq;
using System.Reflection;

namespace Book
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 修改对象属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">修改对象</param>
        /// <param name="target">目标对象</param>
        /// <param name="excludeProperties">不修改属性名称</param>
        public static void Update<T>(this T source, T target, params string[] excludeProperties)
            where T : class, new()
        {
            if (target == null)
            {
                throw new System.ArgumentNullException(nameof(target));
            }

            if (source == null)
            {
                source = new T();
            }

            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (excludeProperties != null && excludeProperties.Contains(property.Name))
                {
                    continue;
                }

                property.SetValue(source, property.GetValue(target));
            }
        }

        public static object[] ObjToArray<T>(this T obj)
            where T : class, new()
        {
            if (obj == null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            var type = typeof(T);
            var properties = type.GetProperties();
            return properties.Select(a => a.GetValue(obj)).ToArray();
        }

        /// <summary>
        /// 返回指定对象的属性值
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static object GetValue(this PropertyInfo property, object obj, object defaultValue)
        {
            var value = property.GetValue(obj);
            if (value == null)
            {
                value = defaultValue;
            }

            return value;
        }

        public static string TrimAll(this string source, params char[] args)
        {
            string temp = string.Empty;
            do
            {
                temp = source;
                foreach (var arg in args)
                {
                    source = source.Trim(arg);
                }
            } while (temp.Length != source.Length);
            return temp;
        }
    }
}
