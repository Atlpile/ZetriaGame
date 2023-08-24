using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_System : BaseSystem
{
    protected override void OnInit()
    {
        Debug.Log("初始化System");
    }

    public void SystemTest()
    {
        Debug.Log("使用System中的方法");
        GameModule.SendCommand(new Test_Command());

    }
}
