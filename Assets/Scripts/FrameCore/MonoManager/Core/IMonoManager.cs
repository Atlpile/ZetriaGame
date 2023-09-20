using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IMonoManager
    {
        T GetManager<T>() where T : class, IManager;
    }
}


