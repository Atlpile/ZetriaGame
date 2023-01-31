using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInfo
{
    public GameObject parentObj;
    public GameObject childrenObj;
    public Stack<GameObject> poolStack = new Stack<GameObject>();

    public ObjectPoolInfo(GameObject obj, GameObject poolRoot)
    {
        parentObj = new GameObject(obj.name + "Pool");
        parentObj.transform.SetParent(poolRoot.transform);
        childrenObj = obj;
    }

    //FIXME：修复RectTransfom的取入和取出
    //解决方案1：坐标转换
    //解决方案2：创建UIPoolRoot

    public void ReturnToObjectPool(GameObject obj)
    {
        if (obj.transform is RectTransform)
        {

        }

        obj.SetActive(false);
        poolStack.Push(obj);
        obj.transform.SetParent(parentObj.transform);
    }

    public GameObject GetObjectInPool(Transform parent)
    {
        GameObject obj = poolStack.Pop();

        if (obj.transform is RectTransform)
        {

        }

        obj.SetActive(true);
        obj.transform.SetParent(parent);
        return obj;
    }

    public void FillObjectPool()
    {
        GameObject fillObj = GameObject.Instantiate(childrenObj, parentObj.transform);
        fillObj.name = childrenObj.name;
    }

    public void FillObjectPool(int count)
    {
        GameObject fillObj;
        for (int i = 0; i < count; i++)
        {
            fillObj = GameObject.Instantiate(childrenObj, parentObj.transform);
            fillObj.name = childrenObj.name;
        }
    }

}

public class ObjectPoolManager
{
    private GameObject poolRoot;
    private Dictionary<string, ObjectPoolInfo> ObjectPoolsDic = new Dictionary<string, ObjectPoolInfo>();

    public ObjectPoolManager()
    {

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

    public GameObject GetOrLoadObject(string name, E_ResourcesPath path, Transform parent = null)
    {
        if (!ObjectPoolsDic.ContainsKey(name))
        {
            GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name);
            resObj.name = name;
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
            ObjectPoolsDic.Add(obj.name, new ObjectPoolInfo(obj, poolRoot));
            ObjectPoolsDic[obj.name].ReturnToObjectPool(obj);
        }
    }

    public void AddObjectFromResources(string name, E_ResourcesPath path)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (ObjectPoolsDic.ContainsKey(name))
        {
            Debug.LogWarning("对象池中存在该名称的对象");
        }
        else
        {
            GameObject resObj = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(path, name);
            resObj.name = name;
            ObjectPoolsDic.Add(name, new ObjectPoolInfo(resObj, poolRoot));
            ObjectPoolsDic[name].ReturnToObjectPool(resObj);
        }
    }

    public void ClearPool()
    {
        ObjectPoolsDic.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }
}

