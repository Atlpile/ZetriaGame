using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class TestStructure : BaseGameStructure<TestStructure>
    {
        protected override void RegisterModule()
        {
            // Debug.Log("执行模块注册");
            AddModel<ITestModel>(new TestModel());
            AddSystem<ITestSystem>(new TestSystem());
            AddUtility<ITestUtility>(new TestUtility());
        }
    }
}


