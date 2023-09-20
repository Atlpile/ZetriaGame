using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public static class ExtensionMethod
    {
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
}


