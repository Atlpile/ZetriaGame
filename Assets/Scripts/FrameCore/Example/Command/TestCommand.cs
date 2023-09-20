using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class TestCommand : BaseCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("执行TestCommand逻辑");

            var model = GameStructure.GetModel<ITestModel>();
            Debug.Log("当前值：" + model.FloatValue);
            model.FloatValue--;
            Debug.Log("修改后值：" + model.FloatValue);
        }
    }
}


