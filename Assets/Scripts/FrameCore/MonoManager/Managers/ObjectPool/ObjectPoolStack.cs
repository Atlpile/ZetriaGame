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


        public void PushObj(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_rootObj.transform);
            obj.GetComponent<IPoolObject>().OnRelease();
            _PoolStackContainer.Push(obj);
        }

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

        public GameObject PopObj()
        {
            if (_PoolStackContainer.Count > 0)
            {
                GameObject obj = _PoolStackContainer.Pop();
                obj.GetComponent<IPoolObject>().OnPop();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                FillObj(1);
                return PopObj();
            }
        }

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


