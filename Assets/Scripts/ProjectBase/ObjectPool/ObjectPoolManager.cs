using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//对象池优化：初始时AddObject，使用时LoadObject

public class ObjectPoolManager
{
    private Dictionary<string, PoolStack> PoolContainer = new Dictionary<string, PoolStack>();
    private GameObject poolRoot;


    public ObjectPoolManager()
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");
    }


    public void AddObject(GameObject obj)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(obj.name))
        {
            Debug.LogWarning("对象池中已有" + obj.name + "资源");
        }
        else
        {
            PoolContainer.Add(obj.name, new PoolStack(obj, poolRoot));
        }
    }

    public void AddObject(E_ResourcesPath path, string assetName)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(assetName))
        {
            Debug.LogWarning("对象池中已有" + assetName + "资源");
        }
        else
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(path, assetName, resObj =>
            {
                resObj.name = assetName;

                PoolContainer.Add(assetName, new PoolStack(resObj, poolRoot));
                PoolContainer[assetName].PushObj(resObj);
            });
        }
    }

    public void AddObject(E_ResourcesPath path, string assetName, int count)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(assetName))
        {
            Debug.LogWarning("对象池中已有" + assetName + "资源");
        }
        else
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(path, assetName, resObj =>
            {
                resObj.name = assetName;

                PoolContainer.Add(assetName, new PoolStack(resObj, poolRoot));
                PoolContainer[assetName].PushObj(resObj);
                PoolContainer[assetName].FillObj(count);
            });
        }
    }

    public GameObject GetObject(string name, Transform parent = null)
    {
        if (PoolContainer.ContainsKey(name))
        {
            return PoolContainer[name].DynamicPopObj(parent);
        }
        else
        {
            Debug.LogError("ObjectPoolManager: 对象池中不存在" + name + "的对象,请检查对象池是否添加该对象");
            return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(obj.name))
        {
            PoolContainer[obj.name].PushObj(obj);
        }
        else
        {
            Debug.LogError("对象池中未添加过该对象, 不能返回至对象池");
            // Debug.LogWarning("对象池中未添加过该对象，已在对象池中记录该对象");
            // AddObject_New(obj);
        }
    }

    public void Clear()
    {
        PoolContainer.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }
}

