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
    1.Debugger控制调试是否显示
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
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (player != null)
                return player;

            Debug.Log("场景中不存在Player,获取Player失败");
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

    public void ClearSceneInfo()
    {
        ObjectPoolManager.Clear();
        AudioManager.Clear();
        EventManager.Clear();
        ResourcesLoader.Clear();
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
                LoadMainScene();
                break;
        }
    }

    private void TestInput()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     m_UIManager.ShowPanel<LoadingPanel>();
        // }
        // else if (Input.GetKeyDown(KeyCode.D))
        // {
        //     m_UIManager.HidePanel<LoadingPanel>();
        // }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveLoadManager.ClearData<GameData>(Consts.GameData);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.BGM, "bgm_01");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Destroy);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_fire");
    }

    private void LoadMainScene()
    {
        LoadingPanel panel = UIManager.ShowPanel<LoadingPanel>();
        panel.WaitComplete(() =>
        {
            UIManager.HidePanel<LoadingPanel>(true);
            MainPanel mainPanel = UIManager.ShowPanel<MainPanel>(true, () =>
            {
                UIManager.GetExistPanel<MainPanel>().SetPanelInteractiveStatus(true);
            });
            mainPanel.SetPanelInteractiveStatus(false);
        });
    }

    public void LoadCurrentScene()
    {
        StartCoroutine(IE_LoadCurrentScene());
    }

    private IEnumerator IE_LoadCurrentScene()
    {
        AudioManager.BGMSetting(E_AudioSettingType.Stop);
        //UI淡入后执行以下内容
        yield return new WaitForSeconds(1f);
        // Debug.Log("显示FadePanel");
        yield return UIManager.ShowPanel<FadePanel>(true, () =>
        {
            //过渡完成后执行以下内容
            UIManager.HidePanel<GamePanel>();
            InputController.SetInputStatus(false);

            //FIXME:重新加载当前场景时音效不会消失
            //解决方案1：不重新加载场景，重新恢复场景属性（加载场景预制体）
            //解决方案2：使音频在加载场景时不被消除（PoolRoot设为DontDestroyOnLoad）
            ClearSceneInfo();
            SceneLoader.LoadCurrentScene();
            // Debug.Log("显示FadePanel完成");
        });

        //ATTENTION：等待时间不能超过UI的过渡时间
        //UI淡出后执行以下内容
        yield return new WaitForSeconds(1.5f);
        InputController.SetInputStatus(true);
        UIManager.ShowPanel<GamePanel>();
        // Debug.Log("隐藏FadePanel");
        UIManager.HidePanel<FadePanel>(true);

    }

}
