using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IPoolObject
    {
        /// <summary>
        /// 池对象初始化时调用
        /// </summary>
        void OnInit();
        /// <summary>
        /// 池对象激活后调用
        /// </summary>
        void OnPop();
        /// <summary>
        /// 池对象失活前调用
        /// </summary>
        void OnRelease();
        /// <summary>
        /// 返回对象池前调用
        /// </summary>
        void OnPush();
    }
}


