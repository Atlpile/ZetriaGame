using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_InitPanel
{
    None, Main, Game, Input, Loading,
}

public class GameManager : MonoBehaviour
{
    public E_InitPanel panel;

    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    public SceneLoader m_SceneLoader { get; set; }
    public ResourceLoader m_ResourcesLoader { get; set; }

    public EventManager m_EventManager { get; set; }
    public ObjectPoolManager m_ObjectPoolManager { get; set; }
    public SaveLoadManager m_SaveLoadManager { get; set; }
    public AudioManager m_AudioManager { get; set; }
    public UIManager m_UIManager { get; set; }

    public InputController m_InputController { get; set; }
    public GameController m_GameController { get; set; }
    // public GameDataManager m_GameDataManager { get; set; }
    public ItemManager m_ItemManager { get; set; }



    private void Awake()
    {
        if (s_instance != null)
            Destroy(this.gameObject);
        else
            s_instance = this;

        DontDestroyOnLoad(this);

        Init();
    }

    private void Init()
    {
        m_SceneLoader = new SceneLoader();
        m_ResourcesLoader = new ResourceLoader();

        m_EventManager = new EventManager();
        m_ObjectPoolManager = new ObjectPoolManager();
        m_SaveLoadManager = new SaveLoadManager();
        m_AudioManager = new AudioManager();
        m_UIManager = new UIManager();


        m_InputController = new InputController();
        m_GameController = new GameController();
        // m_GameDataManager = new GameDataManager();

        m_ItemManager = new ItemManager();

    }

    private void Start()
    {
        InitUI();

        Application.targetFrameRate = 144;
    }

    private void Update()
    {
        m_GameController.UpdateInput();

        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     m_UIManager.ShowPanel<LoadingPanel>();
        // }
        // else if (Input.GetKeyDown(KeyCode.D))
        // {
        //     m_UIManager.HidePanel<LoadingPanel>();
        // }
    }


    private void InitUI()
    {
        switch (panel)
        {
            case E_InitPanel.None:
                break;
            case E_InitPanel.Main:
                m_UIManager.ShowPanel<MainPanel>();
                break;
            case E_InitPanel.Input:
                m_UIManager.ShowPanel<InputPanel>();
                break;
            case E_InitPanel.Game:
                m_UIManager.ShowPanel<GamePanel>();
                break;
            case E_InitPanel.Loading:
                m_UIManager.ShowPanel<LoadingPanel>();
                break;
        }
    }




}
