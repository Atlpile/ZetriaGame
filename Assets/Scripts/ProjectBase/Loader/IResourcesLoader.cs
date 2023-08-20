using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourcesLoader
{
    T Load<T>(E_ResourcesPath path, string name, bool canCreateGameObject = true) where T : UnityEngine.Object;
    void LoadAsync<T>(E_ResourcesPath path, string name, Action<T> CompleteCallBack, bool canCreateGameObject = true) where T : UnityEngine.Object;
    void UnLoad();
}
