using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    //ATTENTION：该脚本对象修改了初始执行时间，会早于其它的Awake

    public class MonoManager : MonoBehaviour, IMonoManager
    {
        private static MonoManager s_instance;
        public static MonoManager Instance => s_instance;

        public event Action OnInitManagerEvent;
        private HashSet<IManager> _ManagerContainer = new HashSet<IManager>();

        public bool allowRegisteredObjectPool;
        public bool allowRegisteredUIPanel;

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this);

            new ManagerRegister(this);

            foreach (var manager in _ManagerContainer)
            {
                manager.Init();
            }

            OnInitManagerEvent?.Invoke();
        }

        public void AddManager(IManager manager)
        {
            _ManagerContainer.Add(manager);
        }

        public void AddManager<T>() where T : IManager, new()
        {
            _ManagerContainer.Add(new T());
        }

        public T GetManager<T>() where T : class, IManager
        {
            foreach (IManager manager in _ManagerContainer)
            {
                if (manager is T)
                {
                    return manager as T;
                }
            }
            Debug.LogError("未获取到" + typeof(T).Name + "实例, 请检查场景中是否有Manager游戏对象 或 ManagerRegister是否注册该实例 或 该Manager是否继承接口");
            return null;
        }

    }
}


