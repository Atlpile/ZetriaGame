using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ObjectPoolManager
{
    private GameObject poolRoot;
    private Dictionary<string, PoolStack> ObjectPoolsDic = new Dictionary<string, PoolStack>();

    public ObjectPoolManager()
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");
    }

    public GameObject GetObject(string name, Transform parent = null)
    {
        if (!ObjectPoolsDic.ContainsKey(name))
        {
            Debug.LogError("对象池中不存在该名字的对象,请检查名称是否输入正确");
            return null;
        }
        else if (ObjectPoolsDic[name].poolStack.Count > 0)
        {
            return ObjectPoolsDic[name].GetObjectInPool(parent);
        }
        else
        {
            //递归填充
            ObjectPoolsDic[name].FillObjectPool();
            return GetObject(name, parent);
        }
    }

    public GameObject GetOrLoadObject(string name, E_ResourcesPath path, Transform parent = null, bool canCreate = true)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (!ObjectPoolsDic.ContainsKey(name))
        {
            GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name, canCreate);
            resObj.name = name;
            ObjectPoolsDic.Add(resObj.name, new PoolStack(resObj, poolRoot));
            return resObj;
        }
        else if (ObjectPoolsDic[name].poolStack.Count > 0)
        {
            return ObjectPoolsDic[name].GetObjectInPool(parent);
        }
        else
        {
            //递归填充
            ObjectPoolsDic[name].FillObjectPool();
            return GetOrLoadObject(name, path, parent);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (ObjectPoolsDic.ContainsKey(obj.name))
        {
            ObjectPoolsDic[obj.name].ReturnToObjectPool(obj);
        }
        else
        {
            ObjectPoolsDic.Add(obj.name, new PoolStack(obj, poolRoot));
            ObjectPoolsDic[obj.name].ReturnToObjectPool(obj);
        }
    }

    public void AddObject(GameObject obj, string name)
    {
        if (obj == null)
        {
            Debug.Log("不存在该对象");
            return;
        }
        else
        {
            if (poolRoot == null)
                poolRoot = new GameObject("PoolRoot");

            obj.name = name;
            ObjectPoolsDic.Add(name, new PoolStack(obj, poolRoot));
            ObjectPoolsDic[name].ReturnToObjectPool(obj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path, bool canCreate = true)
    {
        if (ObjectPoolsDic.ContainsKey(name))
        {
            Debug.LogWarning("对象池中存在该名称的对象");
        }
        else
        {
            if (poolRoot == null)
                poolRoot = new GameObject("PoolRoot");

            GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name, canCreate);
            resObj.name = name;
            ObjectPoolsDic.Add(name, new PoolStack(resObj, poolRoot));
            ObjectPoolsDic[name].ReturnToObjectPool(resObj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path, int fillCount, bool canCreate = false)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name, canCreate);
        resObj.name = name;
        ObjectPoolsDic.Add(name, new PoolStack(resObj, poolRoot));
        ObjectPoolsDic[name].FillObjectPool(fillCount - 1);
    }

    public void Clear()
    {
        ObjectPoolsDic.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }
}

