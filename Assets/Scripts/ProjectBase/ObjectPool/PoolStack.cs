using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStack
{
    private GameObject parentObj;
    private GameObject childrenObj;
    private Stack<GameObject> poolStack = new Stack<GameObject>();

    public int StackObjCount => poolStack.Count;


    public PoolStack(GameObject obj, GameObject poolRoot)
    {
        parentObj = new GameObject(obj.name + "_Pool");
        parentObj.transform.SetParent(poolRoot.transform);
        childrenObj = obj;
    }

    public PoolStack(GameObject obj)
    {
        parentObj = new GameObject(obj.name + "_Pool");
        childrenObj = obj;
    }

    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(parentObj.transform);
        poolStack.Push(obj);
    }

    public GameObject Pop(Transform parent)
    {
        GameObject obj = poolStack.Pop();
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.SetParent(parent);
            return obj;
        }
        else
        {
            Debug.LogError("PoolStack为空,不能弹出对象");
            return null;
        }
    }

    public GameObject DynamicPop(Transform parent)
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
            Fill();
            return DynamicPop(parent);
        }
    }

    public void Fill()
    {
        GameObject fillObj = GameObject.Instantiate(childrenObj);
        fillObj.name = childrenObj.name;
        Push(fillObj);
    }

    public void Fill(int count)
    {
        GameObject fillObj;
        for (int i = 0; i < count; i++)
        {
            fillObj = GameObject.Instantiate(childrenObj, parentObj.transform);
            fillObj.name = childrenObj.name;
            Push(fillObj);
        }
    }

}
