using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_InitPanel
{
    None, Main, Game, Input
}

public class GameManager : MonoBehaviour
{
    public E_InitPanel panel;

    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    public SceneLoader m_SceneLoader { get; set; }
    public ResourceLoader m_ResourcesLoader { get; set; }
    public ObjectPoolManager m_ObjectPoolManager { get; set; }
    public AudioController m_AudioController { get; set; }
    public UIManager m_UIManager { get; set; }
    public BinaryDataManager m_BinaryDataManager { get; set; }
    public SaveLoadManager m_SaveLoadManager { get; set; }
    public InputController m_InputController { get; set; }
    public EventManager m_EventManager { get; set; }

    public AmmoManager m_AmmoManager { get; set; }
    public ItemManager m_ItemManager { get; set; }

    public GameData gameData;

    public bool IsPause { get; set; }

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
        m_ObjectPoolManager = new ObjectPoolManager();
        m_AudioController = new AudioController();
        m_UIManager = new UIManager();
        m_BinaryDataManager = new BinaryDataManager();
        m_SaveLoadManager = new SaveLoadManager();
        m_InputController = new InputController();
        m_EventManager = new EventManager();

        m_AmmoManager = new AmmoManager();
        m_ItemManager = new ItemManager();

        gameData = m_BinaryDataManager.LoadData<GameData>("GameData");
    }

    private void Start()
    {
        InitUI();
    }

    private void Update()
    {
        UpdateInput();
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
        }
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.FindGameObjectWithTag("Player"))
        {
            UpdateGameStatus();
        }
    }

    public void UpdateGameStatus()
    {
        IsPause = !IsPause;
        if (IsPause)
        {
            m_UIManager.ShowPanel<PausePanel>();
            Time.timeScale = 0;
            m_InputController.SetInputStatus(false);
        }
        else
        {
            m_UIManager.HidePanel<PausePanel>();
            Time.timeScale = 1;
            m_InputController.SetInputStatus(true);
        }
    }
}
