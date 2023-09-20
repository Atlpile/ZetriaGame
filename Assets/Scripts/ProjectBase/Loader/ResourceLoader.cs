using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader
{
    private readonly Dictionary<E_ResourcesPath, string> ResourceContainer;

    public ResourceLoader()
    {
        ResourceContainer = new Dictionary<E_ResourcesPath, string>
        {
            {E_ResourcesPath.Null,      ""},
            {E_ResourcesPath.Audio,     "Audio/"},
            {E_ResourcesPath.Object,    "Object/"},
            {E_ResourcesPath.UI,        "UI/"},
            {E_ResourcesPath.FX,        "FX/"},
            {E_ResourcesPath.DataSO,    "DataSO/"}
        };
    }

    public T Load<T>(E_ResourcesPath path, string name, bool canCreateGameObject = true) where T : UnityEngine.Object
    {
        T resources = Resources.Load<T>(ResourceContainer[path] + name);

        if (resources == null)
        {
            Debug.LogError("ResourcesLoader: 未找到该资源" + name + ", 请检查Resources文件夹中是否有该资源");
            return null;
        }

        if (resources is GameObject && canCreateGameObject)
            return GameObject.Instantiate(resources);
        else
            return resources;
    }

    public void LoadAsync<T>(string name, Action<T> CompleteCallBack) where T : UnityEngine.Object
    {
        LoadAsync<T>(E_ResourcesPath.Null, name, CompleteCallBack, false);
    }

    public void LoadAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack) where T : UnityEngine.Object
    {
        LoadAsync<T>(path, name, CompleteCallBack, false);
    }

    public void LoadAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack, bool canCreateGameObject = true) where T : UnityEngine.Object
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(path, name, CompleteCallBack, canCreateGameObject));
    }

    public void UnLoad()
    {
        Resources.UnloadUnusedAssets();
        Debug.Log("已卸载未使用的资源");
    }

    private IEnumerator IE_LoadAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack, bool canCreateGameObject) where T : UnityEngine.Object
    {
        ResourceRequest resources = Resources.LoadAsync<T>(ResourceContainer[path] + name);
        yield return resources;

        if (resources.asset == null)
            Debug.LogError("ResourcesLoader: 未找到该资源" + name + ", 请检查Resources文件夹中是否有该资源");

        if (resources.asset is GameObject && canCreateGameObject)
            CompleteCallBack(GameObject.Instantiate(resources.asset) as T);
        else
            CompleteCallBack(resources.asset as T);
    }
}
