using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPoolInfo
{
    public GameObject parentObj;
    public GameObject childrenObj;
    public Stack<GameObject> poolStack = new Stack<GameObject>();

    public ObjectPoolInfo(GameObject obj, GameObject poolRoot)
    {
        parentObj = new GameObject(obj.name + "_Pool");
        parentObj.transform.SetParent(poolRoot.transform);
        childrenObj = obj;
    }

    //FIXME：修复RectTransfom的取入和取出
    //解决方案1：坐标转换
    //解决方案2：创建UIPoolRoot

    public void ReturnToObjectPool(GameObject obj)
    {
        // if (obj.transform is RectTransform)
        // {

        // }

        obj.SetActive(false);
        poolStack.Push(obj);
        obj.transform.SetParent(parentObj.transform);
    }

    public GameObject GetObjectInPool(Transform parent)
    {
        GameObject obj = poolStack.Pop();

        // if (obj.transform is RectTransform)
        // {

        // }

        obj.SetActive(true);
        obj.transform.SetParent(parent);
        return obj;
    }

    public void FillObjectPool()
    {
        GameObject fillObj = GameObject.Instantiate(childrenObj);
        fillObj.name = childrenObj.name;

        ReturnToObjectPool(fillObj);
    }

    public void FillObjectPool(int count)
    {
        GameObject fillObj;
        for (int i = 0; i < count; i++)
        {
            fillObj = GameObject.Instantiate(childrenObj, parentObj.transform);
            fillObj.name = childrenObj.name;

            ReturnToObjectPool(fillObj);
        }
    }

}

public class ObjectPoolManager
{
    private GameObject poolRoot;
    private Dictionary<string, ObjectPoolInfo> ObjectPoolsDic = new Dictionary<string, ObjectPoolInfo>();

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
            // Debug.Log("从对象池中获取对象");
            return ObjectPoolsDic[name].GetObjectInPool(parent);
        }
        else
        {
            //递归填充
            ObjectPoolsDic[name].FillObjectPool();
            // Debug.Log("从对象池中填充对象");
            return GetObject(name, parent);
        }
    }

    public GameObject GetOrLoadObject(string name, E_ResourcesPath path, Transform parent = null)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (!ObjectPoolsDic.ContainsKey(name))
        {
            GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name);
            resObj.name = name;
            ObjectPoolsDic.Add(resObj.name, new ObjectPoolInfo(resObj, poolRoot));
            Debug.Log("从Resources中获取对象");
            return resObj;
        }
        else if (ObjectPoolsDic[name].poolStack.Count > 0)
        {
            Debug.Log("从对象池中获取对象");
            return ObjectPoolsDic[name].GetObjectInPool(parent);
        }
        else
        {
            //递归填充
            ObjectPoolsDic[name].FillObjectPool();
            Debug.Log("从对象池中填充对象");
            return GetOrLoadObject(name, path, parent);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (ObjectPoolsDic.ContainsKey(obj.name))
        {
            // Debug.Log("返回对象池");
            ObjectPoolsDic[obj.name].ReturnToObjectPool(obj);
        }
        else
        {
            // Debug.Log("创建新对象池并添加对象");
            ObjectPoolsDic.Add(obj.name, new ObjectPoolInfo(obj, poolRoot));
            ObjectPoolsDic[obj.name].ReturnToObjectPool(obj);
        }
    }

    public void AddObject(GameObject obj, string name)
    {
        if (obj == null)
        {
            Debug.Log("");
            return;
        }
        else
        {
            if (poolRoot == null)
                poolRoot = new GameObject("PoolRoot");

            obj.name = name;
            ObjectPoolsDic.Add(name, new ObjectPoolInfo(obj, poolRoot));
            ObjectPoolsDic[name].ReturnToObjectPool(obj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path)
    {
        if (ObjectPoolsDic.ContainsKey(name))
        {
            Debug.LogWarning("对象池中存在该名称的对象");
        }
        else
        {
            if (poolRoot == null)
                poolRoot = new GameObject("PoolRoot");

            GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name);
            resObj.name = name;
            ObjectPoolsDic.Add(name, new ObjectPoolInfo(resObj, poolRoot));
            ObjectPoolsDic[name].ReturnToObjectPool(resObj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path, int fillCount)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name);
        resObj.name = name;
        ObjectPoolsDic.Add(name, new ObjectPoolInfo(resObj, poolRoot));
        ObjectPoolsDic[name].FillObjectPool(fillCount - 1);
    }

    public void ClearPool()
    {
        ObjectPoolsDic.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }
}

