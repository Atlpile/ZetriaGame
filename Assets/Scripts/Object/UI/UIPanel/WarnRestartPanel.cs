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

        button_Yes = GetUIComponent<Button>("button_Yes");
        button_No = GetUIComponent<Button>("button_No");
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "button_Yes":
                RestartCurrentLevel();
                break;
            case "button_No":
                NotRestart();
                break;
        }
    }

    private void RestartCurrentLevel()
    {
        GameManager.Instance.m_UIManager.HidePanel<WarnRestartPanel>();

        GameManager.Instance.m_SceneLoader.LoadCurrentScene();
        GameManager.Instance.m_GameController.UpdateGameStatus();
        GameManager.Instance.m_InputController.SetInputStatus(true);

        GameManager.Instance.m_ObjectPoolManager.Clear();
        GameManager.Instance.m_AudioManager.Clear();
        GameManager.Instance.m_EventManager.Clear();
    }

    private void NotRestart()
    {
        GameManager.Instance.m_UIManager.HidePanel<WarnRestartPanel>();
    }


}
