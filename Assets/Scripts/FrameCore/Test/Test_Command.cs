using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Command : BaseCommand
{
    protected override void OnExecute()
    {
        var model = GameModule.GetModel<Test_Model>();
        Debug.Log("减去前的血量为：" + model.mp);
        model.mp--;
        Debug.Log("减去后的血量为：" + model.mp);
    }
}
