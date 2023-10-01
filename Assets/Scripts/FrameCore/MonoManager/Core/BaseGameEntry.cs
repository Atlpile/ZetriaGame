using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IGameEntry
    {
        void Execute();
    }

    public abstract class BaseGameEntry : BaseComponent, IGameEntry
    {
        public abstract void Execute();
    }
}


