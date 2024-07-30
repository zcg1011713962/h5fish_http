using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json;

/// <summary>
/// 解析JSON，仿Javascript风格
/// </summary>
public static class JsonHelper
{
    public static T ParseFromStr<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static string ConvertToStr(object jsonObject)
    {
        return JsonConvert.SerializeObject(jsonObject);
    }

    // 生成一个json串
    public static string genJson(Dictionary<string, object> data, bool changeTimeToLocal = false)
    {
        StringWriter sw = new StringWriter();
        JsonWriter writer = new JsonTextWriter(sw);
        genJsonStr(data, sw, writer, changeTimeToLocal);
        writer.Flush();
        return sw.GetStringBuilder().ToString();
    }

    private static void genJsonStr(Dictionary<string, object> data, StringWriter sw, JsonWriter writer, bool changeTimeToLocal)
    {
        writer.WriteStartObject();
        foreach (var item in data)
        {
            writer.WritePropertyName(item.Key);

            if (item.Value is List<Dictionary<string, object>>)
            {
                writer.WriteStartArray();
                List<Dictionary<string, object>> dataList = (List<Dictionary<string, object>>)item.Value;
                for (int i = 0; i < dataList.Count; i++)
                {
                    genJsonStr(dataList[i], sw, writer, changeTimeToLocal);
                }
                writer.WriteEndArray();
            }
            else if (item.Value is Dictionary<string, object>)
            {
                genJsonStr((Dictionary<string, object>)item.Value, sw, writer, changeTimeToLocal);
            }
            else if (item.Value is DateTime)
            {
                var v = (DateTime)item.Value;
                if (changeTimeToLocal)
                {
                    v = v.ToLocalTime();
                }
                writer.WriteValue(v.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                writer.WriteValue(item.Value);
            }
        }
        writer.WriteEndObject();
    }
}

