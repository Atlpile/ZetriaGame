using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour
{
    public const string MANAGER_PATH = "Misc/GameManager";
    public const string MAIN_SCENE_NAME = "Main";
    private void Start()
    {
        GameObject.Instantiate(Resources.Load(MANAGER_PATH));
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }
}
