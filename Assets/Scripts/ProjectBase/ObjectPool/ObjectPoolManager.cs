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

    public GameObject GetObject(string name, Transform parent = null)
    {
        if (!PoolContainer.ContainsKey(name))
        {
            Debug.LogError("对象池中不存在该名字的对象,请检查名称是否输入正确");
            return null;
        }
        else if (PoolContainer[name].StackObjCount > 0)
        {
            return PoolContainer[name].Pop(parent);
        }
        else
        {
            //递归填充
            PoolContainer[name].Fill();
            return GetObject(name, parent);
        }
    }

    public GameObject GetOrLoadObject(string name, E_ResourcesPath path, Transform parent = null, bool canCreate = true)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (!PoolContainer.ContainsKey(name))
        {
            GameObject resObj = GameManager.Instance.ResourcesLoader.Load<GameObject>(path, name, canCreate);
            resObj.name = name;
            PoolContainer.Add(resObj.name, new PoolStack(resObj, poolRoot));
            return resObj;
        }
        else if (PoolContainer[name].StackObjCount > 0)
        {
            return PoolContainer[name].Pop(parent);
        }
        else
        {
            //递归填充
            PoolContainer[name].Fill();
            return GetOrLoadObject(name, path, parent);
        }
    }


    public GameObject GetObject_New(string name, Transform parent = null)
    {
        if (PoolContainer.ContainsKey(name))
        {
            return PoolContainer[name].DynamicPop(parent);
        }
        else
        {
            Debug.LogError("ObjectPoolManager: 对象池中不存在" + name + "的对象,请检查对象池是否添加该对象");
            return null;
        }
    }

    public void AddObject_New(string assetName)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(assetName))
        {
            Debug.LogWarning("对象池中已有" + assetName + "资源");
        }
        else
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.Object, assetName, resObj =>
            {
                resObj.name = assetName;

                PoolContainer.Add(assetName, new PoolStack(resObj));
                PoolContainer[assetName].Push(resObj);
            });
        }
    }

    public void AddObject_New(GameObject obj)
    {
        PoolContainer.Add(obj.name, new PoolStack(obj));
        PoolContainer[obj.name].Push(obj);
    }

    public void ReturnObject_New(GameObject obj)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(obj.name))
        {
            PoolContainer[obj.name].Push(obj);
        }
        else
        {
            Debug.LogWarning("对象池中未添加过该对象，已在对象池中记录该对象");
            AddObject_New(obj);
        }
    }


    public void ReturnObject(GameObject obj)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (PoolContainer.ContainsKey(obj.name))
        {
            PoolContainer[obj.name].Push(obj);
        }
        else
        {
            PoolContainer.Add(obj.name, new PoolStack(obj, poolRoot));
            PoolContainer[obj.name].Push(obj);
        }
    }

    public void AddObject(GameObject obj, string name)
    {
        if (obj == null)
        {
            Debug.LogError("Obj为空,请检查是否存在Obj");
            return;
        }
        else
        {
            if (poolRoot == null)
                poolRoot = new GameObject("PoolRoot");

            obj.name = name;
            PoolContainer.Add(name, new PoolStack(obj, poolRoot));
            PoolContainer[name].Push(obj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path, bool canCreate = true)
    {
        if (PoolContainer.ContainsKey(name))
        {
            Debug.LogWarning("对象池中存在该名称的对象");
        }
        else
        {
            if (poolRoot == null)
                poolRoot = new GameObject("PoolRoot");

            GameObject resObj = GameManager.Instance.ResourcesLoader.Load<GameObject>(path, name, canCreate);
            resObj.name = name;
            PoolContainer.Add(name, new PoolStack(resObj, poolRoot));
            PoolContainer[name].Push(resObj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path, int fillCount, bool canCreate = false)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        GameObject resObj = GameManager.Instance.ResourcesLoader.Load<GameObject>(path, name, canCreate);
        resObj.name = name;
        PoolContainer.Add(name, new PoolStack(resObj, poolRoot));
        PoolContainer[name].Fill(fillCount);
    }

    public bool GetName(string name)
    {
        if (PoolContainer.ContainsKey(name))
            return true;
        else
            return false;
    }

    public void Clear()
    {
        PoolContainer.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }
}

