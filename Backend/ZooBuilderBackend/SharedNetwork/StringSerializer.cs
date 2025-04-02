using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedNetwork
{
    public static class StringSerializer
    {
        public static T Deserialize<T>(string dataString) where T : struct
        {
            var entries = new List<string>();
            string cleanedString = dataString.Substring(1, dataString.Length - 2);
            AddObjectsToList(entries, cleanedString.Split(";"));
            string[] values = entries.ToArray();
            var properties = typeof(T).GetProperties();
            object instance = new T();
            
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.PropertyType.GetInterfaces().Contains(typeof(IStringSerializable)))
                {
                    var method = typeof(StringSerializer).GetMethod(nameof(Deserialize), BindingFlags.Static | BindingFlags.Public);
                    var genericMethod = method.MakeGenericMethod(property.PropertyType);
                    property.SetValue(instance, genericMethod.Invoke(null, new object[] {values[i]}));
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
        
        private static int AddObjectsToList(List<string> objects, string[] dataStrings)
        {
            var index = 0;
            var remainingStrings = new List<string>(dataStrings);
            for (; index < dataStrings.Length; index++)
            {
                string data = dataStrings[index];
                remainingStrings.Remove(data);
                if (data.StartsWith("{"))   //  new object needs to be nested
                {
                    var newList = new List<string> { data.Replace("{", string.Empty) };
                    index += AddObjectsToList(newList, remainingStrings.ToArray());
                    var resultString = "{";
                    for (var i = 0; i < newList.Count; i++)
                    {
                        resultString += newList[i];
                        if (i < newList.Count - 1)
                        {
                            resultString += ";";
                        }
                    }
                    resultString += "}";
                    objects.Add(resultString);
                    continue;
                }
                if (data.EndsWith("}"))
                {
                    objects.Add(data.Replace("}", string.Empty));
                    return index + 1;
                }
                objects.Add(data);
            }

            return index;
        }
    }
}