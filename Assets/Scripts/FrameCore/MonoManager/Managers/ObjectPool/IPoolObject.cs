using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IPoolObject
    {
        void OnInit();
        void OnPop();
        void OnRelease();
        void OnPush();
    }
}


