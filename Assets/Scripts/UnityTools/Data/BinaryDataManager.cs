using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class BinaryDataManager
{
    public static string BINARY_DATA_PATH = Application.streamingAssetsPath + "/Binary/";
    private static string SAVE_PATH = Application.persistentDataPath + "/Data/";
    private Dictionary<string, object> TableDic;

    public BinaryDataManager()
    {
        TableDic = new Dictionary<string, object>();
    }

    /// <summary>
    /// 读取Excel表
    /// </summary>
    /// <typeparam name="T">数据容器类名</typeparam>
    /// <typeparam name="K">数据结构类类名</typeparam>
    /// <returns></returns>
    public void LoadTable<T, K>()
    {
        using (FileStream fileStream = File.Open(BINARY_DATA_PATH + typeof(K).Name + ".table", FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();

            int binaryIndex = 0;

            int rowCount = BitConverter.ToInt32(bytes, binaryIndex);
            binaryIndex += 4;

            int keyNameLength = BitConverter.ToInt32(bytes, binaryIndex);
            binaryIndex += 4;
            string keyName = Encoding.UTF8.GetString(bytes, binaryIndex, keyNameLength);
            binaryIndex += keyNameLength;

            Type dataContainerType = typeof(T);
            object dataContainerObject = Activator.CreateInstance(dataContainerType);
            Type classType = typeof(K);
            FieldInfo[] infos = classType.GetFields();

            for (int i = 0; i < rowCount; i++)
            {
                object dataObject = Activator.CreateInstance(classType);

                foreach (FieldInfo info in infos)
                {
                    if (info.FieldType == typeof(int))
                    {
                        info.SetValue(dataObject, BitConverter.ToInt32(bytes, binaryIndex));
                        binaryIndex += 4;
                    }
                    else if (info.FieldType == typeof(float))
                    {
                        info.SetValue(dataObject, BitConverter.ToSingle(bytes, binaryIndex));
                        binaryIndex += 4;
                    }
                    else if (info.FieldType == typeof(bool))
                    {
                        info.SetValue(dataObject, BitConverter.ToBoolean(bytes, binaryIndex));
                        binaryIndex += 1;
                    }
                    else if (info.FieldType == typeof(string))
                    {
                        int stringLength = BitConverter.ToInt32(bytes, binaryIndex);
                        binaryIndex += 4;

                        info.SetValue(dataObject, Encoding.UTF8.GetString(bytes, binaryIndex, stringLength));
                        binaryIndex += stringLength;
                    }
                }

                object dicObject = dataContainerType.GetField("dataDic").GetValue(dataContainerObject);
                MethodInfo methodInfo = dicObject.GetType().GetMethod("Add");
                object keyValue = classType.GetField(keyName).GetValue(dataObject);
                methodInfo.Invoke(dicObject, new object[] { keyValue, dataObject });
            }

            TableDic.Add(typeof(T).Name, dataContainerObject);

            fileStream.Close();
        }
    }

    /// <summary>
    /// 获取Excel表信息
    /// </summary>
    /// <typeparam name="T">数据容器类名</typeparam>
    public T GetTableInfo<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (TableDic.ContainsKey(tableName))
            return TableDic[tableName] as T;
        else
            return null;
    }


    public void SaveData(string fileName, object dataObj)
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        using (FileStream fileStream = new FileStream(SAVE_PATH + fileName + ".data", FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, dataObj);
            fileStream.Close();
        }
    }

    public T LoadData<T>(string fileName) where T : class
    {
        if (!File.Exists(SAVE_PATH + fileName + ".data"))
        {
            return default(T);
        }
        else
        {
            T dataObj;
            using (FileStream fileStream = File.Open(SAVE_PATH + fileName + ".data", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                dataObj = binaryFormatter.Deserialize(fileStream) as T;
                fileStream.Close();
            }

            return dataObj;
        }
    }

}
