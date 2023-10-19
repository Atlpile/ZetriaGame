using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public sealed class ResourcesManager : BaseManager, IResourcesManager
    {
        private Dictionary<E_ResourcesPath, string> _ResourcePathDic;

        public ResourcesManager(MonoManager manager) : base(manager) { }

        protected override void OnInit()
        {
            _ResourcePathDic = new ResourcesPathConfig().Config;
        }

        /// <summary>
        /// 同步加载单个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="name">资源名称</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>同步加载完成的资源</returns>
        public T LoadAsset<T>(E_ResourcesPath path, string name, bool canCreateGameObject) where T : UnityEngine.Object
        {
            if (!_ResourcePathDic.ContainsKey(path))
            {
                Debug.LogError("配置容器中未添加该路径地址, 请检查Config中是否添加该路径地址");
                return null;
            }

            T asset = Resources.Load<T>(_ResourcePathDic[path] + name);

            if (asset == null)
            {
                Debug.LogError("ResourcesLoader: 未找到该资源" + name + ", 请检查Resources文件夹中是否有该资源");
                return null;
            }

            if (asset is GameObject && canCreateGameObject)
                return GameObject.Instantiate(asset);
            else
                return asset;
        }

        /// <summary>
        /// 同步加载多个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <param name="names">（多个）资源名称</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>同步加载完成的多个资源</returns>
        public T[] LoadAssets<T>(E_ResourcesPath path, bool canCreateGameObject, params string[] names) where T : UnityEngine.Object
        {
            if (!_ResourcePathDic.ContainsKey(path))
            {
                Debug.LogError("配置容器中未添加该路径地址, 请检查Config中是否添加该路径地址");
                return null;
            }

            T[] assets = new T[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                T asset = Resources.Load<T>(_ResourcePathDic[path] + names[i]);

                if (asset == null)
                {
                    Debug.LogError("ResourcesManager: 未找到该资源" + names[i] + ", 请检查Resources文件夹中是否有该资源");
                    assets[i] = null;
                }
                else
                {
                    assets[i] = asset;
                    if (asset is GameObject && canCreateGameObject)
                        GameObject.Instantiate(asset);
                }
            }
            return assets;
        }

        /// <summary>
        /// 同步加载文件夹中所有的资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>同步加载完成的多个资源</returns>
        public T[] LoadAssetsFolder<T>(E_ResourcesPath path, bool canCreateGameObject) where T : UnityEngine.Object
        {
            if (!_ResourcePathDic.ContainsKey(path))
            {
                Debug.LogError("配置容器中未添加该路径地址, 请检查Config中是否添加该路径地址");
                return null;
            }

            T[] assets = Resources.LoadAll<T>(_ResourcePathDic[path]);

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

        /// <summary>
        /// 异步加载单个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="name">资源名称</param>
        /// <param name="CompleteCallBack">异步加载完成后的回调操作</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <typeparam name="T">资源类型</typeparam>
        public void LoadAssetAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack, bool canCreateGameObject) where T : UnityEngine.Object
        {
            Manager.StartCoroutine(IE_LoadAssetAsync(path, name, CompleteCallBack, canCreateGameObject));
        }

        /// <summary>
        /// 异步加载多个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="CompleteCallBack">异步加载完成后的回调操作</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <param name="names">（多个）资源名称</param>
        /// <typeparam name="T">资源类型</typeparam>
        public void LoadAssetsAsync<T>(E_ResourcesPath path, Action<T[]> CompleteCallBack, bool canCreateGameObject, params string[] names) where T : UnityEngine.Object
        {
            Manager.StartCoroutine(IE_LoadAssetsAsync(path, CompleteCallBack, canCreateGameObject, names));
        }

        /// <summary>
        /// 卸载未使用的资源
        /// </summary>
        public void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
            Debug.Log("已卸载未使用的资源");
        }


        private IEnumerator IE_LoadAssetAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack, bool canCreateGameObject) where T : UnityEngine.Object
        {
            if (!_ResourcePathDic.ContainsKey(path))
            {
                Debug.LogError("配置容器中未添加该路径地址, 请检查Config中是否添加该路径地址");
                yield break;
            }

            ResourceRequest request = Resources.LoadAsync<T>(_ResourcePathDic[path] + name);
            yield return request;

            if (request.asset == null)
                Debug.LogError("ResourcesManager: 未找到该资源" + name + ", 请检查Resources文件夹中是否有该资源");

            if (request.asset is GameObject && canCreateGameObject)
                CompleteCallBack(GameObject.Instantiate(request.asset) as T);
            else
                CompleteCallBack(request.asset as T);
        }

        private IEnumerator IE_LoadAssetsAsync<T>(E_ResourcesPath path, Action<T[]> CompleteCallBack, bool canCreateGameObject, params string[] names) where T : UnityEngine.Object
        {
            if (!_ResourcePathDic.ContainsKey(path))
            {
                Debug.LogError("配置容器中未添加该路径地址, 请检查Config中是否添加该路径地址");
                yield break;
            }

            T[] assets = new T[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                ResourceRequest resources = Resources.LoadAsync<T>(_ResourcePathDic[path] + names[i]);
                yield return resources;

                if (resources.asset == null)
                {
                    Debug.LogError("ResourcesManager: 未找到该资源" + names[i] + ", 请检查Resources文件夹中是否有该资源");
                    assets[i] = null;
                }
                else
                {
                    assets[i] = resources.asset as T;
                }
            }

            if (assets is GameObject[] && canCreateGameObject)
            {
                foreach (var item in assets)
                {
                    GameObject.Instantiate(item);
                }
            }

            CompleteCallBack(assets as T[]);
        }

    }
}


