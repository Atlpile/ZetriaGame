using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "New Game":
                Debug.Log("开始新游戏");
                break;
        }
    }
}
