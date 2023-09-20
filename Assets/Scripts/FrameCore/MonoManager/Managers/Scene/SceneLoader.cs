using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameCore
{
    public class SceneLoader : BaseManager, ISceneLoader
    {
        private bool _isLoading;

        public Scene CurrentScene => SceneManager.GetActiveScene();

        public SceneLoader(MonoManager manager) : base(manager) { }

        protected override void OnInit()
        {
            _isLoading = false;
        }

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void LoadScene(E_SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString());
        }
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="buildIndex">BuildSettings中的场景编号</param>
        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="LoadCompleteAction">场景异步加载完成后的回调操作</param>
        public void LoadSceneAsync(string sceneName, Action LoadCompleteAction)
        {
            Manager.StartCoroutine(IE_LoadSceneAsync(sceneName, LoadCompleteAction));
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="LoadCompleteAction">场景异步加载完成后的回调操作（需手动切换场景）</param>
        public void LoadSceneAsync(string sceneName, Action<AsyncOperation> LoadCompleteAction)
        {
            Manager.StartCoroutine(IE_LoadSceneAsync(sceneName.ToString(), LoadCompleteAction));
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="LoadCompleteAction">场景异步加载完成后的回调操作</param>
        public void LoadSceneAsync(E_SceneName sceneName, Action LoadCompleteAction)
        {
            Manager.StartCoroutine(IE_LoadSceneAsync(sceneName.ToString(), LoadCompleteAction));
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="LoadCompleteAction">场景异步加载完成后的回调操作（需手动切换场景）</param>
        public void LoadSceneAsync(E_SceneName sceneName, Action<AsyncOperation> LoadCompleteAction)
        {
            Manager.StartCoroutine(IE_LoadSceneAsync(sceneName.ToString(), LoadCompleteAction));
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="buildIndex">BuildSettings中的场景编号</param>
        /// <param name="LoadCompleteAction">场景异步加载完成后的回调操作</param>
        public void LoadSceneAsync(int buildIndex, Action LoadCompleteAction)
        {
            Manager.StartCoroutine(IE_LoadSceneAsync(buildIndex, LoadCompleteAction));
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="buildIndex">BuildSettings中的场景编号</param>
        /// <param name="LoadCompleteAction">场景异步加载完成后的回调操作（需手动切换场景）</param>
        public void LoadSceneAsync(int buildIndex, Action<AsyncOperation> LoadCompleteAction)
        {
            Manager.StartCoroutine(IE_LoadSceneAsync(buildIndex, LoadCompleteAction));
        }

        private IEnumerator IE_LoadSceneAsync(int buildIndex, Action LoadCompleteAction)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(buildIndex);

            while (!ao.isDone)
            {
                Debug.Log(ao.progress);
                yield return ao.progress;
            }
            LoadCompleteAction?.Invoke();
        }
        private IEnumerator IE_LoadSceneAsync(int buildIndex, Action<AsyncOperation> LoadCompleteAction)
        {
            if (_isLoading == false)
            {
                _isLoading = true;

                AsyncOperation ao = SceneManager.LoadSceneAsync(buildIndex);
                ao.allowSceneActivation = false;

                while (!ao.isDone)
                {
                    if (ao.progress >= 0.9f)
                    {
                        Debug.Log("场景加载完成，但未手动切换场景");
                        LoadCompleteAction?.Invoke(ao);
                    }

                    // Debug.Log(ao.progress);
                    yield return ao.progress;
                }

                _isLoading = false;
            }
        }
        private IEnumerator IE_LoadSceneAsync(string name, Action LoadCompleteAction)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);

            while (!ao.isDone)
            {
                Debug.Log(ao.progress);
                yield return ao.progress;
            }
            LoadCompleteAction?.Invoke();
        }
        private IEnumerator IE_LoadSceneAsync(string name, Action<AsyncOperation> LoadCompleteAction)
        {
            if (_isLoading == false)
            {
                _isLoading = true;

                AsyncOperation ao = SceneManager.LoadSceneAsync(name);
                ao.allowSceneActivation = false;

                while (!ao.isDone)
                {
                    if (ao.progress >= 0.9f)
                    {
                        Debug.Log("场景加载完成，但未手动切换场景");
                        LoadCompleteAction?.Invoke(ao);
                    }

                    // Debug.Log(ao.progress);
                    yield return ao.progress;
                }

                _isLoading = false;
            }
        }
    }
}


