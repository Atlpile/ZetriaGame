using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

public class TestEventCommand : BaseCommand
{
    protected override void OnExecute()
    {
        Debug.Log("执行TestEventCommand逻辑");

        TestEventArgs args = new TestEventArgs("Atlpile", 20, 100, true);
        GameStructure.TriggerGameEvent<TestEventArgs>(args);
    }


}


