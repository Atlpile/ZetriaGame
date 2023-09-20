using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IManager
    {
        bool IsInit { get; set; }
        void Init();
    }

}

