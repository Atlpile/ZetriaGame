using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class ResourcesPathConfig
    {
        public Dictionary<E_ResourcesPath, string> Config;

        public ResourcesPathConfig()
        {
            Config = new Dictionary<E_ResourcesPath, string>
            {
                {E_ResourcesPath.Null,          ""},
                {E_ResourcesPath.Audio,         "Audio/"},
                {E_ResourcesPath.GameObject,    "GameObject/"},
                {E_ResourcesPath.PoolObject,    "PoolObject/"},
                {E_ResourcesPath.UI,            "UI/"},
                {E_ResourcesPath.FX,            "FX/"},
                {E_ResourcesPath.DataSO,        "DataSO/"},
                {E_ResourcesPath.Misc,          "Misc/"}
            };
        }

        public void AddConfig(E_ResourcesPath pathName, string path)
        {
            if (!Config.ContainsKey(pathName))
            {
                Config.Add(pathName, path);
            }
        }
    }
}


