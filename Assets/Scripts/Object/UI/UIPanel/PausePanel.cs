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

        btnResume = GetUIComponent<Button>(nameof(btnResume));
        btnRestartLevel = GetUIComponent<Button>(nameof(btnRestartLevel));
        btnOptions = GetUIComponent<Button>(nameof(btnOptions));
        btnTitleScreen = GetUIComponent<Button>(nameof(btnTitleScreen));
        btnBugReport = GetUIComponent<Button>(nameof(btnBugReport));
        btnExit = GetUIComponent<Button>(nameof(btnExit));
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case nameof(btnResume):
                ResumeGame();
                break;
            case nameof(btnRestartLevel):
                RestartGame();
                break;
            case nameof(btnOptions):
                OpenOptionsPanel();
                break;
            case nameof(btnTitleScreen):
                BackToTitle();
                break;
            case nameof(btnBugReport):
                BugReport();
                break;
            case nameof(btnExit):
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
        //停止BGM
        //停止输入
        //隐藏SettingPanel
        //显示FadePanel（过渡）

        //过渡结束后
        //恢复输入 
        //加载主场景
        //显示LoadingPanel
        //隐藏GamePanel

        //效果结束后
        //隐藏LoadingPanel（过渡）
        //显示MainPanel
        //播放BGM


        GameManager manager = GameManager.Instance;
        manager.AudioManager.BGMSetting(E_AudioSettingType.Stop);
        manager.InputController.SetInputStatus(false);
        manager.UIManager.HidePanel<PausePanel>();
        manager.UIManager.ShowPanel<FadePanel>(true, () =>
        {
            manager.UIManager.HidePanel<GamePanel>();
            manager.InputController.SetInputStatus(true);
            manager.SceneLoader.LoadSceneAsync("Main", () =>
            {
                manager.UIManager.HidePanel<FadePanel>();
                manager.SceneLoader.LoadMainScene();
            });
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
