using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameworkTest : MonoBehaviour, IController
{
    public IGameModule GameModule => Test_Module.InstanceModule;

    private void Start()
    {
        // var model = this.GameModule.GetModel<Test_Model>();
        // Debug.Log(model.isInit);
        // Debug.Log(model.mp);
        // Debug.Log(model.name);
        // Debug.Log(model.speed);

        var system = GameModule.GetSystem<Test_System>();
        system.SystemTest();
    }
}
