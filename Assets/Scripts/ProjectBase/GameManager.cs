using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_InitPanel
{
    None, Main, Game, Input, Loading,
}

/*
    额外功能
    1.使用DebuggerManager管理Debugger
    2.添加基建容器，用于添加基建模块
    3.添加游戏模块容器，用于添加游戏模块
*/

public class GameManager : MonoBehaviour
{
    [SerializeField] private E_InitPanel panel;
    [SerializeField] private bool activeEventDebugger;

    private static GameManager s_instance;
    public static GameManager Instance => s_instance;


    //OPTIMIZE:使用列表添加工具类，使用接口标识工具类
    public ResourceLoader ResourcesLoader { get; set; }
    public SceneLoader SceneLoader { get; set; }

    public EventManager EventManager { get; set; }
    public ObjectPoolManager ObjectPoolManager { get; set; }
    public SaveLoadManager SaveLoadManager { get; set; }
    public AudioManager AudioManager { get; set; }
    public UIManager UIManager { get; set; }

    public InputController InputController { get; set; }
    public GameController GameController { get; set; }
    public ItemManager ItemManager { get; set; }

    public PlayerController Player
    {
        get
        {
            // PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            // if (player != null)
            //     return player;
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerController>(out var player))
                return player;

            DebugTool.Log("场景中不存在Player,获取Player失败");
            return null;
        }
    }



    private void Awake()
    {
        if (s_instance != null)
            Destroy(this.gameObject);
        else
            s_instance = this;

        DontDestroyOnLoad(this);

        InitTools();
        InitExtra();
    }

    private void InitTools()
    {
        ResourcesLoader = new ResourceLoader();
        SceneLoader = new SceneLoader();

        EventManager = new EventManager();
        ObjectPoolManager = new ObjectPoolManager();
        SaveLoadManager = new SaveLoadManager();
        AudioManager = new AudioManager();
        UIManager = new UIManager();
    }

    private void InitExtra()
    {
        InputController = new InputController();
        GameController = new GameController();

        ItemManager = new ItemManager();
    }

    private void Start()
    {
        Application.targetFrameRate = 144;
        LoadGameUI();
    }

    private void Update()
    {
        GameController.UpdateInput();
        TestInput();

        // if (!EventManager.isActiveWarning)
        EventManager.isActiveWarning = activeEventDebugger;
    }



    private void LoadGameUI()
    {
        switch (panel)
        {
            case E_InitPanel.None:
                break;
            case E_InitPanel.Main:
                UIManager.ShowPanel<MainPanel>();
                break;
            case E_InitPanel.Input:
                UIManager.ShowPanel<InputPanel>();
                break;
            case E_InitPanel.Game:
                UIManager.ShowPanel<GamePanel>(true);
                break;
            case E_InitPanel.Loading:
                SceneLoader.LoadMainScene();
                break;
        }
    }

    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveLoadManager.ClearData<GameData>(Consts.GameData);
        }
    }

}
