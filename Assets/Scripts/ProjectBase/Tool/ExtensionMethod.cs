using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static void SortChildObjName(this Transform transform)
    {
        List<Transform> childObjList = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childObjList.Add(transform.GetChild(i));
        }

        childObjList.Sort((a, b) => { return a.name.Length > b.name.Length ? 1 : -1; });

        foreach (var item in childObjList)
        {
            item.SetAsLastSibling();
        }
    }

    public static Transform FindChildObjName(this Transform transform, string childName)
    {
        Transform target = null;
        target = transform.Find(childName);

        if (target != null)
        {
            return target;
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                target = transform.GetChild(i).FindChildObjName(childName);
                if (target != null) return target;
            }
            return target;
        }
    }
}
