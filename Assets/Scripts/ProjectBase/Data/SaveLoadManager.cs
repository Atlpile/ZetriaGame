using UnityEngine;
using LitJson;
using System.IO;

public enum JsonType { JsonUtlity, LitJson, }

public class SaveLoadManager
{
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //设置文件的存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        //序列化得到Json字符串
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
        //把序列化的Json字符串，存储到指定路径的文件中
        File.WriteAllText(path, jsonStr);
    }

    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //获取数据文件的存储路径
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";

        //判断StreamingAssets文件夹是否存在该数据文件，若没有则在persistentDataPath下查找
        if (!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
            // Debug.Log("StreamingAssets未找到" + fileName + "数据文件");
        }

        //如果读写文件夹中都还没有 那就返回一个默认值的对象
        if (!File.Exists(path))
        {
            // Debug.LogWarning("persistentDataPath未找到" + fileName + "数据文件，则创建新数据对象");
            return new T();
        }


        //反序列化
        string jsonStr = File.ReadAllText(path);
        //获取数据对象的类型
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }

        //返回数据对象
        return data;
    }

    public void ClearData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        T data = new T();
        SaveData(data, fileName, type);
        Debug.Log("清空" + data.ToString() + "数据");
    }

    //TODO:额外功能：加密、解密
}
