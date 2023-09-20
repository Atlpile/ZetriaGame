using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public static class SceneLoaderExtensions
    {
        public static void LoadCurrentScene(this ISceneLoader loader)
        {
            loader.LoadScene(loader.CurrentScene.name);
        }
        public static void LoadCurrentSceneAsync(this ISceneLoader loader, Action LoadCompleteAction)
        {
            loader.LoadSceneAsync(loader.CurrentScene.name, LoadCompleteAction);
        }
        public static void LoadCurrentSceneAsync(this ISceneLoader loader, Action<AsyncOperation> LoadCompleteAction)
        {
            loader.LoadSceneAsync(loader.CurrentScene.name, LoadCompleteAction);
        }

        public static void LoadNextScene(this ISceneLoader loader)
        {
            loader.LoadScene(loader.CurrentScene.buildIndex + 1);
        }
        public static void LoadNextSceneAsync(this ISceneLoader loader, Action LoadCompleteAction)
        {
            loader.LoadSceneAsync(loader.CurrentScene.buildIndex + 1, LoadCompleteAction);
        }
        public static void LoadNextSceneAsync(this ISceneLoader loader, Action<AsyncOperation> LoadCompleteAction)
        {
            loader.LoadSceneAsync(loader.CurrentScene.buildIndex + 1, LoadCompleteAction);
        }

        public static void LoadPrevScene(this ISceneLoader loader)
        {
            loader.LoadScene(loader.CurrentScene.buildIndex - 1);
        }
        public static void LoadPrevSceneAsync(this ISceneLoader loader, Action LoadCompleteAction)
        {
            loader.LoadSceneAsync(loader.CurrentScene.buildIndex - 1, LoadCompleteAction);
        }
        public static void LoadPrevSceneAsync(this ISceneLoader loader, Action<AsyncOperation> LoadCompleteAction)
        {
            loader.LoadSceneAsync(loader.CurrentScene.buildIndex - 1, LoadCompleteAction);
        }
    }
}


