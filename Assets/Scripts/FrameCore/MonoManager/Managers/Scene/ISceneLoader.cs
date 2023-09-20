using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameCore
{
    public interface ISceneLoader : IManager
    {
        Scene CurrentScene { get; }

        void LoadScene(string sceneName);
        void LoadScene(E_SceneName sceneName);
        void LoadScene(int buildIndex);

        void LoadSceneAsync(string sceneName, Action LoadCompleteAction);
        void LoadSceneAsync(string sceneName, Action<AsyncOperation> LoadCompleteAction);
        void LoadSceneAsync(E_SceneName sceneName, Action LoadCompleteAction);
        void LoadSceneAsync(E_SceneName sceneName, Action<AsyncOperation> LoadCompleteAction);
        void LoadSceneAsync(int buildIndex, Action LoadCompleteAction);
        void LoadSceneAsync(int buildIndex, Action<AsyncOperation> LoadCompleteAction);

    }
}


