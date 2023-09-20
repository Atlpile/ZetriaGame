using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class ObjectPoolRegister
    {
        public ObjectPoolRegister(IObjectPoolManager manager)
        {
            Debug.Log("注册对象池中的对象");
            manager.AddObjects_FromResourcesAsync
            (
                E_ResourcesPath.PoolObject,
                "TestObject 1",
                "TestObject 2",
                "TestObject 4"
            );
        }
    }
}


