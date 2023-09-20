using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FrameCore
{
    public interface IBinaryDataUtility : IUtility
    {
        void SaveData(string fileName, object dataObj);
        T LoadData<T>(string fileName) where T : class;
    }

    public class BinaryDataUtility : IBinaryDataUtility
    {
        private static string SAVE_PATH = Application.persistentDataPath + "/Data/";

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
}


