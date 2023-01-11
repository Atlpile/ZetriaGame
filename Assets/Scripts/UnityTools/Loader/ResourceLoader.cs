using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceLoader
{
    private Dictionary<E_ResourcesPath, string> ResourcePath;

    public ResourceLoader()
    {
        ResourcePath = new Dictionary<E_ResourcesPath, string>
        {
            {E_ResourcesPath.Audio,"Audio/"},
            {E_ResourcesPath.Entity,"Entity/"},
            {E_ResourcesPath.UI,"UI/"}
        };
    }

    public T Load<T>(E_ResourcesPath path, string name, bool canCreateObject = true) where T : UnityEngine.Object
    {
        T resources = Resources.Load<T>(ResourcePath[path] + name);

        if (resources == null)
        {
            Debug.LogError("ResourcesLoader: 未找到该资源" + name + ", 请检查Resources文件夹中是否有该资源");
            return null;
        }

        if (resources is GameObject && canCreateObject)
            return GameObject.Instantiate(resources);
        else
            return resources;
    }

    public void LoadAsync<T>(E_ResourcesPath path, string name, UnityAction<T> asyncCallBack, bool canCreateObject = true) where T : UnityEngine.Object
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(path, name, asyncCallBack, canCreateObject));
    }

    private IEnumerator IE_LoadAsync<T>(E_ResourcesPath path, string name, UnityAction<T> asyncCallBack, bool canCreateObject) where T : UnityEngine.Object
    {
        ResourceRequest resources = Resources.LoadAsync<T>(ResourcePath[path] + name);
        yield return resources;

        if (resources.asset == null)
        {
            Debug.LogError("ResourcesLoader: 未找到该资源" + name + ", 请检查Resources文件夹中是否有该资源");
        }

        if (resources.asset is GameObject && canCreateObject)
            asyncCallBack(GameObject.Instantiate(resources.asset) as T);
        else
            asyncCallBack(resources.asset as T);
    }

    public void ReleaseResource()
    {
        Resources.UnloadUnusedAssets();
        Debug.Log("已卸载未使用的资源");
    }
}
