using System.Collections;
using System.Collections.Generic;
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

    public override void ShowSelf()
    {
        GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.BGM, "bgm_01", true);
    }

    public override void HideSelf()
    {
        GameManager.Instance.m_AudioController.BGMStop();
    }

    private void NewGame()
    {
        GameManager.Instance.m_UIManager.HidePanel<MainPanel>();

        GameManager.Instance.m_ObjectPoolManager.Clear();
        GameManager.Instance.m_AudioController.Clear();

        GameManager.Instance.m_SceneLoader.LoadSceneAsync("Level0", () =>
        {
            GameManager.Instance.m_UIManager.ShowPanel<GamePanel>();
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
