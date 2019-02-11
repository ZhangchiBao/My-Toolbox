using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Book
{
    public static class ExtensionMethods
    {
        public static void Update<T>(this T source, T target, string keyName = null)
            where T : class, new()
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new System.ArgumentNullException(nameof(target));
            }

            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == keyName)
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

        public static object GetValue(this PropertyInfo property, object obj, object defaultValue)
        {
            var value = property.GetValue(obj);
            if (value == null)
            {
                value = defaultValue;
            }

            return value;
        }
    }
}
