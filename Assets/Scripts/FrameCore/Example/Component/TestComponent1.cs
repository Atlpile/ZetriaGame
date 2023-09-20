using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class TestComponent1 : BaseComponent
    {
        public override IGameStructure GameStructure => TestStructure.Instance;

        private void Start()
        {

            GameStructure.AddGameEvent<TestEventArgs>(TestEventMethod);

        }

        private void OnDestroy()
        {
            GameStructure.RemoveGameEvent<TestEventArgs>(TestEventMethod);
        }

        private void TestEventMethod(TestEventArgs args)
        {
            Debug.Log(this.name + "执行监听的事件");
            Debug.Log(args.name);
            Debug.Log(args.age);
            Debug.Log(args.sex);
            Debug.Log(args.atk);
        }
    }
}


