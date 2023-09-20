using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IResourcesManager : IManager
    {
        /// <summary>
        /// 同步加载单个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="name">资源名称</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>同步加载完成的资源</returns>
        T LoadAsset<T>(E_ResourcesPath path, string name, bool canCreateGameObject) where T : UnityEngine.Object;
        /// <summary>
        /// 同步加载多个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <param name="names">（多个）资源名称</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>同步加载完成的多个资源</returns>
        T[] LoadAssets<T>(E_ResourcesPath path, bool canCreateGameObject, params string[] names) where T : UnityEngine.Object;
        /// <summary>
        /// 同步加载文件夹中所有的资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>同步加载完成的多个资源</returns>
        T[] LoadAssetsFolder<T>(E_ResourcesPath path, bool canCreateGameObject) where T : UnityEngine.Object;
        /// <summary>
        /// 异步加载单个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="name">资源名称</param>
        /// <param name="CompleteCallBack">异步加载完成后的回调操作</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <typeparam name="T">资源类型</typeparam>
        void LoadAssetAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack, bool canCreateGameObject) where T : UnityEngine.Object;
        /// <summary>
        /// 异步加载多个资源
        /// </summary>
        /// <param name="path">配置的Resources路径</param>
        /// <param name="CompleteCallBack">异步加载完成后的回调操作</param>
        /// <param name="canCreateGameObject">若为GameObject，则判断加载完成后是否创建至场景</param>
        /// <param name="names">（多个）资源名称</param>
        /// <typeparam name="T">资源类型</typeparam>
        void LoadAssetsAsync<T>(E_ResourcesPath path, Action<T[]> CompleteCallBack, bool canCreateGameObject, params string[] names) where T : UnityEngine.Object;
        /// <summary>
        /// 卸载未使用的资源
        /// </summary>
        void UnloadUnusedAssets();
    }
}


