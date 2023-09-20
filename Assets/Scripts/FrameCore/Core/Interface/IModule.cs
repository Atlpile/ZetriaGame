using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IModule
    {
        IGameStructure GameStructure { get; set; }
    }
}
