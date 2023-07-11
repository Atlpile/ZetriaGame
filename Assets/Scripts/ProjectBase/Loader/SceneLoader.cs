using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_SceneName { SampleScene, TestScene };

public class SceneLoader
{

    public void Load(string sceneName, UnityAction loadAction)
    {
        SceneManager.LoadScene(sceneName);
        loadAction();
    }

    public void Load(E_SceneName sceneName, UnityAction loadAction)
    {
        SceneManager.LoadScene(Enum.GetName(typeof(E_SceneName), sceneName));
        loadAction();
    }

    public void LoadSceneAsync(string sceneName, UnityAction loadAction)
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(sceneName, loadAction));
    }

    public void LoadSceneAsync(E_SceneName sceneName, UnityAction loadAction)
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(sceneName, loadAction));
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private IEnumerator IE_LoadAsync(string sceneName, UnityAction loadAction)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

        while (!ao.isDone)
        {
            // Debug.Log(ao.progress);
            yield return ao.progress;
        }

        loadAction();
    }

    private IEnumerator IE_LoadAsync(E_SceneName sceneName, UnityAction loadAction)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(Enum.GetName(typeof(E_SceneName), sceneName));

        while (!ao.isDone)
        {
            // Debug.Log(ao.progress);
            yield return ao.progress;
        }

        loadAction();
    }
}
