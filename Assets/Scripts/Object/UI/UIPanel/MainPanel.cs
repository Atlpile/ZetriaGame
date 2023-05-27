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
                Debug.Log("继续游戏");
                break;
            case "btnOptions":
                Debug.Log("打开设置面板");
                break;
            case "btnExit":
                Debug.Log("退出游戏");
                break;
        }
    }

    private void NewGame()
    {
        GameManager.Instance.m_UIManager.HidePanel<MainPanel>();

        GameManager.Instance.m_ObjectPoolManager.ClearPool();
        GameManager.Instance.m_AudioController.ClearAudio();

        GameManager.Instance.m_SceneLoader.LoadSceneAsync("Level0", () =>
        {
            GameManager.Instance.m_UIManager.ShowPanel<GamePanel>();
        });
    }

    private void ContinueGame()
    {

    }

    private void OpenOptionsPanel()
    {

    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
