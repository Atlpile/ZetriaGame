using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Module : BaseGameModule<Test_Module>
{
    protected override void RegisterModule()
    {
        // AddModel<Test_Model>(new Test_Model());
        AddModel<Test_Model>(new Test_Model());

        AddSystem<Test_System>(new Test_System());
        // AddSystem<Test_System>(new Test_System());
    }
}
