using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public static class JsonDataTool
{
    public static void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        File.WriteAllText(path, jsonStr);
    }

    public static T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";

        if (!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
            // Debug.Log("StreamingAssets未找到" + fileName + "数据文件");
        }

        if (!File.Exists(path))
        {
            Debug.LogWarning("persistentDataPath未找到" + fileName + "数据文件，则创建新数据对象");
            return new T();
        }

        string jsonStr = File.ReadAllText(path);
        T data = default;
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }

        return data;
    }
}
