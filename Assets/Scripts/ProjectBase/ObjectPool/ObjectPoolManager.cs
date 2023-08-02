using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//对象池优化：初始时AddObject，使用时GetObject

public class ObjectPoolManager
{
    private Dictionary<string, PoolStack> PoolContainer = new Dictionary<string, PoolStack>();
    private GameObject poolRoot;


    public ObjectPoolManager()
    {
        if (poolRoot == null)
        {
            poolRoot = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(poolRoot);
        }
    }


    public void AddObject(GameObject obj)
    {
        if (PoolContainer.ContainsKey(obj.name))
        {
            Debug.LogWarning("对象池中已有" + obj.name + "资源");
        }
        else
        {
            PoolContainer.Add(obj.name, new PoolStack(obj, poolRoot));
        }
    }

    public void AddObject(E_ResourcesPath path, string assetName, int count = 0)
    {
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

                if (count <= 0)
                {
                    PoolContainer[assetName].PushObj(resObj);
                }
                else
                {
                    PoolContainer[assetName].PushObj(resObj);
                    PoolContainer[assetName].FillObj(count - 1);
                }
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
        if (PoolContainer.ContainsKey(obj.name))
        {
            PoolContainer[obj.name].PushObj(obj);
        }
        else
        {
            Debug.LogError("对象池中未添加过该对象, 不能返回至对象池");
        }
    }

    public void RemovePoolStack(params string[] names)
    {
        foreach (var name in names)
        {
            if (PoolContainer.ContainsKey(name))
            {
                PoolContainer.Remove(name);
                GameObject.Destroy(GameObject.Find(name + "_Pool"));
            }
            else
            {
                Debug.LogWarning("PoolRoot中未找到" + name + "_Pool, 请检查输入的名称是否正确");
            }
        }
    }

    public void RemoveExcept(params string[] names)
    {
        //获取子物体所有名称
        List<string> childNames = new List<string>();
        for (int index = 0; index < poolRoot.transform.childCount; index++)
            childNames.Add(poolRoot.transform.GetChild(index).name);

        //比对所有名称
        foreach (var name in names)
        {
            //检查容器中是否有该名称的对象
            if (PoolContainer.ContainsKey(name))
            {
                //比对单个名称
                foreach (var childName in childNames)
                {
                    if (childName == name + "_Pool")
                    {
                        childNames.Remove(childName);
                        PoolContainer.Remove(name);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("PoolRoot中未找到" + name + "_Pool, 请检查输入的名称是否正确");
            }
        }

        foreach (var childName in childNames)
        {
            GameObject.Destroy(GameObject.Find(childName));
        }
    }


    public void Clear()
    {
        PoolContainer.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }
}

