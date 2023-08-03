using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Button btnNewGame;
    private Button btnContinue;
    private Button btnOptions;
    private Button btnExit;

    protected override void Awake()
    {
        base.Awake();

        GetChildrenAllUIComponent<Button>();

        btnNewGame = GetUIComponent<Button>("btnNewGame");
        btnContinue = GetUIComponent<Button>("btnContinue");
        btnOptions = GetUIComponent<Button>("btnOptions");
        btnExit = GetUIComponent<Button>("btnExit");
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "btnNewGame":
                NewGame();
                break;
            case "btnContinue":
                ContinueGame();
                break;
            case "btnOptions":
                OpenOptionsPanel();
                break;
            case "btnExit":
                ExitGame();
                break;
        }
    }

    public override void Show(TweenCallback ShowCallBack)
    {
        base.Show(ShowCallBack);

        GameManager.Instance.AudioManager.LoadAudioData();
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.BGM, "bgm_01", true);
    }

    public override void Hide(TweenCallback HideCallBack)
    {
        base.Hide(HideCallBack);

        GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Stop);
    }

    private void NewGame()
    {
        GameManager.Instance.UIManager.HidePanel<MainPanel>(true);


        GameManager.Instance.SceneLoader.ClearSceneInfo();

        LoadingPanel panel = GameManager.Instance.UIManager.ShowPanel<LoadingPanel>(true);
        panel.WaitComplete(() =>
        {
            GameManager.Instance.SceneLoader.LoadSceneAsync("Level0", () =>
            {
                GameManager.Instance.AudioManager.LoadAudioData();
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.BGM, "bgm_02", true);
                GameManager.Instance.UIManager.HidePanel<LoadingPanel>(true);
                GameManager.Instance.UIManager.ShowPanel<GamePanel>(true);
            });
        });


    }

    private void ContinueGame()
    {
        Debug.Log("继续游戏");
    }

    private void OpenOptionsPanel()
    {
        GameManager.Instance.UIManager.ShowPanel<SettingPanel>();
    }

    private void ExitGame()
    {
        GameManager.Instance.UIManager.ShowPanel<WarnQuitPanel>();
    }
}
