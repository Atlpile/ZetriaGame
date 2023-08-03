using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    额外功能
    1.使用栈结构存储面板（作用：按ESC键时隐藏最上面的面板）
*/

public class UIManager
{

    private Dictionary<string, BasePanel> PanelContainer = new Dictionary<string, BasePanel>();
    private RectTransform rectCanvas;

    public UIManager()
    {
        //创建Canvas
        if (!GameObject.FindGameObjectWithTag("Canvas"))
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.UI, "Canvas", canvas =>
            {
                rectCanvas = canvas.transform as RectTransform;
                GameObject.DontDestroyOnLoad(canvas);
            });
        }

        //创建EventSystem
        if (!GameObject.FindGameObjectWithTag("EventSystem"))
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.UI, "EventSystem", eventSystem =>
            {
                GameObject.DontDestroyOnLoad(eventSystem);
            });
        }
    }


    #region Not Async

    // /// <summary>
    // /// 显示面板
    // /// </summary>
    // /// <param name="hasTween">是否启用过渡效果</param>
    // /// <param name="TweenEndFunc">过渡完成后执行的内容（需配合hasEffect）</param>
    // /// <typeparam name="T">面板类对象</typeparam>
    // /// <returns>面板类对象</returns>
    // public T ShowPanel<T>(bool hasTween = false, UnityAction TweenEndFunc = null) where T : BasePanel
    // {
    //     string panelName = typeof(T).Name;
    //     if (PanelContainer.ContainsKey(panelName))
    //     {
    //         Debug.LogWarning("UIManager:场景中存在该Panel");
    //         return PanelContainer[panelName] as T;
    //     }
    //     else
    //     {
    //         GameObject uiPrefab = GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, panelName);

    //         uiPrefab.transform.SetParent(rectCanvas);
    //         uiPrefab.transform.localPosition = Vector3.zero;
    //         uiPrefab.transform.localScale = Vector3.one;
    //         (uiPrefab.transform as RectTransform).offsetMax = Vector2.zero;
    //         (uiPrefab.transform as RectTransform).offsetMin = Vector2.zero;

    //         T panel = uiPrefab.GetComponent<T>();
    //         PanelContainer.Add(panelName, panel);

    //         if (hasTween)
    //             PanelContainer[panelName].Show(() => { TweenEndFunc?.Invoke(); });
    //         else
    //             PanelContainer[panelName].Show();

    //         return panel;
    //     }
    // }

    // /// <summary>
    // /// 隐藏面板
    // /// </summary>
    // /// <param name="hasTween">是否显示特效</param>
    // /// <param name="TweenEndFunc">特效显示结束时执行的内容（需配合hasEffect）</param>
    // /// <typeparam name="T">面板类</typeparam>
    // public void HidePanel<T>(bool hasTween = false, UnityAction TweenEndFunc = null) where T : BasePanel
    // {
    //     string panelName = typeof(T).Name;
    //     if (!PanelContainer.ContainsKey(panelName))
    //     {
    //         Debug.LogWarning("UIManager:场景中不存在该Panel");
    //         return;
    //     }
    //     else
    //     {
    //         if (hasTween)
    //         {
    //             PanelContainer[panelName].Hide(() =>
    //             {
    //                 Object.Destroy(PanelContainer[panelName].gameObject);
    //                 PanelContainer.Remove(panelName);
    //                 TweenEndFunc?.Invoke();
    //             });
    //         }
    //         else
    //         {
    //             PanelContainer[panelName].Hide();
    //             Object.Destroy(PanelContainer[panelName].gameObject);
    //             PanelContainer.Remove(panelName);
    //         }
    //     }
    // }

    #endregion


    #region Pool

    public void ShowPanel_Pool_Async<T>(bool hasDuration = false, UnityAction<T> LoadAction = null, UnityAction TweenEndFunc = null) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //场景中有面板
        if (PanelContainer.ContainsKey(panelName))
        {
            Debug.LogWarning("UIManager: 场景中存在" + panelName);
            LoadAction?.Invoke(PanelContainer[panelName] as T);
        }
        //场景中没有面板，则加载面板
        else
        {
            //对象池中是否有面板
            if (GameManager.Instance.ObjectPoolManager.GetPool(panelName))
            {
                GameObject panelPrefab = GameManager.Instance.ObjectPoolManager.GetObject(panelName);

                ShowInit(panelName, panelPrefab);
                ShowPanel<T>(panelName, panelPrefab, hasDuration, TweenEndFunc);
                LoadAction?.Invoke(panelPrefab as T);
            }
            else
            {
                GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.UI, panelName, (panelPrefab) =>
                {
                    ShowInit(panelName, panelPrefab);
                    GameManager.Instance.ObjectPoolManager.AddObject(panelPrefab);
                    ShowPanel<T>(panelName, panelPrefab, hasDuration, TweenEndFunc);
                    LoadAction?.Invoke(panelPrefab as T);
                });
            }
        }
    }

    public T ShowPanel<T>(bool hasDuration = false, UnityAction TweenEndFunc = null) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //场景中有面板
        if (PanelContainer.ContainsKey(panelName))
        {
            Debug.LogWarning("UIManager: 场景中存在" + panelName);
            return PanelContainer[panelName] as T;
        }
        //场景中没有面板，则加载面板
        else
        {
            //对象池中是否有面板
            if (GameManager.Instance.ObjectPoolManager.GetPool(panelName))
            {
                GameObject panelPrefab = GameManager.Instance.ObjectPoolManager.GetObject(panelName);

                ShowInit(panelName, panelPrefab);
                return ShowPanel<T>(panelName, panelPrefab, hasDuration, TweenEndFunc);
            }
            else
            {
                GameObject panelPrefab = GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, panelName);

                ShowInit(panelName, panelPrefab);
                GameManager.Instance.ObjectPoolManager.AddObject(panelPrefab);

                return ShowPanel<T>(panelName, panelPrefab, hasDuration, TweenEndFunc);
            }
        }
    }

    public void HidePanel<T>(bool hasDuration = false) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelContainer.ContainsKey(panelName))
        {
            //有过渡
            if (hasDuration)
            {
                //TODO:Panel处于过渡状态时重复操作的优化
                //解决方案1：处于过渡状态时，不能执行隐藏UI操作
                //解决方案2：处于过渡状态时，可以执行隐藏UI操作，但需要通过状态控制是否处于过渡状态
                PanelContainer[panelName].Hide(() =>
                {
                    GameManager.Instance.ObjectPoolManager.ReturnObject(PanelContainer[panelName].gameObject);
                    PanelContainer.Remove(panelName);
                });

            }
            //无过渡
            else
            {
                PanelContainer[panelName].Hide();
                GameManager.Instance.ObjectPoolManager.ReturnObject(PanelContainer[panelName].gameObject);
                PanelContainer.Remove(panelName);
            }
        }
        else
        {
            Debug.LogWarning("UIManager:场景中不存在该" + panelName);
        }
    }

    private void ShowInit(string panelName, GameObject panelPrefab)
    {
        //初始化UI位置
        panelPrefab.name = panelName;
        panelPrefab.transform.SetParent(rectCanvas);
        panelPrefab.transform.localPosition = Vector3.zero;
        panelPrefab.transform.localScale = Vector3.one;
        (panelPrefab.transform as RectTransform).offsetMax = Vector2.zero;
        (panelPrefab.transform as RectTransform).offsetMin = Vector2.zero;
    }

    private T ShowPanel<T>(string panelName, GameObject panelPrefab, bool hasDuration, UnityAction TweenEndFunc) where T : BasePanel
    {
        //记录面板（脚本）
        T panel = panelPrefab.GetComponent<T>();
        PanelContainer.Add(panelName, panel);

        //有过渡
        if (hasDuration)
        {
            PanelContainer[panelName].Show(() =>
            {
                TweenEndFunc?.Invoke();
            });
        }
        //无过渡
        else
            PanelContainer[panelName].Show();

        return panel;
    }

    #endregion

    public T GetExistPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelContainer.ContainsKey(panelName))
        {
            return PanelContainer[panelName] as T;
        }
        else
        {
            Debug.LogWarning("UIManager:场景中不存在" + panelName);
            return null;
        }
    }

    public void ClearExistPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelContainer.ContainsKey(panelName))
            PanelContainer.Remove(panelName);
        else
            Debug.Log("不存在该面板，不能从字典清除");
    }

}
