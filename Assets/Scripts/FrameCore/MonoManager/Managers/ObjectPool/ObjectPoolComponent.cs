using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    /*
        对象池需要扩展的功能
            1.在Inspector中可以看到注册的对象池（只读）                     【完成】
            3.在Inspector中可以进行对象池预加载                             【完成】
            4.为对象池中弹出的对象添加父节点
            5.在Inspector中可以控制Inspector中添加的对象是否进行预加载      【完成】
    */

    public class ObjectPoolComponent : BaseComponent
    {
        public override IGameStructure GameStructure => null;
        private IObjectPoolManager _ObjectPoolManager;

        [SerializeField] private bool allowRegisterObject;
        [SerializeField] private List<GameObject> RegisterList = new List<GameObject>();
        [SerializeField] private List<GameObject> RegisteredPools = new List<GameObject>();

        private void Awake()
        {
            _ObjectPoolManager = Manager.GetManager<IObjectPoolManager>();
        }

        private void Start()
        {
            _ObjectPoolManager.OnUpdateObjectEvent += DisplayRegisteredObjects;

            if (allowRegisterObject)
                RegisterObject();
        }

        private void OnDestroy()
        {
            _ObjectPoolManager.OnUpdateObjectEvent -= DisplayRegisteredObjects;
        }

        //在Component中注册对象
        private void RegisterObject()
        {
            if (RegisterList.Count == 0)
                return;

            foreach (var obj in RegisterList)
            {
                if (obj.TryGetComponent<IPoolObject>(out var component))
                {
                    _ObjectPoolManager.AddObject(obj);
                }
                else
                {
                    Debug.LogError(obj.name + "没有继承IPoolObject接口, 添加至对象池失败");
                }
            }
        }

        //TODO:该逻辑应写在Editor中
        private void DisplayRegisteredObjects()
        {
            //同步更新对象池
            RegisteredPools = _ObjectPoolManager.GetAllPoolStackObject();
        }

    }

}
