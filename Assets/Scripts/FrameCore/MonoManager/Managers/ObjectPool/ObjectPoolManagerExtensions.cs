using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public static class ObjectPoolManagerExtensions
    {
        public static void AddObject(this IObjectPoolManager manager, GameObject obj)
        {
            manager.AddObject(obj, 0);
        }

        public static void AddObject_FromResources(this IObjectPoolManager manager, string path)
        {
            GameObject obj = manager.ResourcesManager.LoadAsset<GameObject>(path);
            manager.AddObject(obj, 1);
        }
        public static void AddObject_FromResources(this IObjectPoolManager manager, E_ResourcesPath path, string name)
        {
            GameObject obj = manager.ResourcesManager.LoadAsset<GameObject>(path, name);
            manager.AddObject(obj, 1);
        }

        public static void AddObjects_FromResources(this IObjectPoolManager manager, params string[] paths)
        {
            GameObject[] objs = manager.ResourcesManager.LoadAssets<GameObject>(E_ResourcesPath.Null, false, paths);
            foreach (var item in objs)
            {
                manager.AddObject(item, 1);
            }
        }
        public static void AddObjects_FromResources(this IObjectPoolManager manager, E_ResourcesPath path, params string[] names)
        {
            GameObject[] objs = manager.ResourcesManager.LoadAssets<GameObject>(path, false, names);
            foreach (var item in objs)
            {
                manager.AddObject(item, 1);
            }
        }
        public static void AddObjects_FromFolder(this IObjectPoolManager manager, E_ResourcesPath path = E_ResourcesPath.PoolObject)
        {
            GameObject[] objs = manager.ResourcesManager.LoadAssetsFolder<GameObject>(path, false);
            foreach (var item in objs)
            {
                manager.AddObject(item, 1);
            }
        }

        public static void AddObject_FromResourcesAsync(this IObjectPoolManager manager, string path)
        {
            manager.ResourcesManager.LoadAssetAsync<GameObject>(path, obj => manager.AddObject(obj, 1));
        }
        public static void AddObject_FromResourcesAsync(this IObjectPoolManager manager, E_ResourcesPath path, string name)
        {
            manager.ResourcesManager.LoadAssetAsync<GameObject>(path, name, obj => manager.AddObject(obj, 1));
        }

        public static void AddObjects_FromResourcesAsync(this IObjectPoolManager manager, params string[] names)
        {
            manager.ResourcesManager.LoadAssetsAsync<GameObject>
            (
                (objs) =>
                {
                    foreach (var item in objs)
                    {
                        manager.AddObject(item, 1);
                    }
                },
                names
            );
        }
        public static void AddObjects_FromResourcesAsync(this IObjectPoolManager manager, E_ResourcesPath path, params string[] names)
        {
            manager.ResourcesManager.LoadAssetsAsync<GameObject>(path, (objs) =>
            {
                foreach (var item in objs)
                {
                    manager.AddObject(item, 1);
                }
            }, names);
        }
    }

}
