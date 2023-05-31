using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    private Button btnResume;
    private Button btnRestartLevel;
    private Button btnOptions;
    private Button btnTitleScreen;
    private Button btnBugReport;
    private Button btnExit;

    protected override void Awake()
    {
        GetChildrenAllUIComponent<Button>();

        btnResume = GetUIComponent<Button>("btnResume");
        btnRestartLevel = GetUIComponent<Button>("btnRestartLevel");
        btnOptions = GetUIComponent<Button>("btnOptions");
        btnTitleScreen = GetUIComponent<Button>("btnTitleScreen");
        btnBugReport = GetUIComponent<Button>("btnBugReport");
        btnExit = GetUIComponent<Button>("btnExit");
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "btnResume":
                ResumeGame();
                break;
            case "btnRestartLevel":
                RestartGame();
                break;
            case "btnOptions":
                OpenOptionsPanel();
                break;
            case "btnTitleScreen":
                BackToTitle();
                break;
            case "btnBugReport":
                BugReport();
                break;
            case "btnExit":
                ExitGame();
                break;

        }
    }

    private void ResumeGame()
    {
        GameManager.Instance.m_GameController.UpdateGameStatus();
    }

    private void RestartGame()
    {
        GameManager.Instance.m_UIManager.ShowPanel<WarnRestartPanel>();
    }

    private void OpenOptionsPanel()
    {
        GameManager.Instance.m_UIManager.ShowPanel<SettingPanel>();
    }

    private void BackToTitle()
    {
        GameManager.Instance.m_UIManager.HidePanel<GamePanel>();
        GameManager.Instance.m_GameController.UpdateGameStatus();
        GameManager.Instance.m_InputController.SetInputStatus(true);

        GameManager.Instance.m_ObjectPoolManager.Clear();
        GameManager.Instance.m_AudioController.Clear();

        GameManager.Instance.m_SceneLoader.LoadSceneAsync("Main", () =>
        {
            GameManager.Instance.m_UIManager.ShowPanel<MainPanel>();
        });
    }

    private void BugReport()
    {
        Debug.Log("打开错误报告面板");
    }

    private void ExitGame()
    {
        GameManager.Instance.m_UIManager.ShowPanel<WarnQuitPanel>();
    }


}
