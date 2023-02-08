using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static class JsonUtil<T>
{
    /// <summary>
    /// UTF-8格式json打印乱码 问题解决方式
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string JsonUTF8toUnicode(string jsonStr)
    {
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        var str = reg.Replace(jsonStr, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        return str;
    }
    public static void SaveData(string filePath, T t)
    {
        string saveJsonStr = JsonUtility.ToJson(t);
        //string js = JsonConvert.SerializeObject(t);
        File.WriteAllText(filePath, saveJsonStr);

    }
    public static T loadData(string filePath)
    {
        string jsonStr = File.ReadAllText(filePath);


        //var res2 = JsonConvert.DeserializeObject<T>(str);
        return JsonUtility.FromJson<T>(jsonStr);
    }
    public static T loadData(string filePath, string tag)
    {
        StreamReader sr = File.OpenText(filePath);
        string str = sr.ReadToEnd();
        JObject rootJ = JObject.Parse(JsonUTF8toUnicode(str));
        return rootJ.Value<JArray>(tag).ToObject<T>();

    }
}
