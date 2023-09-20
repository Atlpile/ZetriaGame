using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class UtilityContainer : IContainer
    {
        private Dictionary<string, IUtility> _Utilities = new();

        public void Init()
        {

        }

        public void AddUtility<T>(T utility) where T : IUtility
        {
            string utilityName = typeof(T).Name;
            if (!_Utilities.ContainsKey(utilityName))
                _Utilities.Add(utilityName, utility);
        }

        public void RemoveUtility<T>() where T : IUtility
        {
            string utilityName = typeof(T).Name;
            if (_Utilities.ContainsKey(utilityName))
            {
                _Utilities.Remove(utilityName);
            }
            else
            {
                Debug.LogWarning("容器中未找到" + utilityName + ", 请检查容器中是否添加了该Utility");
            }
        }

        public T GetUtility<T>() where T : class, IUtility
        {
            string utilityName = typeof(T).Name;
            if (_Utilities.ContainsKey(utilityName))
            {
                return _Utilities[utilityName] as T;
            }
            else
            {
                Debug.LogWarning("容器中未找到" + utilityName + ", 请检查容器中是否添加了该Utility");
                return null;
            }
        }

        public void ClearUtility()
        {
            _Utilities.Clear();
        }
    }
}


