using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface ITestManager : IManager
    {
        void ManagerTestFunction();
        void TestCoroutineFunction();
    }

    public class TestManager : BaseManager, ITestManager
    {
        public TestManager(MonoManager manager) : base(manager)
        {
            // Debug.Log("Manager：new对象时调用");
        }

        protected override void OnInit()
        {
            // Debug.Log("Manager：Init时调用");
        }

        public void ManagerTestFunction() => Debug.Log("Manager中的测试方法");

        public void TestCoroutineFunction()
        {
            Debug.Log("执行协程方法");
            Manager.StartCoroutine(IE_Test());
        }

        private IEnumerator IE_Test()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("协程1s后执行");
        }
    }
}


