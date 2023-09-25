using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class ManagerRegister
    {
        public ManagerRegister(MonoManager manager)
        {
            manager.AddManager(new ResourcesManager(manager));
            manager.AddManager(new ObjectPoolManager(manager));
            manager.AddManager(new SceneLoader(manager));
            manager.AddManager(new UIManager(manager));
            manager.AddManager(new AudioManager(manager));
            manager.AddManager(new InputManager(manager));
        }
    }

}