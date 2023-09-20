using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_SceneName { SampleScene, TestScene };

public class SceneLoader
{
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
        //BUG:
        // GameManager.Instance.ObjectPoolManager.RemoveExcept("AudioPlayer");
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
        LoadingPanel panel = GameManager.Instance.UIManager.ShowPanel<LoadingPanel>();
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

    public void LoadCurrentSceneInGame()
    {
        GameManager.Instance.StartCoroutine(IE_LoadCurrentSceneInGame());
    }

    private IEnumerator IE_LoadCurrentSceneInGame()
    {
        GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Stop);
        //UI淡入后执行以下内容
        yield return new WaitForSeconds(1f);
        // Debug.Log("显示FadePanel");
        yield return GameManager.Instance.UIManager.ShowPanel<FadePanel>(true, () =>
        {
            //过渡完成后执行以下内容
            GameManager.Instance.UIManager.HidePanel<GamePanel>();
            GameManager.Instance.InputController.SetInputStatus(false);

            ClearSceneInfo();
            LoadCurrentScene();
            // Debug.Log("显示FadePanel完成");
        });

        //ATTENTION：等待时间不能超过UI的过渡时间
        //UI淡出后执行以下内容
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.InputController.SetInputStatus(true);
        GameManager.Instance.UIManager.ShowPanel<GamePanel>();
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.BGM, "bgm_02", true);
        // Debug.Log("隐藏FadePanel");
        GameManager.Instance.UIManager.HidePanel<FadePanel>(true);
    }


}
