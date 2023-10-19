using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public abstract class BaseCommand : ICommand
    {
        public MonoManager Manager => MonoManager.Instance;
        public IGameStructure GameStructure { get; set; }

        void ICommand.Execute() => OnExecute();
        protected abstract void OnExecute();
    }

}

