using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public abstract class BaseComponent : MonoBehaviour, IComponent
    {
        public IMonoManager Manager => MonoManager.Instance;

        public abstract IGameStructure GameStructure { get; }

    }
}


