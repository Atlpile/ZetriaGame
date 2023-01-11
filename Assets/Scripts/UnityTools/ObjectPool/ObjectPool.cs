using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolInfo
{
    public GameObject fatherObject;
    public GameObject childrenObject;
    public Stack<GameObject> poolStack = new Stack<GameObject>();

    public PoolInfo(GameObject obj, GameObject poolRoot)
    {
        fatherObject = new GameObject(obj.name + "Pool");
        fatherObject.transform.parent = poolRoot.transform;
        childrenObject = obj;
    }

    public void ReturnObjectInPool(GameObject obj)
    {
        obj.SetActive(false);
        poolStack.Push(obj);
        obj.transform.parent = fatherObject.transform;
    }

    public GameObject GetObjectInPool()
    {
        GameObject gameObject = poolStack.Pop();
        gameObject.SetActive(true);
        gameObject.transform.parent = null;
        return gameObject;
    }

    public GameObject CreateNewObjectFromPool()
    {
        GameObject newObj = GameObject.Instantiate(childrenObject, fatherObject.transform);
        newObj.name = childrenObject.name;
        return newObj;
    }

}

public class ObjectPool
{
    private GameObject poolRoot;
    public Dictionary<string, PoolInfo> GameObjectPools;

    public ObjectPool()
    {
        GameObjectPools = new Dictionary<string, PoolInfo>();
    }


    public GameObject AddObject(string name, GameObject gameObject)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        gameObject.name = name;
        GameObjectPools.Add(name, new PoolInfo(gameObject, poolRoot));
        GameObjectPools[name].ReturnObjectInPool(gameObject);

        return GameObjectPools[name].GetObjectInPool();
    }

    public GameObject GetPoolObject(string name)
    {
        if (GameObjectPools.ContainsKey(name) && GameObjectPools[name].poolStack.Count > 0)
            return GameObjectPools[name].GetObjectInPool();
        else
            return GameObjectPools[name].CreateNewObjectFromPool();
    }

    public GameObject GetOrLoadObject(string name, E_ResourcesPath path)
    {
        if (GameObjectPools.ContainsKey(name) && GameObjectPools[name].poolStack.Count > 0)
        {
            return GameObjectPools[name].GetObjectInPool();
        }
        else
        {
            GameObject gameObject = GameManager.Instance.ResourcesLoader.Load<GameObject>(path, name);
            gameObject.name = name;
            return gameObject;
        }
    }

    public void GetOrLoadObjectAsync(string name, E_ResourcesPath path, UnityAction<GameObject> loadAction)
    {
        if (GameObjectPools.ContainsKey(name) && GameObjectPools[name].poolStack.Count > 0)
        {
            loadAction(GameObjectPools[name].GetObjectInPool());
        }
        else
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(path, name, (obj) =>
            {
                obj.name = name;
                loadAction(obj);
            });
        }
    }

    public void ReturnObject(string name, GameObject obj, UnityAction ReleaseAction)
    {
        if (poolRoot == null)
            poolRoot = new GameObject("PoolRoot");

        if (GameObjectPools.ContainsKey(name))
        {
            ReleaseAction();
            GameObjectPools[name].ReturnObjectInPool(obj);
        }
        else
        {
            ReleaseAction();
            GameObjectPools.Add(name, new PoolInfo(obj, poolRoot));
            GameObjectPools[name].ReturnObjectInPool(obj);
        }
    }

    public void ClearPool()
    {
        GameObjectPools.Clear();
        poolRoot = null;
        GameObject.Destroy(GameObject.Find("PoolRoot"));
    }

    public int GetPoolStackCount(string name)
    {
        return GameObjectPools[name].poolStack.Count;
    }
}
