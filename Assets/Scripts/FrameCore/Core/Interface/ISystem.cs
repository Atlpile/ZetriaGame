using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface ISystem : IModule
    {
        void Init();
    }
}