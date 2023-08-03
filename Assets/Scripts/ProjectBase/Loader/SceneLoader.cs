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

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void LoadSceneAsync(string sceneName, UnityAction loadAction)
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(sceneName, loadAction));
    }

    public void LoadSceneAsync(E_SceneName sceneName, UnityAction loadAction)
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(sceneName, loadAction));
    }

    public void LoadCurrentSceneAsync(UnityAction LoadAction)
    {
        GameManager.Instance.StartCoroutine(IE_LoadAsync(SceneManager.GetActiveScene().name, LoadAction));
    }


    public void ClearSceneInfo()
    {
        GameManager.Instance.ObjectPoolManager.RemoveExcept("AudioPlayer");
        GameManager.Instance.EventManager.Clear();
        // GameManager.Instance.ResourcesLoader.Clear();
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








    public void LoadMainScene()
    {
        LoadingPanel panel = GameManager.Instance.UIManager.ShowPanel<LoadingPanel>(true);
        panel.WaitComplete(() =>
        {
            GameManager.Instance.UIManager.HidePanel<LoadingPanel>(true);
            MainPanel mainPanel = GameManager.Instance.UIManager.ShowPanel<MainPanel>(true, () =>
            {
                GameManager.Instance.UIManager.GetExistPanel<MainPanel>().SetPanelInteractiveStatus(true);
            });
            mainPanel.SetPanelInteractiveStatus(false);
        });
    }



}
