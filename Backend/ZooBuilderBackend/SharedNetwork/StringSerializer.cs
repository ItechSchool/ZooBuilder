using System;
using System.Linq;
using System.Reflection;

namespace SharedNetwork
{
    public static class StringSerializer
    {
        public static T Deserialize<T>(string dataString) where T : struct
        {
            string cleanedString = dataString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] values = cleanedString.Split(';');
            var properties = typeof(T).GetProperties();

            object instance = new T();
            
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.PropertyType.GetInterfaces().Contains(typeof(IStringSerializable)))
                {
                    var method = typeof(StringSerializer).GetMethod(nameof(Deserialize), BindingFlags.Static | BindingFlags.Public);
                    var genericMethod = method.MakeGenericMethod(property.PropertyType);
                    property.SetValue(instance, genericMethod.Invoke(null, new object[] { values[i] }));
                }
                else
                {
                    property.SetValue(instance, Convert.ChangeType(values[i], property.PropertyType));
                }
            }

            return (T)instance;
        }

        public static string Serialize(object obj)
        {
            var dataString = "{";
            var properties = obj.GetType().GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                dataString += property.GetValue(obj).ToString();
                if (i < properties.Length - 1)
                {
                    dataString += ";";
                }
            }

            return dataString + "}";
        }
    }
}