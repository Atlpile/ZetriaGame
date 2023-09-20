using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface ITestSystem : ISystem
    {
        void SystemTestFunction();
        void SendCommandFunction();
    }

    public class TestSystem : BaseSystem, ITestSystem
    {
        protected override void OnInit()
        {
            Debug.Log("System初始化成功");
        }

        public void SystemTestFunction() => Debug.Log("System中的测试方法");

        public void SendCommandFunction()
        {
            Debug.Log("使用System中的方法,发送Command");
            GameStructure.SendCommand(new TestCommand());
            GameStructure.SendCommand(new TestEventCommand());

        }
    }
}


