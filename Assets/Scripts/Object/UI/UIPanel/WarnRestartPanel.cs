using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarnRestartPanel : BasePanel
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
                RestartCurrentLevel();
                break;
            case nameof(button_No):
                NotRestart();
                break;
        }
    }

    private void RestartCurrentLevel()
    {
        GameManager.Instance.UIManager.HidePanel<WarnRestartPanel>();
        GameManager.Instance.GameController.UpdateGameStatus();
        GameManager.Instance.InputController.SetInputStatus(true);
        GameManager.Instance.EventManager.EventTrigger(E_EventType.PlayerDead);
    }

    private void NotRestart()
    {
        GameManager.Instance.UIManager.HidePanel<WarnRestartPanel>();
    }


}
