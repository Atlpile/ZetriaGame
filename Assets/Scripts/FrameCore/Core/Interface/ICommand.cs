using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface ICommand : IModule
    {
        void Execute();
    }

}

