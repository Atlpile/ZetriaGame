using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameCore
{
    public abstract class BaseManager : IManager
    {
        public bool IsInit { get; set; }
        protected MonoManager Manager { get; set; }

        public BaseManager(MonoManager manager)
        {
            Manager = manager;
        }

        public void Init()
        {
            if (IsInit == false)
            {
                OnInit();
                IsInit = true;
            }
            else
            {
                Debug.LogWarning("Manager已初始化");
            }
        }

        protected abstract void OnInit();
    }

}

