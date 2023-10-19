using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace FrameCore
{
    public sealed class ObjectPoolManager : BaseManager, IObjectPoolManager
    {
        private GameObject _poolRoot;
        private Dictionary<string, ObjectPoolStack> _ObjectPoolContainer = new();
        private Queue<IPoolObject> _PopContainer = new();

        public event Action OnUpdateObjectEvent;
        public IResourcesManager ResourcesManager => Manager.GetManager<IResourcesManager>();

        public ObjectPoolManager(MonoManager manager) : base(manager)
        {

        }

        protected override void OnInit()
        {
            _poolRoot = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(_poolRoot);
            _poolRoot.transform.SetParent(Manager.transform);
        }

        /// <summary>
        /// 向对象池中添加对象
        /// </summary>
        /// <param name="obj">游戏对象</param>
        /// <param name="count">添加的初始数量</param>
        public void AddObject(GameObject obj, int count)
        {
            if (_ObjectPoolContainer.ContainsKey(obj.name))
            {
                Debug.LogWarning("对象池中已有" + obj.name + "资源");
            }
            else
            {
                if (count != 0)
                    _ObjectPoolContainer.Add(obj.name, new ObjectPoolStack(obj, _poolRoot, count));
                else
                    _ObjectPoolContainer.Add(obj.name, new ObjectPoolStack(obj, _poolRoot));

                OnUpdateObjectEvent?.Invoke();
            }
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <param name="name">对象的名称</param>
        /// <param name="parent">设置对象的父级位置</param>
        /// <returns></returns>
        public GameObject GetObject(string name, Transform parent = null)
        {
            if (_ObjectPoolContainer.ContainsKey(name))
            {
                GameObject popObj = _ObjectPoolContainer[name].PopObj();
                // _PopContainer.Enqueue(popObj.GetComponent<IPoolObject>());
                popObj.transform.SetParent(parent);
                return popObj;
            }
            else
            {
                Debug.LogError("ObjectPoolManager: 对象池中不存在" + name + "的对象,请检查对象池是否添加该对象");
                return null;
            }
        }

        public GameObject[] GetObjects(string name, int count, Transform parent = null)
        {
            if (_ObjectPoolContainer.ContainsKey(name))
            {
                GameObject[] popObjs = _ObjectPoolContainer[name].PopObjs(count);
                // _PopContainer.Enqueue(popObj.GetComponent<IPoolObject>());

                foreach (var obj in popObjs)
                {
                    obj.transform.SetParent(parent);
                }
                return popObjs;
            }
            else
            {
                Debug.LogError("ObjectPoolManager: 对象池中不存在" + name + "的对象,请检查对象池是否添加该对象");
                return null;
            }
        }

        /// <summary>
        /// 返回至对象池
        /// </summary>
        /// <param name="obj">想要返回至对象池的对象</param>
        public void ReturnObject(GameObject obj)
        {
            if (_ObjectPoolContainer.ContainsKey(obj.name))
            {
                _ObjectPoolContainer[obj.name].PushObj(obj);
            }
            else
            {
                Debug.LogWarning("对象池中未添加过该对象, 不能返回至对象池, 已销毁该对象");
                GameObject.Destroy(obj);
            }
        }

        /// <summary>
        /// 根据游戏对象，查找对象池是否存在
        /// </summary>
        /// <param name="name">游戏对象名称</param>
        /// <returns></returns>
        public bool GetPoolStackExists(string name) => _ObjectPoolContainer.ContainsKey(name);

        /// <summary>
        /// 移除（多个）对象池
        /// </summary>
        /// <param name="names">游戏对象名称</param>
        public void RemovePoolStack(params string[] names)
        {
            foreach (var name in names)
            {
                if (_ObjectPoolContainer.ContainsKey(name))
                {
                    _ObjectPoolContainer.Remove(name);
                    GameObject.Destroy(GetSubPoolRoot(name));
                }
                else
                {
                    Debug.LogWarning("PoolRoot中未找到" + name + "_Pool, 请检查输入的名称是否正确");
                }
            }

            OnUpdateObjectEvent?.Invoke();
        }

        /// <summary>
        /// 除了指定对象池外，移除其它全部对象池
        /// </summary>
        /// <param name="names">对象名称</param>
        public void RemovePoolStackExcept(params string[] names)
        {
            //获取PoolRoot下的所有SubRoot对象
            List<GameObject> subRootContainer = new();
            for (int index = 0; index < _poolRoot.transform.childCount; index++)
            {
                subRootContainer.Add(_poolRoot.transform.GetChild(index).gameObject);
            }

            //比对所有名称是否匹配
            foreach (var name in names)
            {
                //检查容器中是否有该名称的对象
                if (_ObjectPoolContainer.ContainsKey(name))
                {
                    //比对名称，查找不被移除SubRoot
                    foreach (var subRoot in subRootContainer)
                    {
                        if (subRoot.name == name + "_Pool")
                        {
                            subRootContainer.Remove(subRoot);
                            break;
                        }
                        _ObjectPoolContainer.Remove(subRoot.transform.GetChild(0).name);
                    }
                }
                else
                {
                    Debug.LogWarning("PoolRoot中未找到" + name + "_Pool, 请检查输入的名称是否正确");
                }
            }

            OnUpdateObjectEvent?.Invoke();

            foreach (var subRoot in subRootContainer)
            {
                GameObject.Destroy(subRoot);
            }
        }

        /// <summary>
        /// 获取子对象池对象节点
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <returns></returns>
        public GameObject GetSubPoolRoot(string name)
        {
            try
            {
                GameObject subPoolRoot = _poolRoot.transform.Find(name + "_Pool").gameObject;
                return subPoolRoot;
            }
            catch
            {
                Debug.LogError("ObjectPoolManager: 未获取到子对象池名称" + name + ",请检查名称是否正确");
                return null;
            }
        }

        public List<GameObject> GetAllPoolStackObject()
        {
            List<GameObject> stackObjList = new();
            foreach (var stack in _ObjectPoolContainer.Values)
            {
                stackObjList.Add(stack.SubObj);
            }
            return stackObjList;
        }
    }
}


