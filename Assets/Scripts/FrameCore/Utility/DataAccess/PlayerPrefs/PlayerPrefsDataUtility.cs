using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FrameCore
{
    public interface IPlayerPrefsDataUtility : IUtility
    {
        void SaveData(object data, string keyName);
        object LoadData(Type type, string keyName);
    }

    public class PlayerPrefsDataUtility : IPlayerPrefsDataUtility
    {
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="data">数据类</param>
        /// <param name="keyName">自定义的数据名</param>
        public void SaveData(object data, string keyName)
        {
            //获取对象字段
            Type dataType = data.GetType();
            FieldInfo[] infos = dataType.GetFields();

            //遍历传入字段来存储数据
            string saveKeyName = "";
            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                info = infos[i];

                saveKeyName = keyName + "_" + dataType.Name + "_" + info.FieldType.Name + "_" + info.Name;

                SaveValue(info.GetValue(data), saveKeyName);
            }

            PlayerPrefs.Save();
        }

        //存储对应类型的值
        private void SaveValue(object value, string keyName)
        {
            Type fieldType = value.GetType();

            if (fieldType == typeof(int))
            {
                int rValue = (int)value;
                PlayerPrefs.SetInt(keyName, rValue);
            }
            else if (fieldType == typeof(float))
            {
                PlayerPrefs.SetFloat(keyName, (float)value);
            }
            else if (fieldType == typeof(string))
            {
                PlayerPrefs.SetString(keyName, value.ToString());
            }
            else if (fieldType == typeof(bool))
            {
                PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
            }
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                IList list = value as IList;

                PlayerPrefs.SetInt(keyName, list.Count);
                int index = 0;
                foreach (object obj in list)
                {
                    SaveValue(obj, keyName + index);
                    ++index;
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(fieldType))
            {
                IDictionary dic = value as IDictionary;

                PlayerPrefs.SetInt(keyName, dic.Count);

                int index = 0;
                foreach (object key in dic.Keys)
                {
                    SaveValue(key, keyName + "_key_" + index);
                    SaveValue(dic[key], keyName + "_value_" + index);
                    ++index;
                }
            }
            else
            {
                SaveData(value, keyName);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="type">数据类</param>
        /// <param name="keyName">自定义的数据名</param>
        /// <returns></returns>
        public object LoadData(Type type, string keyName)
        {
            object data = Activator.CreateInstance(type);
            FieldInfo[] infos = type.GetFields();
            string loadKeyName = "";

            FieldInfo info;
            for (int i = 0; i < infos.Length; i++)
            {
                info = infos[i];

                loadKeyName = keyName + "_" + type.Name +
                    "_" + info.FieldType.Name + "_" + info.Name;

                info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
            }
            return data;
        }

        //加载对应类型的值
        private object LoadValue(Type fieldType, string keyName)
        {
            if (fieldType == typeof(int))
            {
                return PlayerPrefs.GetInt(keyName, 0);
            }
            else if (fieldType == typeof(float))
            {
                return PlayerPrefs.GetFloat(keyName, 0);
            }
            else if (fieldType == typeof(string))
            {
                return PlayerPrefs.GetString(keyName, "");
            }
            else if (fieldType == typeof(bool))
            {
                return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
            }
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                int count = PlayerPrefs.GetInt(keyName, 0);

                IList list = Activator.CreateInstance(fieldType) as IList;
                for (int i = 0; i < count; i++)
                {
                    list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));
                }
                return list;
            }
            else if (typeof(IDictionary).IsAssignableFrom(fieldType))
            {
                int count = PlayerPrefs.GetInt(keyName, 0);

                IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
                Type[] kvType = fieldType.GetGenericArguments();
                for (int i = 0; i < count; i++)
                {
                    dic.Add(LoadValue(kvType[0], keyName + "_key_" + i),
                             LoadValue(kvType[1], keyName + "_value_" + i));
                }
                return dic;
            }
            else
            {
                return LoadData(fieldType, keyName);
            }
        }

    }
}


