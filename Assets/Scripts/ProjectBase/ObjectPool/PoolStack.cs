using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStack
{
    private readonly GameObject parentObj;
    private readonly GameObject childrenObj;
    private readonly Stack<GameObject> poolStack = new();

    public int StackObjCount => poolStack.Count;


    public PoolStack(GameObject obj, GameObject poolRoot)
    {
        parentObj = new GameObject(obj.name + "_Pool");
        parentObj.transform.SetParent(poolRoot.transform);
        childrenObj = obj;
    }


    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(parentObj.transform);
        poolStack.Push(obj);
    }

    public GameObject DynamicPopObj(Transform parent)
    {
        if (poolStack.Count > 0)
        {
            GameObject obj = poolStack.Pop();
            obj.SetActive(true);
            obj.transform.SetParent(parent);
            return obj;
        }
        else
        {
            FillObj();
            return DynamicPopObj(parent);
        }
    }

    public void FillObj()
    {
        GameObject fillObj = GameObject.Instantiate(childrenObj);
        fillObj.name = childrenObj.name;
        PushObj(fillObj);
    }

    public void FillObj(int count)
    {
        GameObject fillObj;
        for (int i = 0; i < count; i++)
        {
            fillObj = GameObject.Instantiate(childrenObj, parentObj.transform);
            fillObj.name = childrenObj.name;
            PushObj(fillObj);
        }
    }

}
