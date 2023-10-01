using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IObjectPoolManager : IManager
    {
        IResourcesManager ResourcesManager { get; }

        event Action OnUpdateObjectEvent;

        void AddObject(GameObject obj, int count);
        GameObject GetObject(string name, Transform parent = null);
        GameObject[] GetObjects(string name, int count, Transform parent = null);
        void ReturnObject(GameObject obj);
        bool GetPoolStackExists(string name);
        GameObject GetSubPoolRoot(string name);
        void RemovePoolStack(params string[] names);
        void RemovePoolStackExcept(params string[] names);

        List<GameObject> GetAllPoolStackObject();
    }
}


