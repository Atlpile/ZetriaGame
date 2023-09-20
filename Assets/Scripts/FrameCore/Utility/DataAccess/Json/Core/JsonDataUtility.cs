using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
using Newtonsoft.Json;

namespace FrameCore
{
    public interface IJsonDataUtility : IUtility
    {
        void SaveData(object data, string fileName, E_JsonType type = E_JsonType.LitJson);
        T LoadData<T>(string fileName, E_JsonType type = E_JsonType.LitJson) where T : new();
        void UpdateData<T>(string fileName, Action<T> ChangeDataAction, E_JsonType type = E_JsonType.LitJson) where T : new();
        void CoverNewData<T>(string fileName, E_JsonType type = E_JsonType.LitJson) where T : new();
    }

    public class JsonDataUtility : IJsonDataUtility
    {
        public void SaveData(object data, string fileName, E_JsonType type = E_JsonType.LitJson)
        {
            string path = Application.persistentDataPath + "/" + fileName + ".json";
            string jsonInfo = string.Empty;

            switch (type)
            {
                case E_JsonType.JsonUtility:
                    jsonInfo = JsonUtility.ToJson(data);
                    break;
                case E_JsonType.LitJson:
                    jsonInfo = JsonMapper.ToJson(data);
                    break;
                case E_JsonType.NewtonsoftJson:
                    jsonInfo = JsonConvert.SerializeObject(data);
                    break;
            }

            File.WriteAllText(path, jsonInfo);
        }

        public T LoadData<T>(string fileName, E_JsonType type = E_JsonType.LitJson) where T : new()
        {
            string path = Application.streamingAssetsPath + "/" + fileName + ".json";

            if (!File.Exists(path))
            {
                path = Application.persistentDataPath + "/" + fileName + ".json";
                // Debug.Log("StreamingAssets未找到" + fileName + "数据文件");
            }

            if (!File.Exists(path))
            {
                // Debug.LogWarning("persistentDataPath未找到" + fileName + "数据文件，则创建新数据对象");
                return new T();
            }

            string jsonStr = File.ReadAllText(path);
            T data = default;
            switch (type)
            {
                case E_JsonType.JsonUtility:
                    data = JsonUtility.FromJson<T>(jsonStr);
                    break;
                case E_JsonType.LitJson:
                    data = JsonMapper.ToObject<T>(jsonStr);
                    break;
                case E_JsonType.NewtonsoftJson:
                    data = JsonConvert.DeserializeObject<T>(jsonStr);
                    break;
            }

            return data;
        }

        public void UpdateData<T>(string fileName, Action<T> ChangeDataAction, E_JsonType type = E_JsonType.LitJson) where T : new()
        {
            T data = LoadData<T>(fileName, type);
            ChangeDataAction?.Invoke(data);
            SaveData(data, fileName, type);
        }

        public void CoverNewData<T>(string fileName, E_JsonType type = E_JsonType.LitJson) where T : new()
        {
            T data = new();
            SaveData(data, fileName, type);
            Debug.Log("清空" + data.ToString() + "数据");
        }

        //TODO:额外功能：加密、解密
    }
}


