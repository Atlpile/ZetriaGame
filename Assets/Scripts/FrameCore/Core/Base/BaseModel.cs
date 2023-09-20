using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public abstract class BaseModel : IModel
    {
        public IGameStructure GameStructure { get; set; }

        public void Init() => OnInit();

        protected abstract void OnInit();
    }
}
