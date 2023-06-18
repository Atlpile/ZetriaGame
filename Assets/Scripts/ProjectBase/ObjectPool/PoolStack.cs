using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStack
{
    public GameObject parentObj;
    public GameObject childrenObj;
    public Stack<GameObject> poolStack = new Stack<GameObject>();


    //FIXME：修复RectTransfom的取入和取出
    //解决方案1：坐标转换
    //解决方案2：创建UIPoolRoot
    public PoolStack(GameObject obj, GameObject poolRoot)
    {
        parentObj = new GameObject(obj.name + "_Pool");
        parentObj.transform.SetParent(poolRoot.transform);
        childrenObj = obj;
    }

    public void ReturnToObjectPool(GameObject obj)
    {
        obj.SetActive(false);
        poolStack.Push(obj);
        obj.transform.SetParent(parentObj.transform);
    }

    public GameObject GetObjectInPool(Transform parent)
    {
        GameObject obj = poolStack.Pop();
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
