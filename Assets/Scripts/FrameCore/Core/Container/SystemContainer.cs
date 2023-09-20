using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class SystemContainer : IContainer
    {
        private IGameStructure structure;
        private Dictionary<string, ISystem> _Systems = new();

        public SystemContainer(IGameStructure structure)
        {
            this.structure = structure;
        }

        public void Init()
        {
            foreach (var system in _Systems.Values)
            {
                system.Init();
            }
        }

        public void AddSystem<T>(T system) where T : ISystem
        {
            system.GameStructure = structure;

            string systemName = typeof(T).Name;
            if (!_Systems.ContainsKey(systemName))
            {
                _Systems.Add(systemName, system);
            }
        }

        public void RemoveSystem<T>() where T : ISystem
        {
            string systemName = typeof(T).Name;
            if (_Systems.ContainsKey(systemName))
            {
                _Systems.Remove(systemName);
            }
            else
            {
                Debug.LogWarning("容器中未找到" + systemName + ", 请检查容器中是否添加了该System");
            }
        }

        public T GetSystem<T>() where T : class, ISystem
        {
            string systemName = typeof(T).Name;
            if (_Systems.ContainsKey(systemName))
            {
                return _Systems[systemName] as T;
            }
            else
            {
                Debug.LogWarning("容器中未找到" + systemName + ", 请检查容器中是否添加了该System");
                return null;
            }
        }

        public void ClearSystem()
        {
            _Systems.Clear();
        }
    }
}


