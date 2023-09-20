using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public static class ResourcesManagerExtensions
    {
        public static T LoadAsset<T>(this IResourcesManager manager, string name) where T : UnityEngine.Object
        {
            return manager.LoadAsset<T>(E_ResourcesPath.Null, name, false);
        }
        public static T LoadAsset<T>(this IResourcesManager manager, string name, bool canCreateGameObject) where T : UnityEngine.Object
        {
            return manager.LoadAsset<T>(E_ResourcesPath.Null, name, canCreateGameObject);
        }
        public static T LoadAsset<T>(this IResourcesManager manager, E_ResourcesPath path, string name) where T : UnityEngine.Object
        {
            return manager.LoadAsset<T>(path, name, false);
        }

        public static T[] LoadAssetsFolder<T>(this IResourcesManager manager, string path, bool canCreateGameObject) where T : UnityEngine.Object
        {
            T[] assets = Resources.LoadAll<T>(path);

            if (assets.Length == 0)
            {
                Debug.LogWarning("ResourcesManager: 文件夹中没有资源, 请检查该文件夹中是否添加资源");
                return null;
            }

            if (assets is GameObject[] && canCreateGameObject)
            {
                foreach (var item in assets)
                {
                    GameObject.Instantiate(item);
                }
            }

            return assets;
        }

        public static void LoadAssetAsync<T>(this IResourcesManager manager, string name, Action<T> CompleteCallBack) where T : UnityEngine.Object
        {
            manager.LoadAssetAsync<T>(E_ResourcesPath.Null, name, CompleteCallBack, false);
        }
        public static void LoadAssetAsync<T>(this IResourcesManager manager, string name, Action<T> CompleteCallBack, bool canCreateGameObject) where T : UnityEngine.Object
        {
            manager.LoadAssetAsync<T>(E_ResourcesPath.Null, name, CompleteCallBack, canCreateGameObject);
        }
        public static void LoadAssetAsync<T>(this IResourcesManager manager, E_ResourcesPath path, string name, Action<T> CompleteCallBack) where T : UnityEngine.Object
        {
            manager.LoadAssetAsync<T>(path, name, CompleteCallBack, false);
        }

        public static void LoadAssetsAsync<T>(this IResourcesManager manager, Action<T[]> CompleteCallBack, params string[] names) where T : UnityEngine.Object
        {
            manager.LoadAssetsAsync<T>(E_ResourcesPath.Null, CompleteCallBack, false, names);
        }
        public static void LoadAssetsAsync<T>(this IResourcesManager manager, Action<T[]> CompleteCallBack, bool canCreateGameObject, params string[] names) where T : UnityEngine.Object
        {
            manager.LoadAssetsAsync<T>(E_ResourcesPath.Null, CompleteCallBack, canCreateGameObject, names);
        }
        public static void LoadAssetsAsync<T>(this IResourcesManager manager, E_ResourcesPath path, Action<T[]> CompleteCallBack, params string[] names) where T : UnityEngine.Object
        {
            manager.LoadAssetsAsync<T>(path, CompleteCallBack, false, names);
        }
    }
}


