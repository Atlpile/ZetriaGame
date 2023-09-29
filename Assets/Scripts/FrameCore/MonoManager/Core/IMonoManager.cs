using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IMonoManager
    {
        Action OnInitManagerAction { get; set; }
        T GetManager<T>() where T : class, IManager;
    }
}


