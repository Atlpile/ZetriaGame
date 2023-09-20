using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public abstract class BaseSystem : ISystem
    {
        public IGameStructure GameStructure { get; set; }

        void ISystem.Init() => OnInit();

        protected abstract void OnInit();


    }
}
