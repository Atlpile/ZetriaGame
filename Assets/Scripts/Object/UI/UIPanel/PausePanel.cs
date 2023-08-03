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
        base.Awake();

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
        GameManager.Instance.GameController.UpdateGameStatus();
    }

    private void RestartGame()
    {
        GameManager.Instance.UIManager.ShowPanel<WarnRestartPanel>();
    }

    private void OpenOptionsPanel()
    {
        GameManager.Instance.UIManager.ShowPanel<SettingPanel>();
    }

    private void BackToTitle()
    {
        GameManager.Instance.UIManager.HidePanel<GamePanel>();
        GameManager.Instance.GameController.UpdateGameStatus();
        GameManager.Instance.InputController.SetInputStatus(true);
        GameManager.Instance.SceneLoader.ClearSceneInfo();

        GameManager.Instance.SceneLoader.LoadSceneAsync("Main", () =>
        {
            GameManager.Instance.UIManager.ShowPanel<MainPanel>();
        });
    }

    private void BugReport()
    {
        Debug.Log("打开错误报告面板");
    }

    private void ExitGame()
    {
        GameManager.Instance.UIManager.ShowPanel<WarnQuitPanel>();
    }


}
