using System.Collections;

namespace CMCOnlineStoreApi
{
    public static class Helper
    {
        public static bool HasProperty<T>(string prop) where T : new()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.Name == prop)
                {
                    return true;
                }
            }

            return false;
        }
        public static List<string> GetInvalidOrderingFields<T>(string[] fields) where T : new()
        {
            var result = new List<string>();
            foreach (var f in fields)
            {
                if (!Helper.HasProperty<T>(f))
                {
                    result.Add(f);
                }
            }

            return result;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> input, string queryString, char delimiter = ',')
        {
            if (string.IsNullOrEmpty(queryString))
                return input;

            int i = 0;
            foreach (string sortingField in queryString.Split(delimiter))
            {
                if (i == 0)
                    input = input.OrderBy(x => GetPropertyValue(x, sortingField.Trim()));
                else
                    input = ((IOrderedEnumerable<T>)input).ThenBy(x => GetPropertyValue(x, sortingField.Trim()));

                i++;
            }

            return input;
        }

        public static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        public static bool SearchProperties<T>(T part, string searchString)
        {
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(part);
                if (value is IEnumerable && prop.PropertyType != typeof(string))
                {
                    var enumV = value as IEnumerable;
                    foreach (var v in enumV)
                    {
                        if (v.ToString().Contains(searchString, System.StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                }
                else if (value != null)
                {
                    string valueString = value.ToString();
                    if (valueString.Contains(searchString, System.StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }
    }
}
