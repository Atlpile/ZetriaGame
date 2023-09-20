using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{


    public class ModelContainer : IContainer
    {
        private IGameStructure structure;

        private Dictionary<string, IModel> _Models = new();

        public ModelContainer(IGameStructure structure)
        {
            this.structure = structure;
        }

        public void Init()
        {
            foreach (var model in _Models.Values)
            {
                model.Init();
            }
        }

        public void AddModel<T>(T model) where T : IModel
        {
            model.GameStructure = structure;

            string modelName = typeof(T).Name;
            if (!_Models.ContainsKey(modelName))
                _Models.Add(modelName, model);
        }

        public void RemoveModel<T>() where T : IModel
        {
            string modelName = typeof(T).Name;
            if (_Models.ContainsKey(modelName))
                _Models.Remove(modelName);
            else
                Debug.LogWarning("容器中未找到" + modelName + ", 请检查容器中是否添加了该Model");
        }

        public T GetModel<T>() where T : class, IModel
        {
            string modelName = typeof(T).Name;
            if (_Models.ContainsKey(modelName))
            {
                return _Models[modelName] as T;
            }
            else
            {
                Debug.LogWarning("容器中未找到" + modelName + ", 请检查容器中是否添加了该Model");
                return null;
            }

        }

        public void CleaModel()
        {
            _Models.Clear();
        }
    }
}


