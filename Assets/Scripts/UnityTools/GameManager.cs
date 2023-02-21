using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    public SceneLoader m_SceneLoader { get; set; }
    public ResourceLoader m_ResourcesLoader { get; set; }
    public ObjectPool m_ObjectPool { get; set; }
    public ObjectPoolManager m_ObjectPoolManager { get; set; }
    public AudioManager m_AudioManager { get; set; }
    public AudioController m_AudioController { get; set; }
    public UIManager m_UIManager { get; set; }
    public BinaryDataManager m_BinaryDataManager { get; set; }
    public InputController m_InputController { get; set; }
    public EventManager m_EventManager { get; set; }

    public AmmoManager m_AmmoManager { get; set; }
    public ItemManager m_ItemManager { get; set; }

    public GameData gameData;

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
        m_ObjectPool = new ObjectPool();
        m_ObjectPoolManager = new ObjectPoolManager();
        m_AudioManager = new AudioManager();
        m_AudioController = new AudioController();
        m_UIManager = new UIManager();
        m_BinaryDataManager = new BinaryDataManager();
        m_InputController = new InputController();
        m_EventManager = new EventManager();

        m_AmmoManager = new AmmoManager();
        m_ItemManager = new ItemManager();

        gameData = m_BinaryDataManager.LoadData<GameData>("GameData");
    }

    private void Start()
    {
        // m_UIManager.ShowPanel<GamePanel>();
        // m_UIManager.ShowPanel<MainPanel>();
        m_UIManager.ShowPanel<InputPanel>();
    }



    // private void Update()
    // {
    //     // GameManager.Instance.m_InputManager.UpdateInput();

    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         GameManager.Instance.m_UIManager.HidePanel<GamePanel>();
    //     }
    //     else if (Input.GetMouseButtonDown(1))
    //     {
    //         GameManager.Instance.m_UIManager.ShowPanel<GamePanel>();
    //         // GameManager.Instance.UIManager.ShowPanelAsync<MainPanel>((MainPanel) => { });
    //     }
    // }
}
