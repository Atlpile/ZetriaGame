using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour
{
    private void Start()
    {
        GameObject.Instantiate(Resources.Load(Consts.MANAGER_PATH));
        SceneManager.LoadScene(Consts.MAIN_SCENE_NAME);
    }
}
