using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarnQuitPanel : BasePanel
{
    private Button button_Yes;
    private Button button_No;

    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Button>();

        button_Yes = GetUIComponent<Button>(nameof(button_Yes));
        button_No = GetUIComponent<Button>(nameof(button_No));
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case nameof(button_Yes):
                QuitGame();
                break;
            case nameof(button_No):
                NotQuit();
                break;
        }
    }

    private void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }

    private void NotQuit()
    {
        GameManager.Instance.UIManager.HidePanel<WarnQuitPanel>();
    }


}
