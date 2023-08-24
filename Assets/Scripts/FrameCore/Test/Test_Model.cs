using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Model : BaseModel
{
    public int speed;
    public float mp;
    public bool isInit;
    public string name;

    protected override void OnInit()
    {
        speed = 5;
        mp = 5.5f;
        isInit = true;
        name = "Test";

        Debug.Log("数据修改成功");
    }
}
