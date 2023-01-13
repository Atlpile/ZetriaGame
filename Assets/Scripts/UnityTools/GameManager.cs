using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    public SceneLoader SceneLoader { get; set; }
    public ResourceLoader ResourcesLoader { get; set; }
    public ObjectPool ObjectPool { get; set; }
    public AudioManager AudioManager { get; set; }
    public UIManager UIManager { get; set; }
    public BinaryDataManager DataManager { get; set; }

    private void Awake()
    {
        if (s_instance != null)
            Destroy(gameObject);
        else
            s_instance = this;

        DontDestroyOnLoad(this);

        Init();
    }

    private void Init()
    {
        SceneLoader = new SceneLoader();
        ResourcesLoader = new ResourceLoader();
        ObjectPool = new ObjectPool();
        AudioManager = new AudioManager();
        UIManager = new UIManager();
        DataManager = new BinaryDataManager();
    }
}
