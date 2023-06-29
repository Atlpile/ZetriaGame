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

    public override void Show(TweenCallback ShowCallBack = null)
    {
        base.Show(ShowCallBack);

        GameManager.Instance.m_AudioManager.LoadAudioData();
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.BGM, "bgm_01", true);
    }

    public override void Hide(TweenCallback HideCallback = null)
    {
        base.Hide(HideCallback);

        GameManager.Instance.m_AudioManager.BGMSetting(E_AudioSettingType.Stop);
    }


    private void NewGame()
    {
        GameManager.Instance.m_UIManager.HidePanel<MainPanel>();

        GameManager.Instance.m_ObjectPoolManager.Clear();
        GameManager.Instance.m_AudioManager.Clear();
        GameManager.Instance.m_EventManager.Clear();

        GameManager.Instance.m_SceneLoader.LoadSceneAsync("Level0", () =>
        {
            GameManager.Instance.m_UIManager.ShowPanel<LoadingPanel>();
            GameManager.Instance.m_AudioManager.LoadAudioData();
        });
    }

    private void ContinueGame()
    {
        Debug.Log("继续游戏");
    }

    private void OpenOptionsPanel()
    {
        GameManager.Instance.m_UIManager.ShowPanel<SettingPanel>();
    }

    private void ExitGame()
    {
        GameManager.Instance.m_UIManager.ShowPanel<WarnQuitPanel>();
    }
}
