using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Toolkit.Newtonsoft.Json.Linq;

public static class JsonFlattenExtensions
{
    /// <summary>
    /// Flattens a JObject to a Dictionary.<c>null</c>, <c>""</c>, <c>[]</c> and <c>{}</c> are preserved by default
    /// </summary>
    /// <param name="jsonObject">JObject to flatten</param>
    /// <param name="includeNullAndEmptyValues">Set to false to ignore JSON properties that are null, "", [] and {} when flattening</param>
    public static IDictionary<string, object> Flatten(this JObject jsonObject, bool includeNullAndEmptyValues = true)
    {
        return jsonObject
            .Descendants()
            .Where(p => !p.Any())
            .Aggregate(new Dictionary<string, object>(), (properties, jToken) =>
            {
                var value = (jToken as JValue)?.Value;

                if (!includeNullAndEmptyValues)
                {
                    if (value?.Equals("") == false)
                    {
                        properties.Add(jToken.Path, value);
                    }

                    return properties;
                }

                var strVal = jToken.Value<object>()?.ToString().Trim();
                if (strVal?.Equals("[]") == true)
                {
                    value = Enumerable.Empty<object>();
                }
                else if (strVal?.Equals("{}") == true)
                {
                    value = new object();
                }

                properties.Add(jToken.Path, value);

                return properties;
            });
    }

    /// <summary>
    /// Unflattens an already flattened JSON Dictionary to its original JSON structure
    /// </summary>
    /// <param name="flattenedJsonKeyValues">Dictionary to unflatten</param>
    public static JObject Unflatten(this IDictionary<string, object> flattenedJsonKeyValues)
    {
        JContainer result = null;
        var setting = new JsonMergeSettings
        {
            MergeArrayHandling = MergeArrayHandling.Merge
        };

        foreach (var pathValue in flattenedJsonKeyValues)
        {
            if (result == null)
            {
                result = UnflattenSingle(pathValue);
            }
            else
            {
                result.Merge(UnflattenSingle(pathValue), setting);
            }
        }
        return result as JObject;
    }

    /// <summary>
    /// Get an item from the dictionary and cast it to a type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Get<T>(this IDictionary<string, object> dictionary, string key) => (T)dictionary[key];

    /// <summary>
    /// Update an item in the dictionary
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Set(this IDictionary<string, object> dictionary, string key, object value) => dictionary[key] = value;

    /// <summary>
    /// Try get an item from the dictionary and cast it to a type. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryGet<T>(this IDictionary<string, object> dictionary, string key, out T value)
    {
        object result;
        if (dictionary.TryGetValue(key, out result) && result is T)
        {
            value = (T)result;
            return true;
        }
        value = default(T);
        return false;
    }

    private static JContainer UnflattenSingle(KeyValuePair<string, object> keyValue)
    {
        var path = keyValue.Key;
        var value = keyValue.Value != null ? JToken.FromObject(keyValue.Value) : null;
        var pathSegments = SplitPath(path);

        JContainer lastItem = null;
        //build from leaf to root
        foreach (var pathSegment in pathSegments.Reverse())
        {
            if (!IsJsonArray(pathSegment))
            {
                var obj = new JObject();
                if (lastItem == null)
                {
                    obj.Add(pathSegment, value);
                }
                else
                {
                    obj.Add(pathSegment, lastItem);
                }
                lastItem = obj;

                continue;
            }

            var array = new JArray();
            var index = GetArrayIndex(pathSegment);
            array = FillEmpty(array, index);
            array[index] = lastItem ?? value;
            lastItem = array;

        }
        return lastItem;
    }

    private static IList<string> SplitPath(string path)
    {
        var reg = path.IndexOf("['", StringComparison.Ordinal) > -1
            ? new Regex(@"(?!\.)([^\'\[\]]+)|(?!\[)(\d+)(?=\])")
            : new Regex(@"(?!\.)([^. ^\[\]]+)|(?!\[)(\d+)(?=\])");

        var result = new List<string>();

        foreach (Match match in reg.Matches(path))
        {
            result.Add(match.Value);
        }

        return result;
    }

    private static JArray FillEmpty(JArray array, int index)
    {
        for (var i = 0; i <= index; i++)
        {
            array.Add(null);
        }
        return array;
    }

    private static bool IsJsonArray(string pathSegment) => int.TryParse(pathSegment, out var x);

    private static int GetArrayIndex(string pathSegment)
    {
        if (int.TryParse(pathSegment, out var result))
        {
            return result;
        }
        throw new Exception($"Unable to parse array index: {pathSegment}");
    }
}
