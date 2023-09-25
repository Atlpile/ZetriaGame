using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class ObjectPoolStack
    {
        private GameObject _rootObj;
        private GameObject _subObj;
        private readonly Stack<GameObject> _PoolStackContainer = new();

        public int StackObjCount => _PoolStackContainer.Count;

        public ObjectPoolStack(GameObject obj, GameObject poolRoot)
        {
            _rootObj = new GameObject(obj.name + "_Pool");
            _rootObj.transform.SetParent(poolRoot.transform);
            _subObj = obj;
        }

        public ObjectPoolStack(GameObject obj, GameObject poolRoot, int defaultCount) : this(obj, poolRoot)
        {
            FillObj(defaultCount);
        }

        /// <summary>
        /// 将对象返回至对象池
        /// </summary>
        /// <param name="obj">想要返回的对象</param>
        public void PushObj(GameObject obj)
        {
            obj.transform.SetParent(_rootObj.transform);
            obj.GetComponent<IPoolObject>().OnRelease();
            obj.SetActive(false);
            _PoolStackContainer.Push(obj);
        }

        /// <summary>
        /// 在对象池中填充对象
        /// </summary>
        /// <param name="count">填充数量</param>
        public void FillObj(int count)
        {
            if (count > 0)
            {
                GameObject fillObj;
                for (int i = 0; i < count; i++)
                {
                    fillObj = GameObject.Instantiate(_subObj, _rootObj.transform);
                    fillObj.name = _subObj.name;

                    IPoolObject obj;
                    bool isGet = fillObj.TryGetComponent<IPoolObject>(out obj);
                    if (isGet)
                        obj.OnInit();
                    else
                        Debug.LogError("该对象中没有继承IPoolObject接口, 请检查该组件是否实现IPoolObject接口, 或该对象是否挂载脚本组件");

                    PushObj(fillObj);
                }
            }
            else
            {
                FillObj(1);
            }
        }

        /// <summary>
        /// 从对象池中取出对象
        /// </summary>
        /// <returns>取出的对象</returns>
        public GameObject PopObj()
        {
            if (_PoolStackContainer.Count > 0)
            {
                GameObject obj = _PoolStackContainer.Pop();
                obj.SetActive(true);
                obj.GetComponent<IPoolObject>().OnPop();
                return obj;
            }
            else
            {
                FillObj(1);
                return PopObj();
            }
        }

        /// <summary>
        /// 从对象池中取出多个对象
        /// </summary>
        /// <param name="count">取出数量</param>
        /// <returns>取出多个对象的列表集合</returns>
        public List<GameObject> PopObjs(int count)
        {
            if (count > 0)
            {
                List<GameObject> gameObjects = new();
                //数量足够
                if (_PoolStackContainer.Count > count)
                {
                    foreach (var item in _PoolStackContainer)
                    {
                        GameObject obj = _PoolStackContainer.Pop();
                        obj.SetActive(true);
                        gameObjects.Add(obj);
                    }
                    return gameObjects;
                }
                //数量不够
                else
                {
                    FillObj(count);
                    return PopObjs(count);
                }
            }
            else
            {
                return PopObjs(1);
            }
        }

        /// <summary>
        /// 获取对象池下的所有对象
        /// </summary>
        /// <returns>池中所有对象的集合</returns>
        public List<GameObject> GetSubObjs()
        {
            List<GameObject> gameObjects = new();
            foreach (var item in _PoolStackContainer)
            {
                gameObjects.Add(item);
            }
            return gameObjects;
        }
    }
}


