using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface ITestUtility : IUtility
    {
        void UtilityTestFunction();
    }

    public class TestUtility : ITestUtility
    {
        public void UtilityTestFunction() => Debug.Log("Utility中的测试方法");
    }
}


