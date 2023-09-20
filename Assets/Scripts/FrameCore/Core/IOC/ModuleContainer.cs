using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class ModuleContainer
    {
        private Dictionary<Type, object> _ModuleContainer = new Dictionary<Type, object>();

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="module">模块脚本</param>
        /// <typeparam name="T">模块类</typeparam>
        public void AddModule<T>(T module)
        {
            Type moduleType = typeof(T);
            if (_ModuleContainer.ContainsKey(moduleType))
            {
                //有模块则覆盖原有模块，防止模块重复添加
                _ModuleContainer[moduleType] = module;
            }
            else
            {
                //没有模块，则添加新模块
                _ModuleContainer.Add(moduleType, module);
            }
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类</typeparam>
        public T GetModule<T>() where T : class
        {
            Type moduleType = typeof(T);
            if (_ModuleContainer.TryGetValue(moduleType, out object module))
            {
                return module as T;
            }
            Debug.LogError("模块中不存在" + moduleType.Name + "，请检查模块中是否注册该模块");
            return null;
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <typeparam name="T">模块类</typeparam>
        public void RemoveModule<T>()
        {
            Type moduleType = typeof(T);
            if (_ModuleContainer.TryGetValue(moduleType, out object module))
            {
                _ModuleContainer.Remove(moduleType);
            }
            else
            {
                Debug.LogError("模块中不存在" + moduleType.Name + "，请检查模块中是否注册该模块");
            }
        }

        /// <summary>
        /// 清空模块
        /// </summary>
        public void ClearModule()
        {
            _ModuleContainer.Clear();
        }
    }
}

